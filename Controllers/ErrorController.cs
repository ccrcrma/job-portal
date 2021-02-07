using Microsoft.AspNetCore.Mvc;

namespace job_portal.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}