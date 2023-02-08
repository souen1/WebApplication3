using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication3.Service;
using WebApplication3.Services;
using WebApplication3.ViewModels;
using WebApplication3.Model;

namespace WebApplication3.Pages
{
    public class LoginModel : PageModel
    {

        private UserManager<EncryptUser> userManager { get; }
        private SignInManager<EncryptUser> signInManager { get; }

        private MembershipService _memberService { get; }
        [BindProperty]
        public Login LModel { get; set; }
        private readonly GoogleCaptchaService _captchaService;

        public LoginModel(UserManager<EncryptUser> userManager,
        SignInManager<EncryptUser> signInManager,
        MembershipService memberService, IWebHostEnvironment environment, GoogleCaptchaService captchaService)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            _captchaService = captchaService;
            _memberService = memberService;
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
                    var NRIC = _memberService.GetByEmail(LModel.Email).NRIC;
                    HttpContext.Session.SetString("NRIC", NRIC);
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