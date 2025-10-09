using Volo.Abp.Domain.Entities;

namespace Steer73.RockIT.Vacancies;

public class ActiveVacancyUpdateDto : VacancyFilesCreateUpdateDto, IHasConcurrencyStamp
{
    public string ConcurrencyStamp { get; set; } = null!;
}