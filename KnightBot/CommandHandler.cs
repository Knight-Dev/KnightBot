﻿using System.Threading.Tasks;
using System.Reflection;
using Discord.Commands;
using Discord.WebSocket;
using Discord;
using System;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using KnightBot.Config;
using KnightBot.Modules.Public;
using KnightBot.util;
using System.IO;
using KnightBot.Modules.Economy;
using Discord.Rest;
using System.Threading;
using System.Timers;
using KnightBot.Modules.Admin;

namespace KnightBot
{
    public class CommandHandler : ModuleBase
    {
        private CommandService commands;
        public DiscordSocketClient bot;
        private IServiceProvider map;
        private SocketGuildUser user;

        private int total;

        private BankConfig save = new BankConfig();

        public static readonly string appdir = AppContext.BaseDirectory;

        public CommandHandler(IServiceProvider provider)
        {
            map = provider;
            bot = map.GetService<DiscordSocketClient>();
            bot.UserJoined += AnnounceUserJoined;
            bot.UserLeft += AnnounceLeftUser;
            bot.Ready += SetGame;
            //Send user message to get handled
            bot.MessageReceived += HandleCommand;
            commands = map.GetService<CommandService>();
            bot.MessageReceived += addMoney;
            bot.ChannelCreated += ChannelCreatedAsync;
            bot.ChannelDestroyed += ChannelDeletedAsync;
            bot.RoleCreated += RoleCreatedAsync;
            bot.RoleDeleted += RoleDeletedAsync;
            bot.RoleUpdated += RoleUpdatedAsync;
        }

        public async Task RoleCreatedAsync(SocketRole role)
        {
            var embed = new EmbedBuilder() { Color = Colors.adminCol };
            var footer = new EmbedFooterBuilder() { Text = "KnightBotV2 By KnightDev" + " | " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + " | " + "KnightDev.xyz" };
            embed.Title = ("**User Created A Role**");
            embed.Description = ("Role Name: " + role.Name + "\nRole Id: " + role.Id + "\nTime: " + DateTime.Now.Hour + ":" + DateTime.Now.Minute);
            embed.WithFooter(footer);

            var logchannel = bot.GetChannel(437977945680773130) as SocketTextChannel;
            await logchannel.SendMessageAsync("", false, embed);
        }

        public async Task RoleDeletedAsync(SocketRole role)
        {
            var embed = new EmbedBuilder() { Color = Colors.adminCol };
            var footer = new EmbedFooterBuilder() { Text = "KnightBotV2 By KnightDev" + " | " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + " | " + "KnightDev.xyz" };
            embed.Title = ("**User Deleted A Role**");
            embed.Description = ("Role Name: " + role.Name + "\nRole Id: " + role.Id + "\nTime: " + DateTime.Now.Hour + ":" + DateTime.Now.Minute);
            embed.WithFooter(footer);

            var logchannel = bot.GetChannel(437977945680773130) as SocketTextChannel;
            await logchannel.SendMessageAsync("", false, embed);
        }

