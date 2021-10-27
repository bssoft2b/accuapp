using AccuApp32MVC.Models.Services;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuApp.Services
{
    public class EmailSender : IEmailSender
    {
        public EmailSender(IOptions<AuthMessageSenderOptions> optionsAccessor)
        {
            Options = optionsAccessor.Value;
        }

        public AuthMessageSenderOptions Options { get; } //set only via Secret Manager

        public Task SendEmailAsync(string email, string subject, string message)
        {
            return Execute(Options.SendGridKey, subject, message, email);
        }

        public Task SendEmailAsync(string email, List<string> cc, string subject, string message, string FileContents, string FileName)
        {
            return Execute(Options.SendGridKey, subject, message, email, cc, FileContents, FileName);
        }
        public Task SendEmailAsync(string email, string subject, string message, string FileContents, string FileName)
        {
            return Execute(Options.SendGridKey, subject, message, email, FileContents, FileName);
        }

        public Task Execute(string apiKey, string subject, string message, string email)
        {
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress("noreply@accuapp.com", "AccuApp"),
                Subject = subject,
                PlainTextContent = message,
                HtmlContent = message
            };
            msg.AddTo(new EmailAddress(email));
            return client.SendEmailAsync(msg);
        }

        public Task Execute(string apiKey, string subject, string message, string email, List<string> cc, string FileContents, string FileName)
        {
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress("noreply@accuapp.com", "AccuApp"),
                Subject = subject,
                PlainTextContent = message,
                HtmlContent = message
            };
            msg.AddAttachment(FileName, FileContents);
            msg.AddTo(new EmailAddress(email));
            foreach(var el in cc)
            {
                msg.AddCc(new EmailAddress(el));
            }
            return client.SendEmailAsync(msg);
        }

        public Task Execute(string apiKey, string subject, string message, string email, string FileContents, string FileName)
        {
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress("noreply@accuapp.com", "AccuApp"),
                Subject = subject,
                PlainTextContent = message,
                HtmlContent = message
            };
            msg.AddAttachment(FileName, FileContents);
            msg.AddTo(new EmailAddress(email));
            return client.SendEmailAsync(msg);
        }
    }
}
