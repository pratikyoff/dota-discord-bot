using Bot.Configuration;
using Bot.Models;
using DSharpPlus.Entities;

namespace Bot
{
    public static class DiscordUserExtensions
    {
        public static string SteamId(this DiscordUser discordUser)
        {
            string steamId = PlayerConfiguration.Players.Find(x => x.DiscordId.Equals(discordUser.Id.ToString()))?.SteamId;
            return steamId;
        }

        public static Player ToPlayer(this DiscordUser discordUser)
        {
            return PlayerConfiguration.Players.Find(x => x.DiscordId.Equals(discordUser.Id.ToString()));
        }
    }
}
