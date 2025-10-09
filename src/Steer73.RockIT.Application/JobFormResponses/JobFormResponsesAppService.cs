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
using Steer73.RockIT.JobFormResponses;

namespace Steer73.RockIT.JobFormResponses
{

    [AllowAnonymous]
    public abstract class JobFormResponsesAppServiceBase : RockITAppService
    {

        protected IJobFormResponseRepository _jobFormResponseRepository;
        protected JobFormResponseManager _jobFormResponseManager;

        public JobFormResponsesAppServiceBase(IJobFormResponseRepository jobFormResponseRepository, JobFormResponseManager jobFormResponseManager)
        {

            _jobFormResponseRepository = jobFormResponseRepository;
            _jobFormResponseManager = jobFormResponseManager;

        }

        public virtual async Task<PagedResultDto<JobFormResponseDto>> GetListByJobApplicationIdAsync(GetJobFormResponseListInput input)
        {
            var jobFormResponses = await _jobFormResponseRepository.GetListByJobApplicationIdAsync(
                input.JobApplicationId,
                input.Sorting,
                input.MaxResultCount,
                input.SkipCount);

            return new PagedResultDto<JobFormResponseDto>
            {
                TotalCount = await _jobFormResponseRepository.GetCountByJobApplicationIdAsync(input.JobApplicationId),
                Items = ObjectMapper.Map<List<JobFormResponse>, List<JobFormResponseDto>>(jobFormResponses)
            };
        }

        public virtual async Task<PagedResultDto<JobFormResponseDto>> GetListAsync(GetJobFormResponsesInput input)
        {
            var totalCount = await _jobFormResponseRepository.GetCountAsync(input.FilterText);
            var items = await _jobFormResponseRepository.GetListAsync(input.FilterText, input.Sorting, input.MaxResultCount, input.SkipCount);

            return new PagedResultDto<JobFormResponseDto>
            {
                TotalCount = totalCount,
                Items = ObjectMapper.Map<List<JobFormResponse>, List<JobFormResponseDto>>(items)
            };
        }

        public virtual async Task<JobFormResponseDto> GetAsync(Guid id)
        {
            return ObjectMapper.Map<JobFormResponse, JobFormResponseDto>(await _jobFormResponseRepository.GetAsync(id));
        }

        [AllowAnonymous]
        public virtual async Task DeleteAsync(Guid id)
        {
            await _jobFormResponseRepository.DeleteAsync(id);
        }

        [AllowAnonymous]
        public virtual async Task<JobFormResponseDto> CreateAsync(JobFormResponseCreateDto input)
        {

            var jobFormResponse = await _jobFormResponseManager.CreateAsync(input.JobApplicationId
            , input.FormStructureJson, input.FormResponseJson
            );

            return ObjectMapper.Map<JobFormResponse, JobFormResponseDto>(jobFormResponse);
        }

        [AllowAnonymous]
        public virtual async Task<JobFormResponseDto> UpdateAsync(Guid id, JobFormResponseUpdateDto input)
        {

            var jobFormResponse = await _jobFormResponseManager.UpdateAsync(
            id, input.JobApplicationId
            , input.FormStructureJson, input.FormResponseJson
            );

            return ObjectMapper.Map<JobFormResponse, JobFormResponseDto>(jobFormResponse);
        }
    }
}