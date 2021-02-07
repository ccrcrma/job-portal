using job_portal.Areas.Employer.Models;
using job_portal.Areas.Employer.ViewModels;
using job_portal.Data;
using job_portal.Services;
using job_portal.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using static job_portal.Constants.Constant;
using System;
using System.Threading.Tasks;

namespace job_portal.Areas.Employer.Controllers
{
    [Area("Employer")]
    [Authorize(EmployerPolicy)]
    public class CompanyController : Controller
    {
        private readonly IFileStorageService _fileStorageService;
        private readonly ApplicationContext _context;
        private readonly ILogger<CompanyController> _logger;

        public CompanyController(ApplicationContext context,
            IFileStorageService fileStorageService, ILogger<CompanyController> logger)
        {
            _fileStorageService = fileStorageService;
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> IndexAsync()
        {
            var companies = await _context.Companies.ToListAsync();
            return View(companies);
        }

        [HttpGet("[area]/[controller]/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> DetailAsync(string id)
        {
            id = id.DeHyphenate();
            var company = await _context.Companies
                .Include(c => c.Jobs)
                .FirstOrDefaultAsync(c => c.Name == id);
            if (company == null) return BadRequest();
            return View(company);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateAsync(Guid id, CompanyViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_CompanyUpdatePartial", vm);
            }
            var company = await _context.Companies.FirstOrDefaultAsync(c => c.Id == id);
            if (company == null)
                return BadRequest();
            company = vm.ToModel(company);
            await _context.SaveChangesAsync();
            return Ok(company);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateBrandAsync(Guid id, ImageFormFileViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_ImageFormFilePartial", vm);
            }
            var company = await _context.Companies.FirstOrDefaultAsync(c => c.Id == id);
            if (company == null)
            {
                return BadRequest();
            }

            _fileStorageService.DeleteFile(company.GetBrandImagePath);
            company.BrandImage = await _fileStorageService.SaveFileAsync(vm.FormFile, Company.BaseBrandImageDirectory);
            await _context.SaveChangesAsync();
            var absFilePath = Url.Content($"~/{company.GetBrandImagePath}");
            _logger.LogInformation(absFilePath);
            return Ok(new { Url = absFilePath });
        }
    }
}