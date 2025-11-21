using Steer73.RockIT.Enums;
using System;

namespace Steer73.RockIT.JobAlerts;

public class JobAlertNotificationJobArgs
{
    public Guid VacancyId { get; set; }

    public JobAlertNotificationType NotificationType { get; set; }
}
