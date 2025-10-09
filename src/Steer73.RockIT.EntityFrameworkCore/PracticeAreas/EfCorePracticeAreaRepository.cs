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

namespace Steer73.RockIT.PracticeAreas
{
    public abstract class EfCorePracticeAreaRepositoryBase : EfCoreRepository<RockITDbContext, PracticeArea, Guid>
    {
        public EfCorePracticeAreaRepositoryBase(IDbContextProvider<RockITDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        public virtual async Task<List<PracticeArea>> GetListByPracticeGroupIdAsync(
           Guid practiceGroupId,
           string? sorting = null,
           int maxResultCount = int.MaxValue,
           int skipCount = 0,
           CancellationToken cancellationToken = default)
        {
            var query = (await GetQueryableAsync()).Where(x => x.PracticeGroupId == practiceGroupId);
            query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? PracticeAreaConsts.GetDefaultSorting(false) : sorting);
            return await query.PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken);
        }

        public virtual async Task<long> GetCountByPracticeGroupIdAsync(Guid practiceGroupId, CancellationToken cancellationToken = default)
        {
            return await (await GetQueryableAsync()).Where(x => x.PracticeGroupId == practiceGroupId).CountAsync(cancellationToken);
        }

        public virtual async Task<List<PracticeArea>> GetListAsync(
            string? filterText = null,
            string? name = null,
            bool? isActive = null,
            string? sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            CancellationToken cancellationToken = default)
        {
            var query = ApplyFilter((await GetQueryableAsync()), filterText, name, isActive);
            query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? PracticeAreaConsts.GetDefaultSorting(false) : sorting);
            return await query.PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken);
        }

        public virtual async Task<long> GetCountAsync(
            string? filterText = null,
            string? name = null,
            bool? isActive = null,
            CancellationToken cancellationToken = default)
        {
            var query = ApplyFilter((await GetDbSetAsync()), filterText, name, isActive);
            return await query.LongCountAsync(GetCancellationToken(cancellationToken));
        }

        protected virtual IQueryable<PracticeArea> ApplyFilter(
            IQueryable<PracticeArea> query,
            string? filterText = null,
            string? name = null,
            bool? isActive = null)
        {
            return query
                    .WhereIf(!string.IsNullOrWhiteSpace(filterText), e => e.Name!.Contains(filterText!))
                    .WhereIf(!string.IsNullOrWhiteSpace(name), e => e.Name.Contains(name))
                    .WhereIf(isActive.HasValue, e => e.IsActive == isActive);
        }
    }
}