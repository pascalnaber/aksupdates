targetScope = 'resourceGroup'

param roleDefinitionId string
param principalId string

param roleAssignmentScope string = resourceGroup().id
var roleAssignmentName =  guid(principalId, roleDefinitionId, roleAssignmentScope)

resource roleAssignment 'Microsoft.Authorization/roleAssignments@2020-04-01-preview' = {
  name: roleAssignmentName  
  properties: {    
    principalId: principalId
    roleDefinitionId: roleDefinitionId
  }
}
