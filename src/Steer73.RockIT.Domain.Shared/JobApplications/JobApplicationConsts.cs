using Steer73.RockIT.Enums;

namespace Steer73.RockIT.JobApplications
{
    public static class JobApplicationConsts
    {
        private const string DefaultSorting = "{0}LastName asc,{0}FirstName asc";

        public static string GetDefaultSorting(bool withEntityName)
        {
            return string.Format(DefaultSorting, withEntityName ? "JobApplication." : string.Empty);
        }

        public const int DefaultStringMaxLength = 255;
        public const int FirstNameMaxLength = 255;
        public const int LastNameMaxLength = 255;
        public const int EmailAddressMaxLength = 255;
        public const int TitleMaxLength = 255;
        public const int PhoneNumberMaxLength = 64;
        public const int LandlineMaxLength = 255;
        public const int CurrentRoleMaxLength = 255;
        public const int CurrentCompanyMaxLength = 255;
        public const int CurrentPositionTypeMaxLength = 255;

        public const JobApplicationStatus DefaultStatus = JobApplicationStatus.Pending;
        public const SyncStatus DefaultSyncStatus = SyncStatus.Pending;

        public const int MaxFileSize = 2 * 1024 * 1024;
    }
}