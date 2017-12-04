using System;
using System.Collections.Generic;
using System.Text;

namespace Bot.Models
{
    public class Player
    {
        public string Name { get; set; }
        public string DiscordId { get; set; }
        public string SteamId { get; set; }
        public int TotalMatches { get; set; }
        public int MatchesWon { get; set; }
        public int MatchesLost { get; set; }
    }
}
