using System;
using System.Collections.Generic;
using System.Text;

namespace AksUpdates
{
    public static class Settings
    {
        public const string TenantId = "tenantId";
        public const string ApplicationId = "applicationId";
        public const string SubscriptionId = "subscriptionId";
        public const string ServicePrincipalPassword = "servicePrincipalPassword";

        public const string TableStorageName = "tableStorageName";        
        public const string TableStorageConnectionString = "tableStorageConnectionString";

        public const string TwitterApiKey = "twitterApiKey";
        public const string TwitterApiSecretKey = "twitterApiSecretKey";
        public const string TwitterAccessToken = "twitterAccessToken";
        public const string TwitterAccessTokenSecret = "twitterAccessTokenSecret";

        public const string Toggle_SendNotification = "Toggle_SendNotification";

        public static string GetSetting(string name)
        {
            return System.Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.Process);
        }
    }
}
