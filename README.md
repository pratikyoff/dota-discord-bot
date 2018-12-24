# Dota 2 Discord Bot

The main purpose of this bot is to post a message on your Discord group, whenever you or your friends play a match. But it does some other tasks as well. Below is a list of the functionalities.

## Implemented

1. Dota 2 Game tracking - Post a message on your Discord group after someone finishes a match. Win/Loss, Hero played, KDA and a Dotabuff link of the match is posted.
2. Daily Game tracking - At midnight the number of matches played on that day and the number of wins are displayed.
3. Polls - Initiate a poll, vote on it, and display the results of the poll.
4. Spam Control - Same message sent within 2 seconds of the previous one by a user is deleted.

## Upcoming

1. Gametime tracking - Keep track of time spent by a user on a particular game.

## Complete Documentation

    !doc                                    Display available commands
    !ping                                   Find the ping time between the bot and the Discord server
    !vote <Topic> <option1>|<option2>...    Start a poll with the given topic and options
      !vote <number>                        Vote for an existing poll with the option corresponding to the number
      !vote status                          Display the status of an the existing poll
      !vote end                             End an existing poll
    !RemindMe <Reminder> <Time><Unit>       Set a reminder which evaluates in the given time.
    !abuse <@mention>                       Abuse a user.
      !abuse add <Abuse>                    Add an expletive to the abuse list.
    !medal                                  Display your current dota medal.
      !medal <@mention>                     Display the user's current dota medal.
    !recent <@mention> <Hours>              Display the number of games played by a user in the last <Hours> hours.
                                            If omitted:
                                            <@mention> defaults to - you
                                            <Hours> defaults to 12

## Iterative Development

Make your changes, commit them, and push them to the remote. Next, on the server where the bot is running, stop the bot and run the deploying script:

```bash
./deploy.sh <branch-name>
```

where `<branch-name>` is the name of the branch you want to deploy. The script will automatically download the latest code from the remote, build your bot and run it. Make sure you have execute permissions to run this script.
