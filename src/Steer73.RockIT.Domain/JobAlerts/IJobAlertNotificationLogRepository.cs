using Steer73.RockIT.Enums;
using System;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Steer73.RockIT.JobAlerts;

public interface IJobAlertNotificationLogRepository : IRepository<JobAlertNotificationLog, Guid>
{
    Task<bool> ExistsAsync(
        Guid registrationId,
        Guid vacancyId,
        JobAlertNotificationType notificationType,
        CancellationToken cancellationToken = default);
}
