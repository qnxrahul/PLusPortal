using Steer73.RockIT.PracticeGroups;
using Steer73.RockIT.RoleTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.Guids;
using Volo.Abp.Timing;

namespace Steer73.RockIT.JobAlerts;

public class JobAlertRegistrationManager : DomainService
{
    private readonly IPracticeGroupRepository _practiceGroupRepository;
    private readonly IRepository<RoleType, Guid> _roleTypeRepository;
    private readonly IClock _clock;
    private readonly IGuidGenerator _guidGenerator;

    public JobAlertRegistrationManager(
        IPracticeGroupRepository practiceGroupRepository,
        IRepository<RoleType, Guid> roleTypeRepository,
        IClock clock,
        IGuidGenerator guidGenerator)
    {
        _practiceGroupRepository = practiceGroupRepository;
        _roleTypeRepository = roleTypeRepository;
        _clock = clock;
        _guidGenerator = guidGenerator;
    }

    public virtual async Task<JobAlertRegistration> RegisterAsync(
        JobAlertRegistration? registration,
        string email,
        string? firstName,
        string? lastName,
        IReadOnlyCollection<Guid> practiceGroupIds,
        IReadOnlyCollection<Guid>? roleTypeIds = null,
        CancellationToken cancellationToken = default)
    {
        Check.NotNullOrWhiteSpace(email, nameof(email));
        Check.NotNullOrEmpty(practiceGroupIds, nameof(practiceGroupIds));

        var distinctPracticeGroupIds = practiceGroupIds.Distinct().ToList();
        await EnsurePracticeGroupsExistAsync(distinctPracticeGroupIds, cancellationToken);

        var distinctRoleTypeIds = roleTypeIds?.Distinct().ToList() ?? [];
        if (distinctRoleTypeIds.Any())
        {
            await EnsureRoleTypesExistAsync(distinctRoleTypeIds, cancellationToken);
        }

        var entity = registration ?? new JobAlertRegistration(_guidGenerator.Create(), email, firstName, lastName);

        if (registration is not null)
        {
            entity.SetEmail(email);
            entity.UpdateProfile(firstName, lastName);
            entity.Resubscribe(_clock.Now);
        }

        entity.SetPracticeGroups(distinctPracticeGroupIds);
        entity.SetRoleTypes(distinctRoleTypeIds);

        if (registration is null)
        {
            entity.TouchNotification(_clock.Now);
        }

        return entity;
    }

    public virtual void Unsubscribe(JobAlertRegistration registration)
    {
        registration.Unsubscribe(_clock.Now);
    }

    private async Task EnsurePracticeGroupsExistAsync(
        IReadOnlyCollection<Guid> practiceGroupIds,
        CancellationToken cancellationToken)
    {
        var groups = await _practiceGroupRepository.GetListAsync(
            g => practiceGroupIds.Contains(g.Id) && g.IsActive,
            cancellationToken: cancellationToken);

        if (groups.Count != practiceGroupIds.Count)
        {
            throw new BusinessException("JobAlerts:PracticeGroupNotFound")
                .WithData("Ids", string.Join(",", practiceGroupIds));
        }
    }

    private async Task EnsureRoleTypesExistAsync(
        IReadOnlyCollection<Guid> roleTypeIds,
        CancellationToken cancellationToken)
    {
        var roleTypes = await _roleTypeRepository.GetListAsync(
            rt => roleTypeIds.Contains(rt.Id),
            cancellationToken: cancellationToken);

        if (roleTypes.Count != roleTypeIds.Count)
        {
            throw new BusinessException("JobAlerts:RoleTypeNotFound")
                .WithData("Ids", string.Join(",", roleTypeIds));
        }
    }
}
