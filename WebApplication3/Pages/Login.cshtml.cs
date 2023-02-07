using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication3.Service;
using WebApplication3.Services;
using WebApplication3.ViewModels;

namespace WebApplication3.Pages
{
    public class LoginModel : PageModel
    {

        private UserManager<IdentityUser> userManager { get; }
        private SignInManager<IdentityUser> signInManager { get; }

        [BindProperty]
        public Login LModel { get; set; }
        private readonly GoogleCaptchaService _captchaService;

        public LoginModel(UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager,
        MembershipService memberService, IWebHostEnvironment environment, GoogleCaptchaService captchaService)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            _captchaService = captchaService;
        }


        public void OnGet()
        {
        }


        public async Task<IActionResult> OnPostAsync()
        {
            // verify response token with google
            var captchaResult = await _captchaService.VerifyToken(LModel.Token);
            if (!captchaResult) { return Page(); }
            if (ModelState.IsValid)
            {
                var identityResult = await signInManager.PasswordSignInAsync(LModel.Email, LModel.Password, LModel.RememberMe, true);
                if (identityResult.Succeeded)
                {

                    return RedirectToPage("/Account/MemberDetails");
                }
                else if (identityResult.IsLockedOut)
                {
                    ModelState.AddModelError("", "Your account is locked out. Please wait 10 minutes before try again");
                }

                else
                {
                    ModelState.AddModelError("", "Username or Password incorrect");
                }
            }
            return Page();
        }

    }
}