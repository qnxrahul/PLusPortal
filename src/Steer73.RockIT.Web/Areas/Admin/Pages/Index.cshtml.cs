using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Volo.Abp.Users;

namespace Steer73.RockIT.Web.Areas.Admin.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ICurrentUser _currentUser;

        public IndexModel(ICurrentUser currentUser)
        {
            _currentUser = currentUser;
        }

        public IActionResult OnGet()
        {
            if (_currentUser.IsAuthenticated)
            {
                return LocalRedirect("/Admin/Home/");
            }

            return Page();
        }
    }
}
