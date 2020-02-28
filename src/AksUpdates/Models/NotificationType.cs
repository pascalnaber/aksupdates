using System;
using System.Collections.Generic;
using System.Text;

namespace AksUpdates.Models
{
    public class NotificationType
    {
        public const string AksPreview = "akspreview";
        public const string Aks = "aks";
        public NotificationType(string notificationTypeKey)
        {
            if (notificationTypeKey != AksPreview && notificationTypeKey != Aks)
                throw new Exception($"notificationtypekey {notificationTypeKey} not known");

            this.NotificationTypeKey = notificationTypeKey;
        }

        public string NotificationTypeKey { get; private set; }

        public bool IsPreview
        {
            get { return NotificationTypeKey == AksPreview; }
        }

        public static NotificationType[] NotificationTypes
        {
            get { return new NotificationType[] { new NotificationType(AksPreview), new NotificationType(Aks) }; }
        }
    }
}
