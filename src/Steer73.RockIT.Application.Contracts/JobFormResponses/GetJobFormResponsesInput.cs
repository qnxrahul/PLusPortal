using Volo.Abp.Application.Dtos;
using System;

namespace Steer73.RockIT.JobFormResponses
{
    public abstract class GetJobFormResponsesInputBase : PagedAndSortedResultRequestDto
    {
        public Guid? JobApplicationId { get; set; }

        public string? FilterText { get; set; }

        public GetJobFormResponsesInputBase()
        {

        }
    }
}