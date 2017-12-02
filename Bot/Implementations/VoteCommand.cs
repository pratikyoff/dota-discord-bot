using Bot.Configuration;
using Bot.Contracts;
using Bot.Models;
using DSharpPlus.Entities;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

namespace Bot.Implementations
{
    public class VoteCommand : ICommand
    {
        private static Vote _vote = new Vote();
        public string Process(DiscordMessage message)
        {
            if (CommandConfiguration.VoteCommandString.Length + 2 > message.Content.Length)
                return "What are you doing?";
            string nonCommand = message.Content.Substring(CommandConfiguration.VoteCommandString.Length + 2);
            string[] words = nonCommand.Split(' ');
            int choice = 0;
            bool isChoice = int.TryParse(words[0], out choice);
            if (StatusCommandCheck(words))
                return GetVoteStatus();
            if (_vote.InProgress)
            {
                if (isChoice)
                {
                    if (UserHasAlreadyVoted(message))
                        return "You cannot vote twice.";
                    if (choice == 0) return "No choice exists.";
                    else
                    {
                        if (choice > _vote.Options.Count || choice < 1)
                            return "No such choice present.";
                        _vote.Options[choice].Votes++;
                        _vote.UserVotedStore.Add(message.Author.Id.ToString());
                        return $"{message.Author.Mention} cast the vote as {_vote.Options[choice].Name}.";
                    }
                }
                else
                {
                    return "Cannot create vote while another is in progress.";
                }
            }
            else
            {
                if (isChoice)
                {
                    return "No vote in progress.";
                }
                else
                {
                    if (words.Length == 0)
                        return "No subject in the vote.";
                    if (words.Last().IndexOf('|') < 0)
                        return "No Options provided.";
                    var options = words.Last().Split('|').Where(x => x != null && x.Length > 0);
                    if (options.Count() < 2)
                        return "Atleast two options are needed for a vote to be created.";
                    _vote.Subject = string.Join(" ", words.Take(words.Length - 1));
                    int i = 1;
                    foreach (var option in options)
                    {
                        _vote.Options[i++] = new Option()
                        {
                            Name = option,
                            Votes = 0
                        };
                    }
                    _vote.InProgress = true;
                    TimeoutVote(message);
                    return GetVoteStatus();
                }
            }
        }

        private bool UserHasAlreadyVoted(DiscordMessage message)
        {
            return _vote.UserVotedStore.Contains(message.Author.Id.ToString());
        }

        private static string GetVoteStatus()
        {
            if (_vote.InProgress)
                return $"{_vote.Subject}\n{GetOptionsText(_vote.Options)}";
            else
                return "No vote is in progress.";
        }

        private bool StatusCommandCheck(string[] words)
        {
            return words[0].ToLowerInvariant().Equals("status");
        }

        private static void TimeoutVote(DiscordMessage message)
        {
            Task.Factory.StartNew(() =>
            {
                Thread.Sleep(_vote.TimeoutSeconds * 1000);
                message.RespondAsync($"Vote has been time ended.\nHere are the final results.\n{GetVoteStatus()}").GetAwaiter().GetResult();
                _vote = new Vote();
            });
        }

        private static string GetOptionsText(Dictionary<int, Option> options)
        {
            string optionsText = string.Empty;
            foreach (var numberedOption in options)
            {
                optionsText += $"{numberedOption.Key}. {numberedOption.Value.Name} - {numberedOption.Value.Votes} votes.\n";
            }
            return optionsText;
        }
    }
}
