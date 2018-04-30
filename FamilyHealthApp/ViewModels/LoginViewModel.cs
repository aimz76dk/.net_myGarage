using System.ComponentModel.DataAnnotations;

namespace FamilyHealthApp.ViewModel
{
    // Model for log in view
    public class LoginViewModel
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Username { get; set; }
        
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    
        [Display(Name = "Remember me")]
        public bool RememberMe { get; set; }
    }
}