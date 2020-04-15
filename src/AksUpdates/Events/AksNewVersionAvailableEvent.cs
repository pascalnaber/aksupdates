using AksUpdates.Models;
using MediatR;
using System;

namespace AksUpdates.Events
{
    public class AksNewVersionAvailableEvent : INotification
    {
        public NotificationType NotificationType { get; set; }

        public string RegionKey { get; set; }

        public string Region { get; set; }

        public Version LatestVersion { get; set; }

        public string PreviewVersions { get; set; }
    }
}
