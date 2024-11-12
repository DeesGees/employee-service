using System.ComponentModel.DataAnnotations;
using TeamApp.Models;

namespace TeamApp.ViewModels
{
    public class RegistrationViewModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Surname { get; set; }
        [Required]
        public DateTime BirthDay { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }
        [Required]
        public string Position { get; set; }
        [Required]
        public string Team { get; set; }

        public string? Role { get; set; }

    }
}
