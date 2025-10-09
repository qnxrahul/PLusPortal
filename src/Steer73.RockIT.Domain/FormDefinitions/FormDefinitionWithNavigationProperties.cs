using Steer73.RockIT.Companies;

namespace Steer73.RockIT.FormDefinitions
{
    public abstract class FormDefinitionWithNavigationPropertiesBase
    {
        public FormDefinition FormDefinition { get; set; } = null!;

        public Company Company { get; set; } = null!;
    }
}
