using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AksUpdates.Apis.Storage
{
    public interface IAzureTableReadStorage
    {
        Task<List<AksVersion>> GetAllData();
    }
}