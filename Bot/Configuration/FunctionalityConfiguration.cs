using Bot.Contracts;
using Bot.Implementations;
using System.Collections.Generic;

namespace Bot.Configuration
{
    public static class FunctionalityConfiguration
    {
        private static Functionality _dotaGameTracker = new DotaGameTracker();
        private static Functionality _userStatusTracker = new UserStatusTracker();

        public static List<Functionality> Functionalities { get; set; } = new List<Functionality>()
        {
            _dotaGameTracker,
            _userStatusTracker
        };
    }
}
