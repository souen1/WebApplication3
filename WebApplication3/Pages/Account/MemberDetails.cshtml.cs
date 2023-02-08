using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using WebApplication3.Services;
using Microsoft.AspNetCore.Identity;
using WebApplication3.Model;
using WebApplication3.Service;

namespace WebApplication3.Pages.Account
{
    [Authorize(Roles = "Member")]
    public class MemberDetailsModel : PageModel

    {
        public EncryptUser User = null; 
        public MembershipService _memberService { get; }

        public MemberDetailsModel(
        MembershipService memberService)
        {
            
            _memberService = memberService;
        }
        public void OnGet()
        {
            var NRIC = HttpContext.Session.GetString("NRIC");
            User = _memberService.GetByNRIC(NRIC);
        }

    }
}
