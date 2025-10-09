using System;
using System.Collections.Generic;
using Steer73.RockIT.PracticeAreas;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace Steer73.RockIT.PracticeGroups
{
    public abstract class PracticeGroupDtoBase : FullAuditedEntityDto<Guid>, IHasConcurrencyStamp
    {
        public string Name { get; set; } = null!;
        public bool IsActive { get; set; }

        public string ConcurrencyStamp { get; set; } = null!;

        public List<PracticeAreaDto> PracticeAreas { get; set; } = new();
    }
}