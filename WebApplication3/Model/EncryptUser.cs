using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace WebApplication3.Model
{
    public class EncryptUser : IdentityUser
    {
        public string FName { get; set; }

        public string LName { get; set; }

        public string Gender { get; set; }

        public string NRIC { get; set; }

        public string BirthDate { get; set; } 
        //[Required]
        public string? Resume { get; set; }

        public string WhoAmI { get; set; }

        public string Token { get; set; }

    }
}
