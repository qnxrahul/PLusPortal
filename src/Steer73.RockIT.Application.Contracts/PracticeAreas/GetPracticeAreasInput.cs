using Volo.Abp.Application.Dtos;
using System;

namespace Steer73.RockIT.PracticeAreas
{
    public abstract class GetPracticeAreasInputBase : PagedAndSortedResultRequestDto
    {
        public Guid? PracticeGroupId { get; set; }

        public string? FilterText { get; set; }

        public string? Name { get; set; }
        public bool? IsActive { get; set; }

        public GetPracticeAreasInputBase()
        {

        }
    }
}