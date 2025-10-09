using Steer73.RockIT.Shared;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.Application.Dtos;
using Steer73.RockIT.PracticeGroups;

namespace Steer73.RockIT.Web.Pages.PracticeGroups
{
    public abstract class EditModalModelBase : RockITPageModel
    {
        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public Guid Id { get; set; }

        [BindProperty]
        public PracticeGroupUpdateViewModel PracticeGroup { get; set; }

        protected IPracticeGroupsAppService _practiceGroupsAppService;

        public EditModalModelBase(IPracticeGroupsAppService practiceGroupsAppService)
        {
            _practiceGroupsAppService = practiceGroupsAppService;

            PracticeGroup = new();
        }

        public virtual async Task OnGetAsync()
        {
            var practiceGroup = await _practiceGroupsAppService.GetAsync(Id);
            PracticeGroup = ObjectMapper.Map<PracticeGroupDto, PracticeGroupUpdateViewModel>(practiceGroup);

        }

        public virtual async Task<NoContentResult> OnPostAsync()
        {

            await _practiceGroupsAppService.UpdateAsync(Id, ObjectMapper.Map<PracticeGroupUpdateViewModel, PracticeGroupUpdateDto>(PracticeGroup));
            return NoContent();
        }
    }

    public class PracticeGroupUpdateViewModel : PracticeGroupUpdateDto
    {
    }
}