using Steer73.RockIT.Enums;
using System;
using Volo.Abp.Domain.Entities;

namespace Steer73.RockIT.Vacancies;

public class VacancyRegion : Entity
{
    public Guid VacancyId { get; protected set; }
    public Region Region { get; protected set; }

    public VacancyRegion() { }

    internal VacancyRegion(
        Guid vacancyId, 
        Region region)
    {
        VacancyId = vacancyId;
        Region = region;
    }

    public override object[] GetKeys()
    {
        return [VacancyId, Region];
    }
}
