using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Localization;
using Steer73.RockIT.Enums;
using Steer73.RockIT.Localization;
using Steer73.RockIT.Vacancies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Emailing;
using Volo.Abp.Guids;
using Volo.Abp.Linq;
using Volo.Abp.Timing;

namespace Steer73.RockIT.JobAlerts;

public class JobAlertNotificationBackgroundJob : AsyncBackgroundJob<JobAlertNotificationJobArgs>
{
    private readonly IVacancyRepository _vacancyRepository;
    private readonly IVacancyRoleTypeRepository _vacancyRoleTypeRepository;
    private readonly IJobAlertRegistrationRepository _registrationRepository;
    private readonly IJobAlertNotificationLogRepository _notificationLogRepository;
    private readonly IEmailSender _emailSender;
    private readonly IConfiguration _configuration;
    private readonly IGuidGenerator _guidGenerator;
    private readonly IClock _clock;
    private readonly ILogger<JobAlertNotificationBackgroundJob> _logger;
    private readonly IStringLocalizer<RockITResource> _localizer;

    public JobAlertNotificationBackgroundJob(
        IVacancyRepository vacancyRepository,
        IVacancyRoleTypeRepository vacancyRoleTypeRepository,
        IJobAlertRegistrationRepository registrationRepository,
        IJobAlertNotificationLogRepository notificationLogRepository,
        IEmailSender emailSender,
        IConfiguration configuration,
        IGuidGenerator guidGenerator,
        IClock clock,
        ILogger<JobAlertNotificationBackgroundJob> logger,
        IStringLocalizer<RockITResource> localizer)
    {
        _vacancyRepository = vacancyRepository;
        _vacancyRoleTypeRepository = vacancyRoleTypeRepository;
        _registrationRepository = registrationRepository;
        _notificationLogRepository = notificationLogRepository;
        _emailSender = emailSender;
        _configuration = configuration;
        _guidGenerator = guidGenerator;
        _clock = clock;
        _logger = logger;
        _localizer = localizer;
    }

    public override async Task ExecuteAsync(JobAlertNotificationJobArgs args)
    {
        var cancellationToken = CancellationToken.None;
        var vacancy = await _vacancyRepository.WithDetails()
            .Where(x => x.Id == args.VacancyId)
            .FirstOrDefaultAsync(cancellationToken);

        if (vacancy is null)
        {
            _logger.LogWarning("Job alert notification skipped. Vacancy {VacancyId} not found.", args.VacancyId);
            return;
        }

        var practiceGroupIds = vacancy.Groups.Select(pg => pg.Id).Distinct().ToList();
        if (!practiceGroupIds.Any())
        {
            _logger.LogDebug("Job alert notification skipped. Vacancy {VacancyId} has no practice groups.", args.VacancyId);
            return;
        }

        var vacancyRoleTypes = await _vacancyRoleTypeRepository.GetListOfVacancyRoleTypesAsync(vacancy.Id, cancellationToken);
        var vacancyRoleTypeIds = vacancyRoleTypes.Select(vrt => vrt.RoleTypeId).Distinct().ToList();

        var registrations = await _registrationRepository.GetActiveByPracticeGroupsAsync(practiceGroupIds, cancellationToken);
        if (registrations.Count == 0)
        {
            _logger.LogDebug("Job alert notification skipped. No registrations found for vacancy {VacancyId}.", args.VacancyId);
            return;
        }

        var practiceGroupNames = vacancy.Groups.Select(pg => pg.Name).Where(name => !name.IsNullOrWhiteSpace()).Distinct().ToList();
        var roleTypeNames = vacancyRoleTypes.Select(rt => rt.RoleType?.Name).Where(name => !name.IsNullOrWhiteSpace()).Distinct().ToList();
        var portalUrl = GetPortalBaseUrl();
        var vacancyUrl = $"{portalUrl}/VacancyDetail/{vacancy.Id}";

        foreach (var registration in registrations)
        {
            if (!registration.MatchesRoleTypes(vacancyRoleTypeIds))
            {
                continue;
            }

            var alreadySent = await _notificationLogRepository.ExistsAsync(
                registration.Id,
                vacancy.Id,
                args.NotificationType,
                cancellationToken);

            if (alreadySent)
            {
                continue;
            }

            var unsubscribeLink = $"{portalUrl}/JobAlerts/Unsubscribe/{registration.UnsubscribeToken}";
            var body = BuildNotificationBody(registration, vacancy, practiceGroupNames, roleTypeNames, vacancyUrl, unsubscribeLink, args.NotificationType);
            var subject = BuildSubject(vacancy.Title, args.NotificationType);

            await _emailSender.QueueAsync(
                to: registration.Email,
                subject: subject,
                body: body,
                isBodyHtml: false);

            registration.TouchNotification(_clock.Now);
            await _registrationRepository.UpdateAsync(registration, autoSave: true, cancellationToken: cancellationToken);

            var logEntry = new JobAlertNotificationLog(
                _guidGenerator.Create(),
                registration.Id,
                vacancy.Id,
                args.NotificationType,
                _clock.Now);

            await _notificationLogRepository.InsertAsync(logEntry, autoSave: true, cancellationToken: cancellationToken);
        }
    }

    private string BuildSubject(string vacancyTitle, JobAlertNotificationType notificationType)
    {
        return notificationType switch
        {
            JobAlertNotificationType.BrochurePublished => L("JobAlertBrochureSubject", vacancyTitle),
            _ => L("JobAlertRoleSubject", vacancyTitle)
        };
    }

    private string BuildNotificationBody(
        JobAlertRegistration registration,
        Vacancy vacancy,
        IReadOnlyCollection<string> practiceGroups,
        IReadOnlyCollection<string> roleTypes,
        string vacancyUrl,
        string unsubscribeLink,
        JobAlertNotificationType notificationType)
    {
        var sb = new StringBuilder();
        var greeting = registration.FirstName.IsNullOrWhiteSpace()
            ? L("JobAlertGreetingFallback")
            : string.Format(L("JobAlertGreeting"), registration.FirstName);

        sb.AppendLine(greeting);
        sb.AppendLine();

        if (notificationType == JobAlertNotificationType.BrochurePublished)
        {
            sb.AppendLine(string.Format(L("JobAlertBrochureBody"), vacancy.Title));
        }
        else
        {
            sb.AppendLine(string.Format(L("JobAlertRoleBody"), vacancy.Title));
        }

        if (practiceGroups.Any())
        {
            sb.AppendLine(string.Format(L("JobAlertPracticeGroups"), string.Join(", ", practiceGroups)));
        }

        if (roleTypes.Any())
        {
            sb.AppendLine(string.Format(L("JobAlertRoleTypes"), string.Join(", ", roleTypes)));
        }

        sb.AppendLine();
        sb.AppendLine(string.Format(L("JobAlertViewRole"), vacancyUrl));
        sb.AppendLine();
        sb.AppendLine(string.Format(L("JobAlertUnsubscribeHint"), unsubscribeLink));
        sb.AppendLine();
        sb.AppendLine(L("JobAlertSignature"));

        return sb.ToString();
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

    private string L(string name, params object[] arguments)
    {
        return _localizer[name, arguments];
    }
}
