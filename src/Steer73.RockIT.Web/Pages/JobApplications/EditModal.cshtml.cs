using Steer73.RockIT.Shared;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.Application.Dtos;
using Steer73.RockIT.JobApplications;
using Steer73.RockIT.Enums;

namespace Steer73.RockIT.Web.Pages.JobApplications
{
    public abstract class EditModalModelBase : RockITPageModel
    {
        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public Guid Id { get; set; }

        public JobApplicationStatus? Status { get; set; }

        [BindProperty]
        public JobApplicationUpdateViewModel JobApplication { get; set; }

        public List<SelectListItem> VacancyLookupListRequired { get; set; } = new List<SelectListItem>
        {
        };

        protected IJobApplicationsAppService _jobApplicationsAppService;

        public EditModalModelBase(IJobApplicationsAppService jobApplicationsAppService)
        {
            _jobApplicationsAppService = jobApplicationsAppService;

            JobApplication = new();
        }

        public virtual async Task OnGetAsync()
        {
            var jobApplicationWithNavigationPropertiesDto = await _jobApplicationsAppService.GetWithNavigationPropertiesAsync(Id);
            JobApplication = ObjectMapper.Map<JobApplicationDto, JobApplicationUpdateViewModel>(jobApplicationWithNavigationPropertiesDto.JobApplication);

            VacancyLookupListRequired.AddRange((
                                    await _jobApplicationsAppService.GetVacancyLookupAsync(new LookupRequestDto
                                    {
                                        MaxResultCount = LimitedResultRequestDto.MaxMaxResultCount
                                    })).Items.Select(t => new SelectListItem(t.DisplayName, t.Id.ToString())).ToList()
                        );

            Status = jobApplicationWithNavigationPropertiesDto.JobApplication.Status;
        }

        public virtual async Task<NoContentResult> OnPostAsync()
        {

            await _jobApplicationsAppService.UpdateAsync(Id, ObjectMapper.Map<JobApplicationUpdateViewModel, JobApplicationUpdateDto>(JobApplication));
            return NoContent();
        }
    }

    public class JobApplicationUpdateViewModel : JobApplicationUpdateDto
    {
    }
}