using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuApp.Services
{
    public interface IInvoiceCreator
    {
        JObject CreateInvoice(List<string> args);

        JObject GetNewToken(List<string> args);

    }
}
