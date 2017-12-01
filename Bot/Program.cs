using Bot.Contracts;
using Bot.Implementations;
using DSharpPlus;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace Bot
{
    class Program
    {
        static DiscordClient discord;
        static ILogger logger;

        static void Main(string[] args)
        {
            bool quit = false;
            Console.CancelKeyPress += (x, y) =>
            {
                quit = true;
            };

            logger = new ConsoleLogger();

            AsyncMain();

            while (!quit) { }
            discord.DisconnectAsync().GetAwaiter().GetResult();
            discord.Dispose();
        }

        private static async Task AsyncMain()
        {
            discord = new DiscordClient(new DiscordConfiguration()
            {
                Token = BotDetails.Token,
                TokenType = TokenType.Bot
            });

            await discord.ConnectAsync();
            logger.Log("Discord Connected");

            discord.MessageCreated += async x =>
            {
                if (x.Message.Content.IndexOf(BotDetails.CommandPrefix) == 0)
                {
                    logger.Log(JsonConvert.SerializeObject(x.Message));
                    string command = GetFirstWord(x.Message.Content);

                }
            };
        }

        private static string GetFirstWord(string content)
        {
            string sentence = content.Substring(BotDetails.CommandPrefix.Length);
            string[] words = sentence.Split(' ');
            return words[0];
        }
    }
}
