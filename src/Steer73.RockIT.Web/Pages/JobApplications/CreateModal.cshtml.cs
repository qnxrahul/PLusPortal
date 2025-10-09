using Steer73.RockIT.Shared;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.Application.Dtos;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Steer73.RockIT.JobApplications;

namespace Steer73.RockIT.Web.Pages.JobApplications
{
    public abstract class CreateModalModelBase : RockITPageModel
    {

        [BindProperty]
        public JobApplicationCreateViewModel JobApplication { get; set; }

        public List<SelectListItem> VacancyLookupListRequired { get; set; } = new List<SelectListItem>
        {
        };

        protected IJobApplicationsAppService _jobApplicationsAppService;

        public CreateModalModelBase(IJobApplicationsAppService jobApplicationsAppService)
        {
            _jobApplicationsAppService = jobApplicationsAppService;

            JobApplication = new();
        }

        public virtual async Task OnGetAsync()
        {
            JobApplication = new JobApplicationCreateViewModel();
            VacancyLookupListRequired.AddRange((
                                    await _jobApplicationsAppService.GetVacancyLookupAsync(new LookupRequestDto
                                    {
                                        MaxResultCount = LimitedResultRequestDto.MaxMaxResultCount
                                    })).Items.Select(t => new SelectListItem(t.DisplayName, t.Id.ToString())).ToList()
                        );

            await Task.CompletedTask;
        }

        public virtual async Task<IActionResult> OnPostAsync()
        {

            await _jobApplicationsAppService.CreateAsync(ObjectMapper.Map<JobApplicationCreateViewModel, JobApplicationCreateDto>(JobApplication));
            return NoContent();
        }
    }

    public class JobApplicationCreateViewModel : JobApplicationCreateDto
    {
    }
}