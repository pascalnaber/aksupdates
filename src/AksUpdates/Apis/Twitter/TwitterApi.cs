using System;
using System.Collections.Generic;
using System.Text;
using Tweetinvi;
using Tweetinvi.Models;

namespace AksUpdates.Apis.Twitter
{
    public class TwitterApi : ITwitterApi
    {
        private readonly string consumerApiKey = Settings.GetSetting(Settings.TwitterApiKey);
        private readonly string consumerApiSecret = Settings.GetSetting(Settings.TwitterApiSecretKey);
        private readonly string accessToken = Settings.GetSetting(Settings.TwitterAccessToken); 
        private readonly string accessTokenSecret = Settings.GetSetting(Settings.TwitterAccessTokenSecret);

        public void PostTweet(TweetMessage message)
        {
            var creds = new TwitterCredentials(consumerApiKey, consumerApiSecret, accessToken, accessTokenSecret);

            var tweet = Auth.ExecuteOperationWithCredentials(creds, () => Tweetinvi.Tweet.PublishTweet(message.Tweet));
        }
    }
}
