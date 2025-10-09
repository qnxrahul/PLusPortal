using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;
using Volo.Abp.DependencyInjection;

namespace Steer73.RockIT.Web.Utilities
{
    public class LinkProvider : ILinkProvider, ITransientDependency
    {
        private readonly LinkGenerator _linkGenerator;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LinkProvider(
            LinkGenerator linkGenerator,
            IHttpContextAccessor httpContextAccessor)
        {
            _linkGenerator = linkGenerator;
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetLinkForVacancyDetailsPage(Guid vacancyId)
        {
            var request = _httpContextAccessor.HttpContext.Request;
            var baseUrl = $"{request.Scheme}://{request.Host}";

            // Generate the URL for a specific Razor Page or MVC Action
            var url = _linkGenerator.GetPathByPage(
                httpContext: _httpContextAccessor.HttpContext,
                page: "/VacancyDetail",
                values: new { vacancyId = vacancyId });

            return $"{baseUrl}{url}";
        }
    }
}
