using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Steer73.RockIT.Vacancies;
using Steer73.RockIT.Shared;
using Steer73.RockIT.Web.Pages.Vacancies;

namespace Steer73.RockIT.Web.Admin.Pages.Home.Vacancies
{
    public class IndexModel : IndexModelBase
    {
        public IndexModel(IVacanciesAppService vacanciesAppService)
            : base(vacanciesAppService)
        {
        }
    }
}