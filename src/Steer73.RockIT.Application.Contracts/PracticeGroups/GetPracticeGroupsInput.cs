using Volo.Abp.Application.Dtos;
using System;

namespace Steer73.RockIT.PracticeGroups
{
    public abstract class GetPracticeGroupsInputBase : PagedAndSortedResultRequestDto
    {

        public string? FilterText { get; set; }

        public string? Name { get; set; }
        public bool? IsActive { get; set; }

        public GetPracticeGroupsInputBase()
        {

        }
    }
}