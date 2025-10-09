using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Steer73.RockIT.Companies;
using Steer73.RockIT.Shared;

namespace Steer73.RockIT.Web.Pages.Companies
{
    public abstract class IndexModelBase : AbpPageModel
    {
        public string? NameFilter { get; set; }
        public string? PhoneFilter { get; set; }
        public string? AddressFilter { get; set; }
        public string? PostcodeFilter { get; set; }
        public string? PrimaryContactFilter { get; set; }

        protected ICompaniesAppService _companiesAppService;

        public IndexModelBase(ICompaniesAppService companiesAppService)
        {
            _companiesAppService = companiesAppService;
        }

        public virtual async Task OnGetAsync()
        {

            await Task.CompletedTask;
        }
    }
}