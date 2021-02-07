using job_portal.Data;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using job_portal.Areas.Administration.ViewModels;
using job_portal.Services;
using job_portal.Areas.Administration.Models;
using System.IO;
using job_portal.Extensions;
using job_portal.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace job_portal.Areas.Administration.Controllers
{
    [Area("Administration")]
    [Authorize(Roles = Constants.Constant.AdminRole)]
    public class PostController : Controller
    {
        private readonly IFileStorageService _fileService;
        private readonly ApplicationContext _context;

        public PostController(ApplicationContext context, IFileStorageService fileStorageService)
        {
            _fileService = fileStorageService;
            _context = context;
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAsync(PostViewModel vm)
        {
            // if (HttpContext.Request.Form?.Files[0] != null)
            // {
            //     var file = HttpContext.Request.Form.Files[0];
            //     vm.FormFile = file;
            // }
            // if (!TryValidateModel(vm))
            // {
            //     ModelState.AddModelError(string.Empty, "some validation error occured");
            //     return View(vm);
            // }
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            var post = vm.ToModel();
            post.ImageName = await _fileService.SaveFileAsync(vm.FormFile, Post.PostBaseDirectory);
            await _context.Posts.AddAsync(post);
            await _context.SaveChangesAsync();
            return LocalRedirect("~/");
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> IndexAsync()
        {
            var posts = await _context.Posts.ToListAsync();
            return View(posts);
        }

        [HttpGet]
        [AllowAnonymous]
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

        [HttpGet]
        public async Task<IActionResult> EditAsync(int id)
        {
            var post = await _context.Posts.IgnoreQueryFilters().FirstOrDefaultAsync(post => post.Id == id);
            if (post == null) return NotFound();
            return View((PostEditViewModel)post.ToViewModel());
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> EditAsync(int id, PostEditViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            var post = await _context.Posts.IgnoreQueryFilters().FirstOrDefaultAsync(post => post.Id == id);
            if (post == null) return BadRequest();
            post.Update((PostViewModel)vm);
            if (vm.FormFile != null)
            {
                _fileService.DeleteFile(post.ImagePath);
                post.ImageName = await _fileService.SaveFileAsync(vm.FormFile, Post.PostBaseDirectory);

            }
            await _context.SaveChangesAsync();
            return LocalRedirect("~/");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var post = await _context.Posts.FirstOrDefaultAsync(p => p.Id == id);
            if (post == null) return BadRequest();
            _context.Entry(post).State = EntityState.Deleted;
            _fileService.DeleteFile(post.ImagePath);
            await _context.SaveChangesAsync();
            TempData["alert-type"] = "success";
            TempData["alert-message"] = "post deleted successfully";
            return RedirectToAction("Index", "Dashboard");

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TrashAsync(int id)
        {
            var post = await _context.Posts.FirstOrDefaultAsync(p => p.Id == id);
            if (post == null) return BadRequest();
            ((ISoftDelete)post).Trash();
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Dashboard").WithDanger(string.Empty, "post deleted successfully");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RestoreAsync(int id)
        {
            var post = await _context.Posts.IgnoreQueryFilters().AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
            if (post == null) return BadRequest();
            var deleteableEntity = (ISoftDelete)post;
            deleteableEntity.Restore();
            _context.Entry((Post)(deleteableEntity)).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Dashboard").WithDanger(string.Empty, "post restored successfully");
        }
    }
}