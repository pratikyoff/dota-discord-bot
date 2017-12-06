using System;
using System.Collections.Generic;
using System.Text;

namespace Bot.Models
{
    public class Time
    {
        public List<string> Aliases { get; set; } = new List<string>();
        public int UnitTimeInSeconds { get; set; } = 0;
    }
}
