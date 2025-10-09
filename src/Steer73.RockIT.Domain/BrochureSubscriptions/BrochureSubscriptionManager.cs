using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace Steer73.RockIT.BrochureSubscriptions
{
    public class BrochureSubscriptionManager : DomainService
    {
        protected IBrochureSubscriptionRepository _brochureSubscriptionRepository;

        public BrochureSubscriptionManager(
            IBrochureSubscriptionRepository brochureSubscriptionRepository)
        {
            _brochureSubscriptionRepository = brochureSubscriptionRepository;
        }

        public async Task<BrochureSubscription> CreateAsync(
            Guid vacancyId,
            string firstName, 
            string lastName,
            string emailAddress,
            string phoneNumber,
            string currentRole,
            string currentCompany,
            bool autoSave = false)
        {
            Check.NotNull(vacancyId, nameof(vacancyId));
            Check.NotNullOrWhiteSpace(firstName, nameof(firstName));
            Check.Length(firstName, nameof(firstName), BrochureSubscriptionConsts.DefaultMaxLength);
            Check.NotNullOrWhiteSpace(lastName, nameof(lastName));
            Check.Length(lastName, nameof(lastName), BrochureSubscriptionConsts.DefaultMaxLength);
            Check.NotNullOrWhiteSpace(emailAddress, nameof(emailAddress));
            Check.Length(emailAddress, nameof(emailAddress), BrochureSubscriptionConsts.DefaultMaxLength);
            Check.NotNullOrWhiteSpace(phoneNumber, nameof(phoneNumber));
            Check.Length(phoneNumber, nameof(phoneNumber), BrochureSubscriptionConsts.DefaultMaxLength);
            Check.NotNullOrWhiteSpace(currentRole, nameof(currentRole));
            Check.Length(currentRole, nameof(currentRole), BrochureSubscriptionConsts.DefaultMaxLength);
            Check.NotNullOrWhiteSpace(currentCompany, nameof(currentCompany));
            Check.Length(currentCompany, nameof(currentCompany), BrochureSubscriptionConsts.DefaultMaxLength);

            var brochureSubscription = new BrochureSubscription(
             GuidGenerator.Create(),
             vacancyId, 
             firstName, 
             lastName, 
             emailAddress,
             phoneNumber,
             currentRole, 
             currentCompany
            );

            return await _brochureSubscriptionRepository.InsertAsync(brochureSubscription, autoSave);
        }

        public async Task MarkAsSent(Guid id, DateTime now)
        {
            var brochure = await _brochureSubscriptionRepository.GetAsync(id);
            brochure.SentAt = now;

            // No need to call update async on repository since entity is already tracked. When the unit of work middleware commits transactions, only the brochure's SentAt field will be updated, thereby making query more efficient.
        }
    }
}
