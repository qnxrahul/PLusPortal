using Steer73.RockIT.Shared;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.Application.Dtos;
using Steer73.RockIT.PracticeAreas;
using Steer73.RockIT.PracticeGroups;

namespace Steer73.RockIT.Web.Pages.PracticeAreas
{
    public class EditModalModel : EditModalModelBase
    {
        protected IPracticeGroupsAppService _practiceGroupsAppService;

        public EditModalModel(
            IPracticeAreasAppService practiceAreasAppService,
            IPracticeGroupsAppService practiceGroupsAppService)
            : base(practiceAreasAppService)
        {
            _practiceGroupsAppService = practiceGroupsAppService;
        }

        public override async Task OnGetAsync()
        {
            var practiceArea = await _practiceAreasAppService.GetAsync(Id);

            var practiceGroups = await _practiceGroupsAppService.GetListAsync(
                new GetPracticeGroupsInput 
                {
                    MaxResultCount = 30,
                    IsActive = true
                });

            PracticeArea = ObjectMapper.Map<PracticeAreaDto, PracticeAreaUpdateViewModel>(practiceArea);

            PracticeArea.PracticeGroups = practiceGroups.Items.ToDictionary(x => x.Id, x => x.Name);
        }
    }
}