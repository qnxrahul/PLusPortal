using Steer73.RockIT.MediaSources;
using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace Steer73.RockIT.Vacancies
{
    public abstract class VacancyMediaSourceBase : FullAuditedAggregateRoot<Guid>, ISoftDelete
    {
        public Guid VacancyId { get; set; }
        public Vacancy Vacancy { get; set; }

        public Guid MediaSourceId { get; set; }
        public MediaSource MediaSource { get; set; }

        protected VacancyMediaSourceBase()
        {

        }

        public VacancyMediaSourceBase(Guid id, Guid vacancyId, Guid mediaSourceId)
        {
            Id = id;
            VacancyId = vacancyId;
            MediaSourceId = mediaSourceId;
        }
    }
}
