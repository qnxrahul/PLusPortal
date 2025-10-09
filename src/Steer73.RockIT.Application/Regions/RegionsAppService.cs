using Steer73.RockIT.Enums;
using System;
using System.Linq;
using Volo.Abp.Application.Dtos;

namespace Steer73.RockIT.Regions;

public class RegionsAppService : RockITAppService, IRegionsAppService
{
    public virtual ListResultDto<RegionDto> GetList(string filter = "")
    {
        var dtos = Enum.GetValues(typeof(Region))
            .Cast<Region>()
            .Select(r => new RegionDto
            {
                Id = ((int)r).ToString(),
                Description = r.GetDescription()
            });

        if (!string.IsNullOrWhiteSpace(filter))
        {
            dtos = dtos.Where(r => r.Description.Contains(filter, StringComparison.InvariantCultureIgnoreCase));
        }

        return new ListResultDto<RegionDto>(dtos.OrderBy(r => r.Description).ToList());
    }
}