param kvName string = 'kvGlobal'

resource kv 'Microsoft.KeyVault/vaults@2021-06-01-preview' existing = {
  name: kvName
}

param roleAssignment object = {
  UserId: '39fbc045-15e0-4855-b63a-e6cdf74ef2ea'
  RoleId: '21090545-7ca7-4776-b22c-e363652d74d2'
  uniqueGUID: 'bce4256e-32fa-4eec-801d-b9c3d7a886dd'
}

resource roleDefinition 'Microsoft.Authorization/roleDefinitions@2018-01-01-preview' existing = {
  name: roleAssignment.RoleId
}

resource RA 'Microsoft.Authorization/roleAssignments@2020-08-01-preview' = {
    name: roleAssignment.uniqueGUID
    scope: kv
    properties: {
        roleDefinitionId: roleDefinition.id
        principalId: roleAssignment.UserId
    }
}

output roleAssignment string = RA.id
