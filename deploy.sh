#!/bin/bash -ex

# This script pulls the latest code from the Github repo and redeploys the bot.
# This script is intended to be used only on a Linux machine.

# Usage
# $ ./deploy.sh <branch-name>

# This is what happens, stepwise:
# 1. Pull the latest code from the remote repo
# 2. Deploy the bot

git clean -fd
git reset --hard HEAD
git fetch origin $1
git checkout $1

cd Bot/
dotnet clean
dotnet run
cd ..