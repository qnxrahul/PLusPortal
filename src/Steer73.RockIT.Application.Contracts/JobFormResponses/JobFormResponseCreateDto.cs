using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Steer73.RockIT.JobFormResponses
{
    public abstract class JobFormResponseCreateDtoBase
    {
        public Guid JobApplicationId { get; set; }
        [Required]
        public string FormStructureJson { get; set; } = null!;
        [Required]
        public string FormResponseJson { get; set; } = null!;
    }
}