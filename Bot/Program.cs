using Bot.Configuration;
using Bot.Contracts;
using Bot.Implementations;
using Bot.Universal;
using DSharpPlus;
using DSharpPlus.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Bot
{
    class Program
    {
        public static DiscordClient Discord { get; private set; }
        public static ILogger logger;

        static void Main(string[] args)
        {
            logger = new ConsoleLogger();

            try
            {
                AsyncMain().ConfigureAwait(false).GetAwaiter().GetResult();
            }
            catch { }

            foreach (var functionality in FunctionalityConfiguration.Functionalities)
            {
                functionality.Stop();
            }
            Discord.DisconnectAsync().GetAwaiter().GetResult();
            Discord.Dispose();
        }

        private static async Task AsyncMain()
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            CancellationToken cancellationToken = cancellationTokenSource.Token;
            Console.CancelKeyPress += (x, y) =>
            {
                cancellationTokenSource.Cancel();
            };

            Discord = new DiscordClient(new DiscordConfiguration()
            {
                Token = BotDetails.Token,
                TokenType = TokenType.Bot
            });

            await Discord.ConnectAsync();
            logger.Log("Discord Connected");

            await Discord.UpdateStatusAsync(new DiscordGame(BotDetails.CommandPrefix + "doc"));

            foreach (var functionality in ReflectiveEnumerator.GetInheritedClasses<Functionality>())
            {
                functionality.Start(Discord);
            }

            Discord.MessageCreated += async x =>
            {
                if (x.Message.Content.IndexOf(BotDetails.CommandPrefix) == 0)
                {
                    string command = GetFirstWord(x.Message.Content);
                    string reply = string.Empty;
                    try
                    {
                        reply = CommandConfiguration.Get[command].Process(x.Message);
                    }
                    catch { }
                    logger.Log($"<Message: {x.Message.Content}>\t<Author: {x.Author.Id}>\t<Name: {x.Author.Username}>\t<Reply: {reply}>");
                    await x.Message.RespondAsync(reply);
                }
            };
            await Task.Delay(-1, cancellationToken);
        }

        private static string GetFirstWord(string content)
        {
            string sentence = content.Substring(BotDetails.CommandPrefix.Length);
            string[] words = sentence.Split(' ');
            return words[0];
        }
    }
}