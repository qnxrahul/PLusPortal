using Microsoft.EntityFrameworkCore;
using Steer73.RockIT.EntityFrameworkCore;
using Steer73.RockIT.Enums;
using System;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Steer73.RockIT.JobAlerts;

public class EfCoreJobAlertNotificationLogRepository
    : EfCoreRepository<RockITDbContext, JobAlertNotificationLog, Guid>,
      IJobAlertNotificationLogRepository
{
    public EfCoreJobAlertNotificationLogRepository(IDbContextProvider<RockITDbContext> dbContextProvider)
        : base(dbContextProvider)
    {
    }

    public async Task<bool> ExistsAsync(Guid registrationId, Guid vacancyId, JobAlertNotificationType notificationType, CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync()).AnyAsync(
            x => x.JobAlertRegistrationId == registrationId
                && x.VacancyId == vacancyId
                && x.NotificationType == notificationType,
            cancellationToken);
    }
}
