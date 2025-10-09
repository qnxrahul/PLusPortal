using Steer73.RockIT.FormDefinitions;

namespace Steer73.RockIT.Shared
{
    public abstract class FormDefinitionLookupBase<TKey> : LookupDtoBase<TKey>
    {
        public FormType FormType { get; set; }
    }
}