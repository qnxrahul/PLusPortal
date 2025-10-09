namespace Steer73.RockIT.FormDefinitions
{
    public class FormDefinitionDto : FormDefinitionDtoBase
    {
        //Write your custom code here...
        public int ActiveVacanciesCount { get; set; }
        public int ClosedVacanciesCount { get; set; }
        public int PendingVacanciesCount { get; set; }
    }
}