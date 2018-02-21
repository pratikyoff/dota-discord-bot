using Bot.Configuration;
using Bot.Contracts;
using Bot.Universal;
using DSharpPlus;
using System;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;

namespace Bot.Implementations
{
    public class OfflineTracker : Functionality
    {
        private HttpClient _httpClient = new HttpClient()
        {
            BaseAddress = new Uri(OpenDotaConfiguration.OpenDotaAPIAddress)
        };
        public override async void Run(DiscordClient discord)
        {
            var channel = await discord.GetChannelAsync(BotDetails.BotDumpChannel);
            var lastMessage = (await channel.GetMessagesAsync(1)).FirstOrDefault();
            var matchId = Regex.Match(lastMessage.Content, $"[0-9]+$").Value;
            string matchDetailsString = KeyValueCache.Get(matchId);
            if (matchDetailsString == null)
            {
                matchDetailsString = await NetComm.GetResponseOfURL($"matches/{matchId}", _httpClient);
                KeyValueCache.Put(matchId, matchDetailsString);
            }
            dynamic matchDetails = JsonToFrom.FromJson<dynamic>(matchDetailsString);
            int matchEndTime = (int)matchDetails.start_time + (int)matchDetails.duration;
            foreach (var player in PlayerConfiguration.Players)
            {
                var recentMatches = JsonToFrom.FromJson<dynamic>(await NetComm.GetResponseOfURL($"matches/{matchId}", _httpClient));

            }
        }
    }
}
