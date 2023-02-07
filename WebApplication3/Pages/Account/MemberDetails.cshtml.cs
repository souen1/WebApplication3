using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;

namespace WebApplication3.Pages.Account
{
    [Authorize(Roles = "Member")]
    public class MemberDetailsModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
