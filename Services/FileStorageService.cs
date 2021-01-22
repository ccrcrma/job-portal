using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace job_portal.Services
{
    public class FileStorageService : IFileStorageService
    {
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _config;

        public FileStorageService(IWebHostEnvironment env, IConfiguration config)
        {
            _env = env;
            _config = config;
        }
        public void DeleteFile(string filePath)
        {
            var absFilePath = Path.Combine(_env.WebRootPath, filePath);
            if (System.IO.File.Exists(absFilePath))
            {
                System.IO.File.Delete(absFilePath);
            }
        }
        public async Task<string> SaveFileAsync(IFormFile formFile, string baseDirectoryLocation)
        {
            var folderLocation = Path.Combine(_env.WebRootPath, baseDirectoryLocation);
            if (!Directory.Exists(folderLocation))
            {
                Directory.CreateDirectory(folderLocation);
            }
            var generatedFileName = Guid.NewGuid().ToString() + Path.GetExtension(formFile.FileName);
            var fileAbsPath = Path.Combine(folderLocation, generatedFileName);
            if (!System.IO.File.Exists(fileAbsPath))
            {
                using (var stream = new FileStream(fileAbsPath, FileMode.Create))
                {
                    await formFile.CopyToAsync(stream);
                }
            }
            return generatedFileName;
        }
    }
}