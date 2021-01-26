using job_portal.Extensions;
using job_portal.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace job_portal.Controllers
{
    public class LoginController : Controller
    {
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SignIn(LoginViewModel vm)
        {
            if (!ModelState.IsValid) return View();
            return LocalRedirect("~/").WithSuccess($"user {vm.Email} logged in successfully", string.Empty);
        }
    }
}