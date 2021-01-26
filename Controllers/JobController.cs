using job_portal.Data;
using job_portal.DTOs;
using job_portal.Extensions;
using job_portal.Interfaces;
using job_portal.Models;
using job_portal.Util;
using job_portal.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeStatusAsync(int id)
        {
            _logger.LogInformation($"request to change status for {id}");
            var job = await _context.Jobs.IgnoreQueryFilters().FirstOrDefaultAsync(j => j.Id == id);
            if (job == null) return BadRequest();
            job.ChangePublishedStatus();
            await _context.SaveChangesAsync();
            return Ok(new
            {
                Status = job.Status.ToString(),
                ChangeUrl = Url.RouteUrl("default", new { Controller = "Job", Action = "ChangeStatus", id = $"{id}" })
            });
        }

        [HttpGet]
        public async Task<IActionResult> IndexAsync(int page = 1)
        {
            var jobsQueryable = _context.Jobs
                .IgnoreQueryFilters()
                .OrderByDescending(j => j.UpdatedOn)
                .Select(j => new JobDTO
                {
                    CreatedDate = j.CreatedOn.GetHumanFriendlyDate(),
                    Position = j.Title,
                    Status = new
                    {
                        Text = j.Status.ToString(),
                        ChangeUrl = Url.RouteUrl("default", new { Controller = "Job", action = "ChangeStatus", id = $"{j.Id}" })
                    },
                    Company = "some random company",
                    Url = Url.RouteUrl("default", new { controller = "Job", action = "Detail", id = $"{j.Id}" })
                });
            var vm = await PaginationModal<JobDTO>.CreateAsync(jobsQueryable, pageIndex: page);
            var accept = Request.Headers[HeaderNames.Accept];
            if (accept == "application/json")
            {
                return Ok(new
                {
                    items = vm,
                    metaData = new
                    {
                        BaseUrl = Url.RouteUrl("default", new { controller = "Job", action = "Index" }),
                        HasPrevious = vm.HasPreviousPage,
                        HasNext = vm.HasNextPage,
                        CurrentPage = vm.CurrentPage,
                        TotalPage = vm.TotalPage
                    }
                });
            }

            return View(vm);
        }
        private void SetCategories(JobViewModel vm)
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
            var job = await _context.Jobs.Include(j => j.Category).IgnoreQueryFilters()
            .FirstOrDefaultAsync(j => j.Id == id);
            if (job == null)
                return NotFound();
            var vm = job.ToViewModel();
            SetCategories(vm);
            return View(vm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var job = await _context.Jobs.IgnoreQueryFilters().FirstOrDefaultAsync(j => j.Id == id);
            if (job == null) return BadRequest();
            _context.Entry(job).State = EntityState.Deleted;
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Dashboard").WithSuccess("hurray", "job deleted Successfully");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TrashAsync(int id)
        {
            var job = await _context.Jobs.AsNoTracking().IgnoreQueryFilters().FirstOrDefaultAsync(j => j.Id == id);
            if (job == null)
            {
                return BadRequest();
            }
            var deleteableEntity = (ISoftDelete)job;
            deleteableEntity.Trash();
            _context.Entry((Job)deleteableEntity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Dashboard").WithDanger(string.Empty, $"job with id {id} deleted ");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RestoreAsync(int id)
        {
            var job = await _context.Jobs.IgnoreQueryFilters().AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
            if (job == null) return BadRequest();
            var deleteableEntity = (ISoftDelete)job;
            deleteableEntity.Restore();
            _context.Entry((Job)(deleteableEntity)).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Dashboard").WithDanger(string.Empty, "job restored successfully");
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

        [HttpGet]
        public async Task<IActionResult> DetailAsync(int id)
        {
            var job = await _context.Jobs.IgnoreQueryFilters().FirstOrDefaultAsync(j => j.Id == id);
            if (job == null) return NotFound();
            return View(job);
        }
    }
}