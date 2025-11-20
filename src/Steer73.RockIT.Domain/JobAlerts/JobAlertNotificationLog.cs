using Steer73.RockIT.Enums;
using System;
using Volo.Abp.Domain.Entities;

namespace Steer73.RockIT.JobAlerts;

public class JobAlertNotificationLog : Entity<Guid>
{
    public Guid JobAlertRegistrationId { get; protected set; }

    public Guid VacancyId { get; protected set; }

    public JobAlertNotificationType NotificationType { get; protected set; }

    public DateTime SentAtUtc { get; protected set; }

    protected JobAlertNotificationLog()
    {
    }

    public JobAlertNotificationLog(
        Guid id,
        Guid jobAlertRegistrationId,
        Guid vacancyId,
        JobAlertNotificationType notificationType,
        DateTime sentAtUtc)
    {
        Id = id;
        JobAlertRegistrationId = jobAlertRegistrationId;
        VacancyId = vacancyId;
        NotificationType = notificationType;
        SentAtUtc = sentAtUtc;
    }
}
