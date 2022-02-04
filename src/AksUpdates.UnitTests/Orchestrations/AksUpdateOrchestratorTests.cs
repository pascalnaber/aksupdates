using AksUpdates.Apis.Azure;
using AksUpdates.Events;
using AksUpdates.Models;
using AksUpdates.Models.AksVersionsPerLocation;
using AksUpdates.Orchestrations;
using AksUpdates.Apis.Storage;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace AksUpdates.UnitTests.Orchestrations
{
    public class AksUpdateOrchestratorTests
    {
        [Fact]
        public void ShouldRaiseNewLocationEvent()
        {
            // Arrange
            var mediatrMock = new Mock<IMediator>();
            var azureApiMock = new Mock<IAzureApi>();
            var azureTableStorageMock = new Mock<IAzureTableStorage>();
            var loggerMock = new Mock<ILogger<AksUpdateOrchestrator>>();

            string latestVersion = "1.15.7";
            string supportedLocation = "West Europe";
            string supportedLocationShort = supportedLocation.Replace(" ", "").ToLower();
            var notificationType = new NotificationType(NotificationType.Aks);
            string previewVersions = "1.16.1, 1.16.4, 1.17.0";
            Dictionary<string, Version> storedlatestversionsPerLocation = new Dictionary<string, Version>();
            List<string> storedRegions = new List<string>() { "" };

            azureApiMock.Setup(x => x.GetAksVersionsByRegion(It.IsAny<string>())).ReturnsAsync(File.ReadAllText("get-versions-result-2019-04-01.json"));
            azureApiMock.Setup(x => x.GetAksRegions()).ReturnsAsync(File.ReadAllText("get-provider-containerservice.json"));

            mediatrMock.Setup(c => c.Publish<AksNewRegionAvailableEvent>(
                It.Is<AksNewRegionAvailableEvent>(arg => 
                     arg.LatestVersion == new Version(latestVersion) &&
                     arg.Region == supportedLocation &&
                     arg.RegionKey == supportedLocationShort &&
                     arg.NotificationType == notificationType),                
                default(CancellationToken)));

            // Act
            AksUpdates.Orchestrations.AksUpdateOrchestrator orchestrator = new AksUpdates.Orchestrations.AksUpdateOrchestrator(mediatrMock.Object, azureApiMock.Object, azureTableStorageMock.Object, loggerMock.Object);
            orchestrator.VerifyAksVersionChangesForRegion(supportedLocation, storedlatestversionsPerLocation, notificationType);

            mediatrMock.Verify(c => c.Publish<AksNewRegionAvailableEvent>(It.IsAny<AksNewRegionAvailableEvent>(), default(CancellationToken)), Times.Exactly(1));
            
            // Assert
            mediatrMock.VerifyAll();
        }

        [Fact]
        public void ShouldRaiseNewVersionEvent()
        {
            // Arrange
            var mediatrMock = new Mock<IMediator>();
            var azureApiMock = new Mock<IAzureApi>();
            var azureTableStorageMock = new Mock<IAzureTableStorage>();
            var loggerMock = new Mock<ILogger<AksUpdateOrchestrator>>();

            string latestVersion = "1.15.7";
            string supportedLocation = "West Europe";
            string supportedLocationShort = supportedLocation.Replace(" ", "").ToLower();
            var notificationType = new NotificationType(NotificationType.Aks);
            string previewVersions = "1.16.1, 1.16.4, 1.17.0";
            Dictionary<string, Version> storedlatestversionsPerLocation = new Dictionary<string, Version>() { { supportedLocationShort, new Version("1.14.6") } } ;

            azureApiMock.Setup(x => x.GetAksVersionsByRegion(It.IsAny<string>())).ReturnsAsync(File.ReadAllText("get-versions-result-2019-04-01.json"));

            mediatrMock.Setup(c => c.Publish<AksNewVersionAvailableEvent>(
                It.Is<AksNewVersionAvailableEvent>(arg =>
                     arg.LatestVersion == new Version(latestVersion) &&
                     arg.Region == supportedLocation &&
                     arg.RegionKey == supportedLocationShort &&
                     arg.NotificationType == notificationType),
                default(CancellationToken)));            

            // Act
            AksUpdates.Orchestrations.AksUpdateOrchestrator orchestrator = new AksUpdates.Orchestrations.AksUpdateOrchestrator(mediatrMock.Object, azureApiMock.Object, azureTableStorageMock.Object, loggerMock.Object);
            orchestrator.VerifyAksVersionChangesForRegion(supportedLocation, storedlatestversionsPerLocation, notificationType);

            mediatrMock.Verify(c => c.Publish<AksNewVersionAvailableEvent>(It.IsAny<AksNewVersionAvailableEvent>(), default(CancellationToken)), Times.Exactly(1));

            // Assert
            mediatrMock.VerifyAll();
        }
    }
}
