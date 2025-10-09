namespace Steer73.RockIT.DiversityDatas
{
    public static class DiversityDataConsts
    {
        private const string DefaultSorting = "{0}HappyToCompleteForm asc";

        public static string GetDefaultSorting(bool withEntityName)
        {
            return string.Format(DefaultSorting, withEntityName ? "DiversityData." : string.Empty);
        }

        public const int OtherGenderMaxLength = 255;
        public const int OtherSexMaxLength = 255;
        public const int OtherSexualOrientationMaxLength = 255;
        public const int OtherEthnicityMaxLength = 255;
        public const int OtherReligionOrBeliefMaxLength = 255;
        public const int OtherTypeOfSecondarySchoolMaxLength = 255;
    }
}