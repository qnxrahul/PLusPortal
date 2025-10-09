using Steer73.RockIT.Shared;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.Application.Dtos;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Steer73.RockIT.JobFormResponses;

namespace Steer73.RockIT.Web.Pages.JobFormResponses
{
    public class CreateModalModel : CreateModalModelBase
    {
        public CreateModalModel(IJobFormResponsesAppService jobFormResponsesAppService)
            : base(jobFormResponsesAppService)
        {
        }
    }
}