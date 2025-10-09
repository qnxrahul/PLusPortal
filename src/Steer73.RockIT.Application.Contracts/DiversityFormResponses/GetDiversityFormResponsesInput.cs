using Volo.Abp.Application.Dtos;
using System;

namespace Steer73.RockIT.DiversityFormResponses
{
    public abstract class GetDiversityFormResponsesInputBase : PagedAndSortedResultRequestDto
    {
        public Guid? JobApplicationId { get; set; }

        public string? FilterText { get; set; }

        public GetDiversityFormResponsesInputBase()
        {

        }
    }
}