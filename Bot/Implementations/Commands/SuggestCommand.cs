using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bot.Configuration;
using Bot.Contracts;
using Bot.Universal;
using DSharpPlus.Entities;

namespace Bot.Implementations.Commands
{
    [Command("suggest")]
    public class SuggestCommand : ICommand
    {
        private readonly int _matchesLimit = 500;
        private readonly int _havingAtleastMatches = 10;
        private readonly Random _random = new Random();
        private readonly int _delimiter = 5;
        private readonly int _longestLength = HeroDetails.LongestName().Length;

        public async Task<string> ProcessAsync(DiscordMessage message)
        {
            string url = $"players/{message.Author.SteamId()}/heroes?limit={_matchesLimit}&having={_havingAtleastMatches}";
            var jsonString = await NetComm.GetResponseOfURL(url);
            var heroDetails = JsonToFrom.Deserialize<dynamic>(jsonString);
            List<int> randomedHeroId = GetRandomedHeroIds(heroDetails);

            string toReturn = string.Empty;
            toReturn += $"<@{message.Author.Id}> Here are some recent heroes which you have played:\n";
            for (int i = 0; i < randomedHeroId.Count; i += _delimiter)
            {
                toReturn += '`';
                for (int j = i; j < i + _delimiter && j < randomedHeroId.Count; j++)
                {
                    string currentHeroName = HeroDetails.GetHeroById(randomedHeroId[j]).localized_name;
                    toReturn += $"{currentHeroName}" + GetSpacesForHeroName(currentHeroName);
                }
                toReturn += "`\n";
            }

            return toReturn;
        }

        private string GetSpacesForHeroName(string currentHeroName)
        {
            string spaces = string.Empty;
            for (int i = 0; i < _longestLength - currentHeroName.Length; i++)
            {
                spaces += " ";
            }
            return spaces;
        }

        private List<int> GetRandomedHeroIds(dynamic heroDetails)
        {
            List<dynamic> heroList = new List<dynamic>(heroDetails);
            List<int> random = new List<int>();
            while (heroList.Count > 0)
            {
                int index = _random.Next(heroList.Count);
                random.Add((int)heroList[index].hero_id);
                heroList.RemoveAt(index);
            }
            return random;
        }
    }
}
