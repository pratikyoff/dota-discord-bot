using Bot.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Bot.Universal
{
    public static class NetComm
    {
        public static async Task<string> GetResponseOfURL(string url, HttpClient httpClient = null)
        {
            if (httpClient == null)
            {
                httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri(OpenDotaConfiguration.OpenDotaAPIAddress);
            }

            var response = await httpClient.GetAsync(url);
            var responseStream = await response.Content.ReadAsStringAsync();
            return responseStream;
        }
    }
}
