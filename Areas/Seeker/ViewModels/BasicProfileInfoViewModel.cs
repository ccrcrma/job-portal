using System.ComponentModel.DataAnnotations;

namespace job_portal.Areas.Seeker.ViewModels
{
    public class BasicProfileInfoViewModel
    {
        [Required(ErrorMessage = "{0} is Required")]
        public string Address { get; set; }
        [Required(ErrorMessage = "{0} is Required")]

        [DataType(DataType.PhoneNumber)]
        [RegularExpression("^\\+(?:[0-9]●?){6,14}[0-9]$", ErrorMessage = "NumberFormat eg:+9779812345678")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "{0} is Required")]
        public string Experience { get; set; }

        [Required(ErrorMessage = "{0} is Required")]
        public string Bio { get; set; }

    }
}