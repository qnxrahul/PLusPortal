using Steer73.RockIT.Vacancies;

using System;
using System.Collections.Generic;

namespace Steer73.RockIT.JobApplications
{
    public abstract class JobApplicationWithNavigationPropertiesBase
    {
        public JobApplication JobApplication { get; set; } = null!;

        public Vacancy Vacancy { get; set; } = null!;
        

        
    }
}