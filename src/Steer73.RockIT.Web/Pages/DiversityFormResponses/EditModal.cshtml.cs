using Steer73.RockIT.Shared;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.Application.Dtos;
using Steer73.RockIT.DiversityFormResponses;

namespace Steer73.RockIT.Web.Pages.DiversityFormResponses
{
    public abstract class EditModalModelBase : RockITPageModel
    {
        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public Guid Id { get; set; }

        [BindProperty]
        public DiversityFormResponseUpdateViewModel DiversityFormResponse { get; set; }

        protected IDiversityFormResponsesAppService _diversityFormResponsesAppService;

        public EditModalModelBase(IDiversityFormResponsesAppService diversityFormResponsesAppService)
        {
            _diversityFormResponsesAppService = diversityFormResponsesAppService;

            DiversityFormResponse = new();
        }

        public virtual async Task OnGetAsync()
        {
            var diversityFormResponse = await _diversityFormResponsesAppService.GetAsync(Id);
            DiversityFormResponse = ObjectMapper.Map<DiversityFormResponseDto, DiversityFormResponseUpdateViewModel>(diversityFormResponse);

        }

        public virtual async Task<NoContentResult> OnPostAsync()
        {

            await _diversityFormResponsesAppService.UpdateAsync(Id, ObjectMapper.Map<DiversityFormResponseUpdateViewModel, DiversityFormResponseUpdateDto>(DiversityFormResponse));
            return NoContent();
        }
    }

    public class DiversityFormResponseUpdateViewModel : DiversityFormResponseUpdateDto
    {
    }
}