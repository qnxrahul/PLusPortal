using Volo.Abp.Application.Dtos;
using System;

namespace Steer73.RockIT.DiversityFormResponses
{
    public class GetDiversityFormResponseListInput : PagedAndSortedResultRequestDto
    {
        public Guid JobApplicationId { get; set; }
    }
}