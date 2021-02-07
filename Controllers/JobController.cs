using job_portal.AuthorizationAttribute;
using job_portal.Data;
using job_portal.DTOs;
using job_portal.Extensions;
using job_portal.Interfaces;
using job_portal.Models;
using job_portal.Util;
using job_portal.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using static job_portal.Constants.Constant;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using job_portal.Requirements;

namespace job_portal.Controllers
{
    [MultiplePoliciesAuthorization(andPolicies: false, EmployerPolicy, AdminPolicy)]

    public class JobController : Controller
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly ApplicationContext _context;
        private readonly ILogger<JobController> _logger;

        public JobController(ApplicationContext context,
            ILogger<JobController> logger,
            IAuthorizationService authorizationService)
        {
            _authorizationService = authorizationService;
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
            var job = await _context.Jobs
                .IgnoreQueryFilters()
                .Include(j => j.Company)
                .FirstOrDefaultAsync(j => j.Id == id);
            if (job == null) return BadRequest();
            var authResult = await _authorizationService.AuthorizeAsync(User, job, new OwnsJobRequirement(job.Company.Name));
            if (authResult.Succeeded)
            {
                job.ChangePublishedStatus();
                await _context.SaveChangesAsync();
                return Ok(new
                {
                    Status = job.Status.ToString(),
                    ChangeUrl = Url.RouteUrl("default", new { Controller = "Job", Action = "ChangeStatus", id = $"{id}" })
                });
            }
            else if (User.Identity.IsAuthenticated)
            {
                return new ForbidResult();
            }
            else
            {
                return new ChallengeResult();
            }

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
            var companyName = ((ClaimsIdentity)User.Identity).GetSpecificClaim("CompanyName");
            model.Company = await _context.Companies.FirstOrDefaultAsync(c => c.Name == companyName);
            await _context.Jobs.AddAsync(model);
            _context.Entry(model.Category).State = EntityState.Unchanged;
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Dashboard", new { area = "Employer" }).WithSuccess("congrats", "new job added");
        }

        [HttpGet]
        public async Task<IActionResult> EditAsync(int id)
        {

            var job = await _context.Jobs
                .Include(j => j.Category)
                .Include(j => j.Company)
                .IgnoreQueryFilters()
            .FirstOrDefaultAsync(j => j.Id == id);
            if (job == null)
                return NotFound();

            var authorizationResult = await _authorizationService.AuthorizeAsync(User,
                job, new OwnsJobRequirement(job.Company.Name));

            if (authorizationResult.Succeeded)
            {
                var vm = job.ToViewModel();
                SetCategories(vm);
                return View(vm);
            }
            else if (User.Identity.IsAuthenticated)
            {
                return new ForbidResult();
            }
            else
            {
                return new ChallengeResult();
            }
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
            if (!ModelState.IsValid)
            {
                SetCategories(vm);
                return View(vm);
            }
            var job = await _context.Jobs
                .Include(j => j.Company)
                .FirstOrDefaultAsync(job => job.Id == id);
            if (job == null) return BadRequest();
            var authResult = await _authorizationService.AuthorizeAsync(User, job, new OwnsJobRequirement(job.Company.Name));
            if (authResult.Succeeded)
            {
                job = vm.ToModel(job);
                _context.Entry(job.Category).State = EntityState.Unchanged;
                await _context.SaveChangesAsync();
                return LocalRedirect("~/").WithSuccess("hurray", "job updated successfully");
            }
            else if (User.Identity.IsAuthenticated)
            {
                return new ForbidResult();
            }
            else
            {
                return new ChallengeResult();
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> DetailAsync(int id)
        {
            var job = await _context.Jobs
                .Include(job => job.Company)
                .FirstOrDefaultAsync(j => j.Id == id);
            if (job == null) return NotFound();
            return View(job);
        }
    }
}