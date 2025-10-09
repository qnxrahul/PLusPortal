using Steer73.RockIT.Enums;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Steer73.RockIT.Vacancies
{
    public partial interface IVacancyRepository
    {
        Task<List<Vacancy>> GetListOfVcanciesByFormDefinition(Guid[] listOfFormDefinitions, CancellationToken cancellationToken = default);
        Task<List<VacancyWithNavigationProperties>> GetActiveListWithNavigationPropertiesAsync(
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
            string? sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            bool includeDetails = true,
            CancellationToken cancellationToken = default);

        Task<long> GetActiveCountAsync(
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
            bool ? flagHideVacancy = null,
            Guid? companyId = null,
            Guid? identityUserId = null,
            Guid? practiceGroupId = null,
            Guid? vacancyFormDefinitionId = null,
            Guid? diversityFormDefinitionId = null,
            CancellationToken cancellationToken = default);
    }
}