using Bot.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bot.Configuration
{
    public static class PlayerConfiguration
    {
        public static List<Player> Players { get; set; } = new List<Player>()
        {
            new Player(){Name="bacchan",DiscordId="156757490120392704",SteamId="168208270"},
            new Player(){Name="kapoor",DiscordId="234245021618929674",SteamId="153548141"},
            new Player(){Name="pkyo",DiscordId="162522319737061376",SteamId="190500077"},
            new Player(){Name="vartak",DiscordId="260725572173430784",SteamId="222073001"},
            new Player(){Name="mittu boy",DiscordId="271245661574397952",SteamId="374869768"},
            new Player(){Name="cocky",DiscordId="252827119313354754",SteamId="183398205"},
            new Player(){Name="tushar",DiscordId="460080097299267584",SteamId="443689621"}
        };
    }
}
