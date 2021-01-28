using System.Linq;
using job_portal.Data;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace job_portal.Areas.Seeker.Controllers
{
    [Area("Seeker")]
    [Authorize]
    public class SeekerController : Controller
    {
        private readonly ApplicationContext _context;

        public SeekerController(ApplicationContext context)
        {
            _context = context;
        }
        public IActionResult IndexAsync()
        {
            var myjobs = _context.Jobs.Take(3).ToList();
            return View(myjobs);
        }

    }
}