using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Steer73.RockIT.BrochureSubscriptions;
using Steer73.RockIT.Vacancies;
using Steer73.RockIT.Web.Utilities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Steer73.RockIT.Web.Pages
{
    public class VacancyMoreInfoModel : RockITPageModel
    {
        [BindProperty]
        public BrochureSubscriptionViewModel VacancyMoreInfo { get; set; }
        public bool FormSubmitted = false;

        protected IBrochureSubscriptionsAppService _brochureSubscriptionsAppService;
        protected IVacanciesAppService _vacanciesAppService;
        private readonly string? _plEmailReceiver;
        protected readonly IEmailAppService _emailAppService;
        protected readonly ILinkProvider _linkProvider;
        protected readonly BrochureSubscriptionManager _brochureSubscriptionManager;

        public List<SelectListItem> ReferenceList = new()
        {
            new SelectListItem { Value = "Google", Text = "Google for Jobs"},
            new SelectListItem { Value = "SearchEngine", Text = "Search Engine"},
        };

        public VacancyMoreInfoModel(
            IBrochureSubscriptionsAppService brochureSubscriptionsAppService,
            IVacanciesAppService vacanciesAppService,
            LinkGenerator linkGenerator,
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            IEmailAppService emailAppService,
            ILinkProvider linkProvider,
            BrochureSubscriptionManager brochureSubscriptionManager)
        {
            _brochureSubscriptionsAppService = brochureSubscriptionsAppService;
            _vacanciesAppService = vacanciesAppService;
            _linkProvider = linkProvider;
            _plEmailReceiver = configuration["PlEmailReceiver"];
            _emailAppService = emailAppService;
            _brochureSubscriptionManager = brochureSubscriptionManager;
        }

        public void OnGet(Guid vacancyId)
        {
            VacancyMoreInfo = new BrochureSubscriptionViewModel { VacancyId = vacancyId };
        }

        public async Task<IActionResult> OnPost()
        {
            var brochureSubscriptionDto = await _brochureSubscriptionsAppService.CreateAsync(ObjectMapper.Map<BrochureSubscriptionViewModel, BrochureSubscriptionCreateDto>(VacancyMoreInfo));
            var vacancy = await _vacanciesAppService.GetWithNavigationPropertiesAsync(VacancyMoreInfo.VacancyId);

            // There is brochure
            if (vacancy.Vacancy.BrochureFileId != default)
            {
                await _emailAppService.QueueBrochureEmail(
                    RockITWebConstants.BrochureTemplateEmailPath,
                    _linkProvider.GetLinkForVacancyDetailsPage(brochureSubscriptionDto.VacancyId),
                    brochureSubscriptionDto,
                    vacancy);

                await _brochureSubscriptionManager.MarkAsSent(brochureSubscriptionDto.Id, Clock.Now);
            }
            else
            {
                await _emailAppService.QueueNoBrochureEmail(
                    RockITWebConstants.NoBrochureTemplateEmailPath,
                    _linkProvider.GetLinkForVacancyDetailsPage(brochureSubscriptionDto.VacancyId),
                    brochureSubscriptionDto,
                    vacancy);
            }

            FormSubmitted = true;
            return Page();
        }

        public class BrochureSubscriptionViewModel : BrochureSubscriptionCreateDto
        {

        }
    }
}
