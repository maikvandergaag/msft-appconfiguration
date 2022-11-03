param configStoreName string

param configItems array


resource configStore 'Microsoft.AppConfiguration/configurationStores@2021-10-01-preview' existing = {
  name: configStoreName
}

resource configStoreKeyValue 'Microsoft.AppConfiguration/configurationStores/keyValues@2021-10-01-preview' = [for item in configItems:  {
  parent: configStore
  name: (!item.featureFlag) ? item.name : '.appconfig.featureflag~2F${item.name}'
  properties: {
    value: (!item.featureFlag) ? item.value : '{"id": "${item.name}", "description": "", "enabled": false, "conditions": {"client_filters":[]}}'
    tags: item.tags
    contentType:item.contentType
  }
}]