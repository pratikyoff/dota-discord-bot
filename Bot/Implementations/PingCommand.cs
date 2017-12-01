using Bot.Contracts;
using DSharpPlus.Entities;
using System;

namespace Bot.Configuration
{
    public class PingCommand : ICommand
    {
        public string Process(DiscordMessage message)
        {
            var timestamp = message.CreationTimestamp.DateTime;
            var difference = DateTime.UtcNow - timestamp;
            return difference.TotalMilliseconds + "ms";
        }
    }
}