using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FamilyHealthApp.Models;
using FamilyHealthApp.Services;
using FamilyHealthApp.ViewModel;
using Microsoft.AspNetCore.Identity;

namespace FamilyHealthApp.Controllers
{
    public class MainController : Controller
    {
        // Identity managers 
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        // Email service
        private readonly IEmailSend _emailSend;
        

        public MainController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IEmailSend emailSend)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSend = emailSend;
        }
        
        [HttpGet]
        public IActionResult Index()
        {
            return View();
            
        }

        // Trying to log in
        // Checks for user inputs and determine what to do
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var loginResults = await _signInManager.PasswordSignInAsync(model.Username, model.Password,
                    model.RememberMe, lockoutOnFailure: false);
                
                if (loginResults.Succeeded)
                {
                    return RedirectToAction("Index", "LoggedIn");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login information");
                    return View(model);
                }
            }
           
            return View(model);
            
        }


        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // Trying to register as a user
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Set new user object
                var identityUser = new ApplicationUser
                {
                    UserName = model.Username,
                    Email = model.Username,
                };

                // Create the user with usermanager
                var identityResults = await _userManager.CreateAsync(identityUser, model.Password);

                if (identityResults.Succeeded)
                {
                    // Code is the email confirmation token
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(identityUser);
                    // Set the url / method to call when user hits the link
                    var callbackUrl = Url.Action("ConfirmEmail", "Main", new { userId = identityUser.Id, code = code },
                                                 protocol: HttpContext.Request.Scheme);
                    // Send email to user with the confirmation link
                    await _emailSend.SendEmailAsync(model.Username, "Confirm password", "Please confirm your password by" +
                                                    $"clicking this link: {callbackUrl}");
       
                    // Sign user in
                    await _signInManager.SignInAsync(identityUser, isPersistent: false);
                    return RedirectToAction("Index", "LoggedIn");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Error while creating user");
                    return View(model);
                }
            }
            return View(nameof(Index));
        }

        // This will be called when the user hits the email confirmation link 
        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            // Check to parameters 
            if (userId == null || code == null )
            {
                return View("Error");
            }
            // Try to find the user
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return View("Error");
            }
            // Set users ConfirmedEmail field to true
            var result = await _userManager.ConfirmEmailAsync(user, code);
            return View("ConfirmEmail");

        }
        
        
        [HttpGet]
        public IActionResult ResetPassword()
        {
            return View();
        }

        // Trying to reset the password 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Find user
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    return RedirectToAction("Index", "Main");
                }
                // Resets to the new password
                var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "LoggedIn");
                }
            }
            return View(nameof(Index));
        }
        
        
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    return View("Error");
                }

                // Code is the reset password token used by ResetPassword method in Identity
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                // Set the url / method to call when user hits the link
                var callbackUrl = Url.Action("ResetPassword", "Main", new { userId = user.Id, code = code },
                                           protocol: HttpContext.Request.Scheme);
                // Send the link thru email
                await _emailSend.SendEmailAsync(model.Email, "Reset password", "Please reset your password by" +
                                                $"clicking this link: {callbackUrl}");
            }

            return View("ForgotPasswordConfirmation");
        }

        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        // Logging off and going back 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOff()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Main");
        }


    }
}