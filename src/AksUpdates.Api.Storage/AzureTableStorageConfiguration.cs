﻿using System;
using System.Collections.Generic;
using System.Text;

namespace AksUpdates.Api.Storage
{
    public class AzureTableStorageConfiguration
    {
        public string TableStorageConnectionString { get; set; }

        public string TableStorageName { get; set; }
    }
}
