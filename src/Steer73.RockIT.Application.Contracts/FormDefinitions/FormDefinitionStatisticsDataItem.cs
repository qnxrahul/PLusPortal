using System;

namespace Steer73.RockIT.FormDefinitions
{
    public class FormDefinitionStatisticsDataItem
    {
        public Guid FormDefinitionId { get; set; }
        public int VacanciesTotal { get; set; }
        public int ActiveVacanciesCount { get; set; }
        public int ClosedVacanciesCount { get; set; }
        public int PendingVacanciesCount { get; set; }
    }
}
