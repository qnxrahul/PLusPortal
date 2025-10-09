using Steer73.RockIT.Enums;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Steer73.RockIT.DiversityDatas
{
    public partial interface IDiversityDataRepository : IRepository<DiversityData, Guid>
    {
        Task<List<DiversityData>> GetListByJobApplicationIdAsync(
    Guid jobApplicationId,
    string? sorting = null,
    int maxResultCount = int.MaxValue,
    int skipCount = 0,
    CancellationToken cancellationToken = default
);

        Task<long> GetCountByJobApplicationIdAsync(Guid jobApplicationId, CancellationToken cancellationToken = default);

        Task<List<DiversityData>> GetListAsync(
                    string? filterText = null,
                    YesNo? happyToCompleteForm = null,
                    AgeRange? ageRange = null,
                    GenderOrSex? gender = null,
                    string? otherGender = null,
                    YesNoPreferNotToSay? genderIdentitySameAsBirth = null,
                    GenderOrSex? sex = null,
                    string? otherSex = null,
                    SexualOrientation? sexualOrientation = null,
                    string? otherSexualOrientation = null,
                    Ethnicity? ethnicity = null,
                    string? otherEthnicity = null,
                    Religion? religionOrBelief = null,
                    string? otherReligionOrBelief = null,
                    YesNoPreferNotToSay? disability = null,
                    EducationLevel? educationLevel = null,
                    TypeOfSecondarySchool? typeOfSecondarySchool = null,
                    string? otherTypeOfSecondarySchool = null,
                    YesNoPreferNotToSayDontKnow? higherEducationQualifications = null,
                    string? sorting = null,
                    int maxResultCount = int.MaxValue,
                    int skipCount = 0,
                    CancellationToken cancellationToken = default
                );

        Task<long> GetCountAsync(
            string? filterText = null,
            YesNo? happyToCompleteForm = null,
            AgeRange? ageRange = null,
            GenderOrSex? gender = null,
            string? otherGender = null,
            YesNoPreferNotToSay? genderIdentitySameAsBirth = null,
            GenderOrSex? sex = null,
            string? otherSex = null,
            SexualOrientation? sexualOrientation = null,
            string? otherSexualOrientation = null,
            Ethnicity? ethnicity = null,
            string? otherEthnicity = null,
            Religion? religionOrBelief = null,
            string? otherReligionOrBelief = null,
            YesNoPreferNotToSay? disability = null,
            EducationLevel? educationLevel = null,
            TypeOfSecondarySchool? typeOfSecondarySchool = null,
            string? otherTypeOfSecondarySchool = null,
            YesNoPreferNotToSayDontKnow? higherEducationQualifications = null,
            CancellationToken cancellationToken = default);
    }
}