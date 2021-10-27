using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace AccuApp.Services
{
    public class InvoiceCreator : IInvoiceCreator
    {
        public JObject CreateInvoice(List<string> args)
        {
            for (int i = 0; i < args.Count; i++)
            {
                args[i] = args[i].Replace("___", " ");
            }
            Guid requestid = Guid.NewGuid();
            var client = new RestClient("https://sandbox-quickbooks.api.intuit.com/v3/company/" + args[3] + "/invoice?requestid=" + requestid);
            var request = new RestRequest(Method.POST);
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Authorization", "Bearer " + args[4]);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("User-Agent", "APIExplorer");
            string strJsonData = "{ \"Line\" : [";
            for (int i = 6; i < args.Count; i += 3)
            {
                if (i + 3 < args.Count)
                {
                    strJsonData += "{\"Amount\":\"" + args[i] + "\",\"DetailType\": \"SalesItemLineDetail\",\"SalesItemLineDetail\":{\"ItemRef\":{\"value\":\"" + args[i + 1] + "\",\"name\": \"" + args[i + 2] + "\"}}},";
                }
                else
                {
                    strJsonData += "{\"Amount\":\"" + args[i] + "\",\"DetailType\": \"SalesItemLineDetail\",\"SalesItemLineDetail\":{\"ItemRef\":{\"value\":\"" + args[i + 1] + "\",\"name\": \"" + args[i + 2] + "\"}}}";
                }
            }
            strJsonData += "],\"CustomerRef\": {\"value\": \"" + args[5] + "\"}}";
            request.AddParameter("application/json", strJsonData, ParameterType.RequestBody);
            var response = client.Execute(request);
            return JObject.Parse(response.Content);
        }

        public JObject GetNewToken(List<string> args)
        {
            string clientID = args[1];
            string clientSecret = args[2];
            string refreshToken = args[4];
            var client = new RestClient("https://oauth.platform.intuit.com/oauth2/v1/tokens/bearer ");
            var request = new RestRequest(Method.POST);
            request.AddHeader("Accept", "application/json");
            var authorizationTextBytes = System.Text.Encoding.UTF8.GetBytes(clientID + ":" + clientSecret);
            request.AddHeader("Authorization", "Basic " + System.Convert.ToBase64String(authorizationTextBytes));
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddHeader("Host", "oauth.platform.intuit.com");
            request.AddHeader("Cache-Control", "no-cache");
            request.AddParameter("grant_type", "refresh_token");
            request.AddParameter("refresh_token", refreshToken);
            IRestResponse tokenResponse = client.Execute(request);
            return JObject.Parse(tokenResponse.Content);
        }

    }
}
