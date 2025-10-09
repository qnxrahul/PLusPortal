using Microsoft.AspNetCore.Authorization;
using Steer73.RockIT.PracticeGroups;
using Steer73.RockIT.Vacancies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.ObjectMapping;

namespace Steer73.RockIT.ApplicantPortal
{
    [AllowAnonymousAttribute]
    public class ApplicantPortalAppService : RockITAppService, IApplicantPortalAppService
    {
        protected readonly IPracticeGroupRepository _practiceGroupRepository;
        protected readonly IVacancyRepository _vacancyRepository;

        public ApplicantPortalAppService(
            IPracticeGroupRepository practiceGroupRepository,
            IVacancyRepository vacancyRepository
            )
        {
            _practiceGroupRepository = practiceGroupRepository;
            _vacancyRepository = vacancyRepository;
        }

        public virtual async Task<PagedResultDto<PracticeGroupDto>> GetPracticeGroupsAsync(GetPracticeGroupsInput input)
        {
            var totalCount = await _practiceGroupRepository.GetCountAsync(input.FilterText, input.Name, input.IsActive);
            var items = await _practiceGroupRepository.GetListAsync(input.FilterText, input.Name, input.IsActive, input.Sorting, input.MaxResultCount, input.SkipCount);

            return new PagedResultDto<PracticeGroupDto>
            {
                TotalCount = totalCount,
                Items = ObjectMapper.Map<List<PracticeGroup>, List<PracticeGroupDto>>(items)
            };
        }

        public virtual async Task<PagedResultDto<VacancyWithNavigationPropertiesDto>> GetActiveVacanciesAsync(GetVacanciesInput input)
        {
            var totalCount = await _vacancyRepository.GetActiveCountAsync(input.FilterText, input.Title, input.Reference, input.Region, input.Role, input.Benefits, input.Location, input.Salary, input.RoleType, input.Description, input.FormalInterviewDate, input.SecondInterviewDate, input.ExternalPostingDateMin, input.ExternalPostingDateMax, input.ClosingDateMin, input.ClosingDateMax, input.ExpiringDateMin, input.ExpiringDateMax, input.ShowDiversity,input.FlagHideVacancy, input.CompanyId, input.IdentityUserId, input.PracticeGroupId, input.VacancyFormDefinitionId, input.DiversityFormDefinitionId);
            var items = await _vacancyRepository.GetActiveListWithNavigationPropertiesAsync(input.FilterText, input.Title, input.Reference, input.Region, input.Role, input.Benefits, input.Location, input.Salary, input.RoleType, input.Description, input.FormalInterviewDate, input.SecondInterviewDate, input.ExternalPostingDateMin, input.ExternalPostingDateMax, input.ClosingDateMin, input.ClosingDateMax, input.ExpiringDateMin, input.ExpiringDateMax, input.ShowDiversity, input.FlagHideVacancy, input.CompanyId, input.IdentityUserId, input.PracticeGroupId, input.VacancyFormDefinitionId, input.DiversityFormDefinitionId, input.Sorting, input.MaxResultCount, input.SkipCount);

            return new PagedResultDto<VacancyWithNavigationPropertiesDto>
            {
                TotalCount = totalCount,
                Items = ObjectMapper.Map<List<VacancyWithNavigationProperties>, List<VacancyWithNavigationPropertiesDto>>(items)
            };
        }

        public async Task<VacancyWithNavigationPropertiesDto> GetVacancy(Guid vacancyId)
        {
            return ObjectMapper.Map<VacancyWithNavigationProperties, VacancyWithNavigationPropertiesDto>
               (await _vacancyRepository.GetWithNavigationPropertiesAsync(vacancyId));
        }

        public async Task<PracticeGroupDto> GetPracticeGroup(Guid id)
        {
            return ObjectMapper.Map<PracticeGroup, PracticeGroupDto>
               (await _practiceGroupRepository.GetAsync(id));
        }

        public async Task<List<VacancyWithNavigationPropertiesDto>> SearchJobPostings(string searchQuery)
        {
            var vacancies = await _vacancyRepository.GetActiveListWithNavigationPropertiesAsync(searchQuery);
            var vacanciesDto = ObjectMapper.Map<List<VacancyWithNavigationProperties>, List<VacancyWithNavigationPropertiesDto>>(vacancies);
            return vacanciesDto;
        }
    }
}