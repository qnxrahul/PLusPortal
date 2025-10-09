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

namespace Steer73.RockIT.Companies
{
    public abstract class EfCoreCompanyRepositoryBase : EfCoreRepository<RockITDbContext, Company, Guid>
    {
        public EfCoreCompanyRepositoryBase(IDbContextProvider<RockITDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        public virtual async Task DeleteAllAsync(
            string? filterText = null,
                        string? name = null,
            string? phone = null,
            string? address = null,
            string? postcode = null,
            string? primaryContact = null,
            CancellationToken cancellationToken = default)
        {

            var query = await GetQueryableAsync();

            query = ApplyFilter(query, filterText, name, phone, address, postcode, primaryContact);

            var ids = query.Select(x => x.Id);
            await DeleteManyAsync(ids, cancellationToken: GetCancellationToken(cancellationToken));
        }

        public virtual async Task<List<Company>> GetListAsync(
            string? filterText = null,
            string? name = null,
            string? phone = null,
            string? address = null,
            string? postcode = null,
            string? primaryContact = null,
            string? sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            CancellationToken cancellationToken = default)
        {
            var query = ApplyFilter((await GetQueryableAsync()), filterText, name, phone, address, postcode, primaryContact);
            query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? CompanyConsts.GetDefaultSorting(false) : sorting);
            return await query.PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken);
        }

        public virtual async Task<long> GetCountAsync(
            string? filterText = null,
            string? name = null,
            string? phone = null,
            string? address = null,
            string? postcode = null,
            string? primaryContact = null,
            CancellationToken cancellationToken = default)
        {
            var query = ApplyFilter((await GetDbSetAsync()), filterText, name, phone, address, postcode, primaryContact);
            return await query.LongCountAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<IReadOnlyCollection<Company>> GetByExternalRefIds(
            List<int> externalRefIds,
            CancellationToken cancellationToken = default)
        {
            var query = (await GetDbSetAsync())
                .Where(c => c.ExternalRefId != null && externalRefIds.Contains((int)c.ExternalRefId));
            
            return await query.ToListAsync(GetCancellationToken(cancellationToken));
        }

        protected virtual IQueryable<Company> ApplyFilter(
            IQueryable<Company> query,
            string? filterText = null,
            string? name = null,
            string? phone = null,
            string? address = null,
            string? postcode = null,
            string? primaryContact = null)
        {
            return query
                    .WhereIf(!string.IsNullOrWhiteSpace(filterText), e => e.Name!.Contains(filterText!) || e.Phone!.Contains(filterText!) || e.Address!.Contains(filterText!) || e.Postcode!.Contains(filterText!) || e.PrimaryContact!.Contains(filterText!))
                    .WhereIf(!string.IsNullOrWhiteSpace(name), e => e.Name.Contains(name))
                    .WhereIf(!string.IsNullOrWhiteSpace(phone), e => e.Phone.Contains(phone))
                    .WhereIf(!string.IsNullOrWhiteSpace(address), e => e.Address.Contains(address))
                    .WhereIf(!string.IsNullOrWhiteSpace(postcode), e => e.Postcode.Contains(postcode))
                    .WhereIf(!string.IsNullOrWhiteSpace(primaryContact), e => e.PrimaryContact.Contains(primaryContact));
        }
    }
}