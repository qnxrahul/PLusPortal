using System;
using System.ComponentModel.DataAnnotations;

namespace Steer73.RockIT.JobAlerts;

public class JobAlertUnsubscribeTokenDto
{
    [Required]
    public Guid Token { get; set; }
}
