using Steer73.RockIT.Shared;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.Application.Dtos;
using Steer73.RockIT.FormDefinitions;
using Steer73.RockIT.Vacancies;
using Steer73.RockIT.Companies;

namespace Steer73.RockIT.Web.Pages.FormDefinitions
{
    public class EditModalModel : EditModalModelBase
    {
        public EditModalModel(IFormDefinitionsAppService formDefinitionsAppService,
            IVacanciesAppService vacanciesAppService,
            ICompaniesAppService companiesAppService
            )
            : base(formDefinitionsAppService, vacanciesAppService, companiesAppService)
        {
        }
    }
}