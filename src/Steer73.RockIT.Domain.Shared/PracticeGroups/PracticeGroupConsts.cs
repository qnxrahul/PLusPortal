namespace Steer73.RockIT.PracticeGroups
{
    public static class PracticeGroupConsts
    {
        private const string DefaultSorting = "{0}Name asc";

        public static string GetDefaultSorting(bool withEntityName)
        {
            return string.Format(DefaultSorting, withEntityName ? "PracticeGroup." : string.Empty);
        }

        public const int NameMaxLength = 255;
    }
}