using Bot.Configuration;
using Bot.Contracts;
using Bot.Models;
using DSharpPlus.Entities;

namespace Bot.Implementations
{
    public class VoteCommand : ICommand
    {
        public string Process(DiscordMessage message)
        {
            string nonCommand = message.Content.Substring(CommandConfiguration.VoteCommandString.Length + 2);
            
            return nonCommand;
        }
    }
}
