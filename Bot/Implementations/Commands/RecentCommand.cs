using System;
using System.Linq;
using System.Threading.Tasks;
using Bot.Contracts;
using Bot.Models;
using Bot.Universal;
using DSharpPlus.Entities;

namespace Bot.Implementations.Commands
{
    [Command("recent")]
    public class RecentCommand : ICommand
    {
        private readonly double _maxHours = 72;

        public async Task<string> ProcessAsync(DiscordMessage message)
        {
            var words = message.WordsWithoutCommand();

            double timeInHours = 12;
            Player player = message.Author.ToPlayer();

            if (words.Count() == 1)
            {
                var success = double.TryParse(words.First(), out double time);
                if (success)
                {
                    timeInHours = time > _maxHours ? timeInHours : time;
                }
                else
                {
                    player = message.MentionedUsers.First().ToPlayer() ?? player;
                }
            }

            if (words.Count() == 2)
            {
                var success = double.TryParse(words.ElementAt(1), out double time);
                if (success)
                {
                    timeInHours = time > _maxHours ? _maxHours : time;
                }
                player = message.MentionedUsers.First().ToPlayer() ?? player;
            }

            var result = await GetStatsAsync(player, timeInHours);

            return result;
        }

        private async Task<string> GetStatsAsync(Player player, double timeInHours)
        {
            string url = $"players/{player.SteamId}/wl?date={timeInHours / 24}";
            var responseInJson = await NetComm.GetResponseOfURL(url);
            dynamic response = responseInJson.Deserialize<dynamic>();
            int wins = response.win;
            int losses = response.lose;
            string content = $"<@{player.DiscordId}>'s score: {wins}/{losses} in last {timeInHours} hours.";
            return content;
        }
    }
}
