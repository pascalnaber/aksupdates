﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tweetinvi;
//using Tweetinvi;
using Tweetinvi.Models;

namespace AksUpdates.Apis.Twitter
{
    public class TwitterApi : ITwitterApi
    {
        private readonly string consumerApiKey = Settings.GetSetting(Settings.TwitterApiKey);
        private readonly string consumerApiSecret = Settings.GetSetting(Settings.TwitterApiSecretKey);
        private readonly string accessToken = Settings.GetSetting(Settings.TwitterAccessToken); 
        private readonly string accessTokenSecret = Settings.GetSetting(Settings.TwitterAccessTokenSecret);

        public async Task PostTweet(TweetMessage message)
        {
            var twitterClient = new TwitterClient(consumerApiKey, consumerApiSecret, accessToken, accessTokenSecret);
            await twitterClient.Tweets.PublishTweetAsync(message.Tweet);

            //var creds = new TwitterCredentials(consumerApiKey, consumerApiSecret, accessToken, accessTokenSecret);
            //Tweetinvi.
            //var tweet = Tweetinvi.Auth.ExecuteOperationWithCredentials(creds, () => Tweetinvi.Tweet.PublishTweet(message.Tweet));
        }
    }
}
