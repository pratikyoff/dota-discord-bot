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
        public static string CleanCommandString { get => "clean"; }
        public static string ExpletiveCommandString { get => "abuse"; }
        public static string ExitCommandString { get => "exit"; }

        private static VoteCommand _voteCommand = new VoteCommand();
        private static PingCommand _pingCommand = new PingCommand();
        private static DocumentationCommand _docCommand = new DocumentationCommand();
        private static RemindMeCommand _remindMeCommand = new RemindMeCommand();
        private static CleanCommand _cleanCommand = new CleanCommand();
        private static ExpletiveCommand _expletiveCommand = new ExpletiveCommand();
        private static ExitCommand _exitCommand = new ExitCommand();


        public static Dictionary<string, ICommand> Get = new Dictionary<string, ICommand>(StringComparer.OrdinalIgnoreCase)
        {
            { VoteCommandString , _voteCommand },
            { PingCommandString , _pingCommand },
            { DocCommandString , _docCommand },
            { RemindMeCommandString , _remindMeCommand },
            { CleanCommandString , _cleanCommand },
            { ExpletiveCommandString , _expletiveCommand },
            { ExitCommandString , _exitCommand },
        };
    }
}
