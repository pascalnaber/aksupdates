using Microsoft.WindowsAzure.Storage.Table;
using System;

namespace AksUpdates.Api.Storage
{
    public class AksLatestVersionEntity : TableEntity
    {
        public AksLatestVersionEntity(string partitionKey, string rowKey, string latestVersion, string region)
        {
            this.PartitionKey = partitionKey;
            this.RowKey = rowKey;
            this.LatestVersion = latestVersion;
            this.Region = region;            
        }

        public AksLatestVersionEntity() { }

        public string LatestVersion { get; set; }

        public string Region { get; set; }
    }
}
