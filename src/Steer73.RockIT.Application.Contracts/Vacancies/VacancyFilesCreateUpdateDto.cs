using System;
using System.ComponentModel;

namespace Steer73.RockIT.Vacancies;

public abstract class VacancyFilesCreateUpdateDto
{
    [DisplayName("Additional File")]
    public Guid? AdditionalFileId { get; set; }

    [DisplayName("Brochure File")]
    public Guid? BrochureFileId { get; set; }
}
