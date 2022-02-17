param functionAppName string
param tenantId string
param subscriptionId string
param applicationId string
param servicePrincipalPassword string
param twitterApiKey string
param twitterApiSecretKey string
param twitterAccessToken string
param twitterAccessTokenSecret string
param toggleSendNotifications bool
param storageAccountName string
param tableStorageName string

resource storageAccount 'Microsoft.Storage/storageAccounts@2021-06-01' existing = {
  name: storageAccountName
}

resource functionAppStagingAppsettings 'Microsoft.Web/sites/slots/config@2016-08-01' = {
  name: '${functionAppName}/appsettings'
  properties: {
    tenantId: tenantId
    subscriptionId: subscriptionId
    applicationId: applicationId
    servicePrincipalPassword: servicePrincipalPassword
    tableStorageName: tableStorageName
    tableStorageConnectionString: 'DefaultEndpointsProtocol=https;AccountName=${storageAccount.name};EndpointSuffix=${environment().suffixes.storage};AccountKey=${listKeys(storageAccount.id, storageAccount.apiVersion).keys[0].value}'
    twitterApiKey: twitterApiKey
    twitterApiSecretKey: twitterApiSecretKey
    twitterAccessToken: twitterAccessToken
    twitterAccessTokenSecret: twitterAccessTokenSecret
    toggle_SendNotification: toggleSendNotifications
  }
}
