namespace Steer73.RockIT.Companies
{
    public static class CompanyConsts
    {
        private const string DefaultSorting = "{0}Name asc";

        public static string GetDefaultSorting(bool withEntityName)
        {
            return string.Format(DefaultSorting, withEntityName ? "Company." : string.Empty);
        }

        public const int NameMaxLength = 255;
        public const int PhoneMaxLength = 50;
        public const int AddressMaxLength = 255;
        public const int PostcodeMaxLength = 50;
        public const int PrimaryContactMaxLength = 255;
        public const int LogoUrlMaxLength = 255;

        public const int ExternalRequestTimeoutInSec = 25;

        public const string BackgroundWorkerLogUserName = "Background Worker";
        public const string BackgroundWorkerLogApplicationName = "Steer73.RockIT.Web";
        public const string BackgroundWorkerLogEntryComment = "Ezekia Api v1 Integration";
        public const string BackgroundWorkerLogSendApplicantDataUrl = "BackgroundJob/SendApplicantData";
        public const string BackgroundWorkerLogSendApplicantDocumentsUrl = "BackgroundJob/SendApplicantDocuments";
        public const string BackgroundWorkerLogUploadCompaniesUrl = "BackgroundWorker/UploadCompanies";
        public const string BackgroundWorkerLogSendVacancyDataUrl = "BackgroundJob/SendVacancyData";
        public const string BackgroundWorkerLogSendVacancyDocumentsUrl = "BackgroundWorker/SendVacancyDocuments";
    }
}