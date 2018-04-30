using System.ComponentModel.DataAnnotations;

namespace FamilyHealthApp.ViewModel
{
    // Model for resetting password view
    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        
        [Required]
        [StringLength(60, ErrorMessage = "Password must be between 6 and 60 charaters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "Both passwords must match")]
        public string ConfirmPassword { get; set; }

        public string Code
        { get; set; }
    }
}