﻿using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.Commands
{
    static class PingCommand
    {
        public static void createCommand(DiscordClient _client)
        {
            _client.GetService<CommandService>().CreateCommand("ping")
                .Description("Bot answers with Pong!")
                .Do(async e =>
                {
                    await e.Channel.SendMessage(e.User.Mention + " Pong!");
                    await Task.Delay(1000);
                    await e.Channel.SendMessage(e.User.Mention + " Pong! after 1000 ms!");

                    _client.Log.Info(e.User + " used !ping command", null);
                });
        }
    }
}
