using Bot.Contracts;
using System;
using DSharpPlus;
using System.Collections.Generic;
using DSharpPlus.Entities;
using Bot.Configuration;
using System.Linq;

namespace Bot.Implementations
{
    public class UserStatusTracker : Functionality
    {
        public override void Run(DiscordClient discord)
        {
            var members = GetAllNonBotMembers(discord);
            members.ForEach(x => Console.WriteLine(x.Presence));
        }

        public List<DiscordMember> GetAllNonBotMembers(DiscordClient discord)
        {
            DiscordGuild guild = discord.GetGuildAsync(GuildConfiguration.Id).GetAwaiter().GetResult();
            var members = guild.GetAllMembersAsync().GetAwaiter().GetResult().Where(x => !x.IsBot).ToList();
            return members;
        }
    }
}
