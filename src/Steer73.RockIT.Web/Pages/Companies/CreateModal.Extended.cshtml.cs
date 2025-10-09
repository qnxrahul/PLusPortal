using Steer73.RockIT.Shared;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.Application.Dtos;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Steer73.RockIT.Companies;

namespace Steer73.RockIT.Web.Pages.Companies
{
    public class CreateModalModel : CreateModalModelBase
    {
        public CreateModalModel(ICompaniesAppService companiesAppService)
            : base(companiesAppService)
        {
        }
    }
}