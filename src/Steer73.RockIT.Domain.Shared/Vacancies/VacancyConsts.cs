namespace Steer73.RockIT.Vacancies
{
    public static class VacancyConsts
    {
        private const string DefaultSorting = "{0}Title asc";

        public static string GetDefaultSorting(bool withEntityName)
        {
            return string.Format(DefaultSorting, withEntityName ? "Vacancy." : string.Empty);
        }

        public const int TitleMaxLength = 255;
        public const int ReferenceMaxLength = 255;
        public const int RegionMaxLength = 255;
        public const int MediaSourcesMaxLength = 1024;
        public const int RoleMaxLength = 255;
        public const int BenefitsMaxLength = 255;
        public const int LocationMaxLength = 255;
        public const int SalaryMaxLength = 255;
        public const int RoleTypeMaxLength = 255;
        public const int LinkedInUrlMaxLength = 255;
    }
}