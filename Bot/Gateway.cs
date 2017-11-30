﻿using System.Net;
using System.Text;

namespace Bot
{
    public class Gateway
    {
        private static string _cacheUrlEntry = "GatewayUrl";

        public static string GetUrl()
        {
            if (KeyValueCache.Get(_cacheUrlEntry) == null)
            {
                HttpWebRequest httpWebRequest = WebRequest.CreateHttp(BotDetails.ApiUrl + "gateway");
                httpWebRequest.Method = "GET";
                HttpWebResponse response = (HttpWebResponse)httpWebRequest.GetResponseAsync().GetAwaiter().GetResult();

                byte[] buffer = new byte[response.ContentLength];
                response.GetResponseStream().Read(buffer, 0, buffer.Length);
                string bufferString = Encoding.ASCII.GetString(buffer);

                return bufferString;
            }
            else
                return KeyValueCache.Get(_cacheUrlEntry);
        }
    }
}
