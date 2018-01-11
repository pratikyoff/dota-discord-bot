using Bot.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using DSharpPlus.Entities;
using System.Threading.Tasks;
using System.Threading;

namespace Bot.Implementations
{
    public class ExitCommand : ICommand
    {
        public Task<string> Process(DiscordMessage message)
        {
            if (!message.Author.Id.ToString().Equals("162522319737061376"))
                return Task.FromResult("Not authorized");
            Program.CancellationTokenSource.CancelAfter(1 * 1000);
            return Task.FromResult("Exiting.");
        }
    }
}
