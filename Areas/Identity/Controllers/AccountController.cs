using System.Diagnostics;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using job_portal.Areas.Identity.Models;
using job_portal.Areas.Identity.ViewModels;
using job_portal.Extensions;
using job_portal.Models;
using job_portal.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;

namespace job_portal.Areas.Identity.Controllers
{
    [Area("Identity")]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<AccountController> _logger;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IMailService _mailService;

        public AccountController(UserManager<ApplicationUser> userManager,
            ILogger<AccountController> logger,
            SignInManager<ApplicationUser> signInManager,
            IMailService mailService)
        {
            _userManager = userManager;
            _logger = logger;
            _signInManager = signInManager;
            _mailService = mailService;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignInAsync(LoginViewModel vm)
        {
            if (!ModelState.IsValid) return View();
            var user = await _userManager.FindByEmailAsync(vm.Email);
            if (user != null)
            {
                var result = await _signInManager.PasswordSignInAsync(user, vm.Password, vm.RememberMe, false);
                if (result.Succeeded)
                {
                    return Ok(new { Url = "/" });
                }
                else
                {
                    if (result.IsNotAllowed)
                    {
                        ModelState.AddModelError("errors", "Please Verify Email to logIn");
                    }
                    else if (result.IsLockedOut)
                    {
                        ModelState.AddModelError("errors", "User is locked out");
                    }
                    else if (result.RequiresTwoFactor)
                    {
                        ModelState.AddModelError("errors", "Requires Two factor authentication");
                    }
                    else
                    {
                        ModelState.AddModelError("errors", "Username or Password Incorrect");
                    }
                    return new BadRequestObjectResult(ModelState);
                }

            }
            else
            {
                ModelState.AddModelError("errors", "UserName Or Password Incorrect");
                return new BadRequestObjectResult(ModelState);
            }


        }


        [HttpGet]
        public IActionResult ResetPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("ResetPassword")]
        public async Task<IActionResult> ResetPasswordAsync(string email)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) return BadRequest();
            await SendResetPasswordLink(user);
            return RedirectToAction("ResetPassword")
                .WithSuccess($"password reset link has been sent to your email {email}", string.Empty);
        }

        private async Task SendResetPasswordLink(ApplicationUser user)
        {
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var callbackUrl = Url.Action("ConfirmResetPassword", "Account", new { Area = "Identity", code = code, userId = user.Id }, Request.Scheme);
            var messageBody = $"Plase <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'> click here </a> to reset your password ";
            MailRequest mailRequest = new MailRequest
            {
                To = user.Email,
                Subject = "Password Reset",
                Body = messageBody
            };
            await _mailService.SendMailAsync(mailRequest);

        }

        [HttpGet]
        public async Task<IActionResult> ConfirmResetPasswordAsync(string code, string userId)
        {
            if (string.IsNullOrEmpty(code) && string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Error", "Home");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return RedirectToAction("Error", "Home");
            }

            var codeBytes = WebEncoders.Base64UrlDecode(code);
            code = Encoding.UTF8.GetString(codeBytes);
            var result = await _userManager.VerifyUserTokenAsync(
                user, _userManager.Options.Tokens.PasswordResetTokenProvider, "ResetPassword", code);
            ViewBag.token = code;
            ViewBag.userId = userId;
            if (result)
            {
                return View();
            }
            return RedirectToAction("Error", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmResetPasswordAsync(string password, string token, string userId)
        {
            if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(token) || string.IsNullOrEmpty(userId))
            {
                return BadRequest();
            }
            var user = await _userManager.FindByIdAsync(userId);
            var res = await _userManager.ResetPasswordAsync(user, token, password);
            if (res.Succeeded)
            {
                return LocalRedirect("~/").WithSuccess("congrats", "your password reset was successful");
            }
            return RedirectToAction("Error", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogoutAsync()
        {
            await _signInManager.SignOutAsync();
            return LocalRedirect("~/");
        }

        [HttpGet]
        public IActionResult RegisterSeekerUser()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterSeekerUserAsync(SeekerUserRegistrationViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);
            var user = new ApplicationUser
            {
                Email = vm.Email,
                FirstName = vm.FirstName,
                LastName = vm.LastName,
                MiddleName = vm.MiddleName,
                Gender = vm.Gender,
                DOB = vm.DOB,
                UserName = vm.Email
            };
            var result = await _userManager.CreateAsync(user, vm.Password);
            if (result.Succeeded)
            {
                _logger.LogInformation("user created successfully");
                await SendEmailConfirmationLinkAsync(user);
                if (_userManager.Options.SignIn.RequireConfirmedEmail)
                {
                    return RedirectToAction("RegisterConfirmation", new { email = vm.Email })
                        .WithSuccess("New user created successfully", string.Empty);
                }
                else
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return LocalRedirect("~/").WithSuccess("New user created and signed In ", string.Empty);
                }
            }

            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                    return View(vm);
                }
            }
            return LocalRedirect("~/").WithSuccess("User created successfully", "Verify Email to login");
        }

        private async Task SendEmailConfirmationLinkAsync(ApplicationUser user)
        {
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var callbackUrl = Url.Action(
                "ConfirmEmail",
                "Account",
                new { Area = "Identity", userId = user.Id, code = code },
                Request.Scheme);

            var mailRequest = new MailRequest()
            {
                To = user.Email,
                Subject = "Email Confirmation",
                Body = $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>."

            };
            await _mailService.SendMailAsync(mailRequest);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendEmailConfirmationLinkAsync(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return BadRequest();
            }
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return BadRequest();
            }
            await SendEmailConfirmationLinkAsync(user);
            return RedirectToAction("RegisterConfirmation", new { email = email });
        }
        [HttpGet]
        public IActionResult RegisterEmployerUser()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> RegisterConfirmationAsync(string email)
        {
            if (email == null) return LocalRedirect("~/");
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return NotFound($"Unable to load user with email '{email}'");
            }
            ViewBag.Email = email;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RegisterEmployerUser(EmployerUserRegistrationViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);
            return LocalRedirect("~/").WithSuccess("company account created successfully", string.Empty);
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmailAsync(string userId, string code)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return LocalRedirect("~/");
            var decodedCodeByteArray = WebEncoders.Base64UrlDecode(code);
            var decodedString = Encoding.UTF8.GetString(decodedCodeByteArray);
            var result = await _userManager.ConfirmEmailAsync(user, decodedString);
            if (result.Succeeded) return View("ConfirmEmailThanks");
            return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}