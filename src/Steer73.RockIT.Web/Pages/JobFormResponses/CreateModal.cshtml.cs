using Steer73.RockIT.Shared;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.Application.Dtos;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Steer73.RockIT.JobFormResponses;

namespace Steer73.RockIT.Web.Pages.JobFormResponses
{
    public abstract class CreateModalModelBase : RockITPageModel
    {
        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public Guid JobApplicationId { get; set; }

        [BindProperty]
        public JobFormResponseCreateViewModel JobFormResponse { get; set; }

        protected IJobFormResponsesAppService _jobFormResponsesAppService;

        public CreateModalModelBase(IJobFormResponsesAppService jobFormResponsesAppService)
        {
            _jobFormResponsesAppService = jobFormResponsesAppService;

            JobFormResponse = new();
        }

        public virtual async Task OnGetAsync()
        {
            JobFormResponse = new JobFormResponseCreateViewModel();

            await Task.CompletedTask;
        }

        public virtual async Task<IActionResult> OnPostAsync()
        {

            JobFormResponse.JobApplicationId = JobApplicationId;
            await _jobFormResponsesAppService.CreateAsync(ObjectMapper.Map<JobFormResponseCreateViewModel, JobFormResponseCreateDto>(JobFormResponse));
            return NoContent();
        }
    }

    public class JobFormResponseCreateViewModel : JobFormResponseCreateDto
    {
    }
}