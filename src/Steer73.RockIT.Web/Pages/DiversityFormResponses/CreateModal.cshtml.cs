using Steer73.RockIT.Shared;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.Application.Dtos;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Steer73.RockIT.DiversityFormResponses;

namespace Steer73.RockIT.Web.Pages.DiversityFormResponses
{
    public abstract class CreateModalModelBase : RockITPageModel
    {
        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public Guid JobApplicationId { get; set; }

        [BindProperty]
        public DiversityFormResponseCreateViewModel DiversityFormResponse { get; set; }

        protected IDiversityFormResponsesAppService _diversityFormResponsesAppService;

        public CreateModalModelBase(IDiversityFormResponsesAppService diversityFormResponsesAppService)
        {
            _diversityFormResponsesAppService = diversityFormResponsesAppService;

            DiversityFormResponse = new();
        }

        public virtual async Task OnGetAsync()
        {
            DiversityFormResponse = new DiversityFormResponseCreateViewModel();

            await Task.CompletedTask;
        }

        public virtual async Task<IActionResult> OnPostAsync()
        {

            DiversityFormResponse.JobApplicationId = JobApplicationId;
            await _diversityFormResponsesAppService.CreateAsync(ObjectMapper.Map<DiversityFormResponseCreateViewModel, DiversityFormResponseCreateDto>(DiversityFormResponse));
            return NoContent();
        }
    }

    public class DiversityFormResponseCreateViewModel : DiversityFormResponseCreateDto
    {
    }
}