using AksUpdates.Models.AksVersionsPerLocation;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AksUpdates.Extensions
{
    public static class JsonExtensions
    {
        public static IEnumerable<string> GetLocationsOfferingAks(this string jsonForContainerService)
        {
            JObject providerJson = JObject.Parse(jsonForContainerService);
            return providerJson.SelectToken("$..resourceTypes[?(@.resourceType=='managedClusters')].locations").Children().Select(l => l.Value<string>());
        }

        public static Version GetLatestVersion(this string json, bool isPreview)
        {
            return GetAllVersions(json, isPreview).Max();
        }

        public static IList<Version> GetAllPreviewVersions(this string json)
        {
            return GetAllVersions(json, true);
        }

        public static IList<Version> GetAllVersions(this string json, bool isPreview)
        {
            var dynJson = JsonConvert.DeserializeObject<Rootobject>(json);

            List<Version> versions = new List<Version>();

            foreach (var item in dynJson.properties.orchestrators)
            {
                if (item.isPreview == isPreview)
                {
                    string orchestratorVersion = item.orchestratorVersion.ToString();
                    versions.Add(new Version(orchestratorVersion));
                }
            }

            return versions;
        }

    }
}
