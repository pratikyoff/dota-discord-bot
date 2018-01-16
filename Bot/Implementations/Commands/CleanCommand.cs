using Bot.Contracts;
using DSharpPlus.Entities;
using System.Threading.Tasks;

namespace Bot.Implementations
{
    [Command("clean")]
    public class CleanCommand : ICommand
    {
        public async Task<string> Process(DiscordMessage message)
        {
            if (!message.Author.Id.ToString().Equals("162522319737061376"))
                return "Not authorized";
            var botFeedChannel = await Program.Discord.GetChannelAsync(BotDetails.BotFeedChannel);
            var botDumpChannel = await Program.Discord.GetChannelAsync(BotDetails.BotDumpChannel);
            var messages = await botFeedChannel.GetMessagesAsync();
            foreach (var chatMsg in messages)
            {
                if (chatMsg.Content.IndexOf("played") >= 0)
                {
                    await chatMsg.DeleteAsync();
                    await botDumpChannel.SendMessageAsync(chatMsg.Content + " DELETED.");
                }
            }
            return "Done";
        }
    }
}
