using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Steer73.RockIT.JobApplications;
using Steer73.RockIT.Shared;
using Steer73.RockIT.Enums;

namespace Steer73.RockIT.Web.Pages.JobApplications
{
    public abstract class IndexModelBase : AbpPageModel
    {
        public string? FirstNameFilter { get; set; }
        public string? LastNameFilter { get; set; }
        public string? EmailAddressFilter { get; set; }
        public string? TitleFilter { get; set; }
        public string? PhoneNumberFilter { get; set; }
        public string? LandlineFilter { get; set; }
        public string? CurrentRoleFilter { get; set; }
        public string? CurrentCompanyFilter { get; set; }
        public string? CurrentPositionTypeFilter { get; set; }
        public JobApplicationStatus? Status { get; set; }
        [SelectItems(nameof(VacancyLookupList))]
        public Guid VacancyIdFilter { get; set; }
        public List<SelectListItem> VacancyLookupList { get; set; } = new List<SelectListItem>
        {
            new SelectListItem(string.Empty, "")
        };

        protected IJobApplicationsAppService _jobApplicationsAppService;

        public IndexModelBase(IJobApplicationsAppService jobApplicationsAppService)
        {
            _jobApplicationsAppService = jobApplicationsAppService;
        }

        public virtual async Task OnGetAsync()
        {
            VacancyLookupList.AddRange((
                    await _jobApplicationsAppService.GetVacancyLookupAsync(new LookupRequestDto
                    {
                        MaxResultCount = LimitedResultRequestDto.MaxMaxResultCount
                    })).Items.Select(t => new SelectListItem(t.DisplayName, t.Id.ToString())).ToList()
            );

            await Task.CompletedTask;
        }
    }
}