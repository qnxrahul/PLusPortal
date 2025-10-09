using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Steer73.RockIT.PracticeAreas
{
    public partial interface IPracticeAreasAppService : IApplicationService
    {

        Task<PagedResultDto<PracticeAreaDto>> GetListByPracticeGroupIdAsync(GetPracticeAreaListInput input);

        Task<PagedResultDto<PracticeAreaDto>> GetListAsync(GetPracticeAreasInput input);

        Task<PracticeAreaDto> GetAsync(Guid id);

        Task DeleteAsync(Guid id);

        Task<PracticeAreaDto> CreateAsync(PracticeAreaCreateDto input);

        Task<PracticeAreaDto> UpdateAsync(Guid id, PracticeAreaUpdateDto input);
    }
}