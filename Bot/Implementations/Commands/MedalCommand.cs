using Bot.Configuration;
using Bot.Contracts;
using Bot.Universal;
using DSharpPlus.Entities;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Bot.Implementations.Commands
{
    [Command("medal")]
    public class MedalCommand : ICommand
    {
        public MedalCommand()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(OpenDotaConfiguration.OpenDotaAPIAddress)
            };
        }

        private HttpClient _httpClient;

        public async Task<string> Process(DiscordMessage message)
        {
            var words = message.Content.Split(' ');
            string toReturn = string.Empty;
            if (words.Length == 1)
            {
                toReturn = await GetMedal(message.Author.Id);
            }
            else
            {
                foreach (var user in message.MentionedUsers)
                {
                    toReturn += $"{await GetMedal(user.Id)}\n";
                }
                toReturn = toReturn.TrimEnd('\n');
            }
            return toReturn;
        }

        private async Task<string> GetMedal(ulong id)
        {
            string steamId = PlayerConfiguration.Players.Where(x => x.DiscordId.Equals(id.ToString())).FirstOrDefault().SteamId;
            var url = $"players/{steamId}";
            var jsonString = await NetComm.GetResponseOfURL(url, _httpClient);
            var playerInfo = JsonToFrom.FromJson<dynamic>(jsonString);
            string playerMedal = (string)playerInfo.rank_tier;
            var actualMedal = (Medal)(int)char.GetNumericValue(playerMedal[0]);
            var star = (int)char.GetNumericValue(playerMedal[1]);
            return $"<@{id}> is {actualMedal}[{star}]";
        }
    }
}
