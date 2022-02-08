using AksUpdates.Apis.Twitter;
using AksUpdates.Events;
using AksUpdates.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AksUpdates.EventHandlers
{
    public class AksVersionChangeEventHandler_SendNotification : 
        INotificationHandler<AksNewVersionAvailableEvent>,
        INotificationHandler<AksNewRegionAvailableEvent>
    {
        private const string hashTags = "#azure #aks #kubernetes";
        private readonly ITwitterApi twitterApi;
        private readonly ILogger logger;

        public AksVersionChangeEventHandler_SendNotification(ITwitterApi twitterApi, ILogger<AksVersionChangeEventHandler_SendNotification> logger)
        {
            this.twitterApi = twitterApi;
            this.logger = logger;
        }

        public async Task Handle(AksNewVersionAvailableEvent notification, CancellationToken cancellationToken)
        {
            string tweet = BuildTweetMessage(notification);
            
            await twitterApi.PostTweet(new TweetMessage(tweet));
            
            //return Task.CompletedTask;
        }

        public async Task Handle(AksNewRegionAvailableEvent notification, CancellationToken cancellationToken)
        {
            string tweet = BuildTweetMessage(notification);
            
            await twitterApi.PostTweet(new TweetMessage(tweet));
            
            //return Task.CompletedTask;
        }

        public string BuildTweetMessage(AksNewVersionAvailableEvent notification)
        {
            //if (notification.NotificationType.IsPreview)
            //{
            //    return $"Region {notification.Region} in Azure has a new preview version of AKS available: {notification.LatestVersion}" +
            //        $"{Environment.NewLine}" +
            //        $"Available preview versions: {notification.PreviewVersions}" +
            //        $"{Environment.NewLine}" +
            //        $"{hashTags}";
            //}
            //else
                return $"Region {notification.Region} in Azure has a new version of AKS available: {notification.LatestVersion}" +
                        $"{Environment.NewLine}" +
                        $"{hashTags}";
        }

        public string BuildTweetMessage(AksNewRegionAvailableEvent notification)
        {
            //if (notification.NotificationType.IsPreview)
            //    return $"New region {notification.Region} available in Azure supporting AKS preview version {notification.LatestVersion}" +
            //         $"{Environment.NewLine}" +
            //         $"The following preview versions are available: {notification.PreviewVersions}" +
            //         $"{Environment.NewLine}" +
            //         $"{hashTags}";
            //else
                return $"New region {notification.Region} available in Azure supporting AKS version {notification.LatestVersion}" +
                     $"{Environment.NewLine}" +
                     $"{hashTags}";
        }
    }
}
