using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Steer73.RockIT.Permissions;
using Steer73.RockIT.PracticeGroups;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace Steer73.RockIT.Web.Pages.JobAlerts.Registrations;

[Authorize(RockITSharedPermissions.JobAlertRegistrations.Default)]
public class IndexModel : RockITPageModel
{
    private readonly IPracticeGroupsAppService _practiceGroupsAppService;

    public List<SelectListItem> PracticeGroups { get; set; }

    public IndexModel(IPracticeGroupsAppService practiceGroupsAppService)
    {
        _practiceGroupsAppService = practiceGroupsAppService;
        PracticeGroups = new List<SelectListItem>();
    }

    public async Task OnGetAsync()
    {
        var input = new GetPracticeGroupsInput
        {
            MaxResultCount = LimitedResultRequestDto.MaxMaxResultCount,
            SkipCount = 0,
            Sorting = nameof(PracticeGroupDto.Name)
        };

        var practiceGroups = await _practiceGroupsAppService.GetListAsync(input);

        PracticeGroups = practiceGroups.Items
            .OrderBy(pg => pg.Name)
            .Select(pg => new SelectListItem(pg.Name, pg.Id.ToString()))
            .ToList();
    }
}
