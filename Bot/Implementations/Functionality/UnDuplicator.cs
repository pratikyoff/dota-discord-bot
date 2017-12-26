using Bot.Contracts;
using System.Collections.Generic;
using DSharpPlus;
using DSharpPlus.Entities;

namespace Bot.Implementations
{
    public class UnDuplicator : Functionality
    {
        private Dictionary<ulong, DiscordMessage> _lastMessage = new Dictionary<ulong, DiscordMessage>();
        public override void Run(DiscordClient discord)
        {
            Program.Discord.MessageCreated += async x =>
            {
                if (_lastMessage.ContainsKey(x.Author.Id))
                {
                    var timeDiff = x.Message.CreationTimestamp - _lastMessage[x.Author.Id].CreationTimestamp;
                    if (timeDiff.TotalSeconds < 2 && _lastMessage[x.Author.Id].Content.Equals(x.Message.Content))
                    {
                        try
                        {
                            await x.Message.DeleteAsync();
                            Program.logger.Log($"Spam controlled from {x.Author.Username} <{x.Author.Id}>");
                        }
                        catch { }
                    }
                }
                _lastMessage[x.Author.Id] = x.Message;
            };
        }
    }
}
