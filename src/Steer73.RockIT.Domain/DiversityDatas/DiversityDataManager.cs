using Steer73.RockIT.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;

namespace Steer73.RockIT.DiversityDatas
{
    public abstract class DiversityDataManagerBase : DomainService
    {
        protected IDiversityDataRepository _diversityDataRepository;

        public DiversityDataManagerBase(IDiversityDataRepository diversityDataRepository)
        {
            _diversityDataRepository = diversityDataRepository;
        }

        public virtual async Task<DiversityData> CreateAsync(
        Guid jobApplicationId, YesNo? happyToCompleteForm = null, AgeRange? ageRange = null, GenderOrSex? gender = null, string? otherGender = null, YesNoPreferNotToSay? genderIdentitySameAsBirth = null, GenderOrSex? sex = null, string? otherSex = null, SexualOrientation? sexualOrientation = null, string? otherSexualOrientation = null, Ethnicity? ethnicity = null, string? otherEthnicity = null, Religion? religionOrBelief = null, string? otherReligionOrBelief = null, YesNoPreferNotToSay? disability = null, EducationLevel? educationLevel = null, TypeOfSecondarySchool? typeOfSecondarySchool = null, string? otherTypeOfSecondarySchool = null, YesNoPreferNotToSayDontKnow? higherEducationQualifications = null)
        {
            Check.Length(otherGender, nameof(otherGender), DiversityDataConsts.OtherGenderMaxLength);
            Check.Length(otherSex, nameof(otherSex), DiversityDataConsts.OtherSexMaxLength);
            Check.Length(otherSexualOrientation, nameof(otherSexualOrientation), DiversityDataConsts.OtherSexualOrientationMaxLength);
            Check.Length(otherEthnicity, nameof(otherEthnicity), DiversityDataConsts.OtherEthnicityMaxLength);
            Check.Length(otherReligionOrBelief, nameof(otherReligionOrBelief), DiversityDataConsts.OtherReligionOrBeliefMaxLength);
            Check.Length(otherTypeOfSecondarySchool, nameof(otherTypeOfSecondarySchool), DiversityDataConsts.OtherTypeOfSecondarySchoolMaxLength);

            var diversityData = new DiversityData(
             GuidGenerator.Create(),
             jobApplicationId, happyToCompleteForm, ageRange, gender, otherGender, genderIdentitySameAsBirth, sex, otherSex, sexualOrientation, otherSexualOrientation, ethnicity, otherEthnicity, religionOrBelief, otherReligionOrBelief, disability, educationLevel, typeOfSecondarySchool, otherTypeOfSecondarySchool, higherEducationQualifications
             );

            return await _diversityDataRepository.InsertAsync(diversityData);
        }

        public virtual async Task<DiversityData> UpdateAsync(
            Guid id,
            Guid jobApplicationId, YesNo? happyToCompleteForm = null, AgeRange? ageRange = null, GenderOrSex? gender = null, string? otherGender = null, YesNoPreferNotToSay? genderIdentitySameAsBirth = null, GenderOrSex? sex = null, string? otherSex = null, SexualOrientation? sexualOrientation = null, string? otherSexualOrientation = null, Ethnicity? ethnicity = null, string? otherEthnicity = null, Religion? religionOrBelief = null, string? otherReligionOrBelief = null, YesNoPreferNotToSay? disability = null, EducationLevel? educationLevel = null, TypeOfSecondarySchool? typeOfSecondarySchool = null, string? otherTypeOfSecondarySchool = null, YesNoPreferNotToSayDontKnow? higherEducationQualifications = null
        )
        {
            Check.Length(otherGender, nameof(otherGender), DiversityDataConsts.OtherGenderMaxLength);
            Check.Length(otherSex, nameof(otherSex), DiversityDataConsts.OtherSexMaxLength);
            Check.Length(otherSexualOrientation, nameof(otherSexualOrientation), DiversityDataConsts.OtherSexualOrientationMaxLength);
            Check.Length(otherEthnicity, nameof(otherEthnicity), DiversityDataConsts.OtherEthnicityMaxLength);
            Check.Length(otherReligionOrBelief, nameof(otherReligionOrBelief), DiversityDataConsts.OtherReligionOrBeliefMaxLength);
            Check.Length(otherTypeOfSecondarySchool, nameof(otherTypeOfSecondarySchool), DiversityDataConsts.OtherTypeOfSecondarySchoolMaxLength);

            var diversityData = await _diversityDataRepository.GetAsync(id);

            diversityData.JobApplicationId = jobApplicationId;
            diversityData.HappyToCompleteForm = happyToCompleteForm;
            diversityData.AgeRange = ageRange;
            diversityData.Gender = gender;
            diversityData.OtherGender = otherGender;
            diversityData.GenderIdentitySameAsBirth = genderIdentitySameAsBirth;
            diversityData.Sex = sex;
            diversityData.OtherSex = otherSex;
            diversityData.SexualOrientation = sexualOrientation;
            diversityData.OtherSexualOrientation = otherSexualOrientation;
            diversityData.Ethnicity = ethnicity;
            diversityData.OtherEthnicity = otherEthnicity;
            diversityData.ReligionOrBelief = religionOrBelief;
            diversityData.OtherReligionOrBelief = otherReligionOrBelief;
            diversityData.Disability = disability;
            diversityData.EducationLevel = educationLevel;
            diversityData.TypeOfSecondarySchool = typeOfSecondarySchool;
            diversityData.OtherTypeOfSecondarySchool = otherTypeOfSecondarySchool;
            diversityData.HigherEducationQualifications = higherEducationQualifications;

            return await _diversityDataRepository.UpdateAsync(diversityData);
        }

    }
}