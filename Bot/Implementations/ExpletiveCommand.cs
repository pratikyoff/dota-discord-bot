using Bot.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using DSharpPlus.Entities;
using Bot.Configuration;
using System.Linq;
using Bot.Universal;
using System.Text.RegularExpressions;

namespace Bot.Implementations
{
    public class ExpletiveCommand : ICommand
    {
        private ICrypter _crypter;

        public ExpletiveCommand()
        {
            _crypter = new Crypter();
        }

        public string Process(DiscordMessage message)
        {
            if (CommandConfiguration.ExpletiveCommandString.Length + 2 > message.Content.Length)
                return "Are you stupid?";
            string nonCommand = message.Content.Substring(CommandConfiguration.ExpletiveCommandString.Length + 2);
            string[] words = nonCommand.Split(' ');
            string[] subCommands = { "add", "confirm", "delete", "status" };
            if (IsMention(words[0]))
            {
                if (words.Length > 1)
                {
                    return "What are you doing?";
                }
                else
                {
                    return GetRandomAbuse(message.MentionedUsers.First().Id);
                }
            }
            else
            {
                switch (words[0].ToLowerInvariant())
                {
                    case "add": break;
                }
            }
            return string.Empty;
        }

        private string GetRandomAbuse(ulong id)
        {
            int totalLines = FileOperations.GetTotalNoOfLinesInFile(ExpletiveConfig.StoredExpletivesFile);
            int randomLineNo = (new Random()).Next(totalLines);
            string line = FileOperations.ReadLineNo(ExpletiveConfig.StoredExpletivesFile, randomLineNo);
            string decrypted = _crypter.Decrypt(line);
            string userText = ":user:";
            decrypted = decrypted.Replace(userText, $"<@{id}>");
            return decrypted;
        }

        private bool IsMention(string word)
        {
            string userId = string.Empty;
            try
            {
                userId = Regex.Matches(word, "[0-9]+").First().Value;
            }
            catch
            {
                return false;
            }
            if (IsAValidUser(userId))
            {
                return true;
            }
            return false;
        }

        private bool IsAValidUser(string userId)
        {
            foreach (var player in PlayerConfiguration.Players)
            {
                if (player.DiscordId.Equals(userId))
                    return true;
            }
            return false;
        }
    }
}
