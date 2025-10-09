using Steer73.RockIT.Shared;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.Application.Dtos;
using Steer73.RockIT.DiversityDatas;

namespace Steer73.RockIT.Web.Pages.DiversityDatas
{
    public abstract class EditModalModelBase : RockITPageModel
    {
        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public Guid Id { get; set; }

        [BindProperty]
        public DiversityDataUpdateViewModel DiversityData { get; set; }

        protected IDiversityDatasAppService _diversityDatasAppService;

        public EditModalModelBase(IDiversityDatasAppService diversityDatasAppService)
        {
            _diversityDatasAppService = diversityDatasAppService;

            DiversityData = new();
        }

        public virtual async Task OnGetAsync()
        {
            var diversityData = await _diversityDatasAppService.GetAsync(Id);
            DiversityData = ObjectMapper.Map<DiversityDataDto, DiversityDataUpdateViewModel>(diversityData);

        }

        public virtual async Task<NoContentResult> OnPostAsync()
        {

            await _diversityDatasAppService.UpdateAsync(Id, ObjectMapper.Map<DiversityDataUpdateViewModel, DiversityDataUpdateDto>(DiversityData));
            return NoContent();
        }
    }

    public class DiversityDataUpdateViewModel : DiversityDataUpdateDto
    {
    }
}