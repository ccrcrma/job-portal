using job_portal.Areas.Seeker.ViewModels;
using job_portal.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace job_portal.Areas.Seeker.Controllers
{
    [Area("Seeker")]
    public class AccountController : Controller
    {
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(RegistrationViewModel vm)
        {
            if(!ModelState.IsValid) return View(vm);
            return LocalRedirect("~/").WithSuccess("User created successfully", "Verify Email to login");
        }
    }
}