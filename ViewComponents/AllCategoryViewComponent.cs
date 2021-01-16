using System.Threading.Tasks;
using job_portal.Data;
using job_portal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace job_portal.ViewComponents
{
    public class AllCategoryViewComponent : ViewComponent
    {
        private readonly ApplicationContext _context;

        public AllCategoryViewComponent(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var allPostCategories = await _context.Set<PostCategory>().ToListAsync();
            return View(allPostCategories);
        }
    }
}