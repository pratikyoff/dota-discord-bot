using Bot.Contracts;
using System.Collections.Generic;
using DSharpPlus;
using DSharpPlus.Entities;
using Bot.Configuration;

namespace Bot.Implementations
{
    public class MemberTracker : Functionality
    {
        public static Dictionary<ulong, DiscordMember> Members = new Dictionary<ulong, DiscordMember>();

        public MemberTracker()
        {
            DiscordGuild guild = Program.Discord.GetGuildAsync(GuildConfiguration.Id).GetAwaiter().GetResult();
            var members = guild.Members;
            foreach (var member in members)
            {
                Members[member.Id] = member;
            }
        }

        public override void Run(DiscordClient discord)
        {
            Program.Discord.GuildMemberAdded += async x => Members[x.Member.Id] = x.Member;
            Program.Discord.GuildMemberRemoved += async x => Members.Remove(x.Member.Id);
        }
    }
}
