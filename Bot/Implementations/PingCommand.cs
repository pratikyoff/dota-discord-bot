using Bot.Contracts;
using DSharpPlus.Entities;

namespace Bot.Configuration
{
    public class PingCommand : ICommand
    {
        public string Process(DiscordMessage message)
        {
            return Program.Discord.Ping + "ms";
        }
    }
}