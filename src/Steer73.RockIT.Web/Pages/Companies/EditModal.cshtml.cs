using Steer73.RockIT.Shared;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.Application.Dtos;
using Steer73.RockIT.Companies;

namespace Steer73.RockIT.Web.Pages.Companies
{
    public abstract class EditModalModelBase : RockITPageModel
    {
        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public Guid Id { get; set; }

        [BindProperty]
        public CompanyUpdateViewModel Company { get; set; }

        protected ICompaniesAppService _companiesAppService;

        public EditModalModelBase(ICompaniesAppService companiesAppService)
        {
            _companiesAppService = companiesAppService;

            Company = new();
        }

        public virtual async Task OnGetAsync()
        {
            var company = await _companiesAppService.GetAsync(Id);
            Company = ObjectMapper.Map<CompanyDto, CompanyUpdateViewModel>(company);

        }

        public virtual async Task<NoContentResult> OnPostAsync()
        {

            await _companiesAppService.UpdateAsync(Id, ObjectMapper.Map<CompanyUpdateViewModel, CompanyUpdateDto>(Company));
            return NoContent();
        }
    }

    public class CompanyUpdateViewModel : CompanyUpdateDto
    {
    }
}