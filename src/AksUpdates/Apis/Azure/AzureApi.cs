using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace AksUpdates.Apis.Azure
{
    public class AzureApi : IAzureApi
    {
        private readonly string subscriptionId = Settings.GetSetting(Settings.SubscriptionId);
        private readonly string tenantId = Settings.GetSetting(Settings.TenantId);
        private readonly string servicePrincipalApplicationId = Settings.GetSetting(Settings.ApplicationId);
        private readonly string servicePrincipalPassword = Settings.GetSetting(Settings.ServicePrincipalPassword);

        public AzureApi()
        {

        }

        private static HttpClient httpClient = new HttpClient();

        private const string AuthenticateURI = "https://login.microsoftonline.com/{0}/oauth2/token";
        private const string AuthenticateBody = "grant_type=client_credentials&client_id={0}&resource=https%3A%2F%2Fmanagement.core.windows.net%2F&client_secret={1}";

        private string authorizationToken = null;

        public async Task<string> GetAuthorizationToken()
        {
            if (authorizationToken == null)
            {
                string password = WebUtility.UrlEncode(servicePrincipalPassword);

                string authenticateRequestUri = string.Format(AuthenticateURI, tenantId);
                string authenticateBody = string.Format(AuthenticateBody, servicePrincipalApplicationId, password);
                string authenticateContentType = "application/x-www-form-urlencoded";

                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(authenticateContentType));

                using (HttpContent body = new StringContent(authenticateBody))
                {
                    body.Headers.ContentType = new MediaTypeHeaderValue(authenticateContentType);

                    using (HttpResponseMessage response = await httpClient.PostAsync(authenticateRequestUri, body))
                    using (HttpContent content = response.Content)
                    {
                        Task<string> result = content.ReadAsStringAsync();
                        JObject jsonBody = JObject.Parse(result.Result);
                        authorizationToken = jsonBody.GetValue("access_token").ToString();                        
                    }
                }
            }
            return authorizationToken;
        }

        public async Task<string> GetAksRegions()
        {
            string token = await GetAuthorizationToken();

            var providersURI = $"https://management.azure.com/subscriptions/{this.subscriptionId}/providers/Microsoft.ContainerService?api-version=2018-02-01";
            var json = await ExecuteGetOnAzureApi(providersURI, token);
            return json;
        }

        public async Task<string> GetAksVersionsByRegion(string location)
        {
            string token = await GetAuthorizationToken();

            var aksVersionsUri = $"https://management.azure.com/subscriptions/{this.subscriptionId}/providers/Microsoft.ContainerService/locations/{location}/orchestrators?api-version=2021-10-01&resource-type=managedClusters";
            var json = await ExecuteGetOnAzureApi(aksVersionsUri, token);

            if (json.Contains("error"))
                throw new Exception("Exception occured while fetching AKS information on Azure", new Exception(json));

            return json;
        }

        private async Task<string> ExecuteGetOnAzureApi(string uri, string token)
        {
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Add("Authorization", String.Format("Bearer {0}", token));

            using (HttpResponseMessage response = await httpClient.GetAsync(uri))
            using (HttpContent content = response.Content)
            {
                return await content.ReadAsStringAsync();
            }
        }
    }
}
