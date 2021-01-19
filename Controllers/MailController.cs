using job_portal.Models;
using System.Threading.Tasks;
using job_portal.Services;
using Microsoft.AspNetCore.Mvc;
using job_portal.ViewModels;

namespace job_portal.Controllers
{
    public class MailController : Controller
    {
        private readonly IMailService _mailService;


        public MailController(IMailService mailService)
        {
            _mailService = mailService;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MailLinkToFriendAsync(JobLinkViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                
            }
            var mailRequest = new MailRequest();
            await _mailService.SendMailAsync(mailRequest);
            return RedirectToAction("Home", "Home");
        }
    }
}