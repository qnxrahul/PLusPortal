using JetBrains.Annotations;
using Steer73.RockIT.JobAlerts;
using System;
using System.Collections.Generic;
using System.Linq;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace Steer73.RockIT.JobAlerts;

public class JobAlertRegistration : FullAuditedAggregateRoot<Guid>
{
    [NotNull]
    public virtual string Email { get; protected set; } = null!;

    [CanBeNull]
    public virtual string? FirstName { get; protected set; }

    [CanBeNull]
    public virtual string? LastName { get; protected set; }

    public virtual bool IsSubscribed { get; protected set; }

    public virtual DateTime? UnsubscribedAt { get; protected set; }

    public virtual Guid UnsubscribeToken { get; protected set; }

    public virtual DateTime? LastNotificationUtc { get; protected set; }

    protected virtual List<JobAlertRegistrationPracticeGroup> PracticeGroupsInternal { get; set; } = [];

    protected virtual List<JobAlertRegistrationRoleType> RoleTypesInternal { get; set; } = [];

    public virtual IReadOnlyCollection<JobAlertRegistrationPracticeGroup> PracticeGroups => PracticeGroupsInternal;

    public virtual IReadOnlyCollection<JobAlertRegistrationRoleType> RoleTypes => RoleTypesInternal;

    protected JobAlertRegistration()
    {
    }

    public JobAlertRegistration(
        Guid id,
        string email,
        string? firstName,
        string? lastName)
    {
        Id = id;
        SetEmail(email);
        UpdateProfile(firstName, lastName);
        IsSubscribed = true;
        UnsubscribeToken = Guid.NewGuid();
    }

    public void SetEmail(string email)
    {
        Check.NotNullOrWhiteSpace(email, nameof(email));
        Check.Length(email, nameof(email), JobAlertRegistrationConsts.EmailMaxLength, 0);
        Email = email.Trim().ToLowerInvariant();
    }

    public void UpdateProfile(string? firstName, string? lastName)
    {
        if (!firstName.IsNullOrWhiteSpace())
        {
            Check.Length(firstName, nameof(firstName), JobAlertRegistrationConsts.NameMaxLength, 0);
            FirstName = firstName!.Trim();
        }
        else
        {
            FirstName = null;
        }

        if (!lastName.IsNullOrWhiteSpace())
        {
            Check.Length(lastName, nameof(lastName), JobAlertRegistrationConsts.NameMaxLength, 0);
            LastName = lastName!.Trim();
        }
        else
        {
            LastName = null;
        }
    }

    public void SetPracticeGroups(IReadOnlyCollection<Guid> practiceGroupIds)
    {
        Check.NotNullOrEmpty(practiceGroupIds, nameof(practiceGroupIds));

        var distinctIds = practiceGroupIds.Distinct().ToList();
        PracticeGroupsInternal.RemoveAll(link => !distinctIds.Contains(link.PracticeGroupId));

        foreach (var id in distinctIds)
        {
            if (PracticeGroupsInternal.Any(pg => pg.PracticeGroupId == id))
            {
                continue;
            }

            PracticeGroupsInternal.Add(new JobAlertRegistrationPracticeGroup(Id, id));
        }
    }

    public void SetRoleTypes(IReadOnlyCollection<Guid> roleTypeIds)
    {
        var distinctIds = roleTypeIds?.Distinct().ToList() ?? [];
        RoleTypesInternal.RemoveAll(link => !distinctIds.Contains(link.RoleTypeId));

        foreach (var id in distinctIds)
        {
            if (RoleTypesInternal.Any(rt => rt.RoleTypeId == id))
            {
                continue;
            }

            RoleTypesInternal.Add(new JobAlertRegistrationRoleType(Id, id));
        }
    }

    public void Unsubscribe(DateTime timestampUtc)
    {
        if (!IsSubscribed)
        {
            return;
        }

        IsSubscribed = false;
        UnsubscribedAt = timestampUtc;
    }

    public void Resubscribe(DateTime timestampUtc)
    {
        IsSubscribed = true;
        UnsubscribedAt = null;
        UnsubscribeToken = Guid.NewGuid();
        LastNotificationUtc ??= timestampUtc;
    }

    public void TouchNotification(DateTime timestampUtc)
    {
        LastNotificationUtc = timestampUtc;
    }

    public bool MatchesRoleTypes(IReadOnlyCollection<Guid> vacancyRoleTypeIds)
    {
        if (!RoleTypesInternal.Any())
        {
            return true;
        }

        if (vacancyRoleTypeIds is null || vacancyRoleTypeIds.Count == 0)
        {
            return false;
        }

        return RoleTypesInternal.Any(link => vacancyRoleTypeIds.Contains(link.RoleTypeId));
    }
}
