using Microsoft.EntityFrameworkCore;
using Steer73.RockIT.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Steer73.RockIT.JobAlerts;

public class EfCoreJobAlertRegistrationRepository
    : EfCoreRepository<RockITDbContext, JobAlertRegistration, Guid>,
      IJobAlertRegistrationRepository
{
    public EfCoreJobAlertRegistrationRepository(
        IDbContextProvider<RockITDbContext> dbContextProvider)
        : base(dbContextProvider)
    {
    }

    public async Task<JobAlertRegistration?> FindByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        var normalized = email?.Trim().ToLowerInvariant() ?? string.Empty;

        return await (await GetQueryableWithDetailsAsync())
            .FirstOrDefaultAsync(x => x.Email == normalized, cancellationToken);
    }

    public async Task<JobAlertRegistration?> FindByUnsubscribeTokenAsync(Guid token, CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableWithDetailsAsync())
            .FirstOrDefaultAsync(x => x.UnsubscribeToken == token, cancellationToken);
    }

    public async Task<List<JobAlertRegistration>> GetActiveByPracticeGroupsAsync(IEnumerable<Guid> practiceGroupIds, CancellationToken cancellationToken = default)
    {
        var ids = practiceGroupIds?
            .Where(id => id != Guid.Empty)
            .Distinct()
            .ToList() ?? [];

        if (!ids.Any())
        {
            return [];
        }

        var query = (await GetQueryableWithDetailsAsync())
            .Where(x => x.IsSubscribed && x.PracticeGroups.Any(pg => ids.Contains(pg.PracticeGroupId)));

        return await query.ToListAsync(cancellationToken);
    }

    public override IQueryable<JobAlertRegistration> WithDetails()
    {
        return ApplyIncludes(base.WithDetails());
    }

    public override IQueryable<JobAlertRegistration> WithDetails(params Expression<Func<JobAlertRegistration, object>>[] propertySelectors)
    {
        return ApplyIncludes(base.WithDetails(propertySelectors));
    }

    private async Task<IQueryable<JobAlertRegistration>> GetQueryableWithDetailsAsync()
    {
        var query = await GetQueryableAsync();
        return ApplyIncludes(query);
    }

    private static IQueryable<JobAlertRegistration> ApplyIncludes(IQueryable<JobAlertRegistration> query)
    {
        return query
            .Include(x => x.PracticeGroups)
                .ThenInclude(pg => pg.PracticeGroup)
            .Include(x => x.RoleTypes)
                .ThenInclude(rt => rt.RoleType);
    }
}
