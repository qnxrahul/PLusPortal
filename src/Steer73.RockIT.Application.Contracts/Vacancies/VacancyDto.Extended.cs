using Steer73.RockIT.Enums;

namespace Steer73.RockIT.Vacancies
{
    public class VacancyDto : VacancyDtoBase
    {
        //Write your custom code here...
        public bool IsActive { get; set; }
        public string VacancyStatus { get; set; } = string.Empty;

        // Add this property
        public string VacancyDetailUrl { get; set; }

    }
}