using System;
using System.ComponentModel.DataAnnotations;
using job_portal.Areas.Employer.Models;

namespace job_portal.Areas.Employer.ViewModels
{
    public class CompanyViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        [Display(Name = "Address")]
        [Required(ErrorMessage = "{0} is Required")]
        [StringLength(200, ErrorMessage = "{0} must be between {2} and {1} characters", MinimumLength = 10)]
        public string Location { get; set; }

        [Required(ErrorMessage = "{0} is Required")]
        [StringLength(1000, ErrorMessage = "{0} must be greater than {2} characters", MinimumLength = 5)]

        public string Info { get; set; }

        [Required(ErrorMessage = "{0} is Required")]
        [StringLength(1000, ErrorMessage = "{0} must be greater than {2} characters", MinimumLength = 5)]


        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        [Required(ErrorMessage = "{0} is Required")]
        [DataType(DataType.Url)]
        [StringLength(100, ErrorMessage = "{0} msut be greater than {2} characters", MinimumLength = 10)]
        public string Website { get; set; }

        [Required(ErrorMessage = "{0} is Required")]
        [StringLength(100, ErrorMessage = "{0} must be greater than {2} characters", MinimumLength = 5)]
        public string Slogan { get; set; }
        public string ImagePath { get; set; }

        public Company ToModel(Company company)
        {
            company.Phone = Phone;
            company.Slogan = Slogan;
            company.Info = Info;
            company.Location = Location;
            company.Website = Website;
            return company;
        }

    }
}