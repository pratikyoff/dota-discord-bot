using System;
using System.Collections.Generic;

namespace Bot.Models
{
    public class Vote
    {
        public string Id { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public Dictionary<int, Option> Options { get; set; } = new Dictionary<int, Option>();
        public bool InProgress { get; set; } = false;
        public int TimeoutSeconds { get => 60; }
        public ulong OwnerId { get; set; }
        public Dictionary<ulong, int> UserChoiceStore { get; set; } = new Dictionary<ulong, int>();
    }
}
