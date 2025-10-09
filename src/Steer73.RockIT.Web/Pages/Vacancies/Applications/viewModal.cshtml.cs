using Microsoft.AspNetCore.Mvc;
using Steer73.RockIT.JobApplications;
using Steer73.RockIT.Vacancies;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Steer73.RockIT.Web.Pages.Vacancies
{
    public abstract class ViewModalModelBase : RockITPageModel
    {
		[HiddenInput]
		[BindProperty(SupportsGet = true)]
		public Guid Id { get; set; }

		[BindProperty]
        public JobApplicationViewModel JobApplication { get; set; }

        protected readonly IJobApplicationsAppService _jobApplicationAppService;
		
        public ViewModalModelBase(
			IJobApplicationsAppService jobApplicationAppService)
        {
			_jobApplicationAppService = jobApplicationAppService;

			JobApplication = new();
        }

        public virtual async Task OnGetAsync()
		{
			var applicationWithNavigationPropertiesDto = await _jobApplicationAppService.GetWithNavigationPropertiesAsync(Id);
			JobApplication = ObjectMapper.Map<JobApplicationWithNavigationPropertiesDto, JobApplicationViewModel>(applicationWithNavigationPropertiesDto);
		}

		public virtual async Task<IActionResult> OnPostApproveAsync(CancellationToken cancellationToken = default)
		{
			await _jobApplicationAppService.ApproveAsync(Id);

			return NoContent();
		}

		public virtual async Task<IActionResult> OnPostRejectAsync(CancellationToken cancellationToken = default)
		{
			await _jobApplicationAppService.RejectAsync(Id);
            
			return NoContent();
		}
	}
	public class JobApplicationViewModel : JobApplicationWithNavigationPropertiesDto
	{
	}
}