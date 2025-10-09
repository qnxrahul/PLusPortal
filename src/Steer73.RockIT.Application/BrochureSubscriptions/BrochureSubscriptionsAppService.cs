using System.Threading.Tasks;
using Volo.Abp;

namespace Steer73.RockIT.BrochureSubscriptions
{
    public class BrochureSubscriptionsAppService : RockITAppService, IBrochureSubscriptionsAppService
    {
        protected BrochureSubscriptionManager _brochureSubscriptionManager;

        public BrochureSubscriptionsAppService(BrochureSubscriptionManager brochureSubscriptionManager)
        {
            _brochureSubscriptionManager = brochureSubscriptionManager;
        }

        public async Task<BrochureSubscriptionDto> CreateAsync(BrochureSubscriptionCreateDto input)
        {
            if (input.VacancyId == default)
            {
                throw new UserFriendlyException(L["The {0} field is required.", L["Company"]]);
            };

            var brochureSubscription = await _brochureSubscriptionManager.CreateAsync(
                input.VacancyId,
                input.FirstName,
                input.LastName,
                input.EmailAddress,
                input.PhoneNumber,
                input.CurrentRole,
                input.CurrentCompany,
                autoSave: true);

            return ObjectMapper.Map<BrochureSubscription, BrochureSubscriptionDto>(brochureSubscription);
        }
    }
}
