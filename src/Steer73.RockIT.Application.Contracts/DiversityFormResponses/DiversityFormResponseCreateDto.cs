using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Steer73.RockIT.DiversityFormResponses
{
    public abstract class DiversityFormResponseCreateDtoBase
    {
        public Guid JobApplicationId { get; set; }
        [Required]
        public string FormStructureJson { get; set; } = null!;
        [Required]
        public string FormResponseJson { get; set; } = null!;
    }
}