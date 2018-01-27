#pragma warning disable CS4014
#pragma warning disable CS1998
using Bot.Contracts;
using System.Collections.Generic;
using DSharpPlus;
using DSharpPlus.Entities;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;

namespace Bot.Implementations
{
    //on hold
    public class AFKVoicePurger //: Functionality
    {
        Dictionary<ulong, CancellationTokenSource> userSubjectToDC = new Dictionary<ulong, CancellationTokenSource>();
        public void Run(DiscordClient discord)
        {
            discord.VoiceStateUpdated += async x =>
            {
                Dictionary<ulong, List<DiscordUser>> channelToUserMapping = new Dictionary<ulong, List<DiscordUser>>();

                PopulateVoiceChatUsers(x.Guild.VoiceStates, channelToUserMapping);

                foreach (var voiceChannel in channelToUserMapping)
                {
                    if (voiceChannel.Value.Count == 1)
                    {
                        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
                        Task.Run(async () =>
                        {
                            Thread.Sleep(10 * 1000);
                            if (!cancellationTokenSource.IsCancellationRequested)
                            {
                                await RemoveUserFromVoice(voiceChannel.Value[0], x.Guild);
                            }
                        }, cancellationTokenSource.Token);
                        userSubjectToDC.Add(voiceChannel.Value[0].Id, cancellationTokenSource);
                    }
                    else
                    {
                        foreach (var user in voiceChannel.Value)
                        {
                            if (userSubjectToDC.ContainsKey(user.Id))
                            {
                                userSubjectToDC[user.Id].Cancel();
                                userSubjectToDC.Remove(user.Id);
                            }
                        }
                    }
                }
            };
        }

        private static void PopulateVoiceChatUsers(IEnumerable<DiscordVoiceState> discordVoiceState, Dictionary<ulong, List<DiscordUser>> channelToUserMapping)
        {
            foreach (var voiceState in discordVoiceState)
            {
                if (voiceState.Channel == null) continue;
                if (!channelToUserMapping.ContainsKey(voiceState.Channel.Id))
                {
                    channelToUserMapping[voiceState.Channel.Id] = new List<DiscordUser>();
                }
                channelToUserMapping[voiceState.Channel.Id].Add(voiceState.User);
            }
        }

        private async Task RemoveUserFromVoice(DiscordUser user, DiscordGuild guild)
        {
            var channel = await guild.CreateChannelAsync("tempVoiceChannel", ChannelType.Voice);
            var member = guild.Members.Where(x => x.Id == user.Id).First();
            await member.PlaceInAsync(channel);
            await channel.DeleteAsync();
            Program.Logger.Log($"Removing user {user.Id} : {user.Username} from voice channel.");
            userSubjectToDC.Remove(user.Id);
        }
    }
}
