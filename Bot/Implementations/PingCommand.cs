using Bot.Contracts;
using DSharpPlus.Entities;
using System.Threading.Tasks;

namespace Bot.Configuration
{
    public class PingCommand : ICommand
    {
        public Task<string> Process(DiscordMessage message)
        {
            return Task.FromResult(Program.Discord.Ping + "ms");
        }
    }
}