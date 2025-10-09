using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace Steer73.RockIT.Web.Controllers.PracticeGroups;

[Route("[controller]/[action]")]
public class PracticeGroupsController : AbpController
{
    [HttpGet]
    public virtual async Task<PartialViewResult> ChildDataGrid(Guid practiceGroupId)
    {
        return PartialView("~/Pages/Shared/PracticeGroups/_ChildDataGrids.cshtml", practiceGroupId);
    }
}