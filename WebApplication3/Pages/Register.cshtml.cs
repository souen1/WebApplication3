using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Nancy.Json;
using System.Net;
using System.Text.Encodings.Web;
using WebApplication3.Service;
using WebApplication3.Services;
using WebApplication3.ViewModels;

//using System.Text.RegularExpressions;
//using System.Drawing;

namespace WebApplication3.Pages
{
    public class RegisterModel : PageModel
    {

        private UserManager<IdentityUser> userManager { get; }
        private SignInManager<IdentityUser> signInManager { get; }

        private RoleManager<IdentityRole> roleManager { get; }

        private readonly MembershipService _membershipservice;

        private readonly GoogleCaptchaService _captchaService;

        private IWebHostEnvironment _webhostenvironment;

        [BindProperty]
        public Register RModel { get; set; }

        [BindProperty]
        public IFormFile? Upload { get; set; }



        public RegisterModel(UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager, RoleManager<IdentityRole> roleManager,MembershipService membershipService, IWebHostEnvironment webHostEnvironment, GoogleCaptchaService captchaService)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
            _membershipservice = membershipService;
            _webhostenvironment = webHostEnvironment;
            _captchaService = captchaService;
        }



        public void OnGet()
        {
        }


        public async Task<IActionResult> OnPostAsync()
        {
            // verify response token with google
            var captchaResult = await _captchaService.VerifyToken(RModel.Token);
            if (!captchaResult) { return Page(); }

            if (ModelState.IsValid)
            {
                Register? register = _membershipservice.GetById(RModel.Email);
                if (register != null)
                {
                    ModelState.AddModelError("RModel.Email", "Email already exixts.");
                    return Page();
                }

                if (Upload != null)
                {
                    if (Upload.Length > 2 * 1024 * 1024)
                    {
                        ModelState.AddModelError("Upload", "File size cannot exceed 2MB.");
                        return Page();
                    }

                    var uploadsFolder = "uploads";
                    var docFile = Guid.NewGuid() + Path.GetExtension(Upload.FileName);
                    var imagePath = Path.Combine(_webhostenvironment.ContentRootPath, "wwwroot", uploadsFolder, docFile);
                    using var fileStream = new FileStream(imagePath, FileMode.Create);
                    await Upload.CopyToAsync(fileStream);
                    RModel.Resume = string.Format("/{0}/{1}", uploadsFolder, docFile);
                }

                
                var user = new IdentityUser()
                {
                    UserName = RModel.Email,
                    Email = RModel.Email
                };
                IdentityRole role = await roleManager.FindByIdAsync("Member");
                if (role == null)
                {
                    IdentityResult result2 = await roleManager.CreateAsync(new IdentityRole("Member"));
                    if (!result2.Succeeded)
                    {
                        ModelState.AddModelError("", "Create member role failed");
                    }
                }
                var result = await userManager.CreateAsync(user, RModel.Password);
                if (result.Succeeded)
                {
                                      
                    result = await userManager.AddToRoleAsync(user, "Member");

                    _membershipservice.AddUser(RModel);
                    
                    return RedirectToPage("Login");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return Page();

                
            }
            return Page();
        }

        //public bool ValidateCaptcha()
        //{
        //    string Response = Request["g-captcha-response"]; // Getting Response String Append to Post Method
        //    bool Valid = false;
        //    // request too google server
        //    HttpWebRequest req = (HttpWebRequest)WebRequest.Create
        //        ("https://www.google.com/recaptcha/api/siteverify?secret=6Lcj91MkAAAAAE-rL7MBvvXRrMVI3lP3mC8j8GXx &response=" + Response);
        //    try
        //    {
        //        //google captcha response
        //        using (WebResponse wResponse = req.GetResponse())
        //        {
        //            using (StreamReader readStream = new StreamReader(wResponse.GetResponseStream()))
        //            {
        //                string jsonResponse = readStream.ReadToEnd();

        //                JavaScriptSerializer js = new JavaScriptSerializer();
        //                MyObject data = js.DeserializeObject<MyObject>(jsonResponse); // Deserialize Json

        //                Valid = Convert.ToBoolean(data.success);
        //            }
        //        }

        //        return Valid;
        //    }
        //    catch (WebException ex)
        //    {
        //        throw ex;
        //    }
        //}





    }
}
