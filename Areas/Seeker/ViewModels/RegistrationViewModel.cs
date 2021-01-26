using System;
using System.ComponentModel.DataAnnotations;
using job_portal.Areas.Seeker.Types;
using job_portal.ViewModels;

namespace job_portal.Areas.Seeker.ViewModels
{
    public class RegistrationViewModel : BaseRegistrationViewModel
    {
        [Required(ErrorMessage = "{0} is Required")]
        [Display(Name = "First Name")]
        [StringLength(100, ErrorMessage = "{0} must be between {2} and {1} characters", MinimumLength = 2)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "{0} is Required")]
        [Display(Name = "Last Name")]
        [StringLength(100, ErrorMessage = "{0} must be between {2} and {1} characters", MinimumLength = 2)]
        public string LastName { get; set; }

        [Display(Name = "Middle Name")]
        [StringLength(100, ErrorMessage = "{0} must be between {2} and {1} characters", MinimumLength = 2)]
        public string MiddleName { get; set; }

        [Required(ErrorMessage = "{0} is Required")]
        [DataType(DataType.Date)]
        [Display(Name = "Date Of Birth")]
        public DateTime DOB { get; set; }

        [Required(ErrorMessage = "{0} is Required")]
        public Gender Gender { get; set; }

    }
}