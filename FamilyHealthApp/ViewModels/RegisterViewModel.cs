using System.ComponentModel.DataAnnotations;

namespace FamilyHealthApp.ViewModel
{
    // Model for register user view
    public class RegisterViewModel
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Username { get; set; }
        
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password must match")]
        public string ConfirmPassword { get; set; }
    }
}