﻿using System.Threading.Tasks;

namespace AksUpdates.Apis.Twitter
{
    public interface ITwitterApi
    {
        //void PostTweet(TweetMessage message);
        Task PostTweet(TweetMessage message);
    }
}