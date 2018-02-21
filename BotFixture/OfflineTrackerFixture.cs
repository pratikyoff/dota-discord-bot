using Bot.Implementations;
using DSharpPlus;
using DSharpPlus.Entities;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace BotFixture
{
    public class OfflineTrackerFixture
    {
        [Fact]
        public void RunFixture()
        {
            Mock<DiscordMessage> mockMessage = new Mock<DiscordMessage>();
            mockMessage.SetupGet(x => x.Content).Returns(MatchDetailsExample.Example);
            Mock<DiscordChannel> mockChannel = new Mock<DiscordChannel>();
            mockChannel.Setup(x => x.GetMessagesAsync(It.IsAny<int>(), null, null, null)).ReturnsAsync(new List<DiscordMessage>() { mockMessage.Object });
            Mock<DiscordClient> mockClient = new Mock<DiscordClient>();
            mockClient.Setup(x => x.GetChannelAsync(It.IsAny<ulong>())).ReturnsAsync(mockChannel.Object);
            OfflineTracker offlineTracker = new OfflineTracker();
            offlineTracker.Start(mockClient.Object);
        }
    }
}
