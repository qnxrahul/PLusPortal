using Steer73.RockIT.Shared;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.Application.Dtos;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Steer73.RockIT.Companies;

namespace Steer73.RockIT.Web.Pages.Companies
{
    public abstract class CreateModalModelBase : RockITPageModel
    {

        [BindProperty]
        public CompanyCreateViewModel Company { get; set; }

        protected ICompaniesAppService _companiesAppService;

        public CreateModalModelBase(ICompaniesAppService companiesAppService)
        {
            _companiesAppService = companiesAppService;

            Company = new();
        }

        public virtual async Task OnGetAsync()
        {
            Company = new CompanyCreateViewModel();

            await Task.CompletedTask;
        }

        public virtual async Task<IActionResult> OnPostAsync()
        {

            await _companiesAppService.CreateAsync(ObjectMapper.Map<CompanyCreateViewModel, CompanyCreateDto>(Company));
            return NoContent();
        }
    }

    public class CompanyCreateViewModel : CompanyCreateDto
    {
    }
}