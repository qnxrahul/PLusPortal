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
    public class CreateModalModel : CreateModalModelBase
    {
        public CreateModalModel(IPracticeGroupsAppService practiceGroupsAppService)
            : base(practiceGroupsAppService)
        {
        }
    }
}