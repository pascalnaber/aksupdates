using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;
using MediatR;
using AksUpdates.Storage;
using AksUpdates.Orchestrations;
using AksUpdates.Apis.Azure;
using AksUpdates.Apis.Twitter;

[assembly: FunctionsStartup(typeof(AksUpdates.Startup))]
namespace AksUpdates
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder), "builder cannot be null");
            
            builder.Services.AddScoped<IAksUpdateOrchestrator, AksUpdateOrchestrator>();
            builder.Services.AddScoped<IAzureApi, AzureApi>();
            builder.Services.AddScoped<ITwitterApi, TwitterApi>();
            builder.Services.AddScoped<IAzureTableStorage, AzureTableStorage>();
            builder.Services.AddMediatR(typeof(Startup));
        }
    }
}
