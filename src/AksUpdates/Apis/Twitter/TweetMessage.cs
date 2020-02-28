using System;
using System.Collections.Generic;
using System.Text;

namespace AksUpdates.Apis.Twitter
{
    public class TweetMessage
    {
        public TweetMessage(string tweet)
        {
            this.Tweet = tweet;
        }

        public string Tweet { get; private set; }
    }
}
