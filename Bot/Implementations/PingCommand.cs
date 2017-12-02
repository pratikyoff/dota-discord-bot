using Bot.Contracts;
using DSharpPlus.Entities;
using System;

namespace Bot.Configuration
{
    public class PingCommand : ICommand
    {
        public string Process(DiscordMessage message)
        {
            return Program.Discord.Ping + "ms";
        }
    }
}