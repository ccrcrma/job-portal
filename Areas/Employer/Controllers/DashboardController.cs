using System.Linq;
using System.Security.Claims;
using job_portal.Data;
using job_portal.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace job_portal.Areas.Employer.Controllers
{
    [Area(areaName: "Employer")]
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
            var companyName = ((ClaimsIdentity)User.Identity).GetSpecificClaim("CompanyName");
            var company = await _context.Companies.FirstOrDefaultAsync(c => c.Name == companyName);
            if (company == null) return BadRequest();
            var vm = company.ToVm();
            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> MyJobsAsync()
        {
            var companyName = ((ClaimsIdentity)User.Identity).GetSpecificClaim("CompanyName");
            var company = await _context.Companies.FirstOrDefaultAsync(c => c.Name == companyName);
            var myJobs = await _context.Jobs.Include(c => c.Company).Where(j => j.CompanyId == company.Id).ToListAsync();
            return View(myJobs);
        }
    }
}