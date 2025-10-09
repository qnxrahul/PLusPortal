using Steer73.RockIT.FormDefinitions;
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
using Steer73.RockIT.PracticeGroups;
using Steer73.RockIT.Vacancies;
using Volo.Abp.Identity;
using Steer73.RockIT.Companies;

namespace Steer73.RockIT.FormDefinitions
{
    public abstract class EfCoreFormDefinitionRepositoryBase : EfCoreRepository<RockITDbContext, FormDefinition, Guid>
    {
        public EfCoreFormDefinitionRepositoryBase(IDbContextProvider<RockITDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        public virtual async Task<List<FormDefinition>> GetListAsync(
            string? filterText = null,
            string? referenceId = null,
            string? name = null,
            string? formDetails = null,
            FormType? formType = null,
            Guid? companyId = null,
            string? sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            CancellationToken cancellationToken = default)
        {
            var query = ApplyFilter((await GetQueryableAsync()), filterText, referenceId, name, formDetails, formType, companyId);
            query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? FormDefinitionConsts.GetDefaultSorting(false) : sorting);
            return await query.PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken);
        }

        public virtual async Task<long> GetCountAsync(
            string? filterText = null,
            string? referenceId = null,
            string? name = null,
            string? formDetails = null,
            FormType? formType = null,
            Guid? companyId = null,
            CancellationToken cancellationToken = default)
        {
            var query = ApplyFilter((await GetDbSetAsync()), filterText, referenceId, name, formDetails, formType, companyId);
            return await query.LongCountAsync(GetCancellationToken(cancellationToken));
        }

        protected virtual IQueryable<FormDefinition> ApplyFilter(
            IQueryable<FormDefinition> query,
            string? filterText = null,
            string? referenceId = null,
            string? name = null,
            string? formDetails = null,
            FormType? formType = null,
            Guid? companyId = null)
        {
            return query
                    .WhereIf(!string.IsNullOrWhiteSpace(filterText), e => e.ReferenceId!.Contains(filterText!) || e.Name!.Contains(filterText!) || e.FormDetails!.Contains(filterText!))
                    .WhereIf(!string.IsNullOrWhiteSpace(referenceId), e => e.ReferenceId.Contains(referenceId))
                    .WhereIf(!string.IsNullOrWhiteSpace(name), e => e.Name.Contains(name))
                    .WhereIf(!string.IsNullOrWhiteSpace(formDetails), e => e.FormDetails.Contains(formDetails))
                    .WhereIf(companyId.HasValue, e => e.CompanyId == companyId)
                    .WhereIf(formType.HasValue, e => e.FormType == formType);
        }

        public virtual async Task<List<FormDefinitionWithNavigationProperties>> GetListWithNavigationPropertiesAsync(
            string? filterText = null,
            string? referenceId = null,
            string? name = null,
            string? formDetails = null,
            FormType? formType = null,
            Guid? companyId = null,
            string? sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryForNavigationPropertiesAsync();
            query = ApplyFilter(query, filterText, referenceId, name, formDetails, companyId, formType);
            query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? FormDefinitionConsts.GetDefaultSorting(true) : sorting);
            return await query.PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken);
        }

        protected virtual async Task<IQueryable<FormDefinitionWithNavigationProperties>> GetQueryForNavigationPropertiesAsync()
        {
            return from formDefinition in (await GetDbSetAsync())
                   join company in (await GetDbContextAsync()).Set<Company>() on formDefinition.CompanyId equals company.Id into companies
                   from company in companies.DefaultIfEmpty()
                   select new FormDefinitionWithNavigationProperties
                   {
                       Company = company,
                       FormDefinition = formDefinition
                   };
        }

        protected virtual IQueryable<FormDefinitionWithNavigationProperties> ApplyFilter(
            IQueryable<FormDefinitionWithNavigationProperties> query,
            string? filterText = null,
            string? referenceId = null,
            string? name = null,
            string? formDetails = null,
            Guid? companyId = null,
            FormType? formType = null)
        {
            return query
                    .WhereIf(!string.IsNullOrWhiteSpace(filterText), e => e.FormDefinition.ReferenceId!.Contains(filterText!) || e.FormDefinition.Name!.Contains(filterText!) || e.FormDefinition.FormDetails!.Contains(filterText!))
                    .WhereIf(!string.IsNullOrWhiteSpace(referenceId), e => e.FormDefinition.ReferenceId.Contains(referenceId))
                    .WhereIf(!string.IsNullOrWhiteSpace(name), e => e.FormDefinition.Name.Contains(name))
                    .WhereIf(!string.IsNullOrWhiteSpace(formDetails), e => e.FormDefinition.FormDetails.Contains(formDetails))
                    .WhereIf(companyId != null && companyId != Guid.Empty, e => e.Company != null && e.Company.Id == companyId)
                    .WhereIf(formType.HasValue, e => e.FormDefinition.FormType == formType);
        }
    }
}