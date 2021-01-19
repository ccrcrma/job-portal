using System.ComponentModel.DataAnnotations;

namespace job_portal.ViewModels
{
    public class JobLinkViewModel
    {
        [Required(ErrorMessage = "{0} is Required")]
        [StringLength(100, ErrorMessage = "{0} must be between {2} and 1", MinimumLength = 2)]
        [Display(Name = "Your Name")]
        public string SenderName { get; set; }

        [Required(ErrorMessage = "{0} is Required")]
        [StringLength(100, ErrorMessage = "{0} must be between {2} and 1", MinimumLength = 2)]
        [Display(Name = "Receiver Name")]
        public string ReceiverName { get; set; }


        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "{0} is Required")]
        [Display(Name = "Your Email")]

        public string SenderEmail { get; set; }


        [Required(ErrorMessage = "{0} is Required")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Receiver Email")]

        public string ReceiverEmail { get; set; }
        public string LinkUrl { get; set; }
        public string Title { get; set; }

    }
}