using job_portal.Areas.Employer.ViewModels;
using job_portal.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace job_portal.Areas.Employer.Controllers
{
    [Area("Employer")]
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
            if (!ModelState.IsValid) return View(vm);
            return LocalRedirect("~/").WithSuccess("company account created successfully", string.Empty);
        }
    }
}