using System;
using System.Collections.Generic;

namespace Bot.Models
{
    public class Vote
    {
        public string Subject { get; set; } = string.Empty;
        public List<Tuple<int, string, int>> Options { get; set; } = new List<Tuple<int, string, int>>();
    }
}
