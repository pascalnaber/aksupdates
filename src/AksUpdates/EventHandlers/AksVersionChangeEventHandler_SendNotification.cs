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
        INotificationHandler<AksNewLocationAvailableEvent>
    {
        private const string hashTags = "#azure #aks #kubernetes";
        private readonly ITwitterApi twitterApi;
        private readonly ILogger logger;

        public AksVersionChangeEventHandler_SendNotification(ITwitterApi twitterApi, ILogger<AksVersionChangeEventHandler_SendNotification> logger)
        {
            this.twitterApi = twitterApi;
            this.logger = logger;
        }

        public Task Handle(AksNewVersionAvailableEvent notification, CancellationToken cancellationToken)
        {
            string tweet = BuildTweetMessage(notification);
            if (Toggles.SendNotification)
                twitterApi.PostTweet(new TweetMessage(tweet));
            else
                logger.LogInformation($"Toggle triggered: would have send tweet: {tweet}");

            return Task.CompletedTask;
        }

        public Task Handle(AksNewLocationAvailableEvent notification, CancellationToken cancellationToken)
        {
            string tweet = BuildTweetMessage(notification);
            if (Toggles.SendNotification)
                twitterApi.PostTweet(new TweetMessage(tweet));
            else
                logger.LogInformation($"Toggle triggered: would have send tweet: {tweet}");

            return Task.CompletedTask;
        }

        public string BuildTweetMessage(AksNewVersionAvailableEvent notification)
        {
            if (notification.NotificationType.IsPreview)
            {
                return $"Location {notification.Location} in Azure has a new preview version of AKS available: {notification.LatestVersion}" +
                    $"{Environment.NewLine}" +
                    $"Available preview versions: {notification.PreviewVersions}" +
                    $"{Environment.NewLine}" +
                    $"{hashTags}";
            }
            else
                return $"Location {notification.Location} in Azure has a new version of AKS available: {notification.LatestVersion}" +
                        $"{Environment.NewLine}" +
                        $"{hashTags}";
        }

        public string BuildTweetMessage(AksNewLocationAvailableEvent notification)
        {
            if (notification.NotificationType.IsPreview)
                return $"New location {notification.Location} available in Azure supporting AKS preview version {notification.LatestVersion}" +
                     $"{Environment.NewLine}" +
                     $"The following preview versions are available: {notification.PreviewVersions}" +
                     $"{Environment.NewLine}" +
                     $"{hashTags}";
            else
                return $"New location {notification.Location} available in Azure supporting AKS version {notification.LatestVersion}" +
                     $"{Environment.NewLine}" +
                     $"{hashTags}";
        }
    }
}
