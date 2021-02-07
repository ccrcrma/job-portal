using System.IO;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace job_portal.Controllers
{
    public class FileController : Controller
    {
        private readonly IConfiguration _configuration;
        public FileController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("download")]
        public IActionResult DownloadFile(string fileName, string documentType, string originalFileName)
        {
            if (string.IsNullOrEmpty(fileName) || string.IsNullOrEmpty(documentType))
            {
                return BadRequest();
            }
            string FilePath;
            string BaseDirectory;
            switch (documentType)
            {
                case "cv":
                    BaseDirectory = _configuration["Documents:Resume"];
                    FilePath = Path.Combine(BaseDirectory, fileName);
                    FilePath = "~/" + FilePath;
                    return File(FilePath, "application/pdf", WebUtility.HtmlDecode(originalFileName));
                case "cover-letter":
                    BaseDirectory = _configuration["Documents:CoverLetter"];
                    FilePath = Path.Combine(BaseDirectory, fileName);
                    FilePath = "~/" + FilePath;
                    return File(FilePath, "application/pdf", WebUtility.HtmlDecode(originalFileName));
                default:
                    return BadRequest();
            }
        }

    }
}