namespace Steer73.RockIT.DiversityFormResponses
{
    public static class DiversityFormResponseConsts
    {
        private const string DefaultSorting = "{0}FormStructureJson asc";

        public static string GetDefaultSorting(bool withEntityName)
        {
            return string.Format(DefaultSorting, withEntityName ? "DiversityFormResponse." : string.Empty);
        }

    }
}