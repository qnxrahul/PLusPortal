using Steer73.RockIT.FormDefinitions;
using Steer73.RockIT.PracticeGroups;
using Volo.Abp.Identity;
using Steer73.RockIT.Companies;
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
using Steer73.RockIT.Enums;

namespace Steer73.RockIT.Vacancies
{
    public abstract class EfCoreVacancyRepositoryBase : EfCoreRepository<RockITDbContext, Vacancy, Guid>
    {
        public EfCoreVacancyRepositoryBase(IDbContextProvider<RockITDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        public virtual async Task<VacancyWithNavigationProperties> GetWithNavigationPropertiesAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();

            return (await WithDetailsAsync()).Where(b => b.Id == id)
                .Select(vacancy => new VacancyWithNavigationProperties
                {
                    Vacancy = vacancy,
                    Company = dbContext.Set<Company>().FirstOrDefault(c => c.Id == vacancy.CompanyId),
                    IdentityUser = dbContext.Set<IdentityUser>().FirstOrDefault(c => c.Id == vacancy.IdentityUserId),
                    FormDefinition = dbContext.Set<FormDefinition>().FirstOrDefault(c => c.Id == vacancy.VacancyFormDefinitionId),
                    FormDefinition1 = dbContext.Set<FormDefinition>().FirstOrDefault(c => c.Id == vacancy.DiversityFormDefinitionId)
                }).FirstOrDefault();
        }

