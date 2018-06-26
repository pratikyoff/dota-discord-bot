using Bot.Contracts;
using DSharpPlus.Entities;
using System.Threading.Tasks;

namespace Bot.Configuration
{
    [Command("ping")]
    public class PingCommand : ICommand
    {
        public Task<string> ProcessAsync(DiscordMessage message)
        {
            return Task.FromResult(Program.Discord.Ping + "ms");
        }
    }
}