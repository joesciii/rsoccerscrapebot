using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;



namespace MainScraper
{
    public class DiscordBot
    {
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

            //send the message. currently doesn't run when required
            _client.Ready += () =>
            ((ISocketMessageChannel)_client.GetChannel(728287924495319120)).SendMessageAsync("New Goal: " + Program.newPostTitle + " | " + " URL: " + Program.newPostURL);

            //continue running until closed
            await Task.Delay(-1);



        }






    }
}