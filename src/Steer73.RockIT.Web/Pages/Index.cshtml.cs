using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Steer73.RockIT.ApplicantPortal;
using Steer73.RockIT.PracticeGroups;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace Steer73.RockIT.Web.Pages;

public class IndexModel : RockITPageModel
{
    private readonly IApplicantPortalAppService _applicantPortalAppService;

    public IndexModel(
        IApplicantPortalAppService applicantPortalAppService
        )
    {
        _applicantPortalAppService = applicantPortalAppService;
    }

    public IReadOnlyList<PracticeGroupDto> PracticeGroups { get; set; }
    public async Task<IActionResult> OnGetAsync()
    {
        PracticeGroups = await GetPracticeGroups();
        return Page();
    }


    public async Task<IReadOnlyList<PracticeGroupDto>> GetPracticeGroups()
    {
        var practiceGroups = await _applicantPortalAppService.GetPracticeGroupsAsync(new GetPracticeGroupsInput { MaxResultCount = LimitedResultRequestDto.MaxMaxResultCount, IsActive = true });
        return practiceGroups.Items;
    }
}
