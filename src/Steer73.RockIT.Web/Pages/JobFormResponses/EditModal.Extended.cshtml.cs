using Steer73.RockIT.Shared;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.Application.Dtos;
using Steer73.RockIT.JobFormResponses;

namespace Steer73.RockIT.Web.Pages.JobFormResponses
{
    public class EditModalModel : EditModalModelBase
    {
        public EditModalModel(IJobFormResponsesAppService jobFormResponsesAppService)
            : base(jobFormResponsesAppService)
        {
        }
    }
}