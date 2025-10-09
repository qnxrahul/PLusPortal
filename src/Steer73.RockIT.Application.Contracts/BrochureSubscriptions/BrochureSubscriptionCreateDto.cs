using Steer73.RockIT.BrochureSubscriptions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Steer73.RockIT.BrochureSubscriptions
{
    public class BrochureSubscriptionCreateDto
    {
        [Required]
        public Guid VacancyId { get; set; }
        [Required]
        [StringLength(BrochureSubscriptionConsts.DefaultMaxLength)]
        public string? FirstName { get; set; }
        [Required]
        [StringLength(BrochureSubscriptionConsts.DefaultMaxLength)]
        public string? LastName { get; set; }
        [Required]
        [StringLength(BrochureSubscriptionConsts.DefaultMaxLength)]
        [EmailAddress]
        public string? EmailAddress { get; set; }
        [Required]
        [StringLength(BrochureSubscriptionConsts.DefaultMaxLength)]
        public string? PhoneNumber { get; set; }
        [Required]
        [StringLength(BrochureSubscriptionConsts.DefaultMaxLength)]
        public string? CurrentRole { get; set; }
        [Required]
        [StringLength(BrochureSubscriptionConsts.DefaultMaxLength)]
        public string? CurrentCompany { get; set; }
        [Description("Where did you see the vacancy ?")]
        public string? Reference { get; set; }
    }
}
