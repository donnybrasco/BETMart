using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using BETMart.BLL._Core;
using BETMart.BLL.Models;
using BETMart.Common;
using Microsoft.Extensions.Configuration;

namespace BETMart.BLL.Notifications
{
    public interface INotificationService
    {
        Task<Response> Send(IMailTemplate mailTemplate);
    }

    public class NotificationService
        : INotificationService
    {
        private readonly IMailService _mailService;
        private readonly IConfiguration _configuration;

        public NotificationService(IMailService mailService, IConfiguration configuration)
        {
            _mailService = mailService;
            _configuration = configuration;
        }
        public async Task<Response> Send(IMailTemplate mailTemplate)
        {
            try
            {
                var mailerEngine = new RazorMailerEngine(_configuration["MailSettings:DefaultEmailTemplateFolder"], _configuration["MailSettings:FromEmailAddress"], "BETMart Notification Service", _mailService);

                var htmlBody = mailerEngine.Create(mailTemplate.TemplateName, mailTemplate);

                var fromAddress = new MailAddress(_configuration["MailSettings:From"], mailTemplate.DisplayName);

                using (var message = new MailMessage()
                {
                    From = fromAddress,
                    Subject = mailTemplate.Subject,
                    IsBodyHtml = true,
                    Body = htmlBody
                })
                {
                    string[] listOfAddresses = mailTemplate.ToAddress.Split(new[] { ",", }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var address in listOfAddresses)
                    {
                        message.To.Add(address);
                    }

                    if (!string.IsNullOrEmpty(mailTemplate.AttachmentFileName))
                    {
                        var attachment = new Attachment(mailTemplate.AttachmentFileName);
                        message.Attachments.Add(attachment);
                    }

                    if (mailTemplate.AttachmentStream != null && mailTemplate.AttachmentStream.Length > 0)
                    {
                        var attachment = new Attachment(mailTemplate.AttachmentStream, mailTemplate.DocumentName, "application/pdf");
                        message.Attachments.Add(attachment);
                    }
                    await mailerEngine.SendAsync(message);
                }
                //
                return new Response
                {
                    IsSuccessful = true,
                    Message = "Successfully sent"
                };
            }
            catch (Exception ex)
            {
                return Response.Fail(ex.Message);
            }
        }
    }
}