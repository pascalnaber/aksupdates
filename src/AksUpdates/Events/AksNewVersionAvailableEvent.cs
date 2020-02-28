using AksUpdates.Models;
using MediatR;
using System;

namespace AksUpdates.Events
{
    public class AksNewVersionAvailableEvent : INotification
    {
        public NotificationType NotificationType { get; set; }

        public string LocationKey { get; set; }

        public string Location { get; set; }

        public Version LatestVersion { get; set; }

        public string PreviewVersions { get; set; }
    }
}
