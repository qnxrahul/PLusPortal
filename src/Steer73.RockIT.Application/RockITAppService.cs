using Microsoft.AspNetCore.Authorization;
using Steer73.RockIT.Localization;
using Steer73.RockIT.PracticeGroups;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Steer73.RockIT;

/* Inherit your application services from this class.
 */
public abstract class RockITAppService : ApplicationService
{
    protected RockITAppService()
    {
        LocalizationResource = typeof(RockITResource);
    }
}
