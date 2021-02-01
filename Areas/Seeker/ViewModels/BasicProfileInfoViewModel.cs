using System.ComponentModel.DataAnnotations;

namespace job_portal.Areas.Seeker.ViewModels
{
    public class BasicProfileInfoViewModel
    {
        [Required(ErrorMessage = "{0} is Required")]
        public string Address { get; set; }
        [Required(ErrorMessage = "{0} is Required")]

        [DataType(DataType.PhoneNumber)]
        [RegularExpression("98[0-9]{2}-[0-9]{2}-[0-9]{2}-[0-9]{2}", ErrorMessage ="Phone Number Format 9812-23-24-24")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "{0} is Required")]
        public string Experience { get; set; }

        [Required(ErrorMessage = "{0} is Required")]
        public string Bio { get; set; }

    }
}