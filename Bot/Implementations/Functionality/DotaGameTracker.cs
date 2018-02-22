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
        private HttpClient _httpClient = new HttpClient();
        private DiscordChannel _botTestingChannel;
        private static readonly List<int> rankedModes = new List<int>() { 5, 6, 7 };

        public override async void Run(DiscordClient discord)
        {
            _botTestingChannel = await discord.GetChannelAsync(BotDetails.BotFeedChannel);
            var listOfPlayers = PlayerConfiguration.Players;
            _httpClient.BaseAddress = new Uri(OpenDotaConfiguration.OpenDotaAPIAddress);
            foreach (var player in listOfPlayers)
            {
                player.TotalMatches = await GetTotalMatches(player);
            }
            while (true)
            {
                try
                {
                    Program.DumpLogger.Log("Fetching Wins and Losses for players.");
                    Dictionary<string, List<Player>> matchIdToPlayersMapping = new Dictionary<string, List<Player>>();
                    foreach (var player in listOfPlayers)
                    {
                        var currentMatches = await GetTotalMatches(player);
                        if (currentMatches != player.TotalMatches)
                        {
                            int extraGames = currentMatches - player.TotalMatches;
                            player.TotalMatches = currentMatches;

                            var jsonString = await NetComm.GetResponseOfURL($"players/{player.SteamId}/matches?limit=1", _httpClient);
                            dynamic lastMatch = JsonToFrom.FromJson<dynamic>(jsonString)[0];
                            string matchId = lastMatch.match_id;
                            if (!matchIdToPlayersMapping.ContainsKey(matchId))
                                matchIdToPlayersMapping[matchId] = new List<Player>();

                            matchIdToPlayersMapping[matchId].Add(player);
                        }
                    }
                    foreach (var matchId in matchIdToPlayersMapping.Keys)
                    {
                        string matchDetailsString = await NetComm.GetResponseOfURL($"matches/{matchId}", _httpClient);
                        dynamic matchDetails = JsonToFrom.FromJson<dynamic>(matchDetailsString);
                        string normalOrRanked = GetNormalOrRankedMatch(matchDetails);
                        string reply = string.Empty;
                        foreach (var player in matchIdToPlayersMapping[matchId])
                        {
                            reply += GenerateMatchString(matchDetails, normalOrRanked, player);
                        }
                        reply += GenerateDotaBuffLink(matchId);
                        await _botTestingChannel.SendMessageAsync(reply);
                        Program.Logger.Log($"Match Details logged for match id: {matchId}");
                    }
                }
                catch (Exception e)
                {
                    Program.DumpLogger.Log($"Exception in {GetType().Name}\nMessage: {e.Message}\nStack Trace: {e.StackTrace}");
                    Thread.Sleep(30 * 1000);
                    continue;
                }
                Thread.Sleep(5 * 60 * 1000);
            }
        }

        public static string GenerateDotaBuffLink(string matchId)
        {
            return $"Dotabuff: {OpenDotaConfiguration.DotabuffMatchUrl}{matchId}";
        }

        public static string GenerateMatchString(dynamic matchDetails, string normalOrRanked, Player player)
        {
            string winOrLose = FindPlayerGameResult(player, matchDetails);
            string hero = FindHero(player, matchDetails);
            string KDA = FindKDA(player, matchDetails);
            return $"<@{player.DiscordId}> **{winOrLose}** a {normalOrRanked} game.\n**Hero**: {hero}\n**KDA**: {KDA}\n";
        }

        public static string GetNormalOrRankedMatch(dynamic matchDetails)
        {
            if (rankedModes.Contains((int)matchDetails.lobby_type))
                return "ranked";
            else return "normal";
        }

        public static string FindHero(Player player, dynamic matchDetails)
        {
            dynamic playerMatchInfo = GetPlayerMatchInfo(player, matchDetails);
            return HeroDetails.HeroList.Where(x => x.id == (int)playerMatchInfo.hero_id).First().localized_name;
        }

        public static dynamic GetPlayerMatchInfo(Player player, dynamic matchDetails)
        {
            return ((IEnumerable<dynamic>)matchDetails.players).Where(y => y.account_id == player.SteamId).First();
        }

        public static string FindKDA(Player player, dynamic matchDetails)
        {
            var playerMatchInfo = GetPlayerMatchInfo(player, matchDetails);
            return $"{playerMatchInfo.kills}/{playerMatchInfo.deaths}/{playerMatchInfo.assists}";
        }

        public static string FindPlayerGameResult(Player player, dynamic matchDetails)
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
                    jsonString = await NetComm.GetResponseOfURL($"players/{player.SteamId}/wl", _httpClient);
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
    }
}
