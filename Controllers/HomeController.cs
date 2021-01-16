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

        public async Task<IActionResult> HomeAsync()
        {
            var categories = await _context.Set<JobCategory>().ToListAsync();
            var testimonials = await _context.Testimonials.ToListAsync();
            var posts = await _context.Posts.ToListAsync();
            var vm = new HomeViewModel { Categories = categories, Testimonials = testimonials, Posts = posts };
            return View(vm);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
