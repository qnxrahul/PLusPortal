using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System;

namespace Steer73.RockIT.FormDefinitions
{
    public partial interface IFormDefinitionsAppService
    {
        //Write your custom code here...
        Task<List<FormDefinitionStatisticsDataItem>> GetFormDefinitionStatisticsDataAsync(DateTime checkDate, Guid[] listOfFormDefinitions = null, CancellationToken cancellationToken = default);
    }
}