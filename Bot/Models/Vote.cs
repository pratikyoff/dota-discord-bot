﻿using System;
using System.Collections.Generic;

namespace Bot.Models
{
    public class Vote
    {
        public string Subject { get; set; } = string.Empty;
        public Dictionary<int, Option> Options { get; set; } = new Dictionary<int, Option>();
        public bool InProgress { get; set; } = false;
        public int TimeoutSeconds { get => 60; }
        public List<string> UserVotedStore { get; set; } = new List<string>();
    }
}
