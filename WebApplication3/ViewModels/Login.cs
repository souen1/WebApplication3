using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication3.ViewModels
{
    public class Login
    {
        

        [Required]
        [DataType(DataType.EmailAddress)]
        [Key]
        public string Email { get; set; }

        [Required, RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{12,}$", ErrorMessage = "Passwords must be at least 8 characters long and contain at least an upper case letter, lower case letter, digit and a symbol.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        public string Token { get; set; }

        
        public bool RememberMe { get; set; }

        
    }
}
