using job_portal.Data;
using job_portal.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace job_portal.Controllers
{
    public class JobController : Controller
    {
        private readonly ApplicationContext _context;

        public JobController(ApplicationContext context)
        {
            _context = context;
        }

        public IActionResult Create()
        {
            return View(new JobViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(JobViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            return RedirectToAction(nameof(DetailAsync), new { id = 3 });
        }

        public async Task<IActionResult> DetailAsync(int id)
        {
            var job = await _context.Jobs.FirstOrDefaultAsync(j => j.Id == id);
            if (job == null) return NotFound();
            return View(job);
        }
    }
}