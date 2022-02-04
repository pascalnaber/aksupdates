using AksUpdates.Apis.Azure;
using AksUpdates.Events;
using AksUpdates.Extensions;
using AksUpdates.Models;
using AksUpdates.Apis.Storage;
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
        private ILogger<AksUpdateOrchestrator> _logger;

        public AksUpdateOrchestrator(IMediator mediator, IAzureApi azureApi, IAzureTableStorage azureTableStorage, ILogger<AksUpdateOrchestrator> logger)
        {
            this.mediator = mediator;
            this.azureApi = azureApi;
            this.azureTableStorage = azureTableStorage;
            this._logger = logger;
        }
                
        public async Task Run()
        {            
            _logger.LogInformation($"Orchestration triggered {nameof(AksUpdateOrchestrator)}");

            var supportedRegions = (await this.azureApi.GetAksRegions()).GetLocationsOfferingAks().ButNot(new [] {"Korea South" });

            foreach (var notificationType in NotificationType.NotificationTypes)
            {
                var storedLatestVersionsPerRegion = await this.azureTableStorage.GetStoredLatestVersions(notificationType.NotificationTypeKey);
                await VerifyAksVersionChangesForRegions(supportedRegions, storedLatestVersionsPerRegion, notificationType);
            }

            _logger.LogInformation($"Orchestration finished {nameof(AksUpdateOrchestrator)}");
        }       

        private async Task VerifyAksVersionChangesForRegions(IEnumerable<string> supportedLocations, IDictionary<string, Version> storedLatestVersionsPerLocation, NotificationType notificationType)
        {
            foreach (var supportedLocation in supportedLocations)
            {
                await VerifyAksVersionChangesForRegion(supportedLocation, storedLatestVersionsPerLocation, notificationType);
            }
        }

        public async Task VerifyAksVersionChangesForRegion(string supportedLocation, IDictionary<string, Version> storedLatestVersionsPerLocation, NotificationType notificationType)
        {
            var regionKey = supportedLocation.Replace(" ", "").ToLower();

            var json = await this.azureApi.GetAksVersionsByRegion(regionKey);
            Version latestVersion = json.GetLatestVersion(notificationType.IsPreview);
            //string allPreviewVersions = String.Join(", ", json.GetAllPreviewVersions());

            if (storedLatestVersionsPerLocation.ContainsKey(regionKey))
            {
                if (latestVersion > storedLatestVersionsPerLocation[regionKey])
                {
                    this._logger?.LogInformation($"New version {latestVersion} available for region {regionKey}");
                    await this.mediator.Publish(new AksNewVersionAvailableEvent() { NotificationType = notificationType, RegionKey = regionKey, Region = supportedLocation, LatestVersion = latestVersion });
                }
            }
            else
            {
                this._logger?.LogInformation($"New region available for version {latestVersion}");
                await this.mediator.Publish(new AksNewRegionAvailableEvent() { NotificationType = notificationType, RegionKey = regionKey, Region = supportedLocation, LatestVersion = latestVersion });
            }
        }
    }
}
