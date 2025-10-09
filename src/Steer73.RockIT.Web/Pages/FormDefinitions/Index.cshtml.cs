using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Steer73.RockIT.FormDefinitions;
using Steer73.RockIT.Shared;
using Steer73.RockIT.Vacancies;
using System.ComponentModel;

namespace Steer73.RockIT.Web.Pages.FormDefinitions
{
    public abstract class IndexModelBase : AbpPageModel
    {
        public string? ReferenceIdFilter { get; set; }
        public string? NameFilter { get; set; }
        public string? FormDetailsFilter { get; set; }
        public FormType? FormTypeFilter { get; set; }

        [DisplayName("Company")]
        public Guid CompanyIdFilter { get; set; }


        protected IFormDefinitionsAppService _formDefinitionsAppService;
        protected IVacanciesAppService _vacanciesAppService;

        public IndexModelBase(
            IFormDefinitionsAppService formDefinitionsAppService,
            IVacanciesAppService vacanciesAppService)
        {
            _formDefinitionsAppService = formDefinitionsAppService;
            _vacanciesAppService = vacanciesAppService;
        }

        public virtual async Task OnGetAsync()
        {
            await Task.CompletedTask;
        }
    }
}