using Steer73.RockIT.Shared;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.Application.Dtos;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Steer73.RockIT.FormDefinitions;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Steer73.RockIT.Vacancies;
using System.ComponentModel.DataAnnotations;

namespace Steer73.RockIT.Web.Pages.FormDefinitions
{
    public abstract class CreateModalModelBase : RockITPageModel
    {

        [BindProperty]
        public FormDefinitionCreateViewModel FormDefinition { get; set; }

        protected IFormDefinitionsAppService _formDefinitionsAppService;
        protected IVacanciesAppService _vacanciesAppService;

        public CreateModalModelBase(
            IFormDefinitionsAppService formDefinitionsAppService,
            IVacanciesAppService vacanciesAppService
            )
        {
            _formDefinitionsAppService = formDefinitionsAppService;
            _vacanciesAppService = vacanciesAppService;

            FormDefinition = new();

        }

        public virtual async Task OnGetAsync()
        {
            FormDefinition = new FormDefinitionCreateViewModel();

            await Task.CompletedTask;
        }

        public virtual async Task<IActionResult> OnPostAsync()
        {
            await _formDefinitionsAppService.CreateAsync(ObjectMapper.Map<FormDefinitionCreateViewModel, FormDefinitionCreateDto>(FormDefinition));
            return NoContent();
        }
    }

    public class FormDefinitionCreateViewModel : FormDefinitionCreateDto
    {
       
    }
}