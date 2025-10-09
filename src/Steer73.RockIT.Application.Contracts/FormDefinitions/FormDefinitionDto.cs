using Steer73.RockIT.FormDefinitions;
using System;
using System.Collections.Generic;

using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace Steer73.RockIT.FormDefinitions
{
    public abstract class FormDefinitionDtoBase : FullAuditedEntityDto<Guid>, IHasConcurrencyStamp
    {
        public string ReferenceId { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? FormDetails { get; set; }
        public FormType FormType { get; set; }
        public Guid CompanyId { get; set; }

        public string ConcurrencyStamp { get; set; } = null!;

    }
}