using System.Threading.Tasks;
using job_portal.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace job_portal.ViewComponents
{
    public class AllTagViewComponent : ViewComponent
    {
        private readonly ApplicationContext _context;

        public AllTagViewComponent(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var allTags = await _context.Tags.ToListAsync();
            return View(allTags);
        }
    }
}