﻿using System;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Discord.Commands;
using System.Linq;
using KnightBot.Config;
using KnightBot.util;
using System.Net.Http;
using System.Net;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Xml.Linq;
using Newtonsoft.Json;
using TwitchLib;
using KnightBot.Modules.Economy;

namespace KnightBot.Modules.Public
{
    public class ImagesModule : ModuleBase
    {
        Errors errors = new Errors();

        private int total;

        private BankConfig save = new BankConfig();


        [Command("doggo")]
        [Summary("Posts random doggo pictures!")]
        public async Task Dog()
        {
            await Program.Logger(new LogMessage(LogSeverity.Debug, "[API]", "The Dog API Is Loading!"));
            using (var client = new HttpClient(new HttpClientHandler { AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate }))
            {
                string websiteurl = "http://random.dog/woof.json";
                client.BaseAddress = new Uri(websiteurl);
                HttpResponseMessage response = client.GetAsync("").Result;
                response.EnsureSuccessStatusCode();
                string result = await response.Content.ReadAsStringAsync();
                var json = JObject.Parse(result);
                string DogImage = json["url"].ToString();


                var embed = new EmbedBuilder()
                {
                    Color = Colors.generalCol
                };

                embed.WithImageUrl(DogImage);
                await Context.Channel.SendMessageAsync("", false, embed);


            }
            await Context.Message.DeleteAsync();

            var result2 = BankConfig.Load("bank/" + Context.User.Id.ToString() + ".json").currentMoney;

            int bal = 10;

            total = result2 + bal;

            save.userID = BankConfig.Load("bank/" + Context.User.Id.ToString() + ".json").userID;
            save.currentMoney = total;
            save.currentPoints = BankConfig.Load("bank/" + Context.User.Id.ToString() + ".json").currentPoints;
            save.Save("bank/" + Context.User.Id.ToString() + ".json");

        }

        [Command("cat")]
        [Summary("Posts random cat pictures!")]
        public async Task Cat()
        {
            await Program.Logger(new LogMessage(LogSeverity.Debug, "[API]", "The Cat API Is Loading!"));
            using (var client = new HttpClient(new HttpClientHandler { AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate }))
            {
                string websiteurl = "http://aws.random.cat//meow";
                client.BaseAddress = new Uri(websiteurl);
                HttpResponseMessage response = client.GetAsync("").Result;
                response.EnsureSuccessStatusCode();
                string result = await response.Content.ReadAsStringAsync();
                var json = JObject.Parse(result);
                string CatImage = json["file"].ToString();


                var embed = new EmbedBuilder()
                {
                    Color = Colors.generalCol
                };

                embed.WithImageUrl(CatImage);
                await Context.Channel.SendMessageAsync("", false, embed);


            }
            await Context.Message.DeleteAsync();

            var result2 = BankConfig.Load("bank/" + Context.User.Id.ToString() + ".json").currentMoney;

            int bal = 10;

            total = result2 + bal;

            save.userID = BankConfig.Load("bank/" + Context.User.Id.ToString() + ".json").userID;
            save.currentMoney = total;
            save.currentPoints = BankConfig.Load("bank/" + Context.User.Id.ToString() + ".json").currentPoints;
            save.Save("bank/" + Context.User.Id.ToString() + ".json");

        }


        [Command("meme")]
        [Summary("Posts random meme pictures!")]
        public async Task Meme()
        {
            await Program.Logger(new LogMessage(LogSeverity.Debug, "[API]", "The Meme API Is Loading!"));
            using (var client = new HttpClient(new HttpClientHandler { AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate }))
            {
                string websiteurl = "https://api.imgflip.com/get_memes";
                client.BaseAddress = new Uri(websiteurl);
                HttpResponseMessage response = client.GetAsync("").Result;
                response.EnsureSuccessStatusCode();
                string result = await response.Content.ReadAsStringAsync();
                var json = JObject.Parse(result);
                string DogImage = json["url"].FirstOrDefault().ToString();


                var embed = new EmbedBuilder()
                {
                    Color = Colors.generalCol
                };

                embed.WithImageUrl(DogImage);
                await Context.Channel.SendMessageAsync("", false, embed);


            }
            await Context.Message.DeleteAsync();
        }


    }
}
