using Bot.Contracts;
using DSharpPlus.Entities;
using System.Threading.Tasks;

namespace Bot.Implementations
{
    [Command("exit")]
    public class ExitCommand : ICommand
    {
        public Task<string> ProcessAsync(DiscordMessage message)
        {
            if (!message.Author.Id.ToString().Equals("162522319737061376"))
                return Task.FromResult("Not authorized");
            Program.CancellationTokenSource.CancelAfter(1 * 1000);
            return Task.FromResult("Exiting.");
        }
    }
}
