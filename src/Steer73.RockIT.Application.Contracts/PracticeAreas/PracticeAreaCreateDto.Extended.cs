using System;
using System.Collections.Generic;

namespace Steer73.RockIT.PracticeAreas
{
    public class PracticeAreaCreateDto : PracticeAreaCreateDtoBase
    {
        //Write your custom code here...
        public Dictionary<Guid, string> PracticeGroups { get; set; }
    }
}