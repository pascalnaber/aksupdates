using System;
using System.Collections.Generic;
using System.Text;

namespace AksUpdates.Api.Storage
{
    public class AksVersion
    {
        public AksVersion(string notificationKey, string regionKey, Version version, string region, DateTimeOffset timestamp)
        {
            this.NotificationKey = notificationKey;
            this.RegionKey = regionKey;
            this.Version = version;
            this.Region = region;
            this.Timestamp = timestamp;
        }

        public string NotificationKey { get; set; }

        public string Region { get; set; }

        public string RegionKey { get; set; }

        public Version Version { get; set; }

        public DateTimeOffset Timestamp{ get; set; }
    }
}
