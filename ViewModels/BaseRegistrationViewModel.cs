using System.ComponentModel.DataAnnotations;

namespace job_portal.ViewModels
{
    public class BaseRegistrationViewModel
    {
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "{0} is Required")]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "{0} is Required")]
        [StringLength(100, ErrorMessage = "{0} must be between {2} and {1} characters", MinimumLength = 5)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password")]
        [Required(ErrorMessage = "{0} is Required")]
        [StringLength(100, ErrorMessage = "{0} must be between {2} and {1} characters", MinimumLength = 5)]
        public string ConfirmPassword { get; set; }
    }
}