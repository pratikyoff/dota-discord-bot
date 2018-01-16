#pragma warning disable CS1998
using Bot.Contracts;
using System;
using DSharpPlus;
using System.Collections.Generic;
using DSharpPlus.Entities;
using Bot.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace Bot.Implementations
{
    public class UserStatusTracker : Functionality
    {
        public override async void Run(DiscordClient discord)
        {
            var members = await GetAllNonBotMembers(discord);
            var botFeedChannel = await discord.GetChannelAsync(BotDetails.BotFeedChannel);
            var botDumpChannel = await discord.GetChannelAsync(BotDetails.BotDumpChannel);
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
                    Program.Logger.Log($"{x.Member.DisplayName} was {x.PresenceBefore.Status} for {GetTimeFormattedString(timeDiff)} and is now {x.Member.Presence.Status}.");
                }
                if (x.PresenceBefore.Game != null && (x.Member.Presence.Game == null || !x.PresenceBefore.Game.Name.Equals(x.Member.Presence.Game.Name)))
                {
                    var timeDiff = DateTime.Now - memberGameStatus[x.Member.Id];
                    memberGameStatus[x.Member.Id] = DateTime.Now;
                    Program.Logger.Log($"{x.Member.DisplayName} played {x.PresenceBefore.Game.Name} for {GetTimeFormattedString(timeDiff)}.");
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

        public async Task<List<DiscordMember>> GetAllNonBotMembers(DiscordClient discord)
        {
            DiscordGuild guild = await discord.GetGuildAsync(GuildConfiguration.Id);
            var members = (await guild.GetAllMembersAsync()).Where(x => !x.IsBot).ToList();
            return members;
        }
    }
}
