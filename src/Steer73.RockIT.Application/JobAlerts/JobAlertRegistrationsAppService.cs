using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Steer73.RockIT.Permissions;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Emailing;

namespace Steer73.RockIT.JobAlerts;

public class JobAlertRegistrationsAppService : RockITAppService, IJobAlertRegistrationsAppService
{
    private readonly IJobAlertRegistrationRepository _registrationRepository;
    private readonly JobAlertRegistrationManager _registrationManager;
    private readonly IEmailSender _emailSender;
    private readonly IConfiguration _configuration;

    public JobAlertRegistrationsAppService(
        IJobAlertRegistrationRepository registrationRepository,
        JobAlertRegistrationManager registrationManager,
        IEmailSender emailSender,
        IConfiguration configuration)
    {
        _registrationRepository = registrationRepository;
        _registrationManager = registrationManager;
        _emailSender = emailSender;
        _configuration = configuration;
    }

    [RemoteService(false)]
    public override async Task OnApplicationInitializationAsync()
    {
        await base.OnApplicationInitializationAsync();
    }

    [AllowAnonymous]
    public async Task<JobAlertRegistrationDto> RegisterAsync(JobAlertRegistrationCreateDto input)
    {
        var normalizedEmail = input.Email.Trim().ToLowerInvariant();
        var existing = await _registrationRepository.FindByEmailAsync(normalizedEmail, CurrentCancellationToken);

        var registration = await _registrationManager.RegisterAsync(
            existing,
            normalizedEmail,
            input.FirstName,
            input.LastName,
            input.PracticeGroupIds ?? [],
            input.RoleTypeIds ?? [],
            CurrentCancellationToken);

        if (existing is null)
        {
            registration = await _registrationRepository.InsertAsync(registration, autoSave: true, cancellationToken: CurrentCancellationToken);
        }
        else
        {
            registration = await _registrationRepository.UpdateAsync(registration, autoSave: true, cancellationToken: CurrentCancellationToken);
        }

        return ObjectMapper.Map<JobAlertRegistration, JobAlertRegistrationDto>(registration);
    }

    [Authorize(RockITSharedPermissions.JobAlertRegistrations.Default)]
    public async Task<PagedResultDto<JobAlertRegistrationDto>> GetListAsync(JobAlertRegistrationListInput input)
    {
        var queryable = _registrationRepository.WithDetails();

        if (!input.Filter.IsNullOrWhiteSpace())
        {
            var filter = input.Filter!.Trim().ToLowerInvariant();
            queryable = queryable.Where(x =>
                x.Email.Contains(filter) ||
                (x.FirstName != null && x.FirstName.ToLower().Contains(filter)) ||
                (x.LastName != null && x.LastName.ToLower().Contains(filter)));
        }

        if (input.PracticeGroupId.HasValue)
        {
            var practiceGroupId = input.PracticeGroupId.Value;
            queryable = queryable.Where(x => x.PracticeGroups.Any(pg => pg.PracticeGroupId == practiceGroupId));
        }

        if (input.RoleTypeId.HasValue)
        {
            var roleTypeId = input.RoleTypeId.Value;
            queryable = queryable.Where(x => x.RoleTypes.Any(rt => rt.RoleTypeId == roleTypeId));
        }

        if (input.IsSubscribed.HasValue)
        {
            queryable = queryable.Where(x => x.IsSubscribed == input.IsSubscribed.Value);
        }

        var sorting = input.Sorting.IsNullOrWhiteSpace()
            ? $"{nameof(JobAlertRegistration.CreationTime)} DESC"
            : input.Sorting!;

        var totalCount = await AsyncExecuter.CountAsync(queryable, CurrentCancellationToken);

        var registrations = await AsyncExecuter.ToListAsync(
            queryable.OrderBy(sorting).Skip(input.SkipCount).Take(input.MaxResultCount),
            CurrentCancellationToken);

        return new PagedResultDto<JobAlertRegistrationDto>(
            totalCount,
            ObjectMapper.Map<List<JobAlertRegistration>, List<JobAlertRegistrationDto>>(registrations));
    }

    [Authorize(RockITSharedPermissions.JobAlertRegistrations.Default)]
    public async Task<JobAlertRegistrationDto> GetAsync(Guid id)
    {
        var entityQuery = _registrationRepository.WithDetails().Where(x => x.Id == id);
        var entity = await AsyncExecuter.FirstOrDefaultAsync(entityQuery, CurrentCancellationToken);

        if (entity is null)
        {
            throw new EntityNotFoundException(typeof(JobAlertRegistration), id);
        }

        return ObjectMapper.Map<JobAlertRegistration, JobAlertRegistrationDto>(entity);
    }

    [AllowAnonymous]
    public async Task UnsubscribeByEmailAsync(JobAlertUnsubscribeDto input)
    {
        var normalizedEmail = input.Email.Trim().ToLowerInvariant();
        var registration = await _registrationRepository.FindByEmailAsync(normalizedEmail, CurrentCancellationToken);
        if (registration is null)
        {
            return;
        }

        await UnsubscribeInternalAsync(registration);
    }

    [AllowAnonymous]
    public async Task UnsubscribeByTokenAsync(JobAlertUnsubscribeTokenDto input)
    {
        var registration = await _registrationRepository.FindByUnsubscribeTokenAsync(input.Token, CurrentCancellationToken);
        if (registration is null)
        {
            return;
        }

        await UnsubscribeInternalAsync(registration);
    }

    private async Task UnsubscribeInternalAsync(JobAlertRegistration registration)
    {
        _registrationManager.Unsubscribe(registration);
        await _registrationRepository.UpdateAsync(registration, autoSave: true, cancellationToken: CurrentCancellationToken);
        await SendUnsubscribeConfirmationEmailAsync(registration);
    }

    private async Task SendUnsubscribeConfirmationEmailAsync(JobAlertRegistration registration)
    {
        var portalUrl = GetPortalBaseUrl();
        var resubscribeLink = $"{portalUrl}/JobAlerts/Register";
        var sb = new StringBuilder();
        var greeting = registration.FirstName.IsNullOrWhiteSpace()
            ? L["JobAlertGreetingFallback"]
            : string.Format(L["JobAlertGreeting"], registration.FirstName);

        sb.AppendLine(greeting);
        sb.AppendLine();
        sb.AppendLine(L["JobAlertUnsubscribeConfirmation"]);
        sb.AppendLine();
        sb.AppendLine(string.Format(L["JobAlertResubscribePrompt"], resubscribeLink));
        sb.AppendLine();
        sb.AppendLine(L["JobAlertSignature"]);

        await _emailSender.QueueAsync(
            to: registration.Email,
            subject: L["JobAlertUnsubscribeSubject"],
            body: sb.ToString(),
            isBodyHtml: false);
    }

    private string GetPortalBaseUrl()
    {
        var baseUrl = _configuration["App1:PortalBaseUrl"];
        if (baseUrl.IsNullOrWhiteSpace())
        {
            return string.Empty;
        }

        return baseUrl.TrimEnd('/');
    }
}
