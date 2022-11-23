@description('Specifies the name of the App Configuration store.')
param name string

@description('Specifies the Azure location where the app configuration store should be created.')
param location string = resourceGroup().location

param configItems array = [
    {
        name: 'Bicep:Config:Value'
        value: 'Test from Bicep'
        contenttype: ''
        featureFlag: false
        tags: {
            Bicep: 'Deployed'
        }
    }
        {
        name: 'Bicep:Secret:KeyVault'
        value: 'https://azkv-appconfiguration.vault.azure.net/secrets/bicep-configuration-secret'
        contenttype: 'application/vnd.microsoft.appconfig.keyvaultref+json;charset=utf-8'
        featureFlag: false
        tags: {
            Bicep: 'Deployed'
        }
    }
    {
        name: 'bicep-featureflag'
        value: ''
        contenttype: 'application/vnd.microsoft.appconfig.ff+json;charset=utf-8'
        featureFlag: true
        tags: {
            Bicep: 'Deployed'
        }
    }
]

resource configStore 'Microsoft.AppConfiguration/configurationStores@2021-10-01-preview' = {
    name: 'azappconfiguration-${name}'
    location: location
    sku: {
        name: 'standard'
    }
    properties:{
        disableLocalAuth: true
        enablePurgeProtection: true
        softDeleteRetentionInDays:7
    }
}

module configValue 'modules/configstorevalue.bicep' = {
    name: 'configstorevalue'
    params: {
        configItems: configItems
        configStoreName: configStore.name
    }
}
