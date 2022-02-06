using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AksUpdates.Apis.Storage
{
    public class AzureTableStorage : IAzureTableStorage, IAzureTableReadStorage
    {        
        private readonly IOptions<AzureTableStorageConfiguration> configuration;
        private readonly string tableStorageConnectionString;
        private readonly string tableStorageName;

        public AzureTableStorage(IOptions<AzureTableStorageConfiguration> configuration)
        {
            this.configuration = configuration;
            tableStorageConnectionString = configuration.Value.TableStorageConnectionString;
            tableStorageName = configuration.Value.TableStorageName;
        }

        public async Task<IDictionary<string, Version>> GetStoredLatestVersions(string key)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(this.tableStorageConnectionString);
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference(this.tableStorageName);

            await table.CreateIfNotExistsAsync();
            TableQuery<AksLatestVersionEntity> query = new TableQuery<AksLatestVersionEntity>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, key));

            TableContinuationToken continuationToken = null;
            TableQuerySegment<AksLatestVersionEntity> tableQueryResult = await table.ExecuteQuerySegmentedAsync(query, continuationToken);

            var locations = new Dictionary<string, Version>();

            foreach (AksLatestVersionEntity entity in tableQueryResult)
            {
                locations.Add(entity.RowKey, new Version(entity.LatestVersion));
            }
            return locations;
        }

        public async Task<List<AksVersion>> GetAllData()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(tableStorageConnectionString);
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference(tableStorageName);

            await table.CreateIfNotExistsAsync();
            
            TableContinuationToken token = null;
            var aksVersions = new List<AksVersion>();
            do
            {
                var queryResult = await table.ExecuteQuerySegmentedAsync(new TableQuery<AksLatestVersionEntity>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "aks")), token);

                foreach (AksLatestVersionEntity entity in queryResult.Results)
                {
                    aksVersions.Add(new AksVersion(entity.RowKey, entity.PartitionKey, new Version(entity.LatestVersion), entity.Region, entity.Timestamp));
                }

                token = queryResult.ContinuationToken;
            }
            while (token != null);

            return aksVersions;
        }

        public async Task AddOrUpdateLatestVersion(AksVersion aksVersion) //string storagePartitionKey, string location, string latestVersion)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(tableStorageConnectionString);
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference(tableStorageName);

            TableOperation tableOperation = TableOperation.InsertOrReplace(new AksLatestVersionEntity(aksVersion.NotificationKey, aksVersion.RegionKey, aksVersion.Version.ToString(), aksVersion.Region));
            await table.ExecuteAsync(tableOperation);
        }
    }
}
