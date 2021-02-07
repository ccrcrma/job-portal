using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using job_portal.Areas.Administration.ViewModels;
using job_portal.Data;
using job_portal.DTOs;
using job_portal.Extensions;
using job_portal.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;

namespace job_portal.Areas.Administration.Controllers
{
    [Area("Administration")]
    [Authorize(Roles = Constants.Constant.AdminRole)]
    public class DashboardController : Controller
    {
        private readonly ApplicationContext _context;

        public DashboardController(ApplicationContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> IndexAsync()
        {
            var posts = _context.Posts
                .AsNoTracking()
                .OrderByDescending(p => p.UpdatedOn)
                .Take(10)
                .Select(p => new JobPostCommonViewModel
                {
                    Id = p.Id,
                    ImagePath = p.ImagePath,
                    Title = p.Title,
                    Status = p.Status,
                    Description = p.Body,
                    UpdatedOn = p.UpdatedOn,
                    EditUrl = Url.RouteUrl("post", new { action = "Edit", id = p.Id }),
                    DeleteRestoreUrl = Url.RouteUrl("post", new { action = "Trash", id = p.Id }),
                })
                .ToList();

            var jobs = _context.Jobs
                .AsNoTracking()
                .OrderByDescending(p => p.UpdatedOn)
                .Take(10)
                .Select(j => new JobPostCommonViewModel
                {
                    Id = j.Id,
                    ImagePath = string.Empty,
                    Title = j.Title,
                    Status = j.Status,
                    UpdatedOn = j.UpdatedOn,
                    Description = j.Description,
                    EditUrl = Url.RouteUrl("default", new { controller = "Job", action = "Edit", id = j.Id }),
                    DeleteRestoreUrl = Url.RouteUrl("default", new { controller = "Job", action = "Trash", id = j.Id }),
                })
                .ToList();

            List<JobPostCommonViewModel> vm = new List<JobPostCommonViewModel>();
            vm.AddRange(posts);
            vm.AddRange(jobs);
            vm = vm.OrderByDescending(p => p.UpdatedOn).Take(5).ToList();
            return View(vm);
        }

        [HttpGet]
        public IActionResult Trash()
        {
            var posts = _context.Posts
                .IgnoreQueryFilters()
                .OrderByDescending(p => p.UpdatedOn)
                .Where(p => p.IsSoftDeleted)
                .Select(p => new JobPostCommonViewModel
                {
                    Id = p.Id,
                    ImagePath = p.ImagePath,
                    Title = p.Title,
                    Status = p.Status,
                    Description = p.Body,
                    UpdatedOn = p.UpdatedOn,
                    DeleteRestoreUrl = Url.RouteUrl("post", new { action = "Restore", id = p.Id }),
                })
                .ToList();

            var jobs = _context.Jobs
                .IgnoreQueryFilters()
                .OrderByDescending(p => p.UpdatedOn)
                .Where(p => p.IsSoftDeleted)
                .Select(p => new JobPostCommonViewModel
                {
                    Id = p.Id,
                    ImagePath = string.Empty,
                    Title = p.Title,
                    Status = p.Status,
                    Description = p.Description,
                    UpdatedOn = p.UpdatedOn,
                    DeleteRestoreUrl = Url.RouteUrl("default", new { controller = "Job", action = "Restore", id = p.Id }),
                })
                .ToList();

            List<JobPostCommonViewModel> vm = new List<JobPostCommonViewModel>();
            vm.AddRange(jobs);
            vm.AddRange(posts);
            vm.OrderByDescending(m => m.UpdatedOn);
            return View(vm);

        }

        [HttpGet]
        public async Task<IActionResult> AllJobs(int page = 1)
        {
            var jobsQueryable = _context.Jobs
                .IgnoreQueryFilters()
                .OrderByDescending(j => j.UpdatedOn)
                .Include(j => j.Company)
                .Select(j => new JobDTO
                {
                    CreatedDate = j.CreatedOn.GetHumanFriendlyDate(),
                    Position = j.Title,
                    Status = new
                    {
                        Text = j.Status.ToString(),
                        ChangeUrl = Url.RouteUrl("default", new { Controller = "Job", action = "ChangeStatus", id = $"{j.Id}" })
                    },
                    Company = j.Company,
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
                        BaseUrl = Url.RouteUrl("dashboard", new { controller = "Dashboard", action = "AllJobs" }),
                        HasPrevious = vm.HasPreviousPage,
                        HasNext = vm.HasNextPage,
                        CurrentPage = vm.CurrentPage,
                        TotalPage = vm.TotalPage
                    }
                });
            }
            return View(vm);
        }
    }
}