using DSharpPlus.Entities;
using System;
using System.Collections.Generic;

namespace Bot
{
    public static class MessageExtensions
    {
        public static IEnumerable<string> WordsWithoutCommand(this DiscordMessage discordMessage)
        {
            var allWords = discordMessage.Content.Split(' ');
            string[] wordsExceptCommand = new string[allWords.Length - 1];
            Array.Copy(allWords, 1, wordsExceptCommand, 0, allWords.Length - 1);
            return wordsExceptCommand;
        }
    }
}
