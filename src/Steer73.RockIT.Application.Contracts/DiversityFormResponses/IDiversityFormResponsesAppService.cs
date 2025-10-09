using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Steer73.RockIT.DiversityFormResponses
{
    public partial interface IDiversityFormResponsesAppService : IApplicationService
    {

        Task<PagedResultDto<DiversityFormResponseDto>> GetListByJobApplicationIdAsync(GetDiversityFormResponseListInput input);

        Task<PagedResultDto<DiversityFormResponseDto>> GetListAsync(GetDiversityFormResponsesInput input);

        Task<DiversityFormResponseDto> GetAsync(Guid id);

        Task DeleteAsync(Guid id);

        Task<DiversityFormResponseDto> CreateAsync(DiversityFormResponseCreateDto input);

        Task<DiversityFormResponseDto> UpdateAsync(Guid id, DiversityFormResponseUpdateDto input);
    }
}