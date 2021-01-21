using job_portal.Data;
using job_portal.Models;
using job_portal.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace job_portal.Controllers
{
    public class JobController : Controller
    {
        private readonly ApplicationContext _context;
        private readonly ILogger<JobController> _logger;

        public JobController(ApplicationContext context, ILogger<JobController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Create()
        {
            var vm = new JobViewModel();
            SetCategories(vm);
            return View(vm);
        }
        public void SetCategories(JobViewModel vm)
        {
            vm.JobCategories = _context.Set<JobCategory>().Select(job => new SelectListItem
            {
                Text = job.Name,
                Value = job.Id.ToString()
            }).ToList();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAsync(JobViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                SetCategories(vm);
                return View(vm);
            }
            var model = vm.ToModel();
            await _context.Jobs.AddAsync(model);
            _context.Entry(model.Category).State = EntityState.Unchanged;
            await _context.SaveChangesAsync();

            TempData["alert-type"] = "success";
            TempData["alert-title"] = "congrats";
            TempData["alert-message"] = "new job added";
            return LocalRedirect("~/");
        }

        [HttpGet]
        public async Task<IActionResult> EditAsync(int id)
        {
            var job = await _context.Jobs.Include(j => j.Category).FirstOrDefaultAsync(j => j.Id == id);
            if (job == null)
                return NotFound();
            var vm = job.ToViewModel();
            SetCategories(vm);
            return View(vm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAsync(int id, JobViewModel vm)
        {
            var job = await _context.Jobs.FirstOrDefaultAsync(job => job.Id == id);
            if (job == null) return BadRequest();
            if (!ModelState.IsValid)
            {
                SetCategories(vm);
                return View(vm);
            }
            job = vm.ToModel(job);
            _context.Entry(job.Category).State = EntityState.Unchanged;
            await _context.SaveChangesAsync();
            return LocalRedirect("~/");
        }

        public async Task<IActionResult> DetailAsync(int id)
        {
            var job = await _context.Jobs.FirstOrDefaultAsync(j => j.Id == id);
            if (job == null) return NotFound();
            return View(job);
        }
    }
}