using System;
using Bot.Configuration;
using Bot.Contracts;
using Bot.Models;
using DSharpPlus;
using System.Net.Http;
using Newtonsoft.Json;
using System.Threading;
using DSharpPlus.Entities;

namespace Bot.Implementations
{
    public class DotaGameTracker : Functionality
    {
        private HttpClient httpClient = new HttpClient();
        private DiscordChannel _botTestingChannel;
        public override void Run(DiscordClient discord)
        {
            _botTestingChannel = discord.GetChannelAsync(BotDetails.BotTestingChannel).GetAwaiter().GetResult();
            var listOfPlayers = PlayerConfiguration.Players;
            httpClient.BaseAddress = new Uri("https://api.opendota.com/api/players/");
            foreach (var player in listOfPlayers)
            {
                player.TotalMatches = GetTotalMatches(player);
            }
            while (true)
            {
                Thread.Sleep(15 * 60 * 1000);
                foreach (var player in listOfPlayers)
                {
                    var currentMatches = GetTotalMatches(player);
                    if (currentMatches != player.TotalMatches)
                    {
                        int extraGames = currentMatches - player.TotalMatches;
                        player.TotalMatches = currentMatches;
                        _botTestingChannel.SendMessageAsync($"<@{player.DiscordId}> played {extraGames} game{extraGames > 1:\"s\":\"\"}.").GetAwaiter().GetResult();
                    }
                }
            }
        }

        private int GetTotalMatches(Player player)
        {
            int[] winLose = GetNumberOfWinsAndLosses(player);
            return winLose[0] + winLose[1];
        }

        private int[] GetNumberOfWinsAndLosses(Player player)
        {
            var response = httpClient.GetAsync($"{player.SteamId}/wl").GetAwaiter().GetResult();
            var jsonString = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            dynamic responseInJson = JsonConvert.DeserializeObject(jsonString);
            int[] winAndLose = new int[2] { responseInJson.win, responseInJson.lose };
            return winAndLose;
        }
    }
}
