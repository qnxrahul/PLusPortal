using Steer73.RockIT.PracticeGroups;
using Steer73.RockIT.Vacancies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace Steer73.RockIT.ApplicantPortal
{
    public interface IApplicantPortalAppService
    {
        Task<PagedResultDto<PracticeGroupDto>> GetPracticeGroupsAsync(GetPracticeGroupsInput input);
        Task<PagedResultDto<VacancyWithNavigationPropertiesDto>> GetActiveVacanciesAsync(GetVacanciesInput input);
        Task<VacancyWithNavigationPropertiesDto> GetVacancy(Guid vacancyId);
        Task<List<VacancyWithNavigationPropertiesDto>> SearchJobPostings(string searchQuery);

        Task<PracticeGroupDto> GetPracticeGroup(Guid id);
    }
}
