using System.Linq;
using System.Threading.Tasks;
using job_portal.Data;
using Microsoft.AspNetCore.Mvc;

namespace job_portal.ViewComponents
{
    public class RecentPostViewComponent : ViewComponent
    {
        private readonly ApplicationContext _context;

        public RecentPostViewComponent(ApplicationContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            var recentPosts = _context.Posts.OrderByDescending(p => p.CreatedOn).Take(5).ToList();
            return View(recentPosts);
        }
    }
}