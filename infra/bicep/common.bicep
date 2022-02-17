targetScope = 'subscription'

param containerRegistryResourceGroupName string
param containerRegistryName string
param servicePrincipalId string

resource rg 'Microsoft.Resources/resourceGroups@2021-04-01' = {
  name: containerRegistryResourceGroupName
  location: deployment().location  
}

module containerRegistry 'modules/ContainerRegistry/registries.bicep' = {
  scope: rg
  name: containerRegistryName
  params: {
    acrName: containerRegistryName
    servicePrincipalId: servicePrincipalId
  }
}
