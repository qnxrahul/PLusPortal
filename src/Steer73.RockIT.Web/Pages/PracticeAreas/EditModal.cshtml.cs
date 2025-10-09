using Steer73.RockIT.Shared;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.Application.Dtos;
using Steer73.RockIT.PracticeAreas;

namespace Steer73.RockIT.Web.Pages.PracticeAreas
{
    public abstract class EditModalModelBase : RockITPageModel
    {
        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public Guid Id { get; set; }

        [BindProperty]
        public PracticeAreaUpdateViewModel PracticeArea { get; set; }

        protected IPracticeAreasAppService _practiceAreasAppService;

        public EditModalModelBase(IPracticeAreasAppService practiceAreasAppService)
        {
            _practiceAreasAppService = practiceAreasAppService;

            PracticeArea = new();
        }

        public virtual async Task OnGetAsync()
        {
            var practiceArea = await _practiceAreasAppService.GetAsync(Id);
            PracticeArea = ObjectMapper.Map<PracticeAreaDto, PracticeAreaUpdateViewModel>(practiceArea);

        }

        public virtual async Task<NoContentResult> OnPostAsync()
        {

            await _practiceAreasAppService.UpdateAsync(Id, ObjectMapper.Map<PracticeAreaUpdateViewModel, PracticeAreaUpdateDto>(PracticeArea));
            return NoContent();
        }
    }

    public class PracticeAreaUpdateViewModel : PracticeAreaUpdateDto
    {
    }
}