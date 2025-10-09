using Steer73.RockIT.FormDefinitions;
using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Steer73.RockIT.FormDefinitions
{
    public abstract class FormDefinitionCreateDtoBase
    {
        [Required]
        [StringLength(FormDefinitionConsts.ReferenceIdMaxLength)]
        public string ReferenceId { get; set; } = null!;
        [Required]
        [StringLength(FormDefinitionConsts.NameMaxLength)]
        public string Name { get; set; } = null!;
        public string? FormDetails { get; set; }
        public FormType FormType { get; set; } = ((FormType[])Enum.GetValues(typeof(FormType)))[0];
        [Required]
        public Guid CompanyId { get; set; }
    }
}