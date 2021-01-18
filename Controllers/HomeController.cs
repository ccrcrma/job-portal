﻿using System;
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
using job_portal.Util;
using static job_portal.ViewModels.SearchFilterViewModel;

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
            var selectedCategories = jobTypes.Where(j => j.Selected == true).Select(j => j.Value).ToList();
            if (selectedCategories.Count > 0)
            {
                jobs = jobs.Where(j => selectedCategories.Contains(j.Type));
            }

            var experienceRequired = searchFilterViewModel.Experience;
            var experienceLevels = experienceRequired.Where(e => e.Selected == true).Select(j => j.Value).ToArray();
            if (experienceLevels.Length > 0)
            {
                var (min, max) = ExperienceLevelInfoHelper.GetNumberOfYears(experienceLevels);
                jobs = jobs.Where(j => j.ExperienceRequired >= min && j.ExperienceRequired <= max);
            }

            var postedDuration = searchFilterViewModel.PostedWithin.Where(d => d.Selected == true).Select(d => d.Value).ToArray();
            if (postedDuration.Length > 0)
            {
                if (PostDurationHelper.ApplyDurationFilter(postedDuration, out int days))
                {
                    jobs = jobs.Where(j => j.CreatedOn.AddDays(days).Day >= DateTime.UtcNow.Day);
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

