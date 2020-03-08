//using AksUpdates.Apis.Twitter;
//using AksUpdates.Models;
//using Microsoft.Azure.WebJobs;
//using Microsoft.Azure.WebJobs.Extensions.DurableTask;
//using Microsoft.Extensions.Logging;
//using System;
//using System.Collections.Generic;
//using System.Text;
//using Tweetinvi;
//using Tweetinvi.Models;

//namespace AksUpdates.Activities
//{
//    public class PostTweet
//    {
//        private readonly string consumerApiKey = Settings.GetSetting(Settings.TwitterApiKey);
//        private readonly string consumerApiSecret = Settings.GetSetting(Settings.TwitterApiSecretKey);
//        private readonly string accessToken = Settings.GetSetting(Settings.TwitterAccessToken);
//        private readonly string accessTokenSecret = Settings.GetSetting(Settings.TwitterAccessTokenSecret);

//        [FunctionName(nameof(PostTweet))]
//        public void Run(
//            [ActivityTrigger] TweetMessage message,
//            ILogger logger)
//        {
//            logger.LogInformation($"Started {nameof(PostTweet)} for { message}.");

//            var creds = new TwitterCredentials(consumerApiKey, consumerApiSecret, accessToken, accessTokenSecret);

//            var tweet = Auth.ExecuteOperationWithCredentials(creds, () => Tweet.PublishTweet(message.Tweet));

//            logger.LogInformation($"Finished {nameof(PostTweet)} with tweet: {tweet.Url}.");
//        }
//    }
//}
