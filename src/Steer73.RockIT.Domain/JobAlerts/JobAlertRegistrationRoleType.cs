using Steer73.RockIT.RoleTypes;
using System;
using Volo.Abp.Domain.Entities;

namespace Steer73.RockIT.JobAlerts;

public class JobAlertRegistrationRoleType : Entity
{
    public Guid JobAlertRegistrationId { get; protected set; }

    public Guid RoleTypeId { get; protected set; }

    public virtual RoleType RoleType { get; protected set; } = null!;

    protected JobAlertRegistrationRoleType()
    {
    }

    internal JobAlertRegistrationRoleType(Guid registrationId, Guid roleTypeId)
    {
        JobAlertRegistrationId = registrationId;
        RoleTypeId = roleTypeId;
    }

    public override object[] GetKeys()
    {
        return [JobAlertRegistrationId, RoleTypeId];
    }
}
