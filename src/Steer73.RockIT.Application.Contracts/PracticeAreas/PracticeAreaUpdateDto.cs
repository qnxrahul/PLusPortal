using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Steer73.RockIT.PracticeAreas
{
    public abstract class PracticeAreaUpdateDtoBase
    {
        public Guid PracticeGroupId { get; set; }
        [Required]
        [StringLength(PracticeAreaConsts.NameMaxLength)]
        public string Name { get; set; } = null!;
        public bool IsActive { get; set; }

    }
}