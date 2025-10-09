using Steer73.RockIT.MediaSources;
using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.Identity;

namespace Steer73.RockIT.Vacancies
{
    public class VacancyContributor : Entity
    {
        public Guid VacancyId { get; set; }
        public virtual Vacancy Vacancy { get; set; } = null!;

        public Guid IdentityUserId { get; set; }
        public virtual IdentityUser IdentityUser { get; set; } = null!;

        public VacancyContributor(Guid vacancyId, Guid identityUserId)
        {
            VacancyId = vacancyId;
            IdentityUserId = identityUserId;
        }

        public override object[] GetKeys()
        {
            return new object[]
                {
                    VacancyId,
                    IdentityUserId
                };
        }
    }
}
