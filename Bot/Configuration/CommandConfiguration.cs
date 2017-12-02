using Bot.Contracts;
using Bot.Implementations;
using System.Collections.Generic;

namespace Bot.Configuration
{
    public static class CommandConfiguration
    {
        public static string VoteCommandString { get => "vote"; }
        public static string PingCommandString { get => "ping"; }

        private static VoteCommand _voteCommand = new VoteCommand();
        private static PingCommand _pingCommand = new PingCommand();

        public static Dictionary<string, ICommand> Get = new Dictionary<string, ICommand>()
        {
            { VoteCommandString , _voteCommand },
            { PingCommandString , _pingCommand }
        };
    }
}
