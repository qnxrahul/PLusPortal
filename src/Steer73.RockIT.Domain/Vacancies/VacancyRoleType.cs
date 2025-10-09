using Steer73.RockIT.RoleTypes;
using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace Steer73.RockIT.Vacancies
{
    public abstract class VacancyRoleTypeBase : FullAuditedAggregateRoot<Guid>, ISoftDelete
    {
        public Guid VacancyId { get; set; }
        public Vacancy Vacancy { get; set; }

        public Guid RoleTypeId { get; set; }
        public RoleType RoleType { get; set; }

        protected VacancyRoleTypeBase()
        {

        }

        public VacancyRoleTypeBase(Guid id, Guid vacancyId, Guid roleTypeId)
        {
            Id = id;
            VacancyId = vacancyId;
            RoleTypeId = roleTypeId;
        }
    }
}
