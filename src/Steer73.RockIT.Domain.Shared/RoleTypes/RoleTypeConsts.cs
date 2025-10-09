namespace Steer73.RockIT.RoleTypes
{
    public static class RoleTypeConsts
    {
        private const string DefaultSorting = "{0}Name asc";

        public static string GetDefaultSorting(bool withEntityName)
        {
            return string.Format(DefaultSorting, withEntityName ? "MediaSource." : string.Empty);
        }

        public const int NameMaxLength = 255;
        public const int DescriptionMaxLength = 255;
    }
}
