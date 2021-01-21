using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using job_portal.Areas.Administration.ViewModels;
using job_portal.Data;
using job_portal.Util;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using job_portal.Services;
using Microsoft.EntityFrameworkCore;

namespace job_portal.Areas.Administration.Controllers
{
    [Area("Administration")]
    [Route("[controller]/[action]")]
    public class TestimonialController : Controller
    {
        private readonly IFileStorageService _fileStorageService;
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<TestimonialController> _logger;
        private readonly IConfiguration _configuration;
        private readonly ApplicationContext _context;

        public TestimonialController(ApplicationContext context,
            ILogger<TestimonialController> logger,
            IFileStorageService fileStorageService,
            IConfiguration configuration,
            IWebHostEnvironment env)
        {
            _fileStorageService = fileStorageService;
            _env = env;
            _logger = logger;
            _configuration = configuration;
            _context = context;
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAsync(TestimonialViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var testimonial = vm.ToModel();
            testimonial.ImageName = await _fileStorageService.SaveFileAsync(vm.FormFile, baseDirectoryLocation: "Testimonial");
            await _context.Testimonials.AddAsync(testimonial);
            await _context.SaveChangesAsync();

            TempData["alert-type"] = "success";
            TempData["alert-title"] = "congrats";
            TempData["alert-body"] = "new testimonial created successfully";
            return LocalRedirect("~/");
        }

        [HttpGet]
        public async Task<IActionResult> EditAsync(int id)
        {
            var testimonial = await _context.Testimonials.FirstOrDefaultAsync(t => t.Id == id);
            if (testimonial == null) return NotFound();
            return View(testimonial.ToViewModel());
        }
    }
}