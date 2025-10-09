using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Steer73.RockIT.JobFormResponses
{
    public partial interface IJobFormResponsesAppService : IApplicationService
    {

        Task<PagedResultDto<JobFormResponseDto>> GetListByJobApplicationIdAsync(GetJobFormResponseListInput input);

        Task<PagedResultDto<JobFormResponseDto>> GetListAsync(GetJobFormResponsesInput input);

        Task<JobFormResponseDto> GetAsync(Guid id);

        Task DeleteAsync(Guid id);

        Task<JobFormResponseDto> CreateAsync(JobFormResponseCreateDto input);

        Task<JobFormResponseDto> UpdateAsync(Guid id, JobFormResponseUpdateDto input);
    }
}