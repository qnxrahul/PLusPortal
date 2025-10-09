using Steer73.RockIT.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Steer73.RockIT.EntityFrameworkCore;

namespace Steer73.RockIT.DiversityDatas
{
    public abstract class EfCoreDiversityDataRepositoryBase : EfCoreRepository<RockITDbContext, DiversityData, Guid>
    {
        public EfCoreDiversityDataRepositoryBase(IDbContextProvider<RockITDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        public virtual async Task<List<DiversityData>> GetListByJobApplicationIdAsync(
           Guid jobApplicationId,
           string? sorting = null,
           int maxResultCount = int.MaxValue,
           int skipCount = 0,
           CancellationToken cancellationToken = default)
        {
            var query = (await GetQueryableAsync()).Where(x => x.JobApplicationId == jobApplicationId);
            query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? DiversityDataConsts.GetDefaultSorting(false) : sorting);
            return await query.PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken);
        }

        public virtual async Task<long> GetCountByJobApplicationIdAsync(Guid jobApplicationId, CancellationToken cancellationToken = default)
        {
            return await (await GetQueryableAsync()).Where(x => x.JobApplicationId == jobApplicationId).CountAsync(cancellationToken);
        }

        public virtual async Task<List<DiversityData>> GetListAsync(
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
            CancellationToken cancellationToken = default)
        {
            var query = ApplyFilter((await GetQueryableAsync()), filterText, happyToCompleteForm, ageRange, gender, otherGender, genderIdentitySameAsBirth, sex, otherSex, sexualOrientation, otherSexualOrientation, ethnicity, otherEthnicity, religionOrBelief, otherReligionOrBelief, disability, educationLevel, typeOfSecondarySchool, otherTypeOfSecondarySchool, higherEducationQualifications);
            query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? DiversityDataConsts.GetDefaultSorting(false) : sorting);
            return await query.PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken);
        }

        public virtual async Task<long> GetCountAsync(
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
            CancellationToken cancellationToken = default)
        {
            var query = ApplyFilter((await GetDbSetAsync()), filterText, happyToCompleteForm, ageRange, gender, otherGender, genderIdentitySameAsBirth, sex, otherSex, sexualOrientation, otherSexualOrientation, ethnicity, otherEthnicity, religionOrBelief, otherReligionOrBelief, disability, educationLevel, typeOfSecondarySchool, otherTypeOfSecondarySchool, higherEducationQualifications);
            return await query.LongCountAsync(GetCancellationToken(cancellationToken));
        }

        protected virtual IQueryable<DiversityData> ApplyFilter(
            IQueryable<DiversityData> query,
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
            YesNoPreferNotToSayDontKnow? higherEducationQualifications = null)
        {
            return query
                    .WhereIf(!string.IsNullOrWhiteSpace(filterText), e => e.OtherGender!.Contains(filterText!) || e.OtherSex!.Contains(filterText!) || e.OtherSexualOrientation!.Contains(filterText!) || e.OtherEthnicity!.Contains(filterText!) || e.OtherReligionOrBelief!.Contains(filterText!) || e.OtherTypeOfSecondarySchool!.Contains(filterText!))
                    .WhereIf(happyToCompleteForm.HasValue, e => e.HappyToCompleteForm == happyToCompleteForm)
                    .WhereIf(ageRange.HasValue, e => e.AgeRange == ageRange)
                    .WhereIf(gender.HasValue, e => e.Gender == gender)
                    .WhereIf(!string.IsNullOrWhiteSpace(otherGender), e => e.OtherGender.Contains(otherGender))
                    .WhereIf(genderIdentitySameAsBirth.HasValue, e => e.GenderIdentitySameAsBirth == genderIdentitySameAsBirth)
                    .WhereIf(sex.HasValue, e => e.Sex == sex)
                    .WhereIf(!string.IsNullOrWhiteSpace(otherSex), e => e.OtherSex.Contains(otherSex))
                    .WhereIf(sexualOrientation.HasValue, e => e.SexualOrientation == sexualOrientation)
                    .WhereIf(!string.IsNullOrWhiteSpace(otherSexualOrientation), e => e.OtherSexualOrientation.Contains(otherSexualOrientation))
                    .WhereIf(ethnicity.HasValue, e => e.Ethnicity == ethnicity)
                    .WhereIf(!string.IsNullOrWhiteSpace(otherEthnicity), e => e.OtherEthnicity.Contains(otherEthnicity))
                    .WhereIf(religionOrBelief.HasValue, e => e.ReligionOrBelief == religionOrBelief)
                    .WhereIf(!string.IsNullOrWhiteSpace(otherReligionOrBelief), e => e.OtherReligionOrBelief.Contains(otherReligionOrBelief))
                    .WhereIf(disability.HasValue, e => e.Disability == disability)
                    .WhereIf(educationLevel.HasValue, e => e.EducationLevel == educationLevel)
                    .WhereIf(typeOfSecondarySchool.HasValue, e => e.TypeOfSecondarySchool == typeOfSecondarySchool)
                    .WhereIf(!string.IsNullOrWhiteSpace(otherTypeOfSecondarySchool), e => e.OtherTypeOfSecondarySchool.Contains(otherTypeOfSecondarySchool))
                    .WhereIf(higherEducationQualifications.HasValue, e => e.HigherEducationQualifications == higherEducationQualifications);
        }
    }
}