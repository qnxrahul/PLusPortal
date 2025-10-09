using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Steer73.RockIT.PracticeAreas;

namespace Steer73.RockIT.Web.Pages.PracticeAreas
{
    public abstract class CreateModalModelBase : RockITPageModel
    {
        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public Guid PracticeGroupId { get; set; }

        [BindProperty]
        public PracticeAreaCreateViewModel PracticeArea { get; set; }

        protected IPracticeAreasAppService _practiceAreasAppService;

        public CreateModalModelBase(
            IPracticeAreasAppService practiceAreasAppService)
        {
            _practiceAreasAppService = practiceAreasAppService;

            PracticeArea = new();
        }

        public virtual async Task OnGetAsync()
        {
            PracticeArea = new PracticeAreaCreateViewModel();

            await Task.CompletedTask;
        }

        public virtual async Task<IActionResult> OnPostAsync()
        {

            PracticeArea.PracticeGroupId = PracticeGroupId;
            await _practiceAreasAppService.CreateAsync(ObjectMapper.Map<PracticeAreaCreateViewModel, PracticeAreaCreateDto>(PracticeArea));
            return NoContent();
        }
    }

    public class PracticeAreaCreateViewModel : PracticeAreaCreateDto
    {
    }
}