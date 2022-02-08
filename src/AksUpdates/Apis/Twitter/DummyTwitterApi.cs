using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AksUpdates.Apis.Twitter
{
    public class DummyTwitterApi : ITwitterApi
    {
        private readonly ILogger<DummyTwitterApi> logger;

        public DummyTwitterApi(ILogger<DummyTwitterApi> logger)
        {
            this.logger = logger;
        }

        public async Task PostTweet(TweetMessage message)
        {
            logger.LogInformation($"Dummy Tweet: would have send tweet: {message.Tweet}");
        }
    }
}
