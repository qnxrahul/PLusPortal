using System;
using System.Collections.Generic;

using Volo.Abp.Application.Dtos;

namespace Steer73.RockIT.PracticeAreas
{
    public abstract class PracticeAreaDtoBase : FullAuditedEntityDto<Guid>
    {
        public Guid PracticeGroupId { get; set; }
        public string Name { get; set; } = null!;
        public bool IsActive { get; set; }

    }
}