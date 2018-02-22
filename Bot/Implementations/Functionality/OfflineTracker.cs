#pragma warning disable CS4014
using Bot.Configuration;
using Bot.Contracts;
using Bot.Models;
using Bot.Universal;
using DSharpPlus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using static Bot.Implementations.DotaGameTracker;

namespace Bot.Implementations
{
    public class OfflineTracker : Functionality
    {
        private HttpClient _httpClient = new HttpClient()
        {
            BaseAddress = new Uri(OpenDotaConfiguration.OpenDotaAPIAddress)
        };
        private Dictionary<string, List<Player>> _matchMap = new Dictionary<string, List<Player>>();

        public override async void Run(DiscordClient discord)
        {
            var channel = await discord.GetChannelAsync(BotDetails.BotFeedChannel);
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
            double diffInTime = GetDiffInDays(matchEndTime);
            foreach (var player in PlayerConfiguration.Players)
            {
                var recentMatches = JsonToFrom.FromJson<dynamic>(await NetComm.GetResponseOfURL($"players/{player.SteamId}/matches?date={diffInTime}", _httpClient));
                foreach (var recentMatch in recentMatches)
                {
                    string recentMatchId = recentMatch.match_id;
                    if (!_matchMap.ContainsKey(recentMatchId))
                        _matchMap[recentMatchId] = new List<Player>();
                    _matchMap[recentMatchId].Add(player);
                }
            }
            foreach (var matchMap in _matchMap)
            {
                if (KeyValueCache.Get(matchMap.Key) == null)
                    KeyValueCache.Put(matchMap.Key, await NetComm.GetResponseOfURL($"matches/{matchMap.Key}", _httpClient));
                dynamic parsedMatch = JsonToFrom.FromJson<dynamic>(KeyValueCache.Get(matchMap.Key));
                string matchString = string.Empty;
                foreach (var player in matchMap.Value)
                {
                    matchString += GenerateMatchString(parsedMatch, GetNormalOrRankedMatch(parsedMatch), player);
                }
                matchString += GenerateDotaBuffLink(matchMap.Key);
                channel.SendMessageAsync(matchString);
            }
        }

        private static double GetDiffInDays(int epochTime)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            epoch = epoch.AddSeconds(epochTime);
            var diffInTime = (DateTime.UtcNow - epoch).TotalDays;
            return diffInTime;
        }
    }
}
