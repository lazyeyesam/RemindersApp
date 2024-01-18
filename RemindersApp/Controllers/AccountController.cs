using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RemindersApp.Models;
using System.Security.Claims;

namespace RemindersApp.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<AppUser> _userManager;
        private SignInManager<AppUser> _signInManager;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Login(string returnUrl, string error)
        {
            var viewModel = new LoginViewModel();
            viewModel.ReturnUrl = returnUrl;
            viewModel.Error = error;
            return View(viewModel);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel viewModel)
        {
            // check the form is returned okay
            if (!ModelState.IsValid)
                return RedirectToAction("Login", new { viewModel.ReturnUrl, viewModel.Error });

            // get the user by their email address
            var appUser = await _userManager.FindByEmailAsync(viewModel.Email);
            if (appUser != null)
            {
                // sign everyone out
                await _signInManager.SignOutAsync();

                //try to login the user by checking the email and password
                var result = await _signInManager.PasswordSignInAsync(appUser, viewModel.Password, false, false);
                if (result.Succeeded)
                    return Redirect(viewModel.ReturnUrl ?? "/");
            }

            // if there is a problem, return to the login page to try again
            viewModel.Error = "Invalid username or password";
            return RedirectToAction("Login", new { viewModel.ReturnUrl, viewModel.Error });
        }

        public ActionResult GoogleSignIn()
        {
            var redirectUrl = Url.Action("GoogleResponse", "Account");
            var properties = _signInManager.ConfigureExternalAuthenticationProperties("Google", redirectUrl);
            return new ChallengeResult("Google", properties);
        }

        public async Task<IActionResult> GoogleResponse()
        {
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
                return RedirectToAction(nameof(Login));

            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, false);
            string[] userInfo =
                {
                    info.Principal.FindFirst(ClaimTypes.Name)!.Value,
                    info.Principal.FindFirst(ClaimTypes.Email)!.Value
                };
            
            if (result.Succeeded)
                return RedirectToAction("Index", "Home");
            else
            {
                var user = new AppUser
                {
                    Email = info.Principal.FindFirst(ClaimTypes.Email)!.Value,
                    UserName = info.Principal.FindFirst(ClaimTypes.Email)!.Value
                };

                var identResult = await _userManager.CreateAsync(user);
                if (identResult.Succeeded)
                {
                    identResult = await _userManager.AddLoginAsync(user, info);
                    if (identResult.Succeeded)
                    {
                        await _signInManager.SignInAsync(user, false);
                        return View(userInfo);
                    }
                }
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateUser(AppUserViewModel viewModel)
        {
            // check the form returned values okay
            if (!ModelState.IsValid)
                return RedirectToAction("Register");

            // check the passwords match
            if (viewModel.Password != viewModel.ConfirmPassword)
            {
                viewModel.Error = "Passwords do not match";
                return RedirectToAction("Register", viewModel);
            }

            // create a user
            var appUser = new AppUser
            {
                UserName = viewModel.Email,
                Email = viewModel.Email
            };

            // try to add the user to the database
            var result = await _userManager.CreateAsync(appUser, viewModel.Password);
            if (result.Succeeded)
                return RedirectToAction("Login", "Account");

            // return any errors to the register page e.g. password too short, user exists already
            viewModel.Error = result.Errors.ToList().First().Description;
            return RedirectToAction("Register", viewModel);
        }

        public IActionResult Register(AppUserViewModel viewModel)
        {
            return View(viewModel);
        }

        [Authorize]
        public async Task<IActionResult> ChangePassword()
        {
            var viewModel = new AppUserViewModel();
            viewModel.Email = User!.Identity!.Name!;
            return View(viewModel);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(AppUserViewModel viewModel)
        {
            // check the form returned values okay
            if (!ModelState.IsValid)
                return RedirectToAction("Index", "Home");

            // check the passwords match
            if (viewModel.Password != viewModel.ConfirmPassword)
            {
                viewModel.Error = "Passwords do not match";
                return RedirectToAction("ChangePassword", viewModel);
            }

            var appUser = await _userManager.FindByEmailAsync(viewModel.Email);
            if (appUser == null)
                return RedirectToAction("Index", "Home");

            // try to add the user to the database
            var result = await _userManager.ChangePasswordAsync(appUser, viewModel.CurrentPassword, viewModel.Password);
            if (result.Succeeded)
                return RedirectToAction("Index", "Home");

            // return any errors to the register page e.g. password too short, user exists already
            viewModel.Error = result.Errors.ToList().First().Description;

            return RedirectToAction("ChangePassword", "Account");
        }
    }
}
