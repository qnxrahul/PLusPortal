namespace Steer73.RockIT.PracticeAreas
{
    public static class PracticeAreaConsts
    {
        private const string DefaultSorting = "{0}Name asc";

        public static string GetDefaultSorting(bool withEntityName)
        {
            return string.Format(DefaultSorting, withEntityName ? "PracticeArea." : string.Empty);
        }

        public const int NameMaxLength = 255;
    }
}