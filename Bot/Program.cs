using System;
using System.Net.Sockets;

namespace Bot
{
    class Program
    {
        static void Main(string[] args)
        {
            TcpClient tcpClient = new TcpClient();
            TcpClientCache.Cache = tcpClient;

            string gatewayUrl = Gateway.GetUrl();
            Console.WriteLine(gatewayUrl);
            Console.ReadKey(true);
        }
    }
}
