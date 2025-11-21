using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Steer73.RockIT.JobAlerts;

public class JobAlertRegistrationCreateDto
{
    [Required]
    [EmailAddress]
    [StringLength(JobAlertRegistrationConsts.EmailMaxLength)]
    public string Email { get; set; } = null!;

    [StringLength(JobAlertRegistrationConsts.NameMaxLength)]
    public string? FirstName { get; set; }

    [StringLength(JobAlertRegistrationConsts.NameMaxLength)]
    public string? LastName { get; set; }

    [Required]
    public List<Guid> PracticeGroupIds { get; set; } = [];

    public List<Guid> RoleTypeIds { get; set; } = [];
}
