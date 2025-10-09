using Steer73.RockIT.Shared;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.Application.Dtos;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Steer73.RockIT.DiversityDatas;

namespace Steer73.RockIT.Web.Pages.DiversityDatas
{
    public abstract class CreateModalModelBase : RockITPageModel
    {
        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public Guid JobApplicationId { get; set; }

        [BindProperty]
        public DiversityDataCreateViewModel DiversityData { get; set; }

        protected IDiversityDatasAppService _diversityDatasAppService;

        public CreateModalModelBase(IDiversityDatasAppService diversityDatasAppService)
        {
            _diversityDatasAppService = diversityDatasAppService;

            DiversityData = new();
        }

        public virtual async Task OnGetAsync()
        {
            DiversityData = new DiversityDataCreateViewModel();

            await Task.CompletedTask;
        }

        public virtual async Task<IActionResult> OnPostAsync()
        {

            DiversityData.JobApplicationId = JobApplicationId;
            await _diversityDatasAppService.CreateAsync(ObjectMapper.Map<DiversityDataCreateViewModel, DiversityDataCreateDto>(DiversityData));
            return NoContent();
        }
    }

    public class DiversityDataCreateViewModel : DiversityDataCreateDto
    {
    }
}