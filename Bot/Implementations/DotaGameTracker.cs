using System;
using Bot.Configuration;
using Bot.Contracts;
using Bot.Models;
using DSharpPlus;
using System.Net.Http;
using Newtonsoft.Json;
using System.Threading;
using DSharpPlus.Entities;
using System.Linq;

namespace Bot.Implementations
{
    public class DotaGameTracker : Functionality
    {
        private HttpClient httpClient = new HttpClient();
        private DiscordChannel _botTestingChannel;
        public override void Run(DiscordClient discord)
        {
            _botTestingChannel = discord.GetChannelAsync(BotDetails.BotFeedChannel).GetAwaiter().GetResult();
            var listOfPlayers = PlayerConfiguration.Players;
            httpClient.BaseAddress = new Uri(OpenDotaConfiguration.OpenDotaAPIAddress);
            foreach (var player in listOfPlayers)
            {
                player.TotalMatches = GetTotalMatches(player);
            }
            while (true)
            {
                Thread.Sleep(5 * 60 * 1000);
                foreach (var player in listOfPlayers)
                {
                    var currentMatches = GetTotalMatches(player);
                    if (currentMatches != player.TotalMatches)
                    {
                        int extraGames = currentMatches - player.TotalMatches;
                        player.TotalMatches = currentMatches;

                        var jsonString = GetResponseOfURL($"players/{player.SteamId}/matches?limit=1");
                        dynamic lastMatch = ((dynamic)JsonConvert.DeserializeObject(jsonString))[0];

                        string matchId = lastMatch.match_id;
                        dynamic matchDetails;
                        if (KeyValueCache.Get(matchId) == null)
                        {
                            string matchDetailsString = GetResponseOfURL($"matches/{matchId}");
                            KeyValueCache.Put(matchId, matchDetailsString);
                            matchDetails = JsonConvert.DeserializeObject(matchDetailsString);
                        }
                        else
                        {
                            matchDetails = JsonConvert.DeserializeObject(KeyValueCache.Get(matchId));
                        }
                        string winOrLose = FindPlayerGameResult(player, matchDetails);
                        string hero = FindHero(lastMatch);
                        string KDA = FindKDA(lastMatch);
                        string DotabuffLink = $"Dotabuff: {OpenDotaConfiguration.DotabuffMatchUrl}{matchId}";
                        _botTestingChannel.SendMessageAsync($"<@{player.DiscordId}> **{winOrLose}** a game.\n**Hero**: {hero}\n**KDA**: {KDA}\n{DotabuffLink}").GetAwaiter().GetResult();
                    }
                }
            }
        }

        private string FindHero(dynamic lastMatch)
        {
            return HeroDetails.HeroList.Where(x => x.id == (int)lastMatch.hero_id).First().localized_name;
        }

        private string FindKDA(dynamic lastMatch)
        {
            return $"{lastMatch.kills}/{lastMatch.deaths}/{lastMatch.assists}";
        }

        private string FindPlayerGameResult(Player player, dynamic matchDetails)
        {
            bool radiantWon = matchDetails.radiant_win;
            bool isPlayerRadiant = false;
            foreach (var gamePlayer in matchDetails.players)
            {
                if (gamePlayer.account_id != null && ((string)gamePlayer.account_id).Equals(player.SteamId))
                {
                    isPlayerRadiant = gamePlayer.isRadiant;
                    break;
                }
            }
            string result = radiantWon == isPlayerRadiant ? "won" : "lost";
            return result;
        }

        private int GetTotalMatches(Player player)
        {
            int[] winLose = GetNumberOfWinsAndLosses(player);
            return winLose[0] + winLose[1];
        }

        private int[] GetNumberOfWinsAndLosses(Player player)
        {
            string jsonString;
            dynamic responseInJson;
            bool isError = false;
            do
            {
                try
                {
                    isError = false;
                    jsonString = GetResponseOfURL($"players/{player.SteamId}/wl");
                    responseInJson = JsonConvert.DeserializeObject(jsonString);
                    int[] winAndLose = new int[2] { responseInJson.win, responseInJson.lose };
                    return winAndLose;
                }
                catch (Exception)
                {
                    isError = true;
                    Thread.Sleep(10 * 1000);
                }
            } while (isError);
            return new int[] { 0, 0 };
        }

        private string GetResponseOfURL(string url)
        {
            var response = httpClient.GetAsync(url).GetAwaiter().GetResult();
            var responseStream = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            return responseStream;
        }
    }
}
