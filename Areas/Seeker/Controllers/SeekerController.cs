using System.Linq;
using job_portal.Data;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using job_portal.Areas.Identity.Models;
using job_portal.Extensions;
using job_portal.Areas.Seeker.ViewModels;
using job_portal.Services;
using job_portal.Util;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace job_portal.Areas.Seeker.Controllers
{
    [Area("Seeker")]
    [Authorize]
    public class SeekerController : Controller
    {
        private readonly ApplicationContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IFileStorageService _fileStorageService;
        private readonly ILogger<SeekerController> _logger;

        public SeekerController(ApplicationContext context,
         UserManager<ApplicationUser> userManager,
         IFileStorageService fileStorageService,
         ILogger<SeekerController> logger)
        {
            _context = context;
            _userManager = userManager;
            _fileStorageService = fileStorageService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> IndexAsync()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.Users
                .Include(u => u.SavedJobs)
                    .ThenInclude(sj => sj.Job)
                .FirstOrDefaultAsync(u => u.Id == userId);
            var userSavedJobs = user.SavedJobs.Select(sj => sj.Job).ToList();
            userSavedJobs.RemoveAll(sj => sj == null);
            return View(userSavedJobs);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveJobAsync(int jobId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var job = await _context.Jobs.FirstOrDefaultAsync(j => j.Id == jobId);
            if (job == null)
            {
                return new BadRequestObjectResult(new { Error = "Job with given id doesn't exist" });
            }
            var user = await _userManager.GetUserAsync(User);
            user.SaveJob(jobId);
            await _context.SaveChangesAsync();
            var successMessage = new
            {
                Url = Url.Action("RemoveJob"),
                AlertMessage = "Job Saved Successfully",
                Text = "Remove Job"
            };
            return new OkObjectResult(successMessage);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveJobAsync(int jobId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var job = await _context.Jobs.FirstOrDefaultAsync(j => j.Id == jobId);
            if (job == null)
            {
                return new BadRequestObjectResult(new { Error = "Job with given Id doesn't exist" });
            }

            var user = await _userManager.GetUserAsync(User);
            user.RemoveJob(jobId);
            await _context.SaveChangesAsync();
            var SuccessMessage = new
            {
                Url = Url.Action("SaveJob"),
                AlertMessage = "Job Remove Successfully",
                Text = "Save Job"
            };
            return new OkObjectResult(SuccessMessage);

        }

        [HttpGet]
        public async Task<IActionResult> ProfileAsync()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _context.Users.Include(u => u.Profile).FirstOrDefaultAsync(u => u.Id == userId);
            // var profile = (Profile)ProxyHelper.UnProxy(user.Profile);
            // profile = profile ?? new Profile();
            return View(user);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> ProfileInfoAsync(BasicProfileInfoViewModel vm)
        {
            if (!ModelState.IsValid) { return PartialView("_BasicProfileInfo", vm); }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.Users.Include(u => u.Profile).FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
                return new BadRequestResult();
            user.Profile.Bio = vm.Bio;
            user.Profile.Experience = vm.Experience;
            user.Profile.Address = vm.Address;
            var phoneNumber = string.Join("", vm.PhoneNumber.Split("-"));
            user.PhoneNumber = "+977" + phoneNumber;
            var token = await _userManager.GenerateChangePhoneNumberTokenAsync(user, phoneNumber);

            _logger.LogInformation(token);
            await _context.SaveChangesAsync();
            return new OkResult();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PictureAsync(PictureViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                var result = PartialView("_FormFilePartial", vm);
                result.StatusCode = Microsoft.AspNetCore.Http.StatusCodes.Status400BadRequest;
                return result;
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.Users.Include(u => u.Profile).FirstOrDefaultAsync(u => u.Id == userId);
            if (user.Profile == null)
            {
                user.Profile = new Profile();
            }
            _fileStorageService.DeleteFile(user.Profile.ImagePath);
            var fileName = await _fileStorageService.SaveFileAsync(vm.FormFile,
                job_portal.Areas.Identity.Models.Profile.BaseImageDir);
            user.Profile.ImageName = fileName;
            await _context.SaveChangesAsync();
            return Ok(new { FileName = fileName });

        }
    }
}