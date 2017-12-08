using Bot.Contracts;
using Bot.Implementations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bot.Configuration
{
    public static class FunctionalityConfiguration
    {
        private static Functionality _gametimeTracker = new GametimeTracker();
        private static Functionality _dotaGameTracker = new DotaGameTracker();
        //private static Functionality _userStatusTracker = new UserStatusTracker();

        public static List<Functionality> Functionalities { get; set; } = new List<Functionality>()
        {
            _gametimeTracker,
            _dotaGameTracker,
            //_userStatusTracker
        };
    }
}
