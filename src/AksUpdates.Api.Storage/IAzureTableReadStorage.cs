using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AksUpdates.Api.Storage
{
    public interface IAzureTableReadStorage
    {
        Task<List<AksVersion>> GetAllData();
    }
}