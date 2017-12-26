using System;
using Bot.Configuration;
using Bot.Contracts;
using Bot.Models;
using Bot.Universal;
using DSharpPlus;
using System.Net.Http;
using System.Threading;
using DSharpPlus.Entities;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bot.Implementations
{
    public class DotaGameTracker : Functionality
    {
        private HttpClient httpClient = new HttpClient();
        private DiscordChannel _botTestingChannel;
        public override async void Run(DiscordClient discord)
        {
            _botTestingChannel = await discord.GetChannelAsync(BotDetails.BotFeedChannel);
            var listOfPlayers = PlayerConfiguration.Players;
            httpClient.BaseAddress = new Uri(OpenDotaConfiguration.OpenDotaAPIAddress);
            foreach (var player in listOfPlayers)
            {
                player.TotalMatches = await GetTotalMatches(player);
            }
            while (true)
            {
                try
                {
                    Dictionary<string, List<Player>> matchIdToPlayersMapping = new Dictionary<string, List<Player>>();
                    foreach (var player in listOfPlayers)
                    {
                        var currentMatches = await GetTotalMatches(player);
                        if (currentMatches != player.TotalMatches)
                        {
                            int extraGames = currentMatches - player.TotalMatches;
                            player.TotalMatches = currentMatches;

                            var jsonString = await GetResponseOfURL($"players/{player.SteamId}/matches?limit=1");
                            dynamic lastMatch = JsonToFrom.FromJson<dynamic>(jsonString)[0];
                            string matchId = lastMatch.match_id;
                            if (!matchIdToPlayersMapping.ContainsKey(matchId))
                                matchIdToPlayersMapping[matchId] = new List<Player>();

                            matchIdToPlayersMapping[matchId].Add(player);
                        }
                    }
                    foreach (var matchId in matchIdToPlayersMapping.Keys)
                    {
                        string matchDetailsString = await GetResponseOfURL($"matches/{matchId}");
                        dynamic matchDetails = JsonToFrom.FromJson<dynamic>(matchDetailsString);
                        string reply = string.Empty;
                        foreach (var player in matchIdToPlayersMapping[matchId])
                        {
                            string winOrLose = FindPlayerGameResult(player, matchDetails);
                            string hero = FindHero(player, matchDetails);
                            string KDA = FindKDA(player, matchDetails);
                            reply += $"<@{player.DiscordId}> **{winOrLose}** a game.\n**Hero**: {hero}\n**KDA**: {KDA}\n";
                        }
                        string DotabuffLink = $"Dotabuff: {OpenDotaConfiguration.DotabuffMatchUrl}{matchId}";
                        await _botTestingChannel.SendMessageAsync($"{reply}{DotabuffLink}");
                        Program.logger.Log($"Match Details logged for match id: {matchId}");
                    }
                }
                catch (Exception e)
                {
                    Program.logger.Log($"Exception in {GetType().Name}\nMessage: {e.Message}\nStack Trace: {e.StackTrace}");
                    Thread.Sleep(30 * 1000);
                    continue;
                }
                Thread.Sleep(5 * 60 * 1000);
            }
        }

        public string FindHero(Player player, dynamic matchDetails)
        {
            dynamic playerMatchInfo = GetPlayerMatchInfo(player, matchDetails);
            return HeroDetails.HeroList.Where(x => x.id == (int)playerMatchInfo.hero_id).First().localized_name;
        }

        private dynamic GetPlayerMatchInfo(Player player, dynamic matchDetails)
        {
            return ((IEnumerable<dynamic>)matchDetails.players).Where(y => y.account_id == player.SteamId).First();
        }

        public string FindKDA(Player player, dynamic matchDetails)
        {
            var playerMatchInfo = GetPlayerMatchInfo(player, matchDetails);
            return $"{playerMatchInfo.kills}/{playerMatchInfo.deaths}/{playerMatchInfo.assists}";
        }

        public string FindPlayerGameResult(Player player, dynamic matchDetails)
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

        private async Task<int> GetTotalMatches(Player player)
        {
            int[] winLose = await GetNumberOfWinsAndLosses(player);
            return winLose[0] + winLose[1];
        }

        private async Task<int[]> GetNumberOfWinsAndLosses(Player player)
        {
            string jsonString;
            dynamic responseInJson;
            bool isError = false;
            do
            {
                try
                {
                    isError = false;
                    jsonString = await GetResponseOfURL($"players/{player.SteamId}/wl");
                    responseInJson = JsonToFrom.FromJson<dynamic>(jsonString);
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

        private async Task<string> GetResponseOfURL(string url)
        {
            var response = await httpClient.GetAsync(url);
            var responseStream = await response.Content.ReadAsStringAsync();
            return responseStream;
        }
    }
}
