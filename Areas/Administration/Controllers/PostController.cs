using job_portal.Data;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using job_portal.Areas.Administration.ViewModels;
using job_portal.Services;
using job_portal.Areas.Administration.Models;
using System.IO;

namespace job_portal.Areas.Administration.Controllers
{
    [Area("Administration")]
    [Route("[controller]/[action]")]
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

        [HttpGet("{id}")]
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

        [HttpGet("{id}")]
        public async Task<IActionResult> EditAsync(int id)
        {
            var post = await _context.Posts.IgnoreQueryFilters().FirstOrDefaultAsync(post => post.Id == id);
            if (post == null) return NotFound();
            return View((PostEditViewModel)post.ToViewModel());
        }
        [HttpPost("{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAsync(int id, PostEditViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            var post = await _context.Posts.IgnoreQueryFilters().FirstOrDefaultAsync(post => post.Id == id);
            if (post == null) return BadRequest();
            post.Update((PostViewModel)vm);
            if(vm.FormFile!=null){
                _fileService.DeleteFile(post.ImagePath);
                post.ImageName = await _fileService.SaveFileAsync(vm.FormFile, Post.PostBaseDirectory);

            }
            await _context.SaveChangesAsync();
            return LocalRedirect("~/");
        }
    }
}