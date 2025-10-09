using Steer73.RockIT.Companies;


namespace Steer73.RockIT.FormDefinitions
{
    public abstract class FormDefinitionWithNavigationPropertiesDtoBase
    {
        public FormDefinitionDto FormDefinition { get; set; } = null!;

        public CompanyDto Company { get; set; } = null!;
    }
}
