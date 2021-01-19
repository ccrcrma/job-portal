using System.Threading.Tasks;
using job_portal.Models;
using job_portal.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace job_portal.Services
{
    public class MailService : IMailService
    {
        private readonly MailSettings _mailSettings;
        private readonly ILogger _logger;
        public MailService(ILogger<MailService> logger, IOptions<MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
            _logger = logger;
        }
        public Task SendMailAsync(MailRequest request)
        {
            _logger.LogInformation("mail send successfully");
            return Task.CompletedTask;
        }
    }
}