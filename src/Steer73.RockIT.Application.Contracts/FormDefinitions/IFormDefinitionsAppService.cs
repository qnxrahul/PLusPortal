using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Steer73.RockIT.FormDefinitions
{
    public partial interface IFormDefinitionsAppService : IApplicationService
    {

        Task<PagedResultDto<FormDefinitionWithNavigationPropertiesDto>> GetListAsync(GetFormDefinitionsInput input);

        Task<FormDefinitionDto> GetAsync(Guid id);

        Task DeleteAsync(Guid id);

        Task<FormDefinitionDto> CreateAsync(FormDefinitionCreateDto input);

        Task<FormDefinitionDto> UpdateAsync(Guid id, FormDefinitionUpdateDto input);
    }
}