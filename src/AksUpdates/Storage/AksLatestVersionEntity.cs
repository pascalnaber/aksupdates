using Microsoft.WindowsAzure.Storage.Table;

namespace AksUpdates.Storage
{
    public class AksLatestVersionEntity : TableEntity
    {
        public AksLatestVersionEntity(string partitionKey, string rowKey, string latestVersion)
        {
            this.PartitionKey = partitionKey;
            this.RowKey = rowKey;
            this.LatestVersion = latestVersion;
        }

        public AksLatestVersionEntity() { }

        public string LatestVersion { get; set; }
    }
}
