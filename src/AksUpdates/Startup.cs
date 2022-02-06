using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;
using MediatR;
using AksUpdates.Apis.Storage;
using AksUpdates.Orchestrations;
using AksUpdates.Apis.Azure;
using AksUpdates.Apis.Twitter;
using AksUpdates.Models;
using Microsoft.Extensions.Configuration;

[assembly: FunctionsStartup(typeof(AksUpdates.Startup))]
namespace AksUpdates
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder), "builder cannot be null");

            if (Toggles.SendNotification)
                builder.Services.AddScoped<ITwitterApi, TwitterApi>();
            else
                builder.Services.AddScoped<ITwitterApi, DummyTwitterApi>();

            builder.Services.AddOptions<AzureTableStorageConfiguration>()
                .Configure<IConfiguration>((settings, configuration) =>
                {
                    configuration.GetSection("azureTableStorageConfiguration").Bind(settings);
                });

            builder.Services.AddScoped<IAksUpdateOrchestrator, AksUpdateOrchestrator>();
            builder.Services.AddScoped<IAzureApi, AzureApi>();            
            builder.Services.AddScoped<IAzureTableStorage, AzureTableStorage>();            
            builder.Services.AddMediatR(typeof(Startup));
        }
    }
}
