using Microsoft.EntityFrameworkCore;
using Steer73.RockIT.EntityFrameworkCore;
using Steer73.RockIT.Enums;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.EntityFrameworkCore;

namespace Steer73.RockIT.Vacancies
{
    public class EfCoreVacancyRepository : EfCoreVacancyRepositoryBase, IVacancyRepository
    {
        public EfCoreVacancyRepository(IDbContextProvider<RockITDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        public async Task<long> GetActiveCountAsync(
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
            bool? flagHideVacancy=null,
            Guid? companyId = null,
            Guid? identityUserId = null,
            Guid? practiceGroupId = null,
            Guid? vacancyFormDefinitionId = null,
            Guid? diversityFormDefinitionId = null,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryForNavigationPropertiesAsync();
            query = ApplyFilter(
                query.Where(x => 
                    x.Vacancy.ExternalPostingDate <= DateOnly.FromDateTime(DateTime.UtcNow) &&
                    x.Vacancy.ExpiringDate >= DateOnly.FromDateTime(DateTime.UtcNow) &&
                    x.Vacancy.Flag_HideVacancy == false
                ), 
                filterText,
                title, 
                reference, 
                region,
                role, 
                benefits, 
                location, 
                salary, 
                roleType, 
                description, 
                formalInterviewDate, 
                secondInterviewDate, 
                externalPostingDateMin, 
                externalPostingDateMax, 
                closingDateMin, 
                closingDateMax, 
                expiringDateMin, 
                expiringDateMax, 
                showDiversity,
                flagHideVacancy,
                companyId,
                identityUserId,
                practiceGroupId,
                vacancyFormDefinitionId,
                diversityFormDefinitionId);

            return await query.LongCountAsync(GetCancellationToken(cancellationToken));
        }

        public async Task<List<VacancyWithNavigationProperties>> GetActiveListWithNavigationPropertiesAsync(
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
            bool? flagHideVacancy=null,
            Guid? companyId = null,
            Guid? identityUserId = null,
            Guid? practiceGroupId = null,
            Guid? vacancyFormDefinitionId = null,
            Guid? diversityFormDefinitionId = null,
            string? sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            bool includeDetails = true,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryForNavigationPropertiesAsync(includeDetails);
            query = ApplyFilter(
                query.Where(x =>
                    x.Vacancy.ExternalPostingDate <= DateOnly.FromDateTime(DateTime.UtcNow) &&
                    x.Vacancy.ExpiringDate >= DateOnly.FromDateTime(DateTime.UtcNow) &&
                   x.Vacancy.Flag_HideVacancy == false
                ),
                filterText,
                title, 
                reference, 
                region, 
                role, 
                benefits, 
                location, 
                salary, 
                roleType, 
                description, 
                formalInterviewDate, 
                secondInterviewDate, 
                externalPostingDateMin, 
                externalPostingDateMax, 
                closingDateMin, 
                closingDateMax, 
                expiringDateMin, 
                expiringDateMax, 
                showDiversity,
                flagHideVacancy,
                companyId,
                identityUserId,
                practiceGroupId,
                vacancyFormDefinitionId,
                diversityFormDefinitionId);

            query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? VacancyConsts.GetDefaultSorting(true) : sorting);
            return await query.PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken);
        }

        public virtual async Task<List<Vacancy>> GetListOfVcanciesByFormDefinition(Guid[] listOfFormDefinitions, CancellationToken cancellationToken = default)
        {
            var dbSet = await GetDbSetAsync();
            var listOfFormDefinitionsNullable = listOfFormDefinitions.Select(x => (Guid?) x).ToArray();
            if (listOfFormDefinitions != null && listOfFormDefinitions.Length > 0)
            {
                return await dbSet
                .Where(x => listOfFormDefinitionsNullable.Contains(x.VacancyFormDefinitionId) || listOfFormDefinitionsNullable.Contains(x.DiversityFormDefinitionId))
                .ToListAsync(cancellationToken);
            }
            return await dbSet
            .Where(x => x.VacancyFormDefinitionId != null || x.DiversityFormDefinitionId != null)
                    .ToListAsync(cancellationToken);
        }
    }
}