using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Steer73.RockIT.Permissions;
using Steer73.RockIT.DiversityDatas;

namespace Steer73.RockIT.DiversityDatas
{

    [Authorize(RockITSharedPermissions.DiversityDatas.Default)]
    public abstract class DiversityDatasAppServiceBase : RockITAppService
    {

        protected IDiversityDataRepository _diversityDataRepository;
        protected DiversityDataManager _diversityDataManager;

        public DiversityDatasAppServiceBase(IDiversityDataRepository diversityDataRepository, DiversityDataManager diversityDataManager)
        {

            _diversityDataRepository = diversityDataRepository;
            _diversityDataManager = diversityDataManager;

        }

        public virtual async Task<PagedResultDto<DiversityDataDto>> GetListByJobApplicationIdAsync(GetDiversityDataListInput input)
        {
            var diversityDatas = await _diversityDataRepository.GetListByJobApplicationIdAsync(
                input.JobApplicationId,
                input.Sorting,
                input.MaxResultCount,
                input.SkipCount);

            return new PagedResultDto<DiversityDataDto>
            {
                TotalCount = await _diversityDataRepository.GetCountByJobApplicationIdAsync(input.JobApplicationId),
                Items = ObjectMapper.Map<List<DiversityData>, List<DiversityDataDto>>(diversityDatas)
            };
        }

        public virtual async Task<PagedResultDto<DiversityDataDto>> GetListAsync(GetDiversityDatasInput input)
        {
            var totalCount = await _diversityDataRepository.GetCountAsync(input.FilterText, input.HappyToCompleteForm, input.AgeRange, input.Gender, input.OtherGender, input.GenderIdentitySameAsBirth, input.Sex, input.OtherSex, input.SexualOrientation, input.OtherSexualOrientation, input.Ethnicity, input.OtherEthnicity, input.ReligionOrBelief, input.OtherReligionOrBelief, input.Disability, input.EducationLevel, input.TypeOfSecondarySchool, input.OtherTypeOfSecondarySchool, input.HigherEducationQualifications);
            var items = await _diversityDataRepository.GetListAsync(input.FilterText, input.HappyToCompleteForm, input.AgeRange, input.Gender, input.OtherGender, input.GenderIdentitySameAsBirth, input.Sex, input.OtherSex, input.SexualOrientation, input.OtherSexualOrientation, input.Ethnicity, input.OtherEthnicity, input.ReligionOrBelief, input.OtherReligionOrBelief, input.Disability, input.EducationLevel, input.TypeOfSecondarySchool, input.OtherTypeOfSecondarySchool, input.HigherEducationQualifications, input.Sorting, input.MaxResultCount, input.SkipCount);

            return new PagedResultDto<DiversityDataDto>
            {
                TotalCount = totalCount,
                Items = ObjectMapper.Map<List<DiversityData>, List<DiversityDataDto>>(items)
            };
        }

        public virtual async Task<DiversityDataDto> GetAsync(Guid id)
        {
            return ObjectMapper.Map<DiversityData, DiversityDataDto>(await _diversityDataRepository.GetAsync(id));
        }

        [Authorize(RockITSharedPermissions.DiversityDatas.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            await _diversityDataRepository.DeleteAsync(id);
        }

        [Authorize(RockITSharedPermissions.DiversityDatas.Create)]
        public virtual async Task<DiversityDataDto> CreateAsync(DiversityDataCreateDto input)
        {

            var diversityData = await _diversityDataManager.CreateAsync(input.JobApplicationId
            , input.HappyToCompleteForm, input.AgeRange, input.Gender, input.OtherGender, input.GenderIdentitySameAsBirth, input.Sex, input.OtherSex, input.SexualOrientation, input.OtherSexualOrientation, input.Ethnicity, input.OtherEthnicity, input.ReligionOrBelief, input.OtherReligionOrBelief, input.Disability, input.EducationLevel, input.TypeOfSecondarySchool, input.OtherTypeOfSecondarySchool, input.HigherEducationQualifications
            );

            return ObjectMapper.Map<DiversityData, DiversityDataDto>(diversityData);
        }

        [Authorize(RockITSharedPermissions.DiversityDatas.Edit)]
        public virtual async Task<DiversityDataDto> UpdateAsync(Guid id, DiversityDataUpdateDto input)
        {

            var diversityData = await _diversityDataManager.UpdateAsync(
            id, input.JobApplicationId
            , input.HappyToCompleteForm, input.AgeRange, input.Gender, input.OtherGender, input.GenderIdentitySameAsBirth, input.Sex, input.OtherSex, input.SexualOrientation, input.OtherSexualOrientation, input.Ethnicity, input.OtherEthnicity, input.ReligionOrBelief, input.OtherReligionOrBelief, input.Disability, input.EducationLevel, input.TypeOfSecondarySchool, input.OtherTypeOfSecondarySchool, input.HigherEducationQualifications
            );

            return ObjectMapper.Map<DiversityData, DiversityDataDto>(diversityData);
        }
    }
}