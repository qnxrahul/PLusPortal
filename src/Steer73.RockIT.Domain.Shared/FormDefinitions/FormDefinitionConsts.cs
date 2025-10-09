namespace Steer73.RockIT.FormDefinitions
{
    public static class FormDefinitionConsts
    {
        private const string DefaultSorting = "{0}ReferenceId asc";

        public static string GetDefaultSorting(bool withEntityName)
        {
            return string.Format(DefaultSorting, withEntityName ? "FormDefinition." : string.Empty);
        }

        public const int ReferenceIdMaxLength = 255;
        public const int NameMaxLength = 1024;
    }
}