        public async Task RoleUpdatedAsync(SocketRole role, SocketRole role2)
        {
            var embed = new EmbedBuilder() { Color = Colors.adminCol };
            var footer = new EmbedFooterBuilder() { Text = "KnightBotV2 By KnightDev" + " | " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + " | " + "KnightDev.xyz" };

            var blank = new EmbedFieldBuilder() { Name = "\u200b", Value = "\u200b" };
            var roleField = new EmbedFieldBuilder() { Name = "Role Name", Value = role.Name, IsInline = true };
            var roleIdField = new EmbedFieldBuilder() { Name = "Role Id", Value = role.Id, IsInline = true };
            var reacField = new EmbedFieldBuilder() { Name = "Add Reactions", Value = role.Permissions.AddReactions, IsInline = true };
            var adminField = new EmbedFieldBuilder() { Name = "Admin", Value = role.Permissions.Administrator, IsInline = true };
            var atfileField = new EmbedFieldBuilder() { Name = "Attach Files", Value = role.Permissions.AttachFiles, IsInline = true };
            var banField = new EmbedFieldBuilder() { Name = "Can Ban", Value = role.Permissions.BanMembers, IsInline = true };
            var nickField = new EmbedFieldBuilder() { Name = "Change Nickname", Value = role.Permissions.ChangeNickname, IsInline = true };
            var connField = new EmbedFieldBuilder() { Name = "Connect", Value = role.Permissions.Connect, IsInline = true };
            var deafField = new EmbedFieldBuilder() { Name = "Deafen Members", Value = role.Permissions.DeafenMembers, IsInline = true };
            var kickField = new EmbedFieldBuilder() { Name = "Can Kick", Value = role.Permissions.KickMembers, IsInline = true };
            var chnlmngField = new EmbedFieldBuilder() { Name = "Manage Channels", Value = role.Permissions.ManageChannels, IsInline = true };
            var mnggldField = new EmbedFieldBuilder() { Name = "Manage Guild", Value = role.Permissions.ManageGuild, IsInline = true };
            var msgmngField = new EmbedFieldBuilder() { Name = "Manage Messages", Value = role.Permissions.ManageMessages, IsInline = true };
            var nickmngField = new EmbedFieldBuilder() { Name = "Manage Nicknames", Value = role.Permissions.ManageNicknames, IsInline = true };
            var mngrolField = new EmbedFieldBuilder() { Name = "Manage Roles", Value = role.Permissions.ManageRoles, IsInline = true };
            var mnghookField = new EmbedFieldBuilder() { Name = "Manage Webhooks", Value = role.Permissions.ManageWebhooks, IsInline = true };
            var menevryField = new EmbedFieldBuilder() { Name = "Mention Everyone", Value = role.Permissions.MentionEveryone, IsInline = true };
            var mvememField = new EmbedFieldBuilder() { Name = "Move Members", Value = role.Permissions.MoveMembers, IsInline = true };
            var mtememField = new EmbedFieldBuilder() { Name = "Mute Members", Value = role.Permissions.MuteMembers, IsInline = true };
            var msghistField = new EmbedFieldBuilder() { Name = "Read Message History", Value = role.Permissions.ReadMessageHistory, IsInline = true };
            var redmsgField = new EmbedFieldBuilder() { Name = "Read Messages", Value = role.Permissions.ReadMessages, IsInline = true };
            var sndmsgField = new EmbedFieldBuilder() { Name = "Send Messages", Value = role.Permissions.SendMessages, IsInline = true };

            embed.Title = ("**User Updated A Role**");
            embed.Description = ("Info For The Recently Updated Role");
            embed.WithThumbnailUrl("https://www.knightdev.xyz/forums/gifimages/Logo2.png");
            embed.WithFooter(footer);
            embed.AddField(blank);
            embed.AddField(roleField);
            embed.AddField(roleIdField);
            embed.AddField(blank);
            embed.AddField(reacField);
            embed.AddField(adminField);
            embed.AddField(atfileField);
            embed.AddField(banField);
            embed.AddField(nickField);
            embed.AddField(connField);
            embed.AddField(deafField);
            embed.AddField(kickField);
            embed.AddField(chnlmngField);
            embed.AddField(mnggldField);
            embed.AddField(msgmngField);
            embed.AddField(nickmngField);
            embed.AddField(mngrolField);
            embed.AddField(mnghookField);
            embed.AddField(menevryField);
            embed.AddField(mvememField);
            embed.AddField(mtememField);
            embed.AddField(msghistField);
            embed.AddField(redmsgField);
            embed.AddField(sndmsgField);
            embed.AddField(blank);


            var logchannel = bot.GetChannel(437977945680773130) as SocketTextChannel;
            await logchannel.SendMessageAsync("", false, embed);
        }

        public async Task ChannelCreatedAsync(SocketChannel chnl)
        {
            var embed = new EmbedBuilder() { Color = Colors.adminCol };
            var footer = new EmbedFooterBuilder() { Text = "KnightBotV2 By KnightDev" + " | " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + " | " + "KnightDev.xyz" };
            embed.Title = ("**User Created A Channel**");
            embed.Description = ("\nChannel Name: " + chnl + "\nTime: " + DateTime.Now.Hour + ":" + DateTime.Now.Minute);
            embed.WithFooter(footer);

            var logchannel = bot.GetChannel(437977945680773130) as SocketTextChannel;
            await logchannel.SendMessageAsync("", false, embed);
        }

