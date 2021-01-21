using job_portal.Models;
using System.Threading.Tasks;
using job_portal.Services;
using Microsoft.AspNetCore.Mvc;
using job_portal.ViewModels;
using System.Net;

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
                var result = PartialView("_InvitationFormData", vm);
                result.StatusCode = (int?)HttpStatusCode.BadRequest;
                return result;
            }

            var mailRequest = new MailRequest();
            mailRequest.To = vm.ReceiverEmail;
            mailRequest.Subject = $"Vacancy for {vm.Title}";
            mailRequest.Body = $"Hello, {vm.ReceiverName}, {vm.SenderName}({vm.SenderEmail}) has send you link for <a href=\"{vm.LinkUrl}\"> click here</a>";
            await _mailService.SendMailAsync(mailRequest);
            TempData["alert-type"] = "success";
            TempData["alert-title"] = "Hurray";
            TempData["alert-message"] = "job link has been successfully sent";
            return Ok();
        }
    }
}