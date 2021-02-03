using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using job_portal.Areas.Identity.Models;
using job_portal.Settings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using Twilio.Rest.Verify.V2.Service;

namespace job_portal.Areas.Identity.Pages.Account
{
    [Authorize]
    public class ConfirmPhoneModel : PageModel
    {
        private readonly TwilioVerifySettings _settings;
        private readonly UserManager<ApplicationUser> _userManager;

        public ConfirmPhoneModel(UserManager<ApplicationUser> userManager,
            IOptions<TwilioVerifySettings> settings)
        {
            _userManager = userManager;
            _settings = settings.Value;
        }

        public string PhoneNumber { get; set; }

        [BindProperty, Required, Display(Name = "Code")]
        public string VerificationCode { get; set; }


        public async Task<IActionResult> OnGetAsync()
        {
            await LoadPhoneNumber();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await LoadPhoneNumber();
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                var verification = await VerificationCheckResource.CreateAsync(
                    to: PhoneNumber,
                    code: VerificationCode,
                    pathServiceSid: _settings.VerificationServiceSID
                );
                if (verification.Status == "approved")
                {
                    var applicationUser = await _userManager.GetUserAsync(User);
                    applicationUser.PhoneNumberConfirmed = true;
                    var updateResult = await _userManager.UpdateAsync(applicationUser);
                    if (updateResult.Succeeded)
                    {
                        return RedirectToPage("ConfirmPhoneSuccess");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "There was an error confirming the verification code, please try again");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, $"There was an error confirming the verification code: {verification.Status}");
                }
            }
            catch (Exception)
            {
                ModelState.AddModelError("",
                    "There was an error confirming the code, please check the verification code is correct and try again");
            }

            return Page();
        }

        private async Task LoadPhoneNumber()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new Exception($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            PhoneNumber = user.PhoneNumber;
        }
    }
}