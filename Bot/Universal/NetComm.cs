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
            if (httpClient == null) httpClient = new HttpClient();

            var response = await httpClient.GetAsync(url);
            var responseStream = await response.Content.ReadAsStringAsync();
            return responseStream;
        }
    }
}
