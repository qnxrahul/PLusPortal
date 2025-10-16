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
using Steer73.RockIT.PracticeAreas;

namespace Steer73.RockIT.PracticeAreas
{

    [AllowAnonymous]
    public abstract class PracticeAreasAppServiceBase : RockITAppService
    {

        protected IPracticeAreaRepository _practiceAreaRepository;
        protected PracticeAreaManager _practiceAreaManager;

        public PracticeAreasAppServiceBase(IPracticeAreaRepository practiceAreaRepository, PracticeAreaManager practiceAreaManager)
        {

            _practiceAreaRepository = practiceAreaRepository;
            _practiceAreaManager = practiceAreaManager;

        }

        public virtual async Task<PagedResultDto<PracticeAreaDto>> GetListByPracticeGroupIdAsync(GetPracticeAreaListInput input)
        {
            var practiceAreas = await _practiceAreaRepository.GetListByPracticeGroupIdAsync(
                input.PracticeGroupId,
                input.Sorting,
                input.MaxResultCount,
                input.SkipCount);

            return new PagedResultDto<PracticeAreaDto>
            {
                TotalCount = await _practiceAreaRepository.GetCountByPracticeGroupIdAsync(input.PracticeGroupId),
                Items = ObjectMapper.Map<List<PracticeArea>, List<PracticeAreaDto>>(practiceAreas)
            };
        }

        public virtual async Task<PagedResultDto<PracticeAreaDto>> GetListAsync(GetPracticeAreasInput input)
        {
            var totalCount = await _practiceAreaRepository.GetCountAsync(input.FilterText, input.Name, input.IsActive);
            var items = await _practiceAreaRepository.GetListAsync(input.FilterText, input.Name, input.IsActive, input.Sorting, input.MaxResultCount, input.SkipCount);

            return new PagedResultDto<PracticeAreaDto>
            {
                TotalCount = totalCount,
                Items = ObjectMapper.Map<List<PracticeArea>, List<PracticeAreaDto>>(items)
            };
        }

        public virtual async Task<PracticeAreaDto> GetAsync(Guid id)
        {
            return ObjectMapper.Map<PracticeArea, PracticeAreaDto>(await _practiceAreaRepository.GetAsync(id));
        }

        [AllowAnonymous]
        public virtual async Task DeleteAsync(Guid id)
        {
            await _practiceAreaRepository.DeleteAsync(id);
        }

        [AllowAnonymous]
        public virtual async Task<PracticeAreaDto> CreateAsync(PracticeAreaCreateDto input)
        {

            var practiceArea = await _practiceAreaManager.CreateAsync(input.PracticeGroupId
            , input.Name, input.IsActive
            );

            return ObjectMapper.Map<PracticeArea, PracticeAreaDto>(practiceArea);
        }

        [AllowAnonymous]
        public virtual async Task<PracticeAreaDto> UpdateAsync(Guid id, PracticeAreaUpdateDto input)
        {

            var practiceArea = await _practiceAreaManager.UpdateAsync(
            id, input.PracticeGroupId
            , input.Name, input.IsActive
            );

            return ObjectMapper.Map<PracticeArea, PracticeAreaDto>(practiceArea);
        }
    }
}