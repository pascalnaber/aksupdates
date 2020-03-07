using AksUpdates.Apis.Azure;
using AksUpdates.Events;
using AksUpdates.Extensions;
using AksUpdates.Models;
using AksUpdates.Storage;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AksUpdates.Orchestrations
{
    public class AksUpdateOrchestrator : IAksUpdateOrchestrator
    {
        private readonly IMediator mediator;
        private readonly IAzureApi azureApi;
        private readonly IAzureTableStorage azureTableStorage;

        public AksUpdateOrchestrator(IMediator mediator, IAzureApi azureApi, IAzureTableStorage azureTableStorage)
        {
            this.mediator = mediator;
            this.azureApi = azureApi;
            this.azureTableStorage = azureTableStorage;
        }
                
        public async Task Run(ILogger log)
        {
            log.LogInformation($"Orchestration triggered {nameof(AksUpdateOrchestrator)}");

            var supportedLocations = (await this.azureApi.GetAksLocations()).GetLocationsOfferingAks();

            foreach (var notificationType in NotificationType.NotificationTypes)
            {
                var storedLatestVersionPerLocation = await this.azureTableStorage.GetStoredLatestVersions(notificationType.NotificationTypeKey);
                await VerifyAksVersionChanges(supportedLocations, storedLatestVersionPerLocation, notificationType);
            }
            
            log.LogInformation($"Orchestration finished {nameof(AksUpdateOrchestrator)}");
        }

        //[FunctionName(nameof(AksUpdateOrchestrator))]
        //public async Task Run(
        //    [OrchestrationTrigger] IDurableOrchestrationContext context, 
        //    ILogger log)
        //{            
        //    log.LogInformation($"Orchestration triggered {nameof(AksUpdateOrchestrator)}");

        //    string token = await this.azureApi.GetAuthorizationToken();

        //    var supportedLocations = GetLocationsOfferingAks(await this.azureApi.GetAksLocations(token));

        //    foreach (var notificationType in NotificationType.NotificationTypes)
        //    {
        //        var storedLatestVersionPerLocation = await this.azureTableStorage.GetStoredLatestVersions(notificationType.NotificationTypeKey);
        //        await VerifyAksVersionChanges(supportedLocations, storedLatestVersionPerLocation, notificationType, token);
        //    }

        //    log.LogInformation($"Orchestration finished {nameof(AksUpdateOrchestrator)}");
        //}

        private async Task VerifyAksVersionChanges(IEnumerable<string> supportedLocations, IDictionary<string, Version> storedLatestVersionPerLocation, NotificationType notificationType)
        {
            foreach (var supportedLocation in supportedLocations)
            {
                var locationKey = supportedLocation.Replace(" ", "").ToLower();

                var json = await this.azureApi.GetAksVersionsByLocation(locationKey);
                Version latestVersion = json.GetLatestVersion(notificationType.IsPreview);
                string allPreviewVersions = String.Join(", ", json.GetAllPreviewVersions());

                if (storedLatestVersionPerLocation.ContainsKey(locationKey))
                {
                    if (latestVersion > storedLatestVersionPerLocation[locationKey])
                    {
                        await this.mediator.Publish(new AksNewVersionAvailableEvent() { NotificationType = notificationType, LocationKey = locationKey, Location = supportedLocation, LatestVersion = latestVersion, PreviewVersions = allPreviewVersions });                        
                    }
                }
                else
                {
                    await this.mediator.Publish(new AksNewLocationAvailableEvent() { NotificationType = notificationType, LocationKey = locationKey, Location = supportedLocation, LatestVersion = latestVersion, PreviewVersions = allPreviewVersions });
                }
            }
        }

    }
}
