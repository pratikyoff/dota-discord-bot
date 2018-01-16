using Bot.Contracts;
using DSharpPlus.Entities;
using Bot.Configuration;
using System.Threading.Tasks;
using System.Linq;

namespace Bot.Implementations
{
    [Command("doc")]
    public class DocumentationCommand : ICommand
    {
        public string _pingText;
        public string _voteText;

        public DocumentationCommand()
        {
            _pingText = CommandConfiguration.GetCommandText<PingCommand>();
            _voteText = CommandConfiguration.GetCommandText<VoteCommand>();
        }

        public Task<string> Process(DiscordMessage message)
        {
            string doc = $"Here are a list of commands presently supported.\n" +
            $"1.Ping - `!{_pingText}` command for checking the latency of the bot.\n" +
            $"2.Vote - `!{_voteText}` command for\n" +
            $"\t1.Initiating a vote - `!{_voteText} <Vote subject> <option1>|<option2>|<option3>`\n" +
            $"\t2.Voting - `!{_voteText} <optionNumber>` eg `!{_voteText} 2`\n" +
            $"\t3.Status - `!{_voteText} status`\n";

            return Task.FromResult(doc);
        }


    }
}
