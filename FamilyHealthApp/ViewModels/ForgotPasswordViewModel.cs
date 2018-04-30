using System.ComponentModel.DataAnnotations;

namespace FamilyHealthApp.ViewModel
{
    // Model for forgot password view
    public class ForgotPasswordViewModel
    {   
        [Required]
        [EmailAddress]
        public string Email { get; set; }   
    }
}