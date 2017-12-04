using Bot.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using DSharpPlus.Entities;
using DSharpPlus;
using Bot.Configuration;
using System.Linq;

namespace Bot.Implementations
{
    public class GametimeTracker : Functionality
    {
        private Dictionary<DiscordMember, object> _gamePlaying = new Dictionary<DiscordMember, object>();

        public override void Run(DiscordClient discord)
        {
            var members = GetAllNonBotMembers(discord);
            foreach (var member in members)
            {
                var currentInfo = discord.GetUserAsync(member.Id).GetAwaiter().GetResult();
                _gamePlaying[member] = member.Presence;
            }
        }

        private List<DiscordMember> GetAllNonBotMembers(DiscordClient discord)
        {
            DiscordGuild guild = discord.GetGuildAsync(GuildConfiguration.Id).GetAwaiter().GetResult();
            var members = guild.GetAllMembersAsync().GetAwaiter().GetResult().Where(x => !x.IsBot).ToList();
            return members;
        }
    }
}
