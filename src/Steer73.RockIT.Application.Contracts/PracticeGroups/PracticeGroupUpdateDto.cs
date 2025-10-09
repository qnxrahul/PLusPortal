using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities;

namespace Steer73.RockIT.PracticeGroups
{
    public abstract class PracticeGroupUpdateDtoBase : IHasConcurrencyStamp
    {
        [Required]
        [StringLength(PracticeGroupConsts.NameMaxLength)]
        public string Name { get; set; } = null!;
        public bool IsActive { get; set; }

        public string ConcurrencyStamp { get; set; } = null!;
    }
}