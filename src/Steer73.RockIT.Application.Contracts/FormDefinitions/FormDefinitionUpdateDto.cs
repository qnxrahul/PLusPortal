using Steer73.RockIT.FormDefinitions;
using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities;
using System.ComponentModel;

namespace Steer73.RockIT.FormDefinitions
{
    public abstract class FormDefinitionUpdateDtoBase : IHasConcurrencyStamp
    {
        [Required]
        [StringLength(FormDefinitionConsts.ReferenceIdMaxLength)]
        public string ReferenceId { get; set; } = null!;
        [Required]
        [StringLength(FormDefinitionConsts.NameMaxLength)]
        public string Name { get; set; } = null!;
        public string? FormDetails { get; set; }
        public FormType FormType { get; set; }

        public string ConcurrencyStamp { get; set; } = null!;
        [DisplayName("Company")]
        [Required]
        public Guid CompanyId { get; set; }
    }
}