using System.ComponentModel.DataAnnotations;

namespace Steer73.RockIT.JobAlerts;

public class JobAlertUnsubscribeDto
{
    [Required]
    [EmailAddress]
    [StringLength(JobAlertRegistrationConsts.EmailMaxLength)]
    public string Email { get; set; } = null!;
}
