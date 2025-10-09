namespace Steer73.RockIT.JobFormResponses
{
    public static class JobFormResponseConsts
    {
        private const string DefaultSorting = "{0}FormStructureJson asc";

        public static string GetDefaultSorting(bool withEntityName)
        {
            return string.Format(DefaultSorting, withEntityName ? "JobFormResponse." : string.Empty);
        }

    }
}