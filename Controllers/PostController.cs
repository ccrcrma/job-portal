using job_portal.Data;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace job_portal.Controllers
{
    public class PostController : Controller
    {
        private readonly ApplicationContext _context;

        public PostController(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> DetailAsync(int id)
        {
            var post = await _context.Posts
                .Include(p => p.Tags)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (post == null)
            {
                return NotFound();
            }
            return View(post);
        }

    }
}