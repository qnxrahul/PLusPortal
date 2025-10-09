using System.Collections.Generic;
using System;

namespace Steer73.RockIT.PracticeAreas
{
    public class PracticeAreaUpdateDto : PracticeAreaUpdateDtoBase
    {
        //Write your custom code here...
        public Dictionary<Guid, string> PracticeGroups { get; set; }
    }
}