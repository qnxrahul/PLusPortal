using Steer73.RockIT.Shared;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.Application.Dtos;
using Steer73.RockIT.Companies;

namespace Steer73.RockIT.Web.Pages.Companies
{
    public class EditModalModel : EditModalModelBase
    {
        public EditModalModel(ICompaniesAppService companiesAppService)
            : base(companiesAppService)
        {
        }
    }
}