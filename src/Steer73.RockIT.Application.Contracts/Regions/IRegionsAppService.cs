using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Steer73.RockIT.Regions;

public interface IRegionsAppService
{
    ListResultDto<RegionDto> GetList(string filter = "");
}
