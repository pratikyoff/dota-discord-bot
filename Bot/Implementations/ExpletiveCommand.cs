using Bot.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using DSharpPlus.Entities;
using Bot.Configuration;
using System.Linq;
using Bot.Universal;
using System.Text.RegularExpressions;
using System.IO;

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
                    case "add":
                        message.DeleteAsync();
                        if (userAlreadyProcessing(message.Author.Id))
                        {
                            return "You cannot submit another expletive until your first one is processed.";
                        }
                        if (words.Select(x => x.Equals(":user:")).Count() == 0)
                        {
                            return "There must be a `:user:` mentioned in the abuse.";
                        }
                        string encryptedExpletive = _crypter.Encrypt(string.Join(' ', words.Skip(1)));
                        string toStore = $"{message.Author.Id}|{encryptedExpletive}";
                        FileOperations.AppendLine(ExpletiveConfig.UnconfiremedExpletivesFile, toStore);
                        return $"{message.Author.Mention}, your abuse has been submitted for processing.";
                    case "status":
                        var members = Program.Discord.GetGuildAsync(GuildConfiguration.Id).GetAwaiter().GetResult().Members;
                        DiscordMember messageAuthor = null;
                        foreach (var member in members)
                        {
                            if (member.Id != message.Author.Id) continue;
                            messageAuthor = member;
                            if (!member.IsOwner)
                            {
                                return "You are not authorized to use this command.";
                            }
                            else break;
                        }
                        using (StreamReader reader = new StreamReader(ExpletiveConfig.UnconfiremedExpletivesFile))
                        {
                            string line = null;
                            messageAuthor.SendMessageAsync("Vote Status:").GetAwaiter().GetResult();
                            while ((line = reader.ReadLine()) != null)
                            {
                                words = line.Split('|');
                                string actualLine = $"{GetNameFromId(words[0], members)} - {_crypter.Decrypt(words[1])}";
                                messageAuthor.SendMessageAsync(actualLine);
                            }
                        }
                        return "Status sent.";
                }
            }
            return string.Empty;
        }

        private string GetNameFromId(string id, IEnumerable<DiscordMember> members)
        {
            foreach (var member in members)
            {
                if (member.Id.ToString().Equals(id))
                    return member.DisplayName;
            }
            return null;
        }

        private bool userAlreadyProcessing(ulong id)
        {
            using (StreamReader sr = new StreamReader(ExpletiveConfig.UnconfiremedExpletivesFile))
            {
                string line = null;
                while ((line = sr.ReadLine()) != null)
                {
                    string idInLine = line.Split('|')[0];
                    if (id.ToString().Equals(idInLine))
                        return true;
                }
            }
            return false;
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
