using Bot.Contracts;
using Bot.Implementations;
using System;
using System.Collections.Generic;

namespace Bot.Configuration
{
    public static class CommandConfiguration
    {
        public static string VoteCommandString { get => "vote"; }
        public static string PingCommandString { get => "ping"; }
        public static string DocCommandString { get => "doc"; }

        private static VoteCommand _voteCommand = new VoteCommand();
        private static PingCommand _pingCommand = new PingCommand();
        private static DocumentationCommand _docCommand = new DocumentationCommand();

        public static Dictionary<string, ICommand> Get = new Dictionary<string, ICommand>(StringComparer.OrdinalIgnoreCase)
        {
            { VoteCommandString , _voteCommand },
            { PingCommandString , _pingCommand },
            { DocCommandString , _docCommand }
        };
    }
}
