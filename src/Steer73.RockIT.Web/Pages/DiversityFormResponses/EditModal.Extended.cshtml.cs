using Steer73.RockIT.Shared;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.Application.Dtos;
using Steer73.RockIT.DiversityFormResponses;

namespace Steer73.RockIT.Web.Pages.DiversityFormResponses
{
    public class EditModalModel : EditModalModelBase
    {
        public EditModalModel(IDiversityFormResponsesAppService diversityFormResponsesAppService)
            : base(diversityFormResponsesAppService)
        {
        }
    }
}