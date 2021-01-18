using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using job_portal.Models;
using job_portal.Data;
using Microsoft.EntityFrameworkCore;
using job_portal.ViewModels;
using static job_portal.Models.Job;

namespace job_portal.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public async Task<IActionResult> HomeAsync([Bind(Prefix = "Filter")] SearchFilterViewModel searchFilter)
        {
            var category = searchFilter.Category;

            var jobs = _context.Jobs.Include(j => j.Category).AsQueryable();
            // if (job_type != null)
            // jobs = jobs.Where(j => j.Type == job_type);
            if (category != null && category.ToLower() != "all")
            {
                category = category.ToLower();
                ViewBag.Category = category;
                jobs = jobs.Where(j => j.Category.Name.ToLower().Equals(category));

            }
            List<Job> jobLists;
            jobLists = jobs.ToList();

            var categories = await _context.Set<JobCategory>().Include(jc => jc.Jobs).ToListAsync();
            var testimonials = await _context.Testimonials.ToListAsync();
            var blogPosts = await _context.Posts.ToListAsync();

            var vm = new HomeViewModel { Jobs = jobLists, Categories = categories, Testimonials = testimonials, Posts = blogPosts };
            return View(vm);
        }

        public async Task<ActionResult> JobFilters([Bind(Prefix = "Filter")] SearchFilterViewModel searchFilterViewModel)
        {
            var category = searchFilterViewModel.Category;
            _logger.LogInformation(category);
            _logger.LogInformation(this.Request.QueryString.ToString());
            var jobs = _context.Jobs.Include(j => j.Category).AsQueryable();
            if (category != null && !category.ToLower().Equals("all"))
            {
                category = category.ToLower();
                ViewBag.Category = category;
                jobs = jobs.Where(j => j.Category.Name.ToLower().Equals(category));

            }
            var jobTypes = searchFilterViewModel.Type;
            if (jobTypes != null && jobTypes.Count > 0)
            {
                var selectedCategories = jobTypes.Where(j => j.Selected == true);
                foreach (var selectedcategory in selectedCategories)
                {
                    jobs = jobs.Where(j => j.Type == selectedcategory.Value);

                }

            }


            List<Job> jobLists;
            jobLists = jobs.ToList();
            return PartialView("_JobFilters", jobLists);

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

