using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Steer73.RockIT.ApplicantPortal;
using Steer73.RockIT.Vacancies;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using Volo.Abp.Users;
using Volo.Abp.Identity;
using Volo.Abp.Data;

namespace Steer73.RockIT.Web.Pages
{
    public class VacancyDetailModel : PageModel
    {
        private readonly IApplicantPortalAppService _applicantPortalAppService;
        private readonly IdentityUserManager _identityUserManager;
        private readonly ICurrentUser _currentUser;

        public Guid VacancyId { get; set; }
        public VacancyWithNavigationPropertiesDto VacancyDto { get; set; }
        public string CompanyRole { get; set; }

        public VacancyDetailModel(
          IApplicantPortalAppService applicantPortalAppService,
          ICurrentUser currentUser,
          IdentityUserManager identityUserManager
          )
        {
            _applicantPortalAppService = applicantPortalAppService;
            _currentUser = currentUser;
            _identityUserManager = identityUserManager;
        }
        public async Task<IActionResult> OnGetAsync(Guid vacancyId)
        {
            VacancyId = vacancyId;
            VacancyDto = await _applicantPortalAppService.GetVacancy(vacancyId);

            if (VacancyDto == null) {
                return Redirect("/");
            }

            var user = await _identityUserManager.GetByIdAsync(VacancyDto.IdentityUser.Id);
            if (user is null) { return Unauthorized(); }
            CompanyRole = user.GetProperty<string>(CustomIdentityUserPropertyNames.CompanyRole);

            return Page();
        }
    }
}
