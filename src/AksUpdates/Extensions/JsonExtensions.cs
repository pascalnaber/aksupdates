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
        public static IEnumerable<string> ButNot(this IEnumerable<string> list, string[] regions)
        {
            var regionList = list.ToList();
            foreach (var region in regions)
            {
                regionList.Remove(region);
            }
            
            return regionList;
        }

        public static IEnumerable<string> GetLocationsOfferingAks(this string jsonForContainerService)
        {
            JObject providerJson = JObject.Parse(jsonForContainerService);
            return providerJson.SelectToken("$..resourceTypes[?(@.resourceType=='managedClusters')].locations").Children().Select(l => l.Value<string>());
        }

        // Example error:
        // {"error":{"code":"NoRegisteredProviderFound","message":"No registered resource provider found for location 'koreasouth' and API version '2019-04-01' for type 'locations/orchestrators'. The supported api-versions are '2017-09-30, 2019-04-01, 2019-06-01, 2019-08-01, 2019-10-01, 2019-11-01, 2020-01-01, 2020-02-01, 2020-03-01, 2020-04-01, 2020-06-01, 2020-07-01, 2020-09-01, 2020-11-01, 2020-12-01, 2021-02-01, 2021-03-01, 2021-05-01, 2021-07-01, 2021-08-01, 2021-09-01, 2021-10-01, 2021-11-01-preview'. The supported locations are 'eastus, westeurope, francecentral, francesouth, centralus, canadaeast, canadacentral, uksouth, ukwest, westcentralus, westus, westus2, australiaeast, australiacentral, australiasoutheast, northeurope, japaneast, japanwest, koreacentral, eastus2, southcentralus, northcentralus, southeastasia, southindia, centralindia, eastasia, southafricanorth, brazilsouth, brazilsoutheast, australiacentral2, jioindiacentral, jioindiawest, swedencentral, westus3, germanynorth, germanywestcentral, switzerlandnorth, switzerlandwest, uaenorth, uaecentral, norwayeast, norwaywest'."}}
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
