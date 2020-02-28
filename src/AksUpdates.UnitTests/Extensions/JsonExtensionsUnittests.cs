using AksUpdates.Extensions;
using AksUpdates.Models.AksVersionsPerLocation;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Xunit;

namespace AksUpdates.UnitTests.Extensions
{
    public class JsonExtensionsUnittests
    {
        [Fact]
        public void Should_GetLatestVersionFromJson()
        {
            string json = File.ReadAllText("get-versions-result-2019-04-01.json");

            Rootobject dynJson = JsonConvert.DeserializeObject<Rootobject>(json);

            Version latestVersion = JsonExtensions.GetLatestVersion(json, false);

            Version expectedVersion = new Version("1.15.7");
            Assert.Equal(expectedVersion, latestVersion);
        }

        [Fact]
        public void Should_GetLatestVersionFromJsonPreview()
        {
            string json = File.ReadAllText("get-versions-result-2019-04-01.json");

            Rootobject dynJson = JsonConvert.DeserializeObject<Rootobject>(json);

            Version latestVersion = JsonExtensions.GetLatestVersion(json, true);

            Version expectedVersion = new Version("1.17.0");
            Assert.Equal(expectedVersion, latestVersion);
        }

       

        [Fact]
        public void Should_GetLocationsWhereK8sIsSupportedFromJson2()
        {
            string json = File.ReadAllText("get-provider-containerservice.json");

            var locations = JsonExtensions.GetLocationsOfferingAks(json);

            Assert.Equal(12, locations.Count());
        }

    }
}
