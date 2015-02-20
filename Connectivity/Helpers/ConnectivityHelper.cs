using Microsoft.WindowsAPICodePack.Net;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace OfflineTools.Connectivity.Helpers
{
    internal class ConnectivityHelper
    {
        private static List<string> Urls = new List<string> { "http://google.com/robots.txt", "https://www.yahoo.com/robots.txt", "http://www.bing.com/robots.txt" };

        public static ConnectivityValues Test()
        {
            foreach (var url in Urls)
            {
                if (!NetworkListManager.IsConnectedToInternet)
                    return ConnectivityValues.Disconnected;

                if (Test(url))
                    return ConnectivityValues.Connected;
            }
            return ConnectivityValues.Disconnected;
        }

        private static bool Test(string url)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.Timeout = new TimeSpan(0, 0, 5);
                    var response = client.GetAsync(url).Result;
                    if (response.IsSuccessStatusCode)
                        return true;
                    else
                        return false;
                }
            }
            catch (HttpRequestException)
            {
                return false;
            }
            catch (AggregateException ae)
            {
                ae.Handle(x =>
                {
                    if (x is TaskCanceledException) //timeout
                        return true;
                    return true; //TODO: investigate
                });
                return false;
            }
        }
    }
}