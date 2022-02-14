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

resource rg 'Microsoft.Resources/resourceGroups@2021-04-01' = {
  name: webappResourceGroup
  location: deployment().location
}

resource rgStorage 'Microsoft.Resources/resourceGroups@2021-04-01' = {
  name: storageResourceGroup
  location: deployment().location
}

module appPlan 'modules/Web/serverfarm-linux.bicep' = {
  scope: rg
  name: hostingplanName
  params: {
    appServicePlanName: hostingplanName
    appServicePlanSkuName: appServicePlanSkuName
    appServicePlanCapacity: appServicePlanCapacity
    
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
  }
}

module storageaccount 'modules/Storage/storageAccount.bicep' = {
  scope: rgStorage
  name: storageAccountName
  params: {
    storageAccountName: storageAccountName
  }
}

resource functionrg 'Microsoft.Resources/resourceGroups@2021-04-01' = {
  name: functionResourceGroup
  location: deployment().location
}

module functionapp 'modules/Web/function.bicep' = {
  scope: functionrg
  name: functionAppName
  params: {
    functionAppName: functionAppName
    appServicePlanName: functionAppName
    storageAccountName: functionStorageAccountName
  }
}
