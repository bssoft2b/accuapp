using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SendGrid.Helpers.Mail;

namespace AccuApp.Services
{
    public interface IFaxSender
    {
        Task SendFaxAsync(string fax, string subject, string message);
    }
}
