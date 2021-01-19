using System.Threading.Tasks;
using job_portal.Models;

namespace job_portal.Services
{
    public interface IMailService
    {
        Task SendMailAsync(MailRequest request);
    }
}