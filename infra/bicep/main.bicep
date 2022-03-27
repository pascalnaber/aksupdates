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
param containerInstanceName string

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

param subDomainName string
param dnsZone string
param dnsName string

resource rg 'Microsoft.Resources/resourceGroups@2021-04-01' = {
  name: webappResourceGroup
  location: location
}

resource rgStorage 'Microsoft.Resources/resourceGroups@2021-04-01' = {
  name: storageResourceGroup
  location: location
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

module containerInstance 'modules/ContainerInstance/containerGroup.bicep' = {
  scope: functionrg
  name: containerInstanceName
  params: {
    containerName: containerInstanceName
    imageName: '${containerImage}:${containerImageTag}'
    dnsName: dnsName
    dnsZone: dnsZone
    location: location
    acrName: acrName
    storageAccountConnectionString: storageaccount.outputs.blobStorageConnectionString
  } 
}

module containerInstanceDnsRecord 'modules/Network/dnsArecord.bicep' = {
  scope: rgStorage
  name: '${containerInstanceName}dnsArecord'
  params: {
    subdomain: subDomainName        
    dnsZone: dnsZone    
    ipAddress: containerInstance.outputs.containerIpv4Address
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
