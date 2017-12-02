using Bot.Contracts;
using Bot.Implementations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bot.Configuration
{
    public static class CommandConfiguration
    {
        private static VoteCommand _voteCommand = new VoteCommand();
        private static PingCommand _pingCommand = new PingCommand();

        public static Dictionary<string, ICommand> Get = new Dictionary<string, ICommand>()
        {
            { "vote", _voteCommand },
            { "ping", _pingCommand }
        };
    }
}
