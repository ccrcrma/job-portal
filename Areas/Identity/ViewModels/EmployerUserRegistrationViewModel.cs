using System.ComponentModel.DataAnnotations;
using job_portal.ViewModels;

namespace job_portal.Areas.Identity.ViewModels
{
    public class EmployerUserRegistrationViewModel : BaseRegistrationViewModel
    {
        [Required(ErrorMessage = "{0} is Required")]
        [Display(Name = "Company Name")]
        [StringLength(200, ErrorMessage = "{0} must be between {2} and {1}", MinimumLength = 3)]
        public string CompanyName { get; set; }

    }
}