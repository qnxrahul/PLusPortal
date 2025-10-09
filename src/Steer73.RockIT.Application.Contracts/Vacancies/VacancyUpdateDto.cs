using Steer73.RockIT.ValidationAttributes;
using System.ComponentModel;
using System;
using Volo.Abp.Domain.Entities;

namespace Steer73.RockIT.Vacancies
{
    public class VacancyUpdateDto : VacancyCreateUpdateDto, IHasConcurrencyStamp
    {
        [DisplayName("Closing Date")]
        [RequiredNonDefault]
        public DateOnly ClosingDate { get; set; }

        [DisplayName("Expiring Date")]
        [RequiredNonDefault]
        [Before(otherProperty: nameof(ClosingDate), otherPropertyDisplayName: "Closing Date")]
        public DateOnly ExpiringDate { get; set; }

        [DisplayName("External Posting Date")]
        [RequiredNonDefault]
        [Before(otherProperty: nameof(ExpiringDate), otherPropertyDisplayName: "Expiring Date")]
        public DateOnly ExternalPostingDate { get; set; }

        public string ConcurrencyStamp { get; set; } = null!;
        public string? LinkedInUrl { get; set; }
    }
}