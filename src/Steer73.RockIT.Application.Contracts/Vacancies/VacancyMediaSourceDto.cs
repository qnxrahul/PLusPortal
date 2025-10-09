using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace Steer73.RockIT.Vacancies
{
    public abstract class VacancyMediaSourceDtoBase : FullAuditedEntityDto<Guid>, IHasConcurrencyStamp
    {
        public Guid MediaSourceId { get; set; }
        public Guid VacancyId { get; set; }

        public string ConcurrencyStamp { get; set; } = null!;
    }
}
