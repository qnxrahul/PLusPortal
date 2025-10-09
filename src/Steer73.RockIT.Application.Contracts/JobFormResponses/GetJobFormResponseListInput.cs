using Volo.Abp.Application.Dtos;
using System;

namespace Steer73.RockIT.JobFormResponses
{
    public class GetJobFormResponseListInput : PagedAndSortedResultRequestDto
    {
        public Guid JobApplicationId { get; set; }
    }
}