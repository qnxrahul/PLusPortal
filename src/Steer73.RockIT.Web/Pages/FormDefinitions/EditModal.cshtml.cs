using Steer73.RockIT.Shared;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.Application.Dtos;
using Steer73.RockIT.FormDefinitions;
using Steer73.RockIT.Vacancies;
using Steer73.RockIT.Companies;

namespace Steer73.RockIT.Web.Pages.FormDefinitions
{
    public abstract class EditModalModelBase : RockITPageModel
    {
        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public Guid Id { get; set; }

        [BindProperty]
        public FormDefinitionUpdateViewModel FormDefinition { get; set; }

        public CompanyDto? SelectedCompany { get; set; }

        protected IFormDefinitionsAppService _formDefinitionsAppService;
        protected IVacanciesAppService _vacanciesAppService;
        protected ICompaniesAppService _companiesAppService;

        public EditModalModelBase(
            IFormDefinitionsAppService formDefinitionsAppService,
            IVacanciesAppService vacanciesAppService,
            ICompaniesAppService companiesAppService
            )
        {
            _formDefinitionsAppService = formDefinitionsAppService;
            _vacanciesAppService = vacanciesAppService;
            _companiesAppService = companiesAppService;

            FormDefinition = new();
        }

        public virtual async Task OnGetAsync()
        {
            var formDefinition = await _formDefinitionsAppService.GetAsync(Id);
            FormDefinition = ObjectMapper.Map<FormDefinitionDto, FormDefinitionUpdateViewModel>(formDefinition);

            if (formDefinition.CompanyId != Guid.Empty)
            {
                SelectedCompany = await _companiesAppService.GetAsync(formDefinition.CompanyId);
            }
        }

        public virtual async Task<NoContentResult> OnPostAsync()
        {

            await _formDefinitionsAppService.UpdateAsync(Id, ObjectMapper.Map<FormDefinitionUpdateViewModel, FormDefinitionUpdateDto>(FormDefinition));
            return NoContent();
        }
    }

    public class FormDefinitionUpdateViewModel : FormDefinitionUpdateDto
    {
        public int ActiveVacanciesCount { get; set; }
        public int ClosedVacanciesCount { get; set; }
        public int PendingVacanciesCount { get; set; }
    }
}