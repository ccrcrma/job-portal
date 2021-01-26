using System.ComponentModel.DataAnnotations;

namespace job_portal.ViewModels
{
    public class LoginViewModel
    {
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "{0} is Required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "{0} is Required")]
        [StringLength(20, ErrorMessage = "{0} must be between {2} and {1} characters", MinimumLength=5)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; }






    }
}