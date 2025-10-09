using Steer73.RockIT.PracticeAreas;
using Steer73.RockIT.PracticeGroups;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace Steer73.RockIT.Web.Pages.PracticeAreas
{
    public class CreateModalModel : CreateModalModelBase
    {
        protected IPracticeGroupsAppService _practiceGroupsAppService;

        public CreateModalModel(
            IPracticeAreasAppService practiceAreasAppService,
            IPracticeGroupsAppService practiceGroupsAppService)
            : base(practiceAreasAppService)
        {
            _practiceGroupsAppService = practiceGroupsAppService;
        }

        public override async Task OnGetAsync()
        {
            PracticeArea = new PracticeAreaCreateViewModel();

            var practiceGroups = await _practiceGroupsAppService.GetListAsync(
                new GetPracticeGroupsInput { 
                    MaxResultCount = LimitedResultRequestDto.MaxMaxResultCount,
                    IsActive = true 
                });

            PracticeArea.PracticeGroups = practiceGroups.Items.ToDictionary(x => x.Id, x => x.Name);

            await Task.CompletedTask;
        }
    }
}