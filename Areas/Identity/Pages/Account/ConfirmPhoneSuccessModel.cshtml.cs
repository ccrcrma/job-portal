using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace job_portal.Areas.Identity.Pages.Account
{
    public class ConfirmPhoneSuccessModel : PageModel
    {
        public IActionResult OnGet()
        {
            return Page();
        }
    }
}