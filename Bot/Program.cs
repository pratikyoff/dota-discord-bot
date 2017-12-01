using System;
using System.Net.Sockets;

namespace Bot
{
    class Program
    {
        static void Main(string[] args)
        {
            bool quit = false;
            Console.CancelKeyPress += (x, y) =>
            {
                quit = true;
            };
            TcpClient tcpClient = new TcpClient();
            TcpClientCache.Cache = tcpClient;

            string gatewayUrl = Gateway.GetUrl();
            Console.WriteLine("Gateway: " + gatewayUrl);

            tcpClient.ConnectAsync(gatewayUrl.Substring(6), 80).GetAwaiter().GetResult();

            while (!quit)
            {
                string read = ReadFromStream(tcpClient);
            }
        }

        private static string ReadFromStream(TcpClient tcpClient)
        {
            int buffer;
            string read = string.Empty;
            NetworkStream stream = tcpClient.GetStream();
            while (stream.DataAvailable)
            {
                buffer = stream.ReadByte();
                read += (char)buffer;
            }
            return read;
        }
    }
}
