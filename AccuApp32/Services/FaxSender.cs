using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuApp.Services
{
    public class FaxSender : IFaxSender
    {
        public Task SendFaxAsync(string fax, string subject, string message)
        {
            throw new NotImplementedException();
        }
    }
}
