using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Steer73.RockIT.DiversityDatas
{
    public partial interface IDiversityDatasAppService : IApplicationService
    {

        Task<PagedResultDto<DiversityDataDto>> GetListByJobApplicationIdAsync(GetDiversityDataListInput input);

        Task<PagedResultDto<DiversityDataDto>> GetListAsync(GetDiversityDatasInput input);

        Task<DiversityDataDto> GetAsync(Guid id);

        Task DeleteAsync(Guid id);

        Task<DiversityDataDto> CreateAsync(DiversityDataCreateDto input);

        Task<DiversityDataDto> UpdateAsync(Guid id, DiversityDataUpdateDto input);
    }
}