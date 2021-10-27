using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SendGrid.Helpers.Mail;

namespace AccuApp.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
        Task SendEmailAsync(string v1, List<string> cc, string v2, string v3, string FileContents, string FileName);
        Task SendEmailAsync(string v1, string v2, string v3, string FileContents, string FileName);
    }
}
