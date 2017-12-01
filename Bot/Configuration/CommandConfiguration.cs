using Bot.Contracts;
using Bot.Implementations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bot.Configuration
{
    public static class CommandConfiguration
    {
        public static Dictionary<string, ICommand> Get = new Dictionary<string, ICommand>()
        {
            { "vote", new VoteCommand() },
            { "ping", new PingCommand() }
        };
    }
}
