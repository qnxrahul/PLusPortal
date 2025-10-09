using Steer73.RockIT.Shared;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.Application.Dtos;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Steer73.RockIT.PracticeGroups;

namespace Steer73.RockIT.Web.Pages.PracticeGroups
{
    public abstract class CreateModalModelBase : RockITPageModel
    {

        [BindProperty]
        public PracticeGroupCreateViewModel PracticeGroup { get; set; }

        protected IPracticeGroupsAppService _practiceGroupsAppService;

        public CreateModalModelBase(IPracticeGroupsAppService practiceGroupsAppService)
        {
            _practiceGroupsAppService = practiceGroupsAppService;

            PracticeGroup = new();
        }

        public virtual async Task OnGetAsync()
        {
            PracticeGroup = new PracticeGroupCreateViewModel();

            await Task.CompletedTask;
        }

        public virtual async Task<IActionResult> OnPostAsync()
        {

            await _practiceGroupsAppService.CreateAsync(ObjectMapper.Map<PracticeGroupCreateViewModel, PracticeGroupCreateDto>(PracticeGroup));
            return NoContent();
        }
    }

    public class PracticeGroupCreateViewModel : PracticeGroupCreateDto
    {
    }
}