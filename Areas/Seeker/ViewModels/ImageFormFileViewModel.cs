using System.ComponentModel.DataAnnotations;
using job_portal.ValidationAttributes;
using Microsoft.AspNetCore.Http;

namespace job_portal.Areas.Seeker.ViewModels
{
    public class ImageFormFileViewModel
    {
        [Required]
        [MaxFileSize(2000)]
        [AllowedFileExtension(".png", ".jpg", ".jpeg")]
        [ValidateFileSignature]
        public IFormFile FormFile { get; set; }

    }
}