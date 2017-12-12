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
            var botFeedChannel = discord.GetChannelAsync(BotDetails.BotFeedChannel).GetAwaiter().GetResult();
            var botDumpChannel = discord.GetChannelAsync(BotDetails.BotDumpChannel).GetAwaiter().GetResult();
            Dictionary<ulong, DateTime> memberStatus = new Dictionary<ulong, DateTime>();
            Dictionary<ulong, DateTime> memberGameStatus = new Dictionary<ulong, DateTime>();
            members.ForEach(x =>
            {
                memberStatus[x.Id] = DateTime.Now;
                memberGameStatus[x.Id] = DateTime.Now;
            });
            discord.PresenceUpdated += async x =>
            {
                if (!memberStatus.ContainsKey(x.Member.Id)) return;
                if (x.PresenceBefore.Status != x.Member.Presence.Status)
                {
                    var timeDiff = DateTime.Now - memberStatus[x.Member.Id];
                    memberStatus[x.Member.Id] = DateTime.Now;
                    await botDumpChannel.SendMessageAsync($"{DateTime.Now}: {x.Member.DisplayName} was {x.PresenceBefore.Status} for {GetTimeFormattedString(timeDiff)} and is now {x.Member.Presence.Status}.");
                }
                if (x.PresenceBefore.Game != null && (x.Member.Presence.Game == null || !x.PresenceBefore.Game.Name.Equals(x.Member.Presence.Game.Name)))
                {
                    var timeDiff = DateTime.Now - memberGameStatus[x.Member.Id];
                    memberGameStatus[x.Member.Id] = DateTime.Now;
                    await botDumpChannel.SendMessageAsync($"{DateTime.Now}: {x.Member.Mention} played {x.PresenceBefore.Game.Name} for {GetTimeFormattedString(timeDiff)}.");
                }
            };
        }

        private string GetTimeFormattedString(TimeSpan timeSpan)
        {
            var reply = string.Empty;
            List<string> timeComponents = new List<string>();
            if (timeSpan.TotalDays >= 1)
            {
                int noOfDays = (int)timeSpan.TotalDays;
                timeComponents.Add(noOfDays + " days");
                timeSpan = timeSpan.Subtract(new TimeSpan(noOfDays, 0, 0, 0));
            }
            if (timeSpan.TotalHours >= 1)
            {
                int noOfHours = (int)timeSpan.TotalHours;
                timeComponents.Add(noOfHours + " hours");
                timeSpan = timeSpan.Subtract(new TimeSpan(noOfHours, 0, 0));
            }
            if (timeSpan.TotalMinutes >= 1)
            {
                int noOfMinutes = (int)timeSpan.TotalMinutes;
                timeComponents.Add(noOfMinutes + " minutes");
                timeSpan = timeSpan.Subtract(new TimeSpan(0, noOfMinutes, 0));
            }
            if (timeSpan.TotalSeconds >= 1)
            {
                int noOfSeconds = (int)timeSpan.TotalSeconds;
                timeComponents.Add(noOfSeconds + " seconds");
                timeSpan = timeSpan.Subtract(new TimeSpan(0, 0, noOfSeconds));
            }
            reply = string.Join(" ", timeComponents);
            return reply;
        }

        public List<DiscordMember> GetAllNonBotMembers(DiscordClient discord)
        {
            DiscordGuild guild = discord.GetGuildAsync(GuildConfiguration.Id).GetAwaiter().GetResult();
            var members = guild.GetAllMembersAsync().GetAwaiter().GetResult().Where(x => !x.IsBot).ToList();
            return members;
        }
    }
}
