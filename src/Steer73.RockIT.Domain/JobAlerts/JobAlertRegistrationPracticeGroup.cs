using Steer73.RockIT.PracticeGroups;
using System;
using Volo.Abp.Domain.Entities;

namespace Steer73.RockIT.JobAlerts;

public class JobAlertRegistrationPracticeGroup : Entity
{
    public Guid JobAlertRegistrationId { get; protected set; }

    public Guid PracticeGroupId { get; protected set; }

    public virtual PracticeGroup PracticeGroup { get; protected set; } = null!;

    protected JobAlertRegistrationPracticeGroup()
    {
    }

    internal JobAlertRegistrationPracticeGroup(Guid registrationId, Guid practiceGroupId)
    {
        JobAlertRegistrationId = registrationId;
        PracticeGroupId = practiceGroupId;
    }

    public override object[] GetKeys()
    {
        return [JobAlertRegistrationId, PracticeGroupId];
    }
}
