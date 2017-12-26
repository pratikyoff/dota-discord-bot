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
        public async Task<string> Process(DiscordMessage message)
        {
            if (CommandConfiguration.VoteCommandString.Length + 2 > message.Content.Length)
                return "What are you doing?";
            string nonCommand = message.Content.Substring(CommandConfiguration.VoteCommandString.Length + 2);
            string[] words = nonCommand.Split(' ');
            bool isChoice = int.TryParse(words[0], out int choice);
            if (StatusCommandCheck(words))
                return GetVoteStatus();
            if (EndCommandCheck(words))
            {
                return EndVote(message);
            }
            if (_vote.InProgress)
            {
                if (isChoice)
                {
                    if (choice > _vote.Options.Count || choice < 1)
                        return "No such choice present.";
                    if (HasUserVoted(message))
                    {
                        if (choice == _vote.UserChoiceStore[message.Author.Id])
                        {
                            return "You have already voted for this choice.";
                        }
                        else
                        {
                            var prevVote = _vote.UserChoiceStore[message.Author.Id];
                            _vote.Options[prevVote].Votes--;
                            _vote.UserChoiceStore[message.Author.Id] = choice;
                            _vote.Options[choice].Votes++;
                            return $"{message.Author.Mention} cast the vote as {_vote.Options[choice].Name}.";
                        }
                    }
                    else
                    {
                        _vote.Options[choice].Votes++;
                        _vote.UserChoiceStore[message.Author.Id] = choice;
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
                    _vote.OwnerId = message.Author.Id;
                    _vote.Id = Guid.NewGuid().ToString();
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
                    TimeoutVote(message, _vote.Id);
                    return GetVoteStatus();
                }
            }
        }

        private string EndVote(DiscordMessage message)
        {
            if (_vote.InProgress)
            {
                if (_vote.OwnerId == message.Author.Id)
                {
                    string reply = $"Vote has been ended by {message.Author.Mention}.\nHere are the final results.\n{GetVoteStatus()}";
                    _vote = new Vote();
                    return reply;
                }
                else
                    return $"Only the vote initiator (<@{_vote.OwnerId}>) can end the vote.";

            }
            else
                return "No vote in progress.";
        }

        private bool EndCommandCheck(string[] words)
        {
            return words[0].ToLowerInvariant().Equals("end");
        }

        private bool HasUserVoted(DiscordMessage message)
        {
            return _vote.UserChoiceStore.ContainsKey(message.Author.Id);
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

        private static void TimeoutVote(DiscordMessage message, string voteId)
        {
            Task.Run(async () =>
            {
                Thread.Sleep(_vote.TimeoutSeconds * 1000);
                if (_vote.InProgress && _vote.Id.Equals(voteId))
                {
                    await message.RespondAsync($"Vote has been time ended.\nHere are the final results.\n{GetVoteStatus()}");
                    _vote = new Vote();
                }
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
