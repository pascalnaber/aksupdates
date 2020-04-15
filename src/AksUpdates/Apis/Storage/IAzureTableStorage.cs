using AksUpdates.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AksUpdates.Apis.Storage
{
    public interface IAzureTableStorage
    {
        Task<IDictionary<string, Version>> GetStoredLatestVersions(string key);
        Task AddOrUpdateLatestVersion(string storagePartitionKey, string location, string latestVersion);
    }
}