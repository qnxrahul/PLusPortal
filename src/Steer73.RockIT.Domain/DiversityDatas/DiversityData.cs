using Steer73.RockIT.Enums;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;
using JetBrains.Annotations;

using Volo.Abp;

namespace Steer73.RockIT.DiversityDatas
{
    public abstract class DiversityDataBase : FullAuditedEntity<Guid>
    {
        public virtual Guid JobApplicationId { get; set; }

        public virtual YesNo? HappyToCompleteForm { get; set; }

        public virtual AgeRange? AgeRange { get; set; }

        public virtual GenderOrSex? Gender { get; set; }

        [CanBeNull]
        public virtual string? OtherGender { get; set; }

        public virtual YesNoPreferNotToSay? GenderIdentitySameAsBirth { get; set; }

        public virtual GenderOrSex? Sex { get; set; }

        [CanBeNull]
        public virtual string? OtherSex { get; set; }

        public virtual SexualOrientation? SexualOrientation { get; set; }

        [CanBeNull]
        public virtual string? OtherSexualOrientation { get; set; }

        public virtual Ethnicity? Ethnicity { get; set; }

        [CanBeNull]
        public virtual string? OtherEthnicity { get; set; }

        public virtual Religion? ReligionOrBelief { get; set; }

        [CanBeNull]
        public virtual string? OtherReligionOrBelief { get; set; }

        public virtual YesNoPreferNotToSay? Disability { get; set; }

        public virtual EducationLevel? EducationLevel { get; set; }

        public virtual TypeOfSecondarySchool? TypeOfSecondarySchool { get; set; }

        [CanBeNull]
        public virtual string? OtherTypeOfSecondarySchool { get; set; }

        public virtual YesNoPreferNotToSayDontKnow? HigherEducationQualifications { get; set; }

        protected DiversityDataBase()
        {

        }

        public DiversityDataBase(Guid id, Guid jobApplicationId, YesNo? happyToCompleteForm = null, AgeRange? ageRange = null, GenderOrSex? gender = null, string? otherGender = null, YesNoPreferNotToSay? genderIdentitySameAsBirth = null, GenderOrSex? sex = null, string? otherSex = null, SexualOrientation? sexualOrientation = null, string? otherSexualOrientation = null, Ethnicity? ethnicity = null, string? otherEthnicity = null, Religion? religionOrBelief = null, string? otherReligionOrBelief = null, YesNoPreferNotToSay? disability = null, EducationLevel? educationLevel = null, TypeOfSecondarySchool? typeOfSecondarySchool = null, string? otherTypeOfSecondarySchool = null, YesNoPreferNotToSayDontKnow? higherEducationQualifications = null)
        {

            Id = id;
            Check.Length(otherGender, nameof(otherGender), DiversityDataConsts.OtherGenderMaxLength, 0);
            Check.Length(otherSex, nameof(otherSex), DiversityDataConsts.OtherSexMaxLength, 0);
            Check.Length(otherSexualOrientation, nameof(otherSexualOrientation), DiversityDataConsts.OtherSexualOrientationMaxLength, 0);
            Check.Length(otherEthnicity, nameof(otherEthnicity), DiversityDataConsts.OtherEthnicityMaxLength, 0);
            Check.Length(otherReligionOrBelief, nameof(otherReligionOrBelief), DiversityDataConsts.OtherReligionOrBeliefMaxLength, 0);
            Check.Length(otherTypeOfSecondarySchool, nameof(otherTypeOfSecondarySchool), DiversityDataConsts.OtherTypeOfSecondarySchoolMaxLength, 0);
            JobApplicationId = jobApplicationId;
            HappyToCompleteForm = happyToCompleteForm;
            AgeRange = ageRange;
            Gender = gender;
            OtherGender = otherGender;
            GenderIdentitySameAsBirth = genderIdentitySameAsBirth;
            Sex = sex;
            OtherSex = otherSex;
            SexualOrientation = sexualOrientation;
            OtherSexualOrientation = otherSexualOrientation;
            Ethnicity = ethnicity;
            OtherEthnicity = otherEthnicity;
            ReligionOrBelief = religionOrBelief;
            OtherReligionOrBelief = otherReligionOrBelief;
            Disability = disability;
            EducationLevel = educationLevel;
            TypeOfSecondarySchool = typeOfSecondarySchool;
            OtherTypeOfSecondarySchool = otherTypeOfSecondarySchool;
            HigherEducationQualifications = higherEducationQualifications;
        }

    }
}