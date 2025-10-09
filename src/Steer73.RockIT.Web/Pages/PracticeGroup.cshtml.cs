using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Steer73.RockIT.ApplicantPortal;
using Steer73.RockIT.PracticeGroups;
using Steer73.RockIT.Vacancies;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace Steer73.RockIT.Web.Pages
{
    public class PracticeGroupModel : PageModel
    {
        private readonly IApplicantPortalAppService _applicantPortalAppService;

        public Guid PracticeGroupId { get; set; }

        public PracticeGroupDto PracticeGroup { get; set; }
        public IReadOnlyList<VacancyWithNavigationPropertiesDto> Vacancies { get; set; }

        public PracticeGroupModel(
          IApplicantPortalAppService applicantPortalAppService
          )
        {
            _applicantPortalAppService = applicantPortalAppService;
        }

        public async Task<IActionResult> OnGetAsync(Guid practiceGroupId)
        {
            PracticeGroupId = practiceGroupId;
            PracticeGroup = await _applicantPortalAppService.GetPracticeGroup(practiceGroupId);
            Vacancies = await GetVacancies();
            return Page();
        }

        private async Task<IReadOnlyList<VacancyWithNavigationPropertiesDto>> GetVacancies()
        {
            var vacancies = await _applicantPortalAppService.GetActiveVacanciesAsync(new GetVacanciesInput() { PracticeGroupId = PracticeGroupId, MaxResultCount = LimitedResultRequestDto.MaxMaxResultCount });
            return vacancies.Items;
        }
    }
}