using Reddit;
using Reddit.Controllers;
using Reddit.Controllers.EventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using Discord;
using Discord.WebSocket;
using System.Threading.Tasks;

namespace MainScraper
{
    public class Program
    {
    //global vars to be passed to discord bot
    public static string stringToCheck { get; set; }
    public static string newPostURL { get; set; }
    public static string newPostTitle { get; set; }


    //init new reddit client and begin monitoring subreddit
    static void Main(string[] args)
    {
        if (args.Length < 2)
        {
            Console.WriteLine("Usage: Example <Reddit App ID> <Reddit Refresh Token> [Reddit Access Token]");
        }
        else
        {
            string appId = args[0];
            string refreshToken = args[1];
            string appSecret = (args.Length > 2 ? args[2] : null);

            RedditClient reddit = new RedditClient(appId: appId, refreshToken: refreshToken, appSecret: appSecret);

            Subreddit sub = reddit.Subreddit("soccer").About();

            Console.WriteLine("Subreddit Name: " + sub.Name);
            Console.WriteLine("Subreddit Fullname: " + sub.Fullname);
            Console.WriteLine("Subreddit Title: " + sub.Title);
            Console.WriteLine("Subreddit Description: " + sub.Description);

            List<Post> newPosts = sub.Posts.New;

            Console.WriteLine("Retrieved " + newPosts.Count.ToString() + " new posts.");

            Console.WriteLine("Monitoring " + sub.Name + " for new posts....");

            sub.Posts.NewUpdated += C_NewPostsUpdated;
            sub.Posts.MonitorNew();

        }
        DiscordBot.RunBot();
    }



    // Event handler for handling monitored new posts as they come in. Filters down to relevant linkposts. 
    public static void C_NewPostsUpdated(object sender, PostsUpdateEventArgs e)
    {
        foreach (Post post in e.Added)
        {
            string[] teams = {"Leeds",
                "Arsenal",
                "Chelsea",
                "Manchester United",
                "Manchester City",
                "Liverpool",
                "Leicester",
                "Bournemouth",
                "Burnley",
                "Brighton",
                "Norwich City",
                "Aston Villa",
                "Everton",
                "Wolves",
                "Tottenham",
                "Spurs",
                "Crystal Palace",
                "West Ham",
                "Sheffield United",
                "Watford",
                "Southampton",
                "Newcastle"};

            string[] mediaSites = { "streamja.com",
                    "streamable.com",
                    "clippituser.tv" };


            //rewrote to avoid LinkPost
            if (!post.Listing.IsSelf)
            {
                stringToCheck = post.Title + post.Listing.URL;
            }

            if (teams.Any(x => stringToCheck.Contains(x)) && mediaSites.Any(x => stringToCheck.Contains(x)))
            {
                newPostTitle = post.Title;
                newPostURL = post.Listing.URL;
                Console.WriteLine("TITLE: " + newPostTitle);
                Console.WriteLine("URL: " + newPostURL);

            }
        }
    }
}
}