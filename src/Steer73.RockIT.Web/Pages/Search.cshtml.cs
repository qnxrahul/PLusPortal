using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Steer73.RockIT.ApplicantPortal;
using Steer73.RockIT.Vacancies;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Steer73.RockIT.Web.Pages
{

    public class SearchModel : PageModel
    {
        private readonly IApplicantPortalAppService _applicantPortalAppService;

        public IReadOnlyList<VacancyWithNavigationPropertiesDto> Vacancies { get; set; }
        public string SearchQuery { get; set; }

        public SearchModel(
          IApplicantPortalAppService applicantPortalAppService
            )
        {
            _applicantPortalAppService = applicantPortalAppService;
        }

        public async Task<IActionResult> OnGetAsync(string query)
        {
            SearchQuery = query;
            var searchResults = await _applicantPortalAppService.SearchJobPostings(SearchQuery);
            Vacancies = searchResults;
            return Page();
        }

    }
}
