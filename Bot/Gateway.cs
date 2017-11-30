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
                var tcpClient = TcpClientCache.Cache;
                tcpClient.ConnectAsync(BotDetails.ApiUrl + "gateway", 80).GetAwaiter().GetResult();
                byte[] buffer = new byte[2048];
                tcpClient.GetStream().Read(buffer, 0, buffer.Length);
                tcpClient.Dispose();
                string bufferInString = Encoding.ASCII.GetString(buffer);
                bufferInString.Trim('\0');
                return bufferInString;
            }
            else
                return KeyValueCache.Get(_cacheUrlEntry);
        }
    }
}
