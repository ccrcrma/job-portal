using job_portal.Areas.Employer.ViewModels;
using job_portal.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace job_portal.Areas.Employer.Controllers
{
    [Area("Employer")]
    public class CompanyController : Controller
    {
        private readonly ApplicationContext _context;
        public CompanyController(ApplicationContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> IndexAsync()
        {
            var companies = await _context.Companies.ToListAsync();
            return View(companies);
        }

        [HttpGet("[area]/[controller]/{id}")]
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
            return Ok();
        }
    }
}