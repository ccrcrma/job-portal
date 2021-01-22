using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace job_portal.Services
{
    public interface IFileStorageService
    {
        void DeleteFile(string filePath);
        Task<string> SaveFileAsync(IFormFile formFile, string baseDirectoryLocation);
    }
}