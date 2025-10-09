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
using Steer73.RockIT.FormDefinitions;

namespace Steer73.RockIT.FormDefinitions
{

    [Authorize(RockITSharedPermissions.FormDefinitions.Default)]
    public abstract class FormDefinitionsAppServiceBase : RockITAppService
    {

        protected IFormDefinitionRepository _formDefinitionRepository;
        protected FormDefinitionManager _formDefinitionManager;

        public FormDefinitionsAppServiceBase(IFormDefinitionRepository formDefinitionRepository, FormDefinitionManager formDefinitionManager)
        {

            _formDefinitionRepository = formDefinitionRepository;
            _formDefinitionManager = formDefinitionManager;

        }

        public virtual async Task<PagedResultDto<FormDefinitionWithNavigationPropertiesDto>> GetListAsync(GetFormDefinitionsInput input)
        {
            var totalCount = await _formDefinitionRepository.GetCountAsync(input.FilterText, input.ReferenceId, input.Name, input.FormDetails, input.FormType, input.CompanyId);
            var items = await _formDefinitionRepository.GetListWithNavigationPropertiesAsync(input.FilterText, input.ReferenceId, input.Name, input.FormDetails, input.FormType, input.CompanyId, input.Sorting, input.MaxResultCount, input.SkipCount);

            return new PagedResultDto<FormDefinitionWithNavigationPropertiesDto>
            {
                TotalCount = totalCount,
                Items = ObjectMapper.Map<List<FormDefinitionWithNavigationProperties>, List<FormDefinitionWithNavigationPropertiesDto>>(items)
            };
        }

        public virtual async Task<FormDefinitionDto> GetAsync(Guid id)
        {
            return ObjectMapper.Map<FormDefinition, FormDefinitionDto>(await _formDefinitionRepository.GetAsync(id));
        }

        [Authorize(RockITSharedPermissions.FormDefinitions.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            await _formDefinitionRepository.DeleteAsync(id);
        }

        [Authorize(RockITSharedPermissions.FormDefinitions.Create)]
        public virtual async Task<FormDefinitionDto> CreateAsync(FormDefinitionCreateDto input)
        {

            var formDefinition = await _formDefinitionManager.CreateAsync(
            input.ReferenceId, input.Name, input.FormType, input.CompanyId, input.FormDetails
            );

            return ObjectMapper.Map<FormDefinition, FormDefinitionDto>(formDefinition);
        }

        [Authorize(RockITSharedPermissions.FormDefinitions.Edit)]
        public virtual async Task<FormDefinitionDto> UpdateAsync(Guid id, FormDefinitionUpdateDto input)
        {

            var formDefinition = await _formDefinitionManager.UpdateAsync(
            id,
            input.ReferenceId, input.Name, input.FormType, input.CompanyId, input.FormDetails, input.ConcurrencyStamp
            );

            return ObjectMapper.Map<FormDefinition, FormDefinitionDto>(formDefinition);
        }
    }
}