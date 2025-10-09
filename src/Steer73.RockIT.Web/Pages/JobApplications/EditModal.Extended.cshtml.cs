using Steer73.RockIT.Shared;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.Application.Dtos;
using Steer73.RockIT.JobApplications;

namespace Steer73.RockIT.Web.Pages.JobApplications
{
    public class EditModalModel : EditModalModelBase
    {
        public EditModalModel(IJobApplicationsAppService jobApplicationsAppService)
            : base(jobApplicationsAppService)
        {
        }
    }
}