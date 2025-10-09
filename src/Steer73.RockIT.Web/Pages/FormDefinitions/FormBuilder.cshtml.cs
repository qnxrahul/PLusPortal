using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Steer73.RockIT.FormDefinitions;
using System;
using System.Threading.Tasks;

namespace Steer73.RockIT.Web.Pages.FormDefinitions
{
    public class FormBuilderModel : RockITPageModel
    {
        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public Guid Id { get; set; }

        [BindProperty]
        public FormDefinitionUpdateViewModel FormDefinition { get; set; }

        protected IFormDefinitionsAppService _formDefinitionsAppService;

        public FormBuilderModel(IFormDefinitionsAppService formDefinitionsAppService)
        {
            _formDefinitionsAppService = formDefinitionsAppService;

            FormDefinition = new();
        }

        public virtual async Task OnGetAsync()
        {
            var formDefinition = await _formDefinitionsAppService.GetAsync(Id);
            FormDefinition = ObjectMapper.Map<FormDefinitionDto, FormDefinitionUpdateViewModel>(formDefinition);
        }
    }
}
