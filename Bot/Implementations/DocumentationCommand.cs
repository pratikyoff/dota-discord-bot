using Bot.Contracts;
using System;
using DSharpPlus.Entities;
using Bot.Configuration;

namespace Bot.Implementations
{
    public class DocumentationCommand : ICommand
    {
        string doc = $"Here are a list of commands presently supported.\n" +
            $"1.Ping - `!{CommandConfiguration.PingCommandString}` command for checking the latency of the bot.\n" +
            $"2.Vote - `!{CommandConfiguration.VoteCommandString}` command for\n" +
            $"\t1.Initiating a vote - `!{CommandConfiguration.VoteCommandString} <Vote subject> <option1>|<option2>|<option3>`\n" +
            $"\t2.Voting - `!{CommandConfiguration.VoteCommandString} <optionNumber>` eg `!{CommandConfiguration.VoteCommandString} 2`\n" +
            $"\t3.Status - `!{CommandConfiguration.VoteCommandString} status`\n";

        public string Process(DiscordMessage message)
        {
            return doc;
        }
    }
}
