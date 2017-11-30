using System.Net.Sockets;

namespace Bot
{
    public class TcpClientGenerator
    {
        public static TcpClient GetTcpClient(string ip, int port = 80)
        {
            return new TcpClient(AddressFamily.InterNetwork);
        }
    }
}
