@description('Location for all resources.')
param location string = resourceGroup().location

@description('Name of the App Service Plan.')
param planName string = 'asp-dev-san-01'

@description('Name of the Web App (must be globally unique).')
param appName string = 'web-dev-san-123654'

@description('Name of the deployment slot.')
param slotName string = 'staging'

@description('App Service Plan SKU. Use S1 or higher to enable deployment slots.')
param skuName string = 'S1'

@description('Instance count (scale-out)')
param workerCount int = 1

@description('Linux runtime stack for the Web App.')
param linuxFxVersion string = 'DOTNETCORE|10.0'

// Create an App Service Plan
resource appServicePlan 'Microsoft.Web/serverfarms@2022-09-01' = {
  name: planName
  location: location
  sku: {
    name: skuName
    tier: 'Standard'
    size: skuName
    capacity: workerCount
  }
  properties: {
    // true for Linux, false for Windows
    reserved: true
  }
}

// Create a Web App
resource webApp 'Microsoft.Web/sites@2022-09-01' = {
  name: appName
  location: location
  properties: {
    serverFarmId: appServicePlan.id
    httpsOnly: true
    siteConfig: {
      linuxFxVersion: linuxFxVersion
    }
  }
}

// Create a deployment slot
resource deploymentSlot 'Microsoft.Web/sites/slots@2022-09-01' = {
  parent: webApp
  name: slotName
  location: location
  properties: {
    serverFarmId: appServicePlan.id
    httpsOnly: true
    siteConfig: {
      linuxFxVersion: linuxFxVersion
    }
  }
}
