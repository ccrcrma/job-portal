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
using job_portal.Models;
using job_portal.Areas.Administration.Models;

namespace job_portal.Areas.Administration.Controllers
{
    [Area("Administration")]
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

        [HttpGet]
        public async Task<IActionResult> IndexAsync()
        {
            var testimonials = await _context.Testimonials.ToListAsync();
            return View(testimonials);
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
            return View((TestimonialUpdateViewModel)testimonial.ToViewModel());
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> EditAsync(int id, TestimonialUpdateViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            var testimonial = await _context.Testimonials.FirstOrDefaultAsync(t => t.Id == id);
            if (testimonial == null) return BadRequest();

            testimonial.Update((TestimonialViewModel)vm);
            if (vm.FormFile != null)
            {
                _fileStorageService.DeleteFile(testimonial.ImagePath);
                testimonial.ImageName = await _fileStorageService.SaveFileAsync(vm.FormFile, Testimonial.BaseDirectory);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("Edit", new { id = id });

        }
    }
}