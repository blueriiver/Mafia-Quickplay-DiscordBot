﻿using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiscordBot.Game;
using Discord.WebSocket;

namespace DiscordBot.Roles
{
    class Cop : MafiaRole
    {
        public Cop(string username) :base("Cop","Every night you can target someone, you will learn their alignment.\n\nEach night you can target someone by saying: `SCAN: [playername]` in your PM!")
        {
            this.rolePM = $"Dear **{username}**,\nYou are the **{Title}**.\n\n{description}\n\nYou win with the **Town** whose goal is to defeat all members of the Mafia.";
            power = "You can now select your cop target. Please do so in the following format: `SCAN: [playername]`";
            PowerRole = true;
        }

        protected override async Task powerHandler(SocketMessage e, GamePlayerList g)
        {
            if (e.Content.StartsWith("SCAN: ") && e.Channel.Id == (await e.Author.GetOrCreateDMChannelAsync() as IMessageChannel).Id)
            {
                string target = e.Content.Replace("SCAN: ", "");
                if(g.inGame(g.Find(target)))
                {
                    Target = g.Find(target);
                    if(Target.User.Nickname != null)
                        await e.Author.SendMessageAsync("", false new EmbedBuilder() {
						title = "Cop Night Start"
						Color = Color.Blue, Description = $"You will be scanning: {Target.User.Username} tonight. Use `SCAN: [playername]` to change your target."});
                    else
                        await e.Author.SendMessageAsync("", false new EmbedBuilder() {
						title = "Cop Chosen"
						Color = Color.Blue, Description = $"You will be scanning: {Target.User.Username} tonight. Use `SCAN: [playername]` to change your target."});
                } else
                {
                    await e.Author.SendMessageAsync("", false new EmbedBuilder() {
					title = "Invalid"
					Color = Color.Blue, Description = $"Your input was invalid. You inputted: {target}"});
                }
            }
        }

        public override async Task<bool> powerResult(IGuildUser user, Player target)
        {
            try
            {
                if (target.Role.Title == "Godfather")
                    await user.SendMessageAsync("", false, new EmbedBuilder() {
                    title = "Godfather",
                    Color = Color.Blue, Description = $"You checked {target.User.Username}, they are: {RoleUtil.Allignment.Town.ToString()}"});
                else if (target.Role.Title == "Miller")
                    await user.SendMessageAsync("", false, new EmbedBuilder() {
                    title = "Miller",
                    Color = Color.Blue, Description = $"You checked {target.User.Username}, they are: {RoleUtil.Allignment.Mafia.ToString()}"});
                else
                    await user.SendMessageAsync("", false, new EmbedBuilder() {
                    title = "Cop Check Result",
                    Color = Color.Blue, Description = $"You checked {target.User.Username}, they are:{target.Role.Allignment}"}); 
            } catch(NullReferenceException)
            {
                await user.SendMessageAsync("", false new EmbedBuilder() {
				title = "Did Not Target"
				Color.Color.Blue, Description = "You didn't target anyone last night."});
            }
            return await base.powerResult(user, target);
        }

    }
}
