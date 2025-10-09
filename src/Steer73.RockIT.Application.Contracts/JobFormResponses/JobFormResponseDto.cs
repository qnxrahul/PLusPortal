using System;
using System.Collections.Generic;

using Volo.Abp.Application.Dtos;

namespace Steer73.RockIT.JobFormResponses
{
    public abstract class JobFormResponseDtoBase : FullAuditedEntityDto<Guid>
    {
        public Guid JobApplicationId { get; set; }
        public string FormStructureJson { get; set; } = null!;
        public string FormResponseJson { get; set; } = null!;

    }
}