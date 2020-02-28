namespace AksUpdates.Apis.Twitter
{
    public interface ITwitterApi
    {
        void PostTweet(TweetMessage message);
    }
}