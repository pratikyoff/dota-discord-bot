#pragma warning disable CS4014
using Bot.Contracts;
using System;
using DSharpPlus;
using System.Timers;
using Bot.Configuration;
using System.Threading.Tasks;
using Bot.Universal;
using System.Net.Http;

namespace Bot.Implementations
{
    public class DailyGamesTracker : Functionality
    {
        private static DateTime _prevISTTime;
        private Uri _baseAddress = new Uri(OpenDotaConfiguration.OpenDotaAPIAddress);
        private HttpClient _httpClient = new HttpClient();

        public DailyGamesTracker()
        {
            _prevISTTime = GetISTTimeNow();
        }

        public override void Run(DiscordClient discord)
        {
            _httpClient.BaseAddress = _baseAddress;
            Timer timer = new Timer(60 * 1000)
            {
                AutoReset = true
            };
            timer.Elapsed += async (sender, args) =>
            {
                var istTimeNow = GetISTTimeNow();
                if (istTimeNow.Day != _prevISTTime.Day)
                {
                    _prevISTTime = istTimeNow;
                    try
                    {
                        await FindAndDisplayTotalMatchesPlayed();
                    }
                    catch (Exception e)
                    {
                        Program.Logger.Log(e.ToString());
                    }
                }
            };
            timer.Start();
        }

        private async Task FindAndDisplayTotalMatchesPlayed()
        {
            var channel = await Program.Discord.GetChannelAsync(BotDetails.BotFeedChannel);
            foreach (var player in PlayerConfiguration.Players)
            {
                var jsonString = await NetComm.GetResponseOfURL($"players/{player.SteamId}/wl?date=1", _httpClient);
                var responseInJson = JsonToFrom.FromJson<dynamic>(jsonString);
                int wins = responseInJson.win;
                int losses = responseInJson.lose;
                if (wins + losses > 0)
                {
                    string content = $"<@{player.DiscordId}> won {wins} game{GetPlural(wins)} out of {wins + losses} game{GetPlural(wins + losses)} today.";
                    channel.SendMessageAsync(content);
                }
            }
        }

        private object GetPlural(int number)
        {
            if (number == 1)
                return string.Empty;
            else return "s";
        }

        public DateTime GetISTTimeNow()
        {
            var current = DateTime.UtcNow;
            current = current.AddHours(5.5);
            return current;
        }
    }
}
