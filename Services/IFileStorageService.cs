using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace job_portal.Services
{
    public interface IFileStorageService
    {
        Task<string> SaveFileAsync(IFormFile formFile, string baseDirectoryLocation);
    }
}