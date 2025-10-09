using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Steer73.RockIT.EntityFrameworkCore;

namespace Steer73.RockIT.JobFormResponses
{
    public abstract class EfCoreJobFormResponseRepositoryBase : EfCoreRepository<RockITDbContext, JobFormResponse, Guid>
    {
        public EfCoreJobFormResponseRepositoryBase(IDbContextProvider<RockITDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        public virtual async Task<List<JobFormResponse>> GetListByJobApplicationIdAsync(
           Guid jobApplicationId,
           string? sorting = null,
           int maxResultCount = int.MaxValue,
           int skipCount = 0,
           CancellationToken cancellationToken = default)
        {
            var query = (await GetQueryableAsync()).Where(x => x.JobApplicationId == jobApplicationId);
            query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? JobFormResponseConsts.GetDefaultSorting(false) : sorting);
            return await query.PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken);
        }

        public virtual async Task<long> GetCountByJobApplicationIdAsync(Guid jobApplicationId, CancellationToken cancellationToken = default)
        {
            return await (await GetQueryableAsync()).Where(x => x.JobApplicationId == jobApplicationId).CountAsync(cancellationToken);
        }

        public virtual async Task<List<JobFormResponse>> GetListAsync(
            string? filterText = null,

            string? sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            CancellationToken cancellationToken = default)
        {
            var query = ApplyFilter((await GetQueryableAsync()), filterText);
            query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? JobFormResponseConsts.GetDefaultSorting(false) : sorting);
            return await query.PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken);
        }

        public virtual async Task<long> GetCountAsync(
            string? filterText = null,

            CancellationToken cancellationToken = default)
        {
            var query = ApplyFilter((await GetDbSetAsync()), filterText);
            return await query.LongCountAsync(GetCancellationToken(cancellationToken));
        }

        protected virtual IQueryable<JobFormResponse> ApplyFilter(
            IQueryable<JobFormResponse> query,
            string? filterText = null
)
        {
            return query
                    .WhereIf(!string.IsNullOrWhiteSpace(filterText), e => e.FormStructureJson!.Contains(filterText!))
;
        }
    }
}