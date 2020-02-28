using AksUpdates.Models.AksVersionsPerLocation;
using AksUpdates.Orchestrations;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace AksUpdates.UnitTests
{
    public class UnitTests
    {
        
        [Fact]
        public void Should_HaveNoChanges()
        {         
            Version latestStoredVersion = new Version("10.1.3");
            Version latestVersion = new Version("10.1.3");
            string supportedLocation = "West Europe";           

            //var message = AksUpdateOrchestrator.BuildTweetMessage(latestStoredVersion, supportedLocation, latestVersion, false, "");

            //Assert.Null(message);
        }

        [Fact]
        public void Should_IdentifyNewLocation()
        {            
            Version latestStoredVersion = new Version();
            Version latestVersion = new Version("10.1.3");
            string supportedLocation = "West Europe";

            //var message = AksUpdateOrchestrator.BuildTweetMessage(latestStoredVersion, supportedLocation, latestVersion, false, "");

            //Assert.Equal("New location West Europe available in Azure supporting AKS version 10.1.3", message);
        }

        [Fact]
        public void Should_IdentifyNewVersion()
        {            
            Version latestStoredVersion = new Version("10.1.1");
            Version latestVersion = new Version("10.1.3");
            string supportedLocation = "West Europe";

            //var message = AksUpdateOrchestrator.BuildTweetMessage(latestStoredVersion, supportedLocation, latestVersion, false, "");

            //Assert.Equal("Location West Europe in Azure has a new version of AKS available: 10.1.3", message);
        }
    }
}
