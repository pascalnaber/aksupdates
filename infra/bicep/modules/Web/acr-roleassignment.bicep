param acrName string
param principalId string

@description('This is the built-in ACRPush role. See https://docs.microsoft.com/azure/role-based-access-control/built-in-roles')
resource acrPullRoleDefinition 'Microsoft.Authorization/roleDefinitions@2018-01-01-preview' existing = {
  scope: subscription()
  name: '7f951dda-4ed3-4680-a7ca-43fe172d538d'
}

// resource acrResourceGroup 'Microsoft.Resources/resourceGroups@2021-04-01' existing = {  
//   name: acrResourceGroupName  
// }

resource acr 'Microsoft.ContainerRegistry/registries@2019-12-01-preview' existing = {
  name: acrName    
}

resource roleAssignment 'Microsoft.Authorization/roleAssignments@2020-04-01-preview' = {
  scope: acr
  name: guid(acr.id, principalId, acrPullRoleDefinition.id)
  properties: {
    roleDefinitionId: acrPullRoleDefinition.id
    principalId: principalId
    principalType: 'ServicePrincipal'
  }
}
