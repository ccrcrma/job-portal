using System.Threading.Tasks;
using job_portal.Models;
using job_portal.Settings;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;

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
        public async Task SendMailAsync(MailRequest request)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("JobPortal", "Jobportal@jobportal.com"));
            message.To.Add(new MailboxAddress(request.To, request.To));
            message.Subject = request.Subject;
            message.Body = new TextPart("html")
            {
                Text = request.Body
            };

            var client = new SmtpClient();
            await client.ConnectAsync(_mailSettings.Host, _mailSettings.Port);
            await client.AuthenticateAsync(_mailSettings.UserName, _mailSettings.Password);
            await client.SendAsync(message);
            client.Dispose();
            _logger.LogInformation("mail send successfully");
        }
    }
}