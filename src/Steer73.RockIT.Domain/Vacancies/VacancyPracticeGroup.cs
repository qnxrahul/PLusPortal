using Steer73.RockIT.PracticeGroups;
using System;
using Volo.Abp.Domain.Entities;

namespace Steer73.RockIT.Vacancies;

public class VacancyPracticeGroup : Entity
{
    public Guid VacancyId { get; protected set; }
    public Guid PracticeGroupId { get; protected set; }
    public virtual PracticeGroup PracticeGroup { get; protected set; }

    public VacancyPracticeGroup() { }

    internal VacancyPracticeGroup(
        Guid vacancyId,
        PracticeGroup practiceGroup)
    {
        VacancyId = vacancyId;
        PracticeGroup = practiceGroup;
        PracticeGroupId = practiceGroup.Id;
    }

    public override object[] GetKeys()
    {
        return [VacancyId, PracticeGroupId];
    }
}
