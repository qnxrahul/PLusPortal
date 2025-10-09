using Volo.Abp.Application.Dtos;
using System;

namespace Steer73.RockIT.DiversityDatas
{
    public class GetDiversityDataListInput : PagedAndSortedResultRequestDto
    {
        public Guid JobApplicationId { get; set; }
    }
}