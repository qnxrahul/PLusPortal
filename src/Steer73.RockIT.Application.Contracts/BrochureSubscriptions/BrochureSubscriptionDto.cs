using System;

namespace Steer73.RockIT.BrochureSubscriptions
{
    public class BrochureSubscriptionDto
    {
        public Guid Id { get; set; }
        public Guid VacancyId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? EmailAddress { get; set; }
        public string? MobileNumber { get; set; }
        public string? CurrentRole { get; set; }
        public string? CurrentCompany { get; set; }
    }
}
