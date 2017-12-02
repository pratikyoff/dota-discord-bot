using Bot.Configuration;
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
        public static DiscordClient Discord { get; private set; }
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
            Discord.DisconnectAsync().GetAwaiter().GetResult();
            Discord.Dispose();
        }

        private static async Task AsyncMain()
        {
            Discord = new DiscordClient(new DiscordConfiguration()
            {
                Token = BotDetails.Token,
                TokenType = TokenType.Bot
            });

            await Discord.ConnectAsync();
            logger.Log("Discord Connected");

            Discord.MessageCreated += async x =>
            {
                if (x.Message.Content.IndexOf(BotDetails.CommandPrefix) == 0)
                {
                    logger.Log(JsonConvert.SerializeObject(x.Message));
                    string command = GetFirstWord(x.Message.Content);
                    string reply = CommandConfiguration.Get[command].Process(x.Message);
                    logger.Log(reply);
                    await x.Message.RespondAsync(reply);
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
