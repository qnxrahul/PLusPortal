using Microsoft.AspNetCore.Authorization;
using Steer73.RockIT.Permissions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace Steer73.RockIT.PracticeGroups
{
    public abstract class PracticeGroupsAppServiceBase : RockITAppService
    {

        protected IPracticeGroupRepository _practiceGroupRepository;
        protected PracticeGroupManager _practiceGroupManager;

        public PracticeGroupsAppServiceBase(IPracticeGroupRepository practiceGroupRepository, PracticeGroupManager practiceGroupManager)
        {

            _practiceGroupRepository = practiceGroupRepository;
            _practiceGroupManager = practiceGroupManager;

        }

        [AllowAnonymous]
        public virtual async Task<PagedResultDto<PracticeGroupDto>> GetListAsync(GetPracticeGroupsInput input)
        {
            var totalCount = await _practiceGroupRepository.GetCountAsync(input.FilterText, input.Name, input.IsActive);
            var items = await _practiceGroupRepository.GetListAsync(input.FilterText, input.Name, input.IsActive, input.Sorting, input.MaxResultCount, input.SkipCount);

            return new PagedResultDto<PracticeGroupDto>
            {
                TotalCount = totalCount,
                Items = ObjectMapper.Map<List<PracticeGroup>, List<PracticeGroupDto>>(items)
            };
        }

        [Authorize(RockITSharedPermissions.PracticeGroups.Default)]
        public virtual async Task<PracticeGroupDto> GetAsync(Guid id)
        {
            return ObjectMapper.Map<PracticeGroup, PracticeGroupDto>(await _practiceGroupRepository.GetAsync(id));
        }

        [AllowAnonymous]
        public virtual async Task DeleteAsync(Guid id)
        {
            await _practiceGroupRepository.DeleteAsync(id);
        }

        [AllowAnonymous]
        public virtual async Task<PracticeGroupDto> CreateAsync(PracticeGroupCreateDto input)
        {

            var practiceGroup = await _practiceGroupManager.CreateAsync(
            input.Name, input.IsActive
            );

            return ObjectMapper.Map<PracticeGroup, PracticeGroupDto>(practiceGroup);
        }

        [AllowAnonymous]
        public virtual async Task<PracticeGroupDto> UpdateAsync(Guid id, PracticeGroupUpdateDto input)
        {

            var practiceGroup = await _practiceGroupManager.UpdateAsync(
            id,
            input.Name, input.IsActive, input.ConcurrencyStamp
            );

            return ObjectMapper.Map<PracticeGroup, PracticeGroupDto>(practiceGroup);
        }

        [AllowAnonymous]
        public virtual async Task<ListResultDto<PracticeGroupAutoCompleteDto>> GetListForAutoCompleteAsync(string filter = "")
        {
            var practiceGroups = await _practiceGroupRepository.GetListAsync(
                filter, 
                isActive: true,
                maxResultCount: 15);

            var dtos = practiceGroups.Select(pg => new PracticeGroupAutoCompleteDto
            {
                Id = pg.Id,
                Name = pg.Name
            }).ToList();

            return new ListResultDto<PracticeGroupAutoCompleteDto>(dtos);
        } 
    }
}