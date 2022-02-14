param appServicePlanResourceId string
param linuxFxVersion string = 'DOCKER|mcr.microsoft.com/appsvc/staticsite:latest'
param webAppName string
param location string = resourceGroup().location
param acrName string
param acrResourceGroupName string
param storageAccountConnectionString string 

resource webApp 'Microsoft.Web/sites@2021-01-01' = {
  name: webAppName
  location: location  
  identity: {
    type: 'SystemAssigned'
  }
  properties: {
    siteConfig: {
      acrUseManagedIdentityCreds: true      
      linuxFxVersion: linuxFxVersion
      alwaysOn: true      
       appSettings: [
         {
          name: 'WEBSITES_ENABLE_APP_SERVICE_STORAGE'
          value: 'false'
         }
         {
          name: 'azureTableStorageConfiguration__tableStorageName'
          value: 'aksupdates'
         }
         {
          name: 'azureTableStorageConfiguration__tableStorageConnectionString'
          value: storageAccountConnectionString
         }
       ]
    }
    serverFarmId: appServicePlanResourceId
  }
}

module acrroleassignment 'acr-roleassignment.bicep' = {
  scope: acrResourceGroup
  name: 'acrroleassignment'
  params: {
    principalId: webApp.identity.principalId    
    acrName: acrName
  }
}




// @description('This is the built-in ACRPush role. See https://docs.microsoft.com/azure/role-based-access-control/built-in-roles')
// resource acrPullRoleDefinition 'Microsoft.Authorization/roleDefinitions@2018-01-01-preview' existing = {
//   scope: subscription()
//   name: '7f951dda-4ed3-4680-a7ca-43fe172d538d'
// }

resource acrResourceGroup 'Microsoft.Resources/resourceGroups@2021-04-01' existing = {
  scope: subscription()
  name: acrResourceGroupName  
}

// // resource acr 'Microsoft.ContainerRegistry/registries@2019-12-01-preview' existing = {
// //   name: acrName
// //   scope: acrResourceGroup
// // }

// resource roleAssignment 'Microsoft.Authorization/roleAssignments@2020-04-01-preview' = {
//   scope: acrResourceId
//   name: guid(acrResourceId, acrPullRoleDefinition.id)
//   properties: {
//     roleDefinitionId: acrPullRoleDefinition.id
//     principalId: webApp.identity.principalId
//     principalType: 'ServicePrincipal'
//   }
// }
