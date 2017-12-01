using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bot.Contracts
{
    public interface ICommand
    {
        string Process(DiscordMessage message);
    }
}
