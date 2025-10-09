using Steer73.RockIT.Companies;
using Volo.Abp.Identity;
using Steer73.RockIT.PracticeGroups;
using Steer73.RockIT.FormDefinitions;
using Steer73.RockIT.FormDefinitions;

using System;
using System.Collections.Generic;

namespace Steer73.RockIT.Vacancies
{
    public abstract class VacancyWithNavigationPropertiesBase
    {
        public Vacancy Vacancy { get; set; } = null!;

        public Company Company { get; set; } = null!;
        public IdentityUser IdentityUser { get; set; } = null!;
        public FormDefinition FormDefinition { get; set; } = null!;
        public FormDefinition FormDefinition1 { get; set; } = null!;



    }
}