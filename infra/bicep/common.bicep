targetScope = 'subscription'

param containerRegistryResourceGroupName string
param containerRegistryName string
param servicePrincipalId string
param location string = deployment().location

resource rg 'Microsoft.Resources/resourceGroups@2021-04-01' = {
  name: containerRegistryResourceGroupName
  location: location
}

module containerRegistry 'modules/ContainerRegistry/registries.bicep' = {
  scope: rg
  name: containerRegistryName
  params: {
    acrAdminUserEnabled: true
    acrName: containerRegistryName
    servicePrincipalId: servicePrincipalId
    location: location
  }
}
