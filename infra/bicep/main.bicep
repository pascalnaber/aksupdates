targetScope = 'subscription'

param webappResourceGroup string
param hostingplanName string
param webAppName string
param containerImage string
param containerImageTag string
param appServicePlanSkuName string
param appServicePlanCapacity int
param acrName string
param acrResourceGroup string 
param storageAccountName string
param storageResourceGroup string
param functionAppName string
param functionStorageAccountName string
param functionResourceGroup string
param location string = deployment().location

param tenantId string
param subscriptionId string
param applicationId string
param servicePrincipalPassword string
param twitterApiKey string
param twitterApiSecretKey string
param twitterAccessToken string
param twitterAccessTokenSecret string
param toggleSendNotifications string
param tableStorageName string

resource rg 'Microsoft.Resources/resourceGroups@2021-04-01' = {
  name: webappResourceGroup
  location: location
}

resource rgStorage 'Microsoft.Resources/resourceGroups@2021-04-01' = {
  name: storageResourceGroup
  location: location
}

module appPlan 'modules/Web/serverfarm-linux.bicep' = {
  scope: rg
  name: hostingplanName
  params: {
    appServicePlanName: hostingplanName
    appServicePlanSkuName: appServicePlanSkuName
    appServicePlanCapacity: appServicePlanCapacity
    location: location
  }
}

module app 'modules/Web/site.bicep' = {
  scope: rg
  name: webAppName
  params: {
    webAppName: webAppName
    appServicePlanResourceId: appPlan.outputs.appServicePlanResourceId
    acrName: acrName
    acrResourceGroupName: acrResourceGroup
    linuxFxVersion: 'DOCKER|${containerImage}:${containerImageTag}'
    storageAccountConnectionString: storageaccount.outputs.blobStorageConnectionString
    location: location
  }
}

module storageaccount 'modules/Storage/storageAccount.bicep' = {
  scope: rgStorage
  name: storageAccountName
  params: {
    storageAccountName: storageAccountName
    location: location
  }
}

resource functionrg 'Microsoft.Resources/resourceGroups@2021-04-01' = {
  name: functionResourceGroup
  location: location
}

module functionapp 'modules/Web/function.bicep' = {
  scope: functionrg
  name: functionAppName
  params: {
    functionAppName: functionAppName
    appServicePlanName: functionAppName
    storageAccountName: functionStorageAccountName
    location: location
  } 
}

module functionappSettings 'modules/Web/functionsettings.bicep' = {
  scope: functionrg
  name: '${functionAppName}settings'
  params: {
    functionAppName: functionAppName
    applicationId: applicationId
    servicePrincipalPassword: servicePrincipalPassword
    subscriptionId: subscriptionId
    tableStorageConnectionString: storageaccount.outputs.blobStorageConnectionString
    tableStorageName: tableStorageName
    tenantId: tenantId
    toggleSendNotifications: toggleSendNotifications
    twitterAccessToken: twitterAccessToken
    twitterAccessTokenSecret: twitterAccessTokenSecret
    twitterApiKey: twitterApiKey
    twitterApiSecretKey: twitterApiSecretKey
    existingFunctionAppStagingAppsettings:  functionapp.outputs.settings
  }
  dependsOn: [
    functionapp
  ]
}
