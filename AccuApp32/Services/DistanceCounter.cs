using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuApp.Services
{
    public class DistanceCounter : IDistanceCounter
    {
        public double CalculateDistance(string start, string end)
        {
            double distance = -1;
            string key = Environment.GetEnvironmentVariable("GoogleMapsApi");
            var client = new RestClient(@"https://maps.googleapis.com/maps/api/distancematrix/json?origins=" + start.Trim().Replace(" ", "+") + "&destinations=" + end.Trim().Replace(" ", "+") + "&mode=driving&language=Eng&key=" + key);
            var request = new RestRequest(Method.GET);
            var response = client.Execute(request);
            JObject resp = JObject.Parse(response.Content);
            if (resp.SelectToken("status").ToString() == "OK")
            {
                foreach (var el in resp.SelectToken("rows"))
                {
                    foreach (var elInner in el.SelectToken("elements"))
                    {
                        try
                        {
                            distance = double.Parse(elInner.SelectToken("distance").SelectToken("value").ToString());
                        }
                        catch(NullReferenceException)
                        {
                            return distance;
                        }
                        catch(FormatException)
                        {
                            return distance;
                        }
                        catch(OverflowException)
                        {
                            return distance;
                        }
                    }
                }
            }
            return distance;
        }
    }
}
