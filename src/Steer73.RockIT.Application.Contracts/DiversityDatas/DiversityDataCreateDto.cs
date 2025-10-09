using Steer73.RockIT.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Steer73.RockIT.DiversityDatas
{
    public abstract class DiversityDataCreateDtoBase
    {
        public Guid JobApplicationId { get; set; }
        public YesNo? HappyToCompleteForm { get; set; }
        public AgeRange? AgeRange { get; set; }
        public GenderOrSex? Gender { get; set; }
        [StringLength(DiversityDataConsts.OtherGenderMaxLength)]
        public string? OtherGender { get; set; }
        public YesNoPreferNotToSay? GenderIdentitySameAsBirth { get; set; }
        public GenderOrSex? Sex { get; set; }
        [StringLength(DiversityDataConsts.OtherSexMaxLength)]
        public string? OtherSex { get; set; }
        public SexualOrientation? SexualOrientation { get; set; }
        [StringLength(DiversityDataConsts.OtherSexualOrientationMaxLength)]
        public string? OtherSexualOrientation { get; set; }
        public Ethnicity? Ethnicity { get; set; }
        [StringLength(DiversityDataConsts.OtherEthnicityMaxLength)]
        public string? OtherEthnicity { get; set; }
        public Religion? ReligionOrBelief { get; set; }
        [StringLength(DiversityDataConsts.OtherReligionOrBeliefMaxLength)]
        public string? OtherReligionOrBelief { get; set; }
        public YesNoPreferNotToSay? Disability { get; set; }
        public EducationLevel? EducationLevel { get; set; }
        public TypeOfSecondarySchool? TypeOfSecondarySchool { get; set; }
        [StringLength(DiversityDataConsts.OtherTypeOfSecondarySchoolMaxLength)]
        public string? OtherTypeOfSecondarySchool { get; set; }
        public YesNoPreferNotToSayDontKnow? HigherEducationQualifications { get; set; }
    }
}