using Microsoft.AspNetCore.Mvc;

namespace Steer73.RockIT.Web.Components.Search
{
    public class SearchViewComponent : ViewComponent
    {

        public virtual IViewComponentResult Invoke(string? query)
        {
            return View("~/Components/Search/Default.cshtml", query);
        }
    }
}
