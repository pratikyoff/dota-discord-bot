using Xunit;
using Bot.Implementations;
using Bot.Models;
using Newtonsoft.Json;

namespace BotFixture
{
    public class GameTrackerFixture
    {
        private dynamic _exampleMatchDetails = JsonConvert.DeserializeObject(MatchDetailsExample.Example);
        private dynamic _player = new Player() { SteamId = "190500077" };

        [Fact]
        public void FindHeroFixture()
        {
            DotaGameTracker dotaGameTracker = new DotaGameTracker();
            string hero = dotaGameTracker.FindHero(_player, _exampleMatchDetails);
            Assert.Equal("Shadow Fiend", hero);
        }

        [Fact]
        public void FindKDAFixture()
        {
            DotaGameTracker dotaGameTracker = new DotaGameTracker();
            string KDA = dotaGameTracker.FindKDA(_player, _exampleMatchDetails);
            Assert.Equal("15/1/8", KDA);
        }
    }
}
