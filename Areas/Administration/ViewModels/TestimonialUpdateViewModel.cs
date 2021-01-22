using System.ComponentModel.DataAnnotations;
using System.IO;
using job_portal.Areas.Administration.Models;
using job_portal.Models;
using job_portal.ValidationAttributes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace job_portal.Areas.Administration.ViewModels
{
    public class TestimonialUpdateViewModel
    {
        [Required(ErrorMessage = "{0} is required")]
        [Display(Name = "Person Name")]
        [StringLength(100, ErrorMessage = "{0} must be between {2} and {1}", MinimumLength = 2)]
        public string PersonName { get; set; }

        [Required(ErrorMessage = "{0} is Required")]
        [StringLength(50, ErrorMessage = "{0} must be between {2} and {1}", MinimumLength = 2)]
        public string Designation { get; set; }

        [Required(ErrorMessage = "{0} is Required")]
        [StringLength(500, ErrorMessage = "{0} must be between {2} and {1}", MinimumLength = 3)]
        public string Message { get; set; }

        [Display(Name = "Change Picture")]
        [MaxFileSize(sizeInKB: 200)]
        [AllowedFileExtension(".png", ".jpeg", ".jpg")]
        [ValidateFileSignature]
        public IFormFile FormFile { get; set; }
        public string ImagePath { get; set; }
        public Testimonial ToModel()
        {
            Testimonial testimonial = new Testimonial
            {
                Name = PersonName,
                Description = Message,
                Designation = Designation
            };
            return testimonial;
        }

        public static explicit operator TestimonialViewModel(TestimonialUpdateViewModel updateViewModel)
        {
            return new TestimonialViewModel
            {
                ImagePath = updateViewModel.ImagePath,
                PersonName = updateViewModel.PersonName,
                Designation = updateViewModel.Designation,
                Message = updateViewModel.Message,
                FormFile = updateViewModel.FormFile
            };
        }
    }
}