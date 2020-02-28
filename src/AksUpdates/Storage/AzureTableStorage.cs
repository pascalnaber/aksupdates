using AksUpdates.Models;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AksUpdates.Storage
{
    public class AzureTableStorage : IAzureTableStorage
    {
        public async Task<IDictionary<string, Version>> GetStoredLatestVersions(string key)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(Settings.GetSetting(Settings.TableStorageConnectionString));
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference(Settings.GetSetting(Settings.TableStorageName));

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

        public async Task AddOrUpdateLatestVersion(string storagePartitionKey, string location, string latestVersion)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(Settings.GetSetting(Settings.TableStorageConnectionString));
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference(Settings.GetSetting(Settings.TableStorageName));

            TableOperation tableOperation = TableOperation.InsertOrReplace(new AksLatestVersionEntity(storagePartitionKey, location, latestVersion));
            await table.ExecuteAsync(tableOperation);
        }
    }
}
