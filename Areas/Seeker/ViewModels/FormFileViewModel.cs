using System.ComponentModel.DataAnnotations;
using job_portal.ValidationAttributes;
using Microsoft.AspNetCore.Http;

namespace job_portal.Areas.Seeker.ViewModels
{
    public class FormFileViewModel
    {
        [Required]
        [MaxFileSize(3000)]
        [AllowedFileExtension(".pdf")]
        [ValidateFileSignature]
        public IFormFile FormFile { get; set; }
        public string Document { get; set; }
    }
}