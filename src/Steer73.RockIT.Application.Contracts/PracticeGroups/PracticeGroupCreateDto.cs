using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Steer73.RockIT.PracticeGroups
{
    public abstract class PracticeGroupCreateDtoBase
    {
        [Required]
        [StringLength(PracticeGroupConsts.NameMaxLength)]
        public string Name { get; set; } = null!;
        public bool IsActive { get; set; }
    }
}