        public async Task ChannelDeletedAsync(SocketChannel chnl)
        {
            var embed = new EmbedBuilder() { Color = Colors.adminCol };
            var footer = new EmbedFooterBuilder() { Text = "KnightBotV2 By KnightDev" + " | " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + " | " + "KnightDev.xyz" };
            embed.Title = ("**User Deleted A Channel**");
            embed.Description = ("\nChannel Name: " + chnl + "\nTime: " + DateTime.Now.Hour + ":" + DateTime.Now.Minute);
            embed.WithFooter(footer);

            var logchannel = bot.GetChannel(437977945680773130) as SocketTextChannel;
            await logchannel.SendMessageAsync("", false, embed);
        }

        

        public async Task addMoney(SocketMessage msg)
        {
            var result = BankConfig.Load("bank/" + user.Id.ToString() + ".json").currentMoney;

            int bal = 10;

            total = result + bal;

            save.userID = BankConfig.Load("bank/" + user.Id.ToString() + ".json").userID;
            save.currentMoney = total;
            save.currentPoints = BankConfig.Load("bank/" + user.Id.ToString() + ".json").currentPoints;
            save.Save("bank/" + user.Id.ToString() + ".json");
        }


        public async Task AnnounceLeftUser(SocketGuildUser user)
        {

            var embed = new EmbedBuilder() { Color = Colors.adminCol };
            var footer = new EmbedFooterBuilder() { Text = "KnightBotV2 By KnightDev" + " | " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + " | " + "KnightDev.xyz" };
            embed.Title = ("**User Left The Discord**");
            embed.Description = ("Username: " + user.Username + "\nTime: " + DateTime.Now.TimeOfDay + "\nTotal Members: " + bot.GetGuild(BotConfig.Load().serverId).MemberCount.ToString());
            embed.WithFooter(footer);

            var logchannel = bot.GetChannel(437977945680773130) as SocketTextChannel;
            await logchannel.SendMessageAsync("", false, embed);

        }

        public async Task AnnounceUserJoined(SocketGuildUser user)
        {

            var server = bot.Guilds.FirstOrDefault(x => x.Id == BotConfig.Load().serverId);
            var guild = server as IGuild;
            await user.AddRoleAsync(guild.Roles.FirstOrDefault(x => x.Name == BotConfig.Load().NewMemberRank));

            var embed = new EmbedBuilder() { Color = Colors.adminCol };
            var footer = new EmbedFooterBuilder() { Text = "KnightBotV2 By KnightDev" + " | " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + " | " + "KnightDev.xyz" };
            embed.Title = ("**User Joined The Discord**");
            embed.Description = ("Username: " + user.Username + "\nTime: " + DateTime.Now.TimeOfDay + "\nTotal Members: " + bot.GetGuild(BotConfig.Load().serverId).MemberCount.ToString());
            embed.WithFooter(footer);
            

            var logchannel = bot.GetChannel(437977945680773130) as SocketTextChannel;
            await logchannel.SendMessageAsync("", false, embed);

        }

        public async Task SetGame()
        {
            await bot.SetGameAsync("Knightdev.xyz");
        }




        public async Task ConfigureAsync()
        {
            await commands.AddModulesAsync(Assembly.GetEntryAssembly());
        }

        public async Task HandleCommand(SocketMessage pMsg)
        {


            //Don't handle the command if it is a system message
            var message = pMsg as SocketUserMessage;
            if (message == null)
                return;
            var context = new SocketCommandContext(bot, message);

            //Mark where the prefix ends and the command begins
            int argPos = 0;
            //Determine if the message has a valid prefix, adjust argPos



            if (message.HasStringPrefix(BotConfig.Load().Prefix, ref argPos))
            {
                if (message.Author.IsBot)
                    return;
                //Execute the command, store the result
                var result = await commands.ExecuteAsync(context, argPos, map);

                //If the command failed, notify the user
                if (!result.IsSuccess && result.ErrorReason != "Unknown command.")
                {
                    var embed = new EmbedBuilder() { Color = Colors.errorcol };
                    var footer = new EmbedFooterBuilder() { Text = "KnightBotV2 By KnightDev" + " | " + DateTime.Today + DateTime.Now };
                    embed.Title = ("**Error**");
                    embed.Description = ($"**Error:** {result.ErrorReason}");
                    embed.WithFooter(footer);
                    await message.Channel.SendMessageAsync("", false, embed);
                }
            }
        }
    }
}