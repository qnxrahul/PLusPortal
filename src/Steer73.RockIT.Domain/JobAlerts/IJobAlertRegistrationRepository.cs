using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Steer73.RockIT.JobAlerts;

public interface IJobAlertRegistrationRepository : IRepository<JobAlertRegistration, Guid>
{
    Task<JobAlertRegistration?> FindByEmailAsync(string email, CancellationToken cancellationToken = default);

    Task<JobAlertRegistration?> FindByUnsubscribeTokenAsync(Guid token, CancellationToken cancellationToken = default);

    Task<List<JobAlertRegistration>> GetActiveByPracticeGroupsAsync(IEnumerable<Guid> practiceGroupIds, CancellationToken cancellationToken = default);
}
