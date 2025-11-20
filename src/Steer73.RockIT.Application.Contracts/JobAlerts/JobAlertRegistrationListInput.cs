using System;
using Volo.Abp.Application.Dtos;

namespace Steer73.RockIT.JobAlerts;

public class JobAlertRegistrationListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }

    public Guid? PracticeGroupId { get; set; }

    public Guid? RoleTypeId { get; set; }

    public bool? IsSubscribed { get; set; }
}
