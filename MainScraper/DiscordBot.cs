using System;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;



namespace MainScraper
{
    public class DiscordBot
    {
        public static string OldURL = "";
        
        //bot named _client
        private DiscordSocketClient _client;
        
        //start new bot
        public static void RunBot()
          => new DiscordBot().StartAsync().GetAwaiter();

        public async Task StartAsync()
        {
            _client = new DiscordSocketClient();

            await _client.LoginAsync(TokenType.Bot, "discord bot token");

            await _client.StartAsync();

            //when ready, run Ready task
            _client.Ready += Ready;

            //Ready task runs until client disconnects. checks newPostURL against temporary OldURL
            //for some reason Ready task only runs as intended when the two functions are separated
            //with nested if. assume due to async nature of SendMessage task.
            async Task Ready()
            {
                Console.WriteLine("Discord bot connected...");
                while (true)
                {
                    if (OldURL != Program.newPostURL)
                    {
                        EmbedBuilder embed = new EmbedBuilder
                        {
                            Title = Program.newPostTitle,
                            Description = Program.newPostURL
                        };
                        if (OldURL != Program.newPostURL)
                        {
                            await ((ISocketMessageChannel)_client.GetChannel(728287924495319120)).SendMessageAsync(embed: embed.Build());
                        }

                    }

                    
                    OldURL = Program.newPostURL;

                    //arbitrary
                    System.Threading.Thread.Sleep(1000);
                }
            }

            //run until closed by user
            await Task.Delay(-1);

        }

    }
}