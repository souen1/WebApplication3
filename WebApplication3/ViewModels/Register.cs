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
    public class Register
    {
        [Required]
        public string FName { get; set; }

        [Required]
        public string LName { get; set; }

        [Required]
        public string Gender { get; set; }

        [Required, RegularExpression(@"^[STFG]\d{7}[A-Z]$", ErrorMessage = "Invalid NRIC."), MaxLength(9)]
        
        public string NRIC { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Birthday")]
        [Column(TypeName = "date")]
        public DateTime BirthDate { get; set; } = new DateTime(DateTime.Now.Year - 18, 1, 1);

        //[Required]
        public string? Resume { get; set; }

        [Required]
        public string WhoAmI { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [Key]
        public string Email { get; set; }

        [Required, RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{12,}$", ErrorMessage = "Passwords must be at least 8 characters long and contain at least an upper case letter, lower case letter, digit and a symbol.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Password and confirmation password does not match")]
        public string ConfirmPassword { get; set; }

        [Required]
        public string Token { get; set; }

    }
}
