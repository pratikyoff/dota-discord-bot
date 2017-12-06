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
        public static string RemindMeCommandString { get => "remindme"; }

        private static VoteCommand _voteCommand = new VoteCommand();
        private static PingCommand _pingCommand = new PingCommand();
        private static DocumentationCommand _docCommand = new DocumentationCommand();
        private static RemindMeCommand _remindMeCommand = new RemindMeCommand();

        public static Dictionary<string, ICommand> Get = new Dictionary<string, ICommand>(StringComparer.OrdinalIgnoreCase)
        {
            { VoteCommandString , _voteCommand },
            { PingCommandString , _pingCommand },
            { DocCommandString , _docCommand },
            { RemindMeCommandString , _remindMeCommand }
        };
    }
}
