using Steer73.RockIT.Shared;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.Application.Dtos;
using Steer73.RockIT.JobFormResponses;

namespace Steer73.RockIT.Web.Pages.JobFormResponses
{
    public abstract class EditModalModelBase : RockITPageModel
    {
        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public Guid Id { get; set; }

        [BindProperty]
        public JobFormResponseUpdateViewModel JobFormResponse { get; set; }

        protected IJobFormResponsesAppService _jobFormResponsesAppService;

        public EditModalModelBase(IJobFormResponsesAppService jobFormResponsesAppService)
        {
            _jobFormResponsesAppService = jobFormResponsesAppService;

            JobFormResponse = new();
        }

        public virtual async Task OnGetAsync()
        {
            var jobFormResponse = await _jobFormResponsesAppService.GetAsync(Id);
            JobFormResponse = ObjectMapper.Map<JobFormResponseDto, JobFormResponseUpdateViewModel>(jobFormResponse);

        }

        public virtual async Task<NoContentResult> OnPostAsync()
        {

            await _jobFormResponsesAppService.UpdateAsync(Id, ObjectMapper.Map<JobFormResponseUpdateViewModel, JobFormResponseUpdateDto>(JobFormResponse));
            return NoContent();
        }
    }

    public class JobFormResponseUpdateViewModel : JobFormResponseUpdateDto
    {
    }
}