        public virtual async Task<List<VacancyWithNavigationProperties>> GetListWithNavigationPropertiesAsync(
            string? filterText = null,
            string? title = null,
            string? reference = null,
            Region? region = null,
            string? role = null,
            string? benefits = null,
            string? location = null,
            string? salary = null,
            string? roleType = null,
            string? description = null,
            string? formalInterviewDate = null,
            string? secondInterviewDate = null,
            DateOnly? externalPostingDateMin = null,
            DateOnly? externalPostingDateMax = null,
            DateOnly? closingDateMin = null,
            DateOnly? closingDateMax = null,
            DateOnly? expiringDateMin = null,
            DateOnly? expiringDateMax = null,
            bool? showDiversity = null,
            bool? flagHideVacancy = null,
            Guid? companyId = null,
            Guid? identityUserId = null,
            Guid? practiceGroupId = null,
            Guid? vacancyFormDefinitionId = null,
            Guid? diversityFormDefinitionId = null,
            VacancyStatus? status = null,
            bool? showContributionVacancies = null,
            string? sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            bool includeDetails = true,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryForNavigationPropertiesAsync(includeDetails);
            query = ApplyFilter(query, filterText, title, reference, region, role, benefits, location, salary, roleType, description, formalInterviewDate, secondInterviewDate, externalPostingDateMin, externalPostingDateMax, closingDateMin, closingDateMax, expiringDateMin, expiringDateMax, showDiversity,flagHideVacancy, companyId, identityUserId, practiceGroupId, vacancyFormDefinitionId, diversityFormDefinitionId, status, showContributionVacancies);
            query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? VacancyConsts.GetDefaultSorting(true) : sorting);
            return await query.PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken);
        }

        protected virtual async Task<IQueryable<VacancyWithNavigationProperties>> GetQueryForNavigationPropertiesAsync(bool includDetails = false)
        {
            return from vacancy in (includDetails? await WithDetailsAsync() : await GetDbSetAsync())
                   join company in (await GetDbContextAsync()).Set<Company>() on vacancy.CompanyId equals company.Id into companies
                   from company in companies.DefaultIfEmpty()
                   join identityUser in (await GetDbContextAsync()).Set<IdentityUser>() on vacancy.IdentityUserId equals identityUser.Id into identityUsers
                   from identityUser in identityUsers.DefaultIfEmpty()
                   join formDefinition in (await GetDbContextAsync()).Set<FormDefinition>() on vacancy.VacancyFormDefinitionId equals formDefinition.Id into formDefinitions
                   from formDefinition in formDefinitions.DefaultIfEmpty()
                   join formDefinition1 in (await GetDbContextAsync()).Set<FormDefinition>() on vacancy.DiversityFormDefinitionId equals formDefinition1.Id into formDefinitions1
                   from formDefinition1 in formDefinitions1.DefaultIfEmpty()
                   select new VacancyWithNavigationProperties
                   {
                       Vacancy = vacancy,
                       Company = company,
                       IdentityUser = identityUser,
                       FormDefinition = formDefinition,
                       FormDefinition1 = formDefinition1
                   };
        }

        protected virtual IQueryable<VacancyWithNavigationProperties> ApplyFilter(
            IQueryable<VacancyWithNavigationProperties> query,
            string? filterText,
            string? title = null,
            string? reference = null,
            Region? region = null,
            string? role = null,
            string? benefits = null,
            string? location = null,
            string? salary = null,
            string? roleType = null,
            string? description = null,
            string? formalInterviewDate = null,
            string? secondInterviewDate = null,
            DateOnly? externalPostingDateMin = null,
            DateOnly? externalPostingDateMax = null,
            DateOnly? closingDateMin = null,
            DateOnly? closingDateMax = null,
            DateOnly? expiringDateMin = null,
            DateOnly? expiringDateMax = null,
            bool? showDiversity = null,
            bool? flagHideVacancy = null,
            Guid? companyId = null,
            Guid? identityUserId = null,
            Guid? practiceGroupId = null,
            Guid? vacancyFormDefinitionId = null,
            Guid? diversityFormDefinitionId = null,
            VacancyStatus? status = null,
            bool? showContributionVacancies = null)
        {
            // Narrow global search to on-screen fields and add ProjectId/EzekiaId and Company.Name
            {
                var _ft = filterText;
                int _ezIdVal = 0;
                var _ftIsInt = !string.IsNullOrWhiteSpace(_ft) && int.TryParse(_ft, out _ezIdVal);
                query = query.WhereIf(!string.IsNullOrWhiteSpace(_ft), e =>
                    (e.Vacancy.ProjectId != null && e.Vacancy.ProjectId.Contains(_ft!))
                    || (_ftIsInt && e.Vacancy.ExternalRefId.HasValue && e.Vacancy.ExternalRefId.Value == _ezIdVal)
                    || e.Vacancy.Title!.Contains(_ft!)
                    || e.Vacancy.Reference!.Contains(_ft!)
                    || e.Vacancy.Role!.Contains(_ft!)
                    || (e.Company != null && e.Company.Name!.Contains(_ft!))
                    || e.Vacancy.Location!.Contains(_ft!)
                );
            }
               
                   .WhereIf(!string.IsNullOrWhiteSpace(title), e => e.Vacancy.Title.Contains(title))
                   .WhereIf(!string.IsNullOrWhiteSpace(reference), e => e.Vacancy.Reference.Contains(reference))
                   .WhereIf(region.HasValue, e => EF.Property<List<VacancyRegion>>(e.Vacancy, "VacancyRegions").Any(vr => vr.Region == region))
                   .WhereIf(!string.IsNullOrWhiteSpace(role), e => e.Vacancy.Role.Contains(role))
                   .WhereIf(!string.IsNullOrWhiteSpace(benefits), e => e.Vacancy.Benefits.Contains(benefits))
                   .WhereIf(!string.IsNullOrWhiteSpace(location), e => e.Vacancy.Location.Contains(location))
                   .WhereIf(!string.IsNullOrWhiteSpace(salary), e => e.Vacancy.Salary.Contains(salary))
                   //.WhereIf(!string.IsNullOrWhiteSpace(roleType), e => e.Vacancy.RoleType.Contains(roleType))
                   .WhereIf(!string.IsNullOrWhiteSpace(description), e => e.Vacancy.Description.Contains(description))
                   .WhereIf(!string.IsNullOrWhiteSpace(formalInterviewDate), e => e.Vacancy.FormalInterviewDate.Contains(formalInterviewDate))
                   .WhereIf(!string.IsNullOrWhiteSpace(secondInterviewDate), e => e.Vacancy.SecondInterviewDate.Contains(secondInterviewDate))
                   .WhereIf(externalPostingDateMin.HasValue, e => e.Vacancy.ExternalPostingDate >= externalPostingDateMin!.Value)
                   .WhereIf(externalPostingDateMax.HasValue, e => e.Vacancy.ExternalPostingDate <= externalPostingDateMax!.Value)
                   .WhereIf(closingDateMin.HasValue, e => e.Vacancy.ClosingDate >= closingDateMin!.Value)
                   .WhereIf(closingDateMax.HasValue, e => e.Vacancy.ClosingDate <= closingDateMax!.Value)
                   .WhereIf(expiringDateMin.HasValue, e => e.Vacancy.ExpiringDate >= expiringDateMin!.Value)
                   .WhereIf(expiringDateMax.HasValue, e => e.Vacancy.ExpiringDate <= expiringDateMax!.Value)
                   .WhereIf(showDiversity.HasValue, e => e.Vacancy.ShowDiversity == showDiversity)
                    .WhereIf(flagHideVacancy.HasValue, e => e.Vacancy.Flag_HideVacancy == flagHideVacancy)
                   .WhereIf(companyId != null && companyId != Guid.Empty, e => e.Company != null && e.Company.Id == companyId)
                   .WhereIf((showContributionVacancies == null || (showContributionVacancies != null && !showContributionVacancies.Value)) && identityUserId != null && identityUserId != Guid.Empty, e => e.IdentityUser != null && e.IdentityUser.Id == identityUserId)
                   .WhereIf(showContributionVacancies != null && showContributionVacancies.Value, e => (e.IdentityUser != null && e.IdentityUser.Id == identityUserId) || e.Vacancy.Contributors.Any(c => c.IdentityUserId == identityUserId))
                   .WhereIf(practiceGroupId != null && practiceGroupId != Guid.Empty, e => EF.Property<List<PracticeGroup>>(e.Vacancy, "PracticeGroups").Any(pg => pg.Id == practiceGroupId!.Value))
                   .WhereIf(vacancyFormDefinitionId != null && vacancyFormDefinitionId != Guid.Empty, e => e.FormDefinition != null && e.FormDefinition.Id == vacancyFormDefinitionId)
                   .WhereIf(diversityFormDefinitionId != null && diversityFormDefinitionId != Guid.Empty, e => e.FormDefinition1 != null && e.FormDefinition1.Id == diversityFormDefinitionId);

            if (status != null)
            {
                var todayDateOnly = DateOnly.FromDateTime(DateTime.UtcNow);

                //TODO: Peer review this logic
                switch (status)
                {
                    case VacancyStatus.Closed:
                        query = query.Where(e => e.Vacancy.ClosingDate <= todayDateOnly);
                        break;

                    case VacancyStatus.Expired:
                        query = query.Where(e => e.Vacancy.ExpiringDate <= todayDateOnly && e.Vacancy.ClosingDate > todayDateOnly);
                        break;

                    case VacancyStatus.Active:
                        query = query.Where(e => e.Vacancy.ExternalPostingDate <= todayDateOnly && e.Vacancy.ClosingDate > todayDateOnly && e.Vacancy.ExpiringDate > todayDateOnly && e.Vacancy.ClosingDate > todayDateOnly);
                        break;

                    case VacancyStatus.Pending:
                        query = query.Where(e => e.Vacancy.ExternalPostingDate > todayDateOnly && e.Vacancy.ClosingDate > todayDateOnly && e.Vacancy.ExpiringDate > todayDateOnly && e.Vacancy.ClosingDate > todayDateOnly);
                        break;
                }
            }

            return query;
        }

        public virtual async Task<List<Vacancy>> GetListAsync(
            string? filterText = null,
            string? title = null,
            string? reference = null,
            Region? region = null,
            string? role = null,
            string? benefits = null,
            string? location = null,
            string? salary = null,
            string? roleType = null,
            string? description = null,
            string? formalInterviewDate = null,
            string? secondInterviewDate = null,
            DateOnly? externalPostingDateMin = null,
            DateOnly? externalPostingDateMax = null,
            DateOnly? closingDateMin = null,
            DateOnly? closingDateMax = null,
            DateOnly? expiringDateMin = null,
            DateOnly? expiringDateMax = null,
            bool? showDiversity = null,
            bool? flagHideVacancy= null,
            string? sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            bool includeDetails = true,
            CancellationToken cancellationToken = default)
        {
            var query = ApplyFilter((includeDetails ? await WithDetailsAsync() : await GetQueryableAsync()), filterText, title, reference, region, role, benefits, location, salary, roleType, description, formalInterviewDate, secondInterviewDate, externalPostingDateMin, externalPostingDateMax, closingDateMin, closingDateMax, expiringDateMin, expiringDateMax, showDiversity, flagHideVacancy);
            query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? VacancyConsts.GetDefaultSorting(false) : sorting);
            return await query.PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken);
        }

        public virtual async Task<long> GetCountAsync(
            string? filterText = null,
            string? title = null,
            string? reference = null,
            Region? region = null,
            string? role = null,
            string? benefits = null,
            string? location = null,
            string? salary = null,
            string? roleType = null,
            string? description = null,
            string? formalInterviewDate = null,
            string? secondInterviewDate = null,
            DateOnly? externalPostingDateMin = null,
            DateOnly? externalPostingDateMax = null,
            DateOnly? closingDateMin = null,
            DateOnly? closingDateMax = null,
            DateOnly? expiringDateMin = null,
            DateOnly? expiringDateMax = null,
            bool? showDiversity = null,
            bool? flagHideVacancy =null,
            Guid? companyId = null,
            Guid? identityUserId = null,
            Guid? practiceGroupId = null,
            Guid? vacancyFormDefinitionId = null,
            Guid? diversityFormDefinitionId = null,
            VacancyStatus? status = null,
            bool? showContributionVacancies = null,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryForNavigationPropertiesAsync();
            query = ApplyFilter(query, filterText, title, reference, region, role, benefits, location, salary, roleType, description, formalInterviewDate, secondInterviewDate, externalPostingDateMin, externalPostingDateMax, closingDateMin, closingDateMax, expiringDateMin, expiringDateMax, showDiversity,flagHideVacancy, companyId, identityUserId, practiceGroupId, vacancyFormDefinitionId, diversityFormDefinitionId, status, showContributionVacancies);
            return await query.LongCountAsync(GetCancellationToken(cancellationToken));
        }

        protected virtual IQueryable<Vacancy> ApplyFilter(
            IQueryable<Vacancy> query,
            string? filterText = null,
            string? title = null,
            string? reference = null,
            Region? region = null,
            string? role = null,
            string? benefits = null,
            string? location = null,
            string? salary = null,
            string? roleType = null,
            string? description = null,
            string? formalInterviewDate = null,
            string? secondInterviewDate = null,
            DateOnly? externalPostingDateMin = null,
            DateOnly? externalPostingDateMax = null,
            DateOnly? closingDateMin = null,
            DateOnly? closingDateMax = null,
            DateOnly? expiringDateMin = null,
            DateOnly? expiringDateMax = null,
            bool? showDiversity = null,
            bool? flagHideVacancy = null,
            Guid? companyId = null)
        {
            return query // screen-oriented search only + project/ezekia id
                    .WhereIf(!string.IsNullOrWhiteSpace(filterText), e =>
                        (e.ProjectId != null && e.ProjectId.Contains(filterText!))
                        || (e.ExternalRefId.HasValue && filterText!.All(char.IsDigit) && e.ExternalRefId.Value == int.Parse(filterText))
                        || e.Title!.Contains(filterText!)
                        || e.Reference!.Contains(filterText!)
                        || e.Role!.Contains(filterText!)
                        || e.Location!.Contains(filterText!)
                        || e.Salary!.Contains(filterText!)
                    )
                    .WhereIf(!string.IsNullOrWhiteSpace(title), e => e.Title.Contains(title))
                    .WhereIf(!string.IsNullOrWhiteSpace(reference), e => e.Reference.Contains(reference))
                    .WhereIf(region.HasValue, e => EF.Property<List<VacancyRegion>>(e, "VacancyRegions").Any(vr => vr.Region == region))
                    .WhereIf(!string.IsNullOrWhiteSpace(role), e => e.Role.Contains(role))
                    .WhereIf(!string.IsNullOrWhiteSpace(benefits), e => e.Benefits.Contains(benefits))
                    .WhereIf(!string.IsNullOrWhiteSpace(location), e => e.Location.Contains(location))
                    .WhereIf(!string.IsNullOrWhiteSpace(salary), e => e.Salary.Contains(salary))
                    //.WhereIf(!string.IsNullOrWhiteSpace(roleType), e => e.RoleType.Contains(roleType))
                    .WhereIf(!string.IsNullOrWhiteSpace(description), e => e.Description.Contains(description))
                    .WhereIf(!string.IsNullOrWhiteSpace(formalInterviewDate), e => e.FormalInterviewDate.Contains(formalInterviewDate))
                    .WhereIf(!string.IsNullOrWhiteSpace(secondInterviewDate), e => e.SecondInterviewDate.Contains(secondInterviewDate))
                    .WhereIf(externalPostingDateMin.HasValue, e => e.ExternalPostingDate >= externalPostingDateMin!.Value)
                    .WhereIf(externalPostingDateMax.HasValue, e => e.ExternalPostingDate <= externalPostingDateMax!.Value)
                    .WhereIf(closingDateMin.HasValue, e => e.ClosingDate >= closingDateMin!.Value)
                    .WhereIf(closingDateMax.HasValue, e => e.ClosingDate <= closingDateMax!.Value)
                    .WhereIf(expiringDateMin.HasValue, e => e.ExpiringDate >= expiringDateMin!.Value)
                    .WhereIf(expiringDateMax.HasValue, e => e.ExpiringDate <= expiringDateMax!.Value)
                    .WhereIf(companyId.HasValue, e => e.CompanyId == companyId)
                    .WhereIf(showDiversity.HasValue, e => e.ShowDiversity == showDiversity)
                    .WhereIf(flagHideVacancy.HasValue, e => e.Flag_HideVacancy == flagHideVacancy);
        }

        public override IQueryable<Vacancy> WithDetails()
        {
            return GetQueryable().IncludeDetails();
        }

        public override async Task<IQueryable<Vacancy>> WithDetailsAsync()
        {
            return (await GetQueryableAsync()).IncludeDetails();
        }
    }
}