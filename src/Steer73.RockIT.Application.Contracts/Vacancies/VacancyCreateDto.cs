using Steer73.RockIT.ValidationAttributes;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Steer73.RockIT.Vacancies
{
    public class VacancyCreateDto : VacancyCreateUpdateDto
    {
        //Write your custom code here...

        [DisplayName("Closing Date")]
        [Required]
        [RequiredNonDefault]
        [AtLeastToday]
        public DateOnly ClosingDate { get; set; }

        [DisplayName("Expiring Date")]
        [Required]
        [RequiredNonDefault]
        [AtLeastToday]
        [Before(otherProperty: nameof(ClosingDate), otherPropertyDisplayName: "Closing Date")]
        public DateOnly ExpiringDate { get; set; }

        [DisplayName("External Posting Date")]
        [Required]
        [RequiredNonDefault]
        [AtLeastToday]
        [Before(otherProperty: nameof(ExpiringDate), otherPropertyDisplayName: "Expiring Date")]
        public DateOnly ExternalPostingDate { get; set; }

        public string? LinkedInUrl { get; set; }

		[Required]
		public int? ExternalRefId { get; set; }

		public string? ProjectId { get; set; }
	}
}