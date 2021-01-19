using job_portal.Data;
using Microsoft.AspNetCore.Mvc;
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

        public async Task<IActionResult> DetailAsync(int id)
        {
            var job = await _context.Jobs.FirstOrDefaultAsync(j => j.Id == id);
            if (job == null) return NotFound();
            return View(job);
        }
    }
}