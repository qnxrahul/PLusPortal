using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Steer73.RockIT.JobAlerts;

public class JobAlertRegistrationDto : FullAuditedEntityDto<Guid>
{
    public string Email { get; set; } = null!;

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public bool IsSubscribed { get; set; }

    public DateTime? UnsubscribedAt { get; set; }

    public List<Guid> PracticeGroupIds { get; set; } = [];

    public List<string> PracticeGroupNames { get; set; } = [];

    public List<Guid> RoleTypeIds { get; set; } = [];

    public List<string> RoleTypeNames { get; set; } = [];
}
