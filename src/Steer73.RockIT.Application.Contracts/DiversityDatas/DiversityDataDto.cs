using Steer73.RockIT.Enums;
using System;
using System.Collections.Generic;

using Volo.Abp.Application.Dtos;

namespace Steer73.RockIT.DiversityDatas
{
    public abstract class DiversityDataDtoBase : FullAuditedEntityDto<Guid>
    {
        public Guid JobApplicationId { get; set; }
        public YesNo? HappyToCompleteForm { get; set; }
        public AgeRange? AgeRange { get; set; }
        public GenderOrSex? Gender { get; set; }
        public string? OtherGender { get; set; }
        public YesNoPreferNotToSay? GenderIdentitySameAsBirth { get; set; }
        public GenderOrSex? Sex { get; set; }
        public string? OtherSex { get; set; }
        public SexualOrientation? SexualOrientation { get; set; }
        public string? OtherSexualOrientation { get; set; }
        public Ethnicity? Ethnicity { get; set; }
        public string? OtherEthnicity { get; set; }
        public Religion? ReligionOrBelief { get; set; }
        public string? OtherReligionOrBelief { get; set; }
        public YesNoPreferNotToSay? Disability { get; set; }
        public EducationLevel? EducationLevel { get; set; }
        public TypeOfSecondarySchool? TypeOfSecondarySchool { get; set; }
        public string? OtherTypeOfSecondarySchool { get; set; }
        public YesNoPreferNotToSayDontKnow? HigherEducationQualifications { get; set; }

    }
}