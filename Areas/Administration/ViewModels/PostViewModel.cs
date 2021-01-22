using System;
using System.ComponentModel.DataAnnotations;
using job_portal.Areas.Administration.Models;
using job_portal.ValidationAttributes;
using Microsoft.AspNetCore.Http;

namespace job_portal.Areas.Administration.ViewModels
{
    public class PostViewModel
    {
        [Required(ErrorMessage = "{0} is Required")]
        [StringLength(200, ErrorMessage = "{0} must be between {2} and {1} characters long")]
        public string Title { get; set; }


        [Required(ErrorMessage = "{0} is Required")]
        public string Content { get; set; }

        [Required]
        [Display(Name = "Picture")]
        [MaxFileSize(5000)]
        [AllowedFileExtension(".png", ".jpeg", ".jpg")]
        [ValidateFileSignature]
        public IFormFile FormFile { get; set; }

        public string ImagePath { get; set; }
        public int Id { get; set; }
        public DateTime CreatedOn { get; set; }





        public Post ToModel()
        {
            return new Post
            {
                Id= Id,
                CreatedOn = CreatedOn,
                Title = Title,
                Body = Content
            };
        }

    }
}