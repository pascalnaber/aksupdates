param functionAppName string
param tenantId string
param subscriptionId string
param applicationId string
param servicePrincipalPassword string
param twitterApiKey string
param twitterApiSecretKey string
param twitterAccessToken string
param twitterAccessTokenSecret string
param toggleSendNotifications string
param tableStorageConnectionString string
param tableStorageName string
param existingFunctionAppStagingAppsettings object

var newSettings = {
  tenantId: tenantId
  subscriptionId: subscriptionId
  applicationId: applicationId
  servicePrincipalPassword: servicePrincipalPassword
  azureTableStorageConfiguration__tableStorageName: tableStorageName
  azureTableStorageConfiguration__tableStorageConnectionString: tableStorageConnectionString
  twitterApiKey: twitterApiKey
  twitterApiSecretKey: twitterApiSecretKey
  twitterAccessToken: twitterAccessToken
  twitterAccessTokenSecret: twitterAccessTokenSecret
  Toggle_SendNotification: toggleSendNotifications 
}

resource functionApp 'Microsoft.Web/sites@2018-11-01' existing = {
  name: functionAppName
}

// resource existingFunctionAppStagingAppsettings 'Microsoft.Web/sites/config@2021-03-01' existing = {
//   parent: functionApp
//  name: 'appsettings'
// }

// var allSettings = 

resource functionAppStagingAppsettings 'Microsoft.Web/sites/config@2021-03-01' = {
  parent: functionApp
  name: 'appsettings'
  properties: union(existingFunctionAppStagingAppsettings, newSettings)
}
