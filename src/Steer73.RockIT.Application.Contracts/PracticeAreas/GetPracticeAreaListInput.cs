using Volo.Abp.Application.Dtos;
using System;

namespace Steer73.RockIT.PracticeAreas
{
    public class GetPracticeAreaListInput : PagedAndSortedResultRequestDto
    {
        public Guid PracticeGroupId { get; set; }
    }
}