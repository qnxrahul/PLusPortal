using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Steer73.RockIT.PracticeGroups;
using Steer73.RockIT.Shared;

namespace Steer73.RockIT.Web.Pages.PracticeGroups
{
    public class IndexModel : IndexModelBase
    {
        public IndexModel(IPracticeGroupsAppService practiceGroupsAppService)
            : base(practiceGroupsAppService)
        {
        }
    }
}