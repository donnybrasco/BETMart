using System.Threading.Tasks;
using BETMart.BLL.Security;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;

namespace BETMart.BLL.Notifications
{
    public interface IMailService
    {
        Task SendAsync(MailModel model);
    }

    public class SMTPMailService : IMailService
    {
        private MailSettings _mailSettings { get; }
        private ILogger<SMTPMailService> _logger { get; }

        public SMTPMailService(IOptions<MailSettings> mailSettings, ILogger<SMTPMailService> logger)
        {
            _mailSettings = mailSettings.Value;
            _logger = logger;
        }

        public async Task SendAsync(MailModel model)
        {
            try
            {
                var email = new MimeMessage { Sender = MailboxAddress.Parse(model.From ?? _mailSettings.From) };
                email.To.Add(MailboxAddress.Parse(model.To));
                email.Subject = model.Subject;
                var builder = new BodyBuilder { HtmlBody = model.Body };
                email.Body = builder.ToMessageBody();
                using var smtp = new SmtpClient();
                await smtp.ConnectAsync(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync(_mailSettings.UserName, _mailSettings.Password);
                await smtp.SendAsync(email);
                await smtp.DisconnectAsync(true);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex.Message, ex);
            }
        }
    }
}
