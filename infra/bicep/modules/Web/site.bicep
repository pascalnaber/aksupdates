param appServicePlanResourceId string
param linuxFxVersion string = 'DOCKER|mcr.microsoft.com/appsvc/staticsite:latest'
param webAppName string
param location string = resourceGroup().location
param acrName string
param acrResourceGroupName string
param storageAccountConnectionString string 
param applicationName string
param dnsZone string

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

// resource dnsTxt 'Microsoft.Network/dnsZones/TXT@2018-05-01' = {
//   name: '${dnsZone}/asuid.${applicationName}'
//   properties: {
//     TTL: 3600
//     TXTRecords: [
//       {
//         value: [
//           '${functionApp.properties.customDomainVerificationId}'
//         ]
//       }
//     ]
//   }
// }

// resource dnsCname 'Microsoft.Network/dnsZones/CNAME@2018-05-01' = {
//   name: '${dnsZone}/${applicationName}'
//   properties: {
//     TTL: 3600
//     CNAMERecord: {
//       cname: '${functionApp.name}.azurewebsites.net'
//     }
//   }
// }
// Enabling Managed certificate for a webapp requires 3 steps
// 1. Add custom domain to webapp with SSL in disabled state
// 2. Generate certificate for the domain
// 3. enable SSL
//
// The last step requires deploying again Microsoft.Web/sites/hostNameBindings - and ARM template forbids this in one deplyment, therefore we need to use modules to chain this.

resource functionAppCustomHost 'Microsoft.Web/sites/hostNameBindings@2020-06-01' = {
  name: '${webAppName}/${applicationName}.${dnsZone}'  
  properties: {
    hostNameType: 'Verified'
    sslState: 'Disabled'
    customHostNameDnsRecordType: 'CName'
    siteName: webApp.name
  }
}

resource functionAppCustomHostCertificate 'Microsoft.Web/certificates@2020-06-01' = {
  name: '${applicationName}.${dnsZone}'
  location: location
  dependsOn: [
    functionAppCustomHost
  ]
  properties: any({
    serverFarmId: appServicePlanResourceId
    canonicalName: '${applicationName}.${dnsZone}'
  })
}

// we need to use a module to enable sni, as ARM forbids using resource with this same type-name combination twice in one deployment.
module functionAppCustomHostEnable './sni-enable.bicep' = {
  name: '${deployment().name}-${applicationName}-sni-enable'
  params: {
    functionAppName: webApp.name
    functionAppHostname: '${functionAppCustomHostCertificate.name}'
    certificateThumbprint: functionAppCustomHostCertificate.properties.thumbprint
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
