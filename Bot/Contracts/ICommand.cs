﻿using DSharpPlus.Entities;
using System.Threading.Tasks;

namespace Bot.Contracts
{
    public interface ICommand
    {
        Task<string> ProcessAsync(DiscordMessage message);
    }
}
