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
using Steer73.RockIT.DiversityFormResponses;

namespace Steer73.RockIT.DiversityFormResponses
{

    [Authorize(RockITSharedPermissions.DiversityFormResponses.Default)]
    public abstract class DiversityFormResponsesAppServiceBase : RockITAppService
    {

        protected IDiversityFormResponseRepository _diversityFormResponseRepository;
        protected DiversityFormResponseManager _diversityFormResponseManager;

        public DiversityFormResponsesAppServiceBase(IDiversityFormResponseRepository diversityFormResponseRepository, DiversityFormResponseManager diversityFormResponseManager)
        {

            _diversityFormResponseRepository = diversityFormResponseRepository;
            _diversityFormResponseManager = diversityFormResponseManager;

        }

        public virtual async Task<PagedResultDto<DiversityFormResponseDto>> GetListByJobApplicationIdAsync(GetDiversityFormResponseListInput input)
        {
            var diversityFormResponses = await _diversityFormResponseRepository.GetListByJobApplicationIdAsync(
                input.JobApplicationId,
                input.Sorting,
                input.MaxResultCount,
                input.SkipCount);

            return new PagedResultDto<DiversityFormResponseDto>
            {
                TotalCount = await _diversityFormResponseRepository.GetCountByJobApplicationIdAsync(input.JobApplicationId),
                Items = ObjectMapper.Map<List<DiversityFormResponse>, List<DiversityFormResponseDto>>(diversityFormResponses)
            };
        }

        public virtual async Task<PagedResultDto<DiversityFormResponseDto>> GetListAsync(GetDiversityFormResponsesInput input)
        {
            var totalCount = await _diversityFormResponseRepository.GetCountAsync(input.FilterText);
            var items = await _diversityFormResponseRepository.GetListAsync(input.FilterText, input.Sorting, input.MaxResultCount, input.SkipCount);

            return new PagedResultDto<DiversityFormResponseDto>
            {
                TotalCount = totalCount,
                Items = ObjectMapper.Map<List<DiversityFormResponse>, List<DiversityFormResponseDto>>(items)
            };
        }

        public virtual async Task<DiversityFormResponseDto> GetAsync(Guid id)
        {
            return ObjectMapper.Map<DiversityFormResponse, DiversityFormResponseDto>(await _diversityFormResponseRepository.GetAsync(id));
        }

        [Authorize(RockITSharedPermissions.DiversityFormResponses.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            await _diversityFormResponseRepository.DeleteAsync(id);
        }

        [Authorize(RockITSharedPermissions.DiversityFormResponses.Create)]
        public virtual async Task<DiversityFormResponseDto> CreateAsync(DiversityFormResponseCreateDto input)
        {

            var diversityFormResponse = await _diversityFormResponseManager.CreateAsync(input.JobApplicationId
            , input.FormStructureJson, input.FormResponseJson
            );

            return ObjectMapper.Map<DiversityFormResponse, DiversityFormResponseDto>(diversityFormResponse);
        }

        [Authorize(RockITSharedPermissions.DiversityFormResponses.Edit)]
        public virtual async Task<DiversityFormResponseDto> UpdateAsync(Guid id, DiversityFormResponseUpdateDto input)
        {

            var diversityFormResponse = await _diversityFormResponseManager.UpdateAsync(
            id, input.JobApplicationId
            , input.FormStructureJson, input.FormResponseJson
            );

            return ObjectMapper.Map<DiversityFormResponse, DiversityFormResponseDto>(diversityFormResponse);
        }
    }
}