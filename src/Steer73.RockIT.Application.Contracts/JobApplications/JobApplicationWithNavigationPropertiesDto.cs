using Steer73.RockIT.Vacancies;

using System;
using Volo.Abp.Application.Dtos;
using System.Collections.Generic;

namespace Steer73.RockIT.JobApplications
{
    public abstract class JobApplicationWithNavigationPropertiesDtoBase
    {
        public JobApplicationDto JobApplication { get; set; } = null!;

        public VacancyDto Vacancy { get; set; } = null!;

    }
}