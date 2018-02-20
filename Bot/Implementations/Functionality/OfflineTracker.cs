using Bot.Contracts;
using DSharpPlus;
using System.Linq;
using System.Text.RegularExpressions;

namespace Bot.Implementations
{
    public class OfflineTracker : Functionality
    {
        public override async void Run(DiscordClient discord)
        {
            var channel = await discord.GetChannelAsync(BotDetails.BotDumpChannel);
            var lastMessage = (await channel.GetMessagesAsync(1)).FirstOrDefault();
            var matchId = Regex.Match(lastMessage.Content, $"[0-9]+$").Value;

        }
    }
}
