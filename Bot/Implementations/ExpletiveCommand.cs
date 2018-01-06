using Bot.Contracts;
using System;
using System.Collections.Generic;
using DSharpPlus.Entities;
using Bot.Configuration;
using System.Linq;
using Bot.Universal;
using System.Text.RegularExpressions;
using System.IO;
using System.Threading.Tasks;

namespace Bot.Implementations
{
    public class ExpletiveCommand : ICommand
    {
        private ICrypter _crypter;
        private IEnumerable<DiscordMember> _members;
        private Dictionary<ulong, DateTime> _lastUseTime;
        private int _timeLimitSecs;

        public ExpletiveCommand()
        {
            _crypter = new Crypter();
            _members = Program.Discord.GetGuildAsync(GuildConfiguration.Id).GetAwaiter().GetResult().Members;
            _lastUseTime = new Dictionary<ulong, DateTime>();
            _timeLimitSecs = 30;
        }

        public async Task<string> Process(DiscordMessage message)
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
                    if (_lastUseTime.Keys.Contains(message.Author.Id))
                    {
                        var diff = DateTime.UtcNow - _lastUseTime[message.Author.Id];
                        int seconds = (int)Math.Ceiling(diff.TotalSeconds);
                        if (seconds < _timeLimitSecs)
                            return $"{message.Author.Mention} You can abuse again in {_timeLimitSecs - seconds} seconds.";
                    }
                    _lastUseTime[message.Author.Id] = DateTime.UtcNow;
                    return GetRandomAbuse(message.MentionedUsers.First().Id);
                }
            }
            else
            {
                switch (words[0].ToLowerInvariant())
                {
                    case "add":
                        await message.DeleteAsync();
                        if (UserAlreadyProcessing(message.Author.Id))
                        {
                            return "You cannot submit another expletive until your first one is processed.";
                        }
                        if (words.Where(x => x.IndexOf(":user:") >= 0).Count() == 0)
                        {
                            return "There must be a `:user:` mentioned in the abuse.";
                        }
                        string encryptedExpletive = _crypter.Encrypt(string.Join(' ', words.Skip(1)));
                        string toStore = $"{message.Author.Id}|{encryptedExpletive}";
                        FileOperations.AppendLine(ExpletiveConfig.UnconfiremedExpletivesFile, toStore);
                        return $"{message.Author.Mention}, your abuse has been submitted for processing.";

                    case "status":
                        DiscordMember messageAuthor = IsMemberAuthorized(message.Author.Id, _members);
                        if (messageAuthor == null) return "You are not authorized to use this command.";
                        using (StreamReader reader = new StreamReader(ExpletiveConfig.UnconfiremedExpletivesFile))
                        {
                            string line = null;
                            await messageAuthor.SendMessageAsync("Vote Status:");
                            while ((line = reader.ReadLine()) != null)
                            {
                                var splitLine = line.Split('|');
                                string actualLine = $"{GetNameFromId(splitLine[0], _members)} - {_crypter.Decrypt(splitLine[1])}";
                                await messageAuthor.SendMessageAsync(actualLine);
                            }
                        }
                        return "Status sent.";

                    case "approve":
                        messageAuthor = IsMemberAuthorized(message.Author.Id, _members);
                        if (messageAuthor == null) return "You are not authorized to use this command.";
                        if (words.Length != 2)
                            return "Usage: `approve @user`";
                        var approvedUser = message.MentionedUsers.First();
                        int lineNo = -1;
                        string expletive = null;
                        string submitter = null;
                        using (StreamReader reader = new StreamReader(ExpletiveConfig.UnconfiremedExpletivesFile))
                        {
                            string line = null;
                            for (int i = 0; (line = reader.ReadLine()) != null; i++)
                            {
                                words = line.Split('|');
                                if (words[0].Equals(approvedUser.Id.ToString()))
                                {
                                    lineNo = i;
                                    expletive = words[1];
                                    submitter = words[0];
                                    break;
                                }
                            }
                        }
                        if (lineNo < 0)
                            return "Submission not found for the user.";
                        FileOperations.AppendLine(ExpletiveConfig.StoredExpletivesFile, expletive);
                        FileOperations.DeleteLine(ExpletiveConfig.UnconfiremedExpletivesFile, lineNo);
                        return $"<@{submitter}>, your abuse has been approved.";

                    case "reject":
                        messageAuthor = IsMemberAuthorized(message.Author.Id, _members);
                        if (messageAuthor == null) return "You are not authorized to use this command.";
                        if (words.Length != 2)
                            return "Usage: `reject @user`";
                        approvedUser = message.MentionedUsers.First();
                        lineNo = -1;
                        expletive = null;
                        submitter = null;
                        using (StreamReader reader = new StreamReader(ExpletiveConfig.UnconfiremedExpletivesFile))
                        {
                            string line = null;
                            for (int i = 0; (line = reader.ReadLine()) != null; i++)
                            {
                                words = line.Split('|');
                                if (words[0].Equals(messageAuthor.Id.ToString()))
                                {
                                    lineNo = i;
                                    expletive = line.Split('|')[1];
                                    submitter = line.Split('|')[0];
                                    break;
                                }
                            }
                        }
                        if (lineNo < 0)
                            return "Submission not found for the user.";
                        FileOperations.DeleteLine(ExpletiveConfig.UnconfiremedExpletivesFile, lineNo);
                        return $"<@{submitter}>, your abuse has been rejected.";
                }
            }
            return string.Empty;
        }

        private DiscordMember IsMemberAuthorized(ulong id, IEnumerable<DiscordMember> members)
        {
            foreach (var member in members)
            {
                if (member.Id != id) continue;
                if (member.IsOwner)
                {
                    return member;
                }
                else break;
            }
            return null;
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

        private bool UserAlreadyProcessing(ulong id)
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
