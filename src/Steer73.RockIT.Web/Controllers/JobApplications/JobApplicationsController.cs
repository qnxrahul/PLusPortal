using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace Steer73.RockIT.Web.Controllers.JobApplications;

[Route("[controller]/[action]")]
public class JobApplicationsController : AbpController
{
    [HttpGet]
    public virtual async Task<PartialViewResult> ChildDataGrid(Guid jobApplicationId)
    {
        return PartialView("~/Pages/Shared/JobApplications/_ChildDataGrids.cshtml", jobApplicationId);
    }
}