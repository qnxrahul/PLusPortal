using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Steer73.RockIT.PracticeGroups
{
    public partial interface IPracticeGroupsAppService : IApplicationService
    {

        Task<PagedResultDto<PracticeGroupDto>> GetListAsync(GetPracticeGroupsInput input);

        Task<PracticeGroupDto> GetAsync(Guid id);

        Task DeleteAsync(Guid id);

        Task<PracticeGroupDto> CreateAsync(PracticeGroupCreateDto input);

        Task<PracticeGroupDto> UpdateAsync(Guid id, PracticeGroupUpdateDto input);

        /// <summary>
        /// For autocomplete
        /// </summary>
        /// <param name="filter"></param>
        /// <returns>Maximum of 5 practice groups</returns>
        Task<ListResultDto<PracticeGroupAutoCompleteDto>> GetListForAutoCompleteAsync(string filter = "");
    }
}