using Bot.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using DSharpPlus.Entities;
using Bot.Configuration;
using System.Linq;

namespace Bot.Implementations
{
    public class CleanCommand : ICommand
    {
        public string Process(DiscordMessage message)
        {
            if (!message.Author.Id.ToString().Equals("162522319737061376"))
                return "Not authorized";
            var botFeedChannel = Program.Discord.GetChannelAsync(BotDetails.BotFeedChannel).GetAwaiter().GetResult();
            var botDumpChannel = Program.Discord.GetChannelAsync(BotDetails.BotDumpChannel).GetAwaiter().GetResult();
            var messages = botFeedChannel.GetMessagesAsync().GetAwaiter().GetResult();
            foreach (var chatMsg in messages)
            {
                if (chatMsg.Content.IndexOf("played") >= 0)
                {
                    chatMsg.DeleteAsync().GetAwaiter().GetResult();
                    botDumpChannel.SendMessageAsync(chatMsg.Content + " DELETED.").GetAwaiter().GetResult();
                }
            }
            return "Done";
        }
    }
}
