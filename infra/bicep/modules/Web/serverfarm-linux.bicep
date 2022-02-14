@description('The location into which the App Service resources should be deployed.')
param location string = resourceGroup().location

@description('The name of the SKU to use when creating the App Service plan.')
param appServicePlanSkuName string

@description('The number of worker instances of your App Service plan that should be provisioned.')
param appServicePlanCapacity int

@description('The name of the App Service plan.')
param appServicePlanName string = 'AppServicePlan'

resource appServicePlan 'Microsoft.Web/serverfarms@2020-06-01' = {
  name: appServicePlanName
  location: location
  properties: {
    reserved: true
  }
  sku: {
    name: appServicePlanSkuName
    capacity: appServicePlanCapacity
  }
  kind: 'linux'
}
output appServicePlanResourceId string = appServicePlan.id
