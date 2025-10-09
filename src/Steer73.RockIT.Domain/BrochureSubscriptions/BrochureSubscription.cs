using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace Steer73.RockIT.BrochureSubscriptions
{
    public class BrochureSubscription : Entity<Guid>
    {
        public Guid VacancyId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string CurrentRole { get; set; }
        public string CurrentCompany { get; set; }

        public DateTime? SentAt { get; set; }

        protected BrochureSubscription()
        {

        }

        public BrochureSubscription(
            Guid id,
            Guid vacancyId, 
            string firstName, 
            string lastName, 
            string emailAddress, 
            string phoneNumber, 
            string currentRole, 
            string currentCompany)
        {

            Id = id;
            Check.NotNull(firstName, nameof(firstName));
            Check.Length(firstName, nameof(firstName), BrochureSubscriptionConsts.DefaultMaxLength, 0);
            Check.NotNull(lastName, nameof(lastName));
            Check.Length(lastName, nameof(lastName), BrochureSubscriptionConsts.DefaultMaxLength, 0);
            Check.NotNull(emailAddress, nameof(emailAddress));
            Check.Length(emailAddress, nameof(emailAddress), BrochureSubscriptionConsts.DefaultMaxLength, 0);
            Check.NotNull(phoneNumber, nameof(phoneNumber));
            Check.Length(phoneNumber, nameof(phoneNumber), BrochureSubscriptionConsts.DefaultMaxLength, 0);
            Check.NotNull(currentRole, nameof(currentRole));
            Check.Length(currentRole, nameof(currentRole), BrochureSubscriptionConsts.DefaultMaxLength, 0);
            Check.NotNull(currentCompany, nameof(currentCompany));
            Check.Length(currentCompany, nameof(currentCompany), BrochureSubscriptionConsts.DefaultMaxLength, 0);
            VacancyId = vacancyId;
            FirstName = firstName;
            LastName = lastName;
            EmailAddress = emailAddress;
            PhoneNumber = phoneNumber;
            CurrentRole = currentRole;
            CurrentCompany = currentCompany;
        }
    }
}
