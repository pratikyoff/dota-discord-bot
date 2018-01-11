using Bot.Contracts;
using Bot.Models;
using DSharpPlus.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System.Text.RegularExpressions;

namespace Bot.Implementations
{
    [Command("remindme")]
    public class RemindMeCommand : ICommand
    {
        private static Time _second = new Time()
        {
            Aliases = new List<string>() { "seconds", "second", "secs", "sec", "s" },
            UnitTimeInSeconds = 1
        };

        private static Time _minute = new Time()
        {
            Aliases = new List<string>() { "minutes", "minute", "mins", "min" },
            UnitTimeInSeconds = 60
        };

        private static Time _hour = new Time()
        {
            Aliases = new List<string>() { "hours", "hour", "hrs", "hr" },
            UnitTimeInSeconds = 60 * 60
        };

        private static Time _day = new Time()
        {
            Aliases = new List<string>() { "days", "day" },
            UnitTimeInSeconds = 24 * 60 * 60
        };

        private List<Time> _listOfTimeConfig = new List<Time>() { _second, _minute, _hour, _day };

        public Task<string> Process(DiscordMessage message)
        {
            string[] words = message.Content.Split(' ');
            if (words.Length < 3)
                return Task.FromResult("Reminder must contain a subject and timer.\nUsage: `!RemindMe I have to do something! 2hours`");
            string timerString = words.Last();
            int timeInSeconds = GetTimeInSeconds(timerString);
            if (timeInSeconds < 0) return Task.FromResult("Enter a valid time.");
            string subject = string.Join(" ", words.Skip(1).SkipLast(1));
            Task.Run(async () =>
            {
                Thread.Sleep(timeInSeconds * 1000);
                await message.RespondAsync($"Reminder for {message.Author.Mention}\nSubject: {subject}");
            });
            return Task.FromResult($"Reminder created {message.Author.Mention}");
        }

        private int GetTimeInSeconds(string timerString)
        {
            foreach (var timeUnit in _listOfTimeConfig)
            {
                foreach (var alias in timeUnit.Aliases)
                {
                    if (Regex.IsMatch(timerString, $@"^[0-9]+(?i){alias}$"))
                    {
                        int time = int.Parse(Regex.Match(timerString, @"[0-9]+").Value);
                        return time * timeUnit.UnitTimeInSeconds;
                    }
                }
            }
            return -1;
        }
    }
}
