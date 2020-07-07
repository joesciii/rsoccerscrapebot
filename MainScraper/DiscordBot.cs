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

            //client connected message
            _client.Ready += () =>
             ((ISocketMessageChannel)_client.GetChannel(728287924495319120)).SendMessageAsync("Connected");

            //when ready, run Ready task
            _client.Ready += Ready;

            //Ready task runs until client disconnects. checks newPostURL against temporary OldURL
            async Task Ready()
            {
                while (true)
                {
                    if (OldURL != Program.newPostURL)
                    {
                        await ((ISocketMessageChannel)_client.GetChannel(728287924495319120)).SendMessageAsync("**New Event: ** " + Program.newPostTitle + "   **|**   " + " **URL: ** " + Program.newPostURL);
                    }

                    OldURL = Program.newPostURL;
                    
                    //arbitrary
                    System.Threading.Thread.Sleep(1000);
                }
            }

            //run until close
            await Task.Delay(-1);

        }

    }
}