using Steer73.RockIT.Companies;
using Volo.Abp.Identity;
using Steer73.RockIT.PracticeGroups;
using Steer73.RockIT.FormDefinitions;
using Steer73.RockIT.FormDefinitions;

using System;
using Volo.Abp.Application.Dtos;
using System.Collections.Generic;

namespace Steer73.RockIT.Vacancies
{
    public abstract class VacancyWithNavigationPropertiesDtoBase
    {
        public VacancyDto Vacancy { get; set; } = null!;

        public CompanyDto Company { get; set; } = null!;
        public IdentityUserDto IdentityUser { get; set; } = null!;
        public PracticeGroupDto PracticeGroup { get; set; } = null!;
        public FormDefinitionDto FormDefinition { get; set; } = null!;
        public FormDefinitionDto FormDefinition1 { get; set; } = null!;

    }
}