@allowed([
    'dev'
    'staging'
    'prod'
])
param environmentSuffix string

@allowed([
  'northeurope'
  'uksouth'
])
param location string

@secure()
param sqlAdminPassword string

@secure()
param ssoClientSecretValue string

//
// Entra ID user to set as admin on database server:
//
param sqlAdminLogin string
param sqlAdminTenantId string
@secure()
param sqlAdminSid string

@description('Optional. If set, will be used as the custom domain. If not set, a default domain will be generated.')
param customDomain string = ''

//param serviceConnectionPrincipalObjectId string

//
// Variables for Resource Names
//
var resourcePrefix = 'rockats'
var locationPrefix = 'neu'

var keyVaultName = '${resourcePrefix}-ro-vault-${environmentSuffix}'

var appInsightsName = '${resourcePrefix}-appinsights-${locationPrefix}-${environmentSuffix}'
var appInsightsLogAnalyticsWorkspaceName = '${resourcePrefix}-log-analytics-${locationPrefix}-${environmentSuffix}'

var appServicePlanName = '${resourcePrefix}-farm-${locationPrefix}-${environmentSuffix}'
var appServiceName = '${resourcePrefix}-webapp-${locationPrefix}-${environmentSuffix}'
var appServiceWarmupName = 'warmup'

var storageAccountName = '${resourcePrefix}storage${locationPrefix}${environmentSuffix}'

var databaseServerName = '${resourcePrefix}-sql-${locationPrefix}-${environmentSuffix}'
var databaseName = '${resourcePrefix}-sqldb-${locationPrefix}-${environmentSuffix}'

// var serviceBusName = '${resourcePrefix}-servicebus-${environmentSuffix}'

var appConfigServiceName = '${resourcePrefix}-azureappconfig-${locationPrefix}-${environmentSuffix}'


// //
// // App Registration Id
// //
// var appRegistrationId = ''
// var appRegistrationObjectId = ''

//
// Application Insights and associated Log Analytics workspace:
//
var appInsightsDailyCapGB = (environmentSuffix == 'prod') ? 2 : (environmentSuffix == 'staging') ? 1 : 1
var logAnalyticsRetentionDays = (environmentSuffix == 'prod') ? 90 : (environmentSuffix == 'staging') ? 90 : 90

var appInsightsConnectionString =  appInsights.properties.ConnectionString


var databaseConnectionString = 'Server=tcp:${databaseServerName}${environment().suffixes.sqlServerHostname},1433;Initial Catalog=${databaseName};Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;Authentication="Active Directory Default";'
// var customDomain = (environmentSuffix == 'prod') ? '<domain>' : 'web-${environmentSuffix}.rockats.s73.co'
var resolvedCustomDomain = empty(customDomain)
  ? 'rockats-webapp-neu-${environmentSuffix}.azurewebsites.net'
  : customDomain

// var functionCustomDomain = (environmentSuffix == 'prod') ? 'rock-functions.rock.s73.co' : 'rock-functions-${environmentSuffix}.rockats.s73.co'

var storageAccountConnectionString = 'DefaultEndpointsProtocol=https;AccountName=${storageAccount.name};AccountKey=${storageAccount.listKeys().keys[0].value};EndpointSuffix=core.windows.net'
// var storageAccountUrl = 'https://${storageAccount.name}.blob.${environment().suffixes.storage}/'

// var serviceBusPrimaryConnectionString = listKeys('${serviceBus.id}/authorizationRules/RootManageSharedAccessKey', serviceBus.apiVersion).primaryConnectionString



//var appConfigSoftDeleteRetentionInDays = 7
//var appConfigSoftEnable = (environmentSuffix == 'prod') ? true : (environmentSuffix == 'staging') ? false : false

// SKUs
var databaseSkuName = (environmentSuffix == 'prod') ? 'S2' : (environmentSuffix == 'staging') ? 'S1' : 'S1'
var appConfigServiceSku = (environmentSuffix == 'prod') ? 'Standard' : (environmentSuffix == 'staging') ? 'Standard' : 'Free'
var appServicePlanSku = (environmentSuffix == 'prod') ? 'P2V3' : (environmentSuffix == 'staging') ? 'P0v3' : 'P0v3'


resource appInsights 'Microsoft.Insights/components@2020-02-02' = {
	name: appInsightsName
	location: location
	kind: 'web'
  properties:{
    Application_Type: 'web'
      Request_Source: 'rest'
      Flow_Type: 'Bluefield'
      RetentionInDays: logAnalyticsRetentionDays
      WorkspaceResourceId: resourceId('Microsoft.OperationalInsights/workspaces', appInsightsLogAnalyticsWorkspaceName)
  }
  dependsOn: [
    appInsightsLogAnalyticsWorkspace
  ]
}

resource appInsightsLogAnalyticsWorkspace 'Microsoft.OperationalInsights/workspaces@2023-09-01' = {
	name: appInsightsLogAnalyticsWorkspaceName
	location: location
    properties: {
	    features: {
          enableLogAccessUsingOnlyResourcePermissions: true
        }
        retentionInDays: logAnalyticsRetentionDays
        sku: {
            name: 'PerGB2018'
        }
        workspaceCapping: {
            dailyQuotaGb: appInsightsDailyCapGB
        }
    }
}

resource appInsightsCap 'microsoft.insights/components/pricingPlans@2017-10-01' = {
	name: 'current'
    parent: appInsights
    properties: {
	    planType: 'Basic'
        cap: appInsightsDailyCapGB
        warningThreshold: 25
        stopSendNotificationWhenHitCap: false
        stopSendNotificationWhenHitThreshold: false
    }
}
        
//
// App service plan:
//

resource appServicePlan 'Microsoft.Web/serverfarms@2023-12-01' = {
  name: appServicePlanName
  location: location
  properties: {
    reserved: false
  }
  sku: {
    name: appServicePlanSku
  }
}

//
// App service and warmup slot
//
resource appService 'Microsoft.Web/sites@2023-12-01' = {
  name: appServiceName
  location: location
  kind: 'app'
  properties: {
    serverFarmId: appServicePlan.id
    httpsOnly: true
    siteConfig: {
      use32BitWorkerProcess: false // Ensures 64-bit platform
      minTlsVersion: '1.2'
      ftpsState: 'Disabled'
      alwaysOn: true
      netFrameworkVersion: 'v8.0'
      webSocketsEnabled: true
      connectionStrings: [
        {
          name:'Default'
          type: 'SQLAzure'
          connectionString:databaseConnectionString
        }
      ]
    }
  }
  identity: {
    type: 'SystemAssigned'
  }
  dependsOn: [
    appInsights
    appConfigService
    storageAccount
  ]
}

resource appServiceSlot 'Microsoft.Web/sites/slots@2023-12-01' = {
  name: appServiceWarmupName
  location: location
  kind: 'app'
  properties: {
    serverFarmId: appServicePlan.id
    httpsOnly: true
    siteConfig: {
      use32BitWorkerProcess: false // Ensures 64-bit platform
      minTlsVersion: '1.2'
      ftpsState: 'Disabled'
      alwaysOn: true
      netFrameworkVersion: 'v8.0'
      webSocketsEnabled: true
      connectionStrings: [
        {
          name:'Default'
          type: 'SQLAzure'
          connectionString:databaseConnectionString
        }
      ]
    }
  }  
  parent: appService
  identity: {
    type: 'SystemAssigned'
  }
  dependsOn: [
    appInsights
    appConfigService
    storageAccount
  ]
}

// Function App
// resource functionApp 'Microsoft.Web/sites@2023-01-01' = {
//   name: functionName
//   location: location
//   kind: 'functionapp'
//   properties: {
//     serverFarmId: appServicePlan.id
//     httpsOnly: true
//     siteConfig: {
//       minTlsVersion: '1.2'
//       ftpsState: 'Disabled'
//       alwaysOn: true
//       netFrameworkVersion: 'v8.0'
//       webSocketsEnabled: true
//       appSettings: [
//         {
//           name: 'FUNCTIONS_WORKER_RUNTIME'
//           value: 'dotnet-isolated'
//         }
//         {
//           name: 'FUNCTIONS_EXTENSION_VERSION'
//           value: '~4'
//         }
//         {
//           name: 'AzureWebJobsStorage'
//           value: storageAccountConnectionString
//         }        
//         {
//           name: 'WEBSITE_CONTENTSHARE'
//           value: toLower(functionName)
//         }    
//         {
//           name: 'WEBSITE_CONTENTAZUREFILECONNECTIONSTRING'
//           value: storageAccountConnectionString
//         }
//         {
//           name: 'APPINSIGHTS_INSTRUMENTATIONKEY'
//           value: appInsightsKey
//         }
//         {
//           name: 'AppConfig:ExpirationMinutes'
//           value: '15'
//         }
//         {
//           name: 'AppConfig:Url'
//           value: appConfigService.properties.endpoint
//         }
//         {
//           name: 'AppConfig:UseAd'
//           value: 'true'
//         }
//         {
//           name: 'AppConfig:BuildConfiguration'
//           value: environmentSuffix
//         }
//         {
//           name: 'AzureStorageAccountSettings:ConnectionString'
//           value: storageAccountConnectionString
//         }
//       ]   
//       connectionStrings: [
//         {
//           name:'Default'
//           type: 'SQLAzure'
//           connectionString:databaseConnectionString
//         }
//       ]   
//     }
//   }
//   identity: {
//     type: 'SystemAssigned'
//   }
//   dependsOn: [
//     appInsights
//     appConfigService
//     storageAccount
//   ]
// }

// resource functionAppSlot 'Microsoft.Web/sites/slots@2023-01-01' = {
//   name: 'warmup'
//   location: location
//   kind: 'functionapp'
//   properties: {
//     serverFarmId: appServicePlan.id
//     httpsOnly: true    
//     siteConfig: {
//       minTlsVersion: '1.2'
//       ftpsState: 'Disabled'
//       alwaysOn: true
//       netFrameworkVersion: 'v8.0'
//       webSocketsEnabled: true
//       appSettings: [
//         {
//           name: 'FUNCTIONS_WORKER_RUNTIME'
//           value: 'dotnet-isolated'
//         }
//         {
//           name: 'FUNCTIONS_EXTENSION_VERSION'
//           value: '~4'
//         }        
//         {
//           name: 'AzureWebJobsStorage'
//           value: storageAccountConnectionString
//         }        
//         {
//           name: 'WEBSITE_CONTENTSHARE'
//           value: toLower(functionName)
//         }        
//         {
//           name: 'WEBSITE_CONTENTAZUREFILECONNECTIONSTRING'
//           value: storageAccountConnectionString
//         }
//         {
//           name: 'APPINSIGHTS_INSTRUMENTATIONKEY'
//           value: appInsightsKey
//         }
//         {
//           name: 'AppConfig:ExpirationMinutes'
//           value: '15'
//         }
//         {
//           name: 'AppConfig:Url'
//           value: appConfigService.properties.endpoint
//         }
//         {
//           name: 'AppConfig:UseAd'
//           value: 'true'
//         }
//         {
//           name: 'AppConfig:BuildConfiguration'
//           value: environmentSuffix
//         }
//         {
//           name: 'AzureStorageAccountSettings:ConnectionString'
//           value: storageAccountConnectionString
//         }
//       ]   
//       connectionStrings: [
//         {
//           name:'Default'
//           type: 'SQLAzure'
//           connectionString:databaseConnectionString
//         }
//       ]   
//     }
//   }
//   parent: functionApp
//   identity: {
//     type: 'SystemAssigned'
//   }
//   dependsOn: [
//     appInsights
//     appConfigService
//     storageAccount
//   ]
// }

// //Website app settings
resource appSettingsWebsite 'Microsoft.Web/sites/config@2023-12-01' = {
  name: 'appsettings'
  properties: {
    'AppConfig:BuildConfiguration': environmentSuffix
    'AppConfig:ExpirationMinutes': '1'
    'AppConfig:Url': appConfigService.properties.endpoint
    'AppConfig:UseAd': 'true'
    APPLICATIONINSIGHTS_CONNECTION_STRING: appInsightsConnectionString
    'AzureMonitor:ConnectionString': appInsightsConnectionString
    'AzureStorageAccountSettings:ConnectionString': storageAccountConnectionString
    WEBSITE_LOAD_USER_PROFILE: '1'
    'Settings:Abp.Account.IsSelfRegistrationEnabled': 'false'
  }
  parent: appService
}

resource appSettingsWebsiteSlot 'Microsoft.Web/sites/slots/config@2023-12-01' = {
  name: 'appsettings'  
  properties: {
    'AppConfig:BuildConfiguration': environmentSuffix
    'AppConfig:ExpirationMinutes': '1'
    'AppConfig:Url': appConfigService.properties.endpoint
    'AppConfig:UseAd': 'true'
    APPLICATIONINSIGHTS_CONNECTION_STRING: appInsightsConnectionString
    'AzureMonitor:ConnectionString': appInsightsConnectionString
    'AzureStorageAccountSettings:ConnectionString': storageAccountConnectionString
    WEBSITE_LOAD_USER_PROFILE: '1'
    'Settings:Abp.Account.IsSelfRegistrationEnabled': 'false'
  }
  parent:appServiceSlot
}

// Custom domain & ssl Possibly not needed for prod
resource hostBinding 'Microsoft.Web/sites/hostNameBindings@2023-12-01' = {
  parent: appService
  name: resolvedCustomDomain
  properties: {
    hostNameType: 'Verified'
    sslState: 'Disabled'
    customHostNameDnsRecordType: 'CName'
    siteName: appService.name
  }
}

// resource functionHostBinding 'Microsoft.Web/sites/hostNameBindings@2022-09-01' = {
//   parent: functionApp
//   name: functionCustomDomain
//   properties: {
//     hostNameType: 'Verified'
//     sslState: 'Disabled'
//     customHostNameDnsRecordType: 'CName'
//     siteName: functionApp.name
//   }
// }

resource certificate 'Microsoft.Web/certificates@2023-12-01' = if (!empty(customDomain)) {
  name: '${resolvedCustomDomain}-certificate'
  location: location
  dependsOn: [
    hostBinding
  ]
  properties: {
    serverFarmId: appServicePlan.id
    canonicalName: resolvedCustomDomain
  }
}


// resource functionCertificate 'Microsoft.Web/certificates@2022-09-01' = {
//   name: '${functionCustomDomain}-certificate'
//   location: location
//   dependsOn: [
//     functionHostBinding
//   ]
//   properties: any({
//     serverFarmId: appServicePlan.id
//     canonicalName: functionCustomDomain
//   })
// }

///
/// Storage
///
resource storageAccount 'Microsoft.Storage/storageAccounts@2023-05-01' = {
	name: storageAccountName
	location: location
	sku: {
		name: 'Standard_LRS'
	}
	kind: 'StorageV2'
    properties: {
	    accessTier: 'Hot'
        minimumTlsVersion: 'TLS1_2'
        allowBlobPublicAccess: true        
        publicNetworkAccess: 'Enabled'        
    }
}

// Blob Containers
// resource storageProfilePictureContainer 'Microsoft.Storage/storageAccounts/blobServices/containers@2021-06-01' = {
//   name: '${storageAccountName}/default/<to fill>'
//   properties: {
//     publicAccess: 'Blob'    
//     defaultEncryptionScope: '$account-encryption-key'
//     denyEncryptionScopeOverride: false
//     immutableStorageWithVersioning: {
//       enabled: false
//     }
//   }
//   dependsOn: [
//     storageAccount
//   ]
// }

///
/// SQL Server & Database
///

resource sqlServer 'Microsoft.Sql/servers@2023-05-01-preview' = {
  name: databaseServerName
  location: location
  properties: {
    administratorLogin: 'dbadmin'
    administratorLoginPassword: sqlAdminPassword
    minimalTlsVersion: '1.2'
  }
}

resource sqlAdmins 'Microsoft.Sql/servers/administrators@2021-05-01-preview' = {
  parent: sqlServer
  name: 'ActiveDirectory'
  properties: {
    administratorType: 'ActiveDirectory'
    login: sqlAdminLogin
    tenantId: sqlAdminTenantId
    sid: sqlAdminSid
  }
}

//Set the DB to allow Azure IPs:
resource sqlServerAllowAzureFirewall 'Microsoft.Sql/servers/firewallRules@2023-05-01-preview' = {
  parent: sqlServer
  name: 'AllowAllWindowsAzureIps'
  properties: {
    startIpAddress: '0.0.0.0'
    endIpAddress: '0.0.0.0'
  }
}

resource sqlDatabase 'Microsoft.Sql/servers/databases@2023-05-01-preview' = {
  name: databaseName
  location: location
  parent: sqlServer
  properties:{
    requestedBackupStorageRedundancy: 'Geo'
  }
  sku: {
    name: databaseSkuName
    tier: 'Standard'
  }
}

///
/// Key Vault
///
resource keyVault 'Microsoft.KeyVault/vaults@2023-07-01' = {
	name: keyVaultName
	location: location
	properties: {
		sku: {
			family: 'A'
			name: 'standard'
		}
		tenantId: subscription().tenantId
        enabledForDeployment: true
        enabledForTemplateDeployment: true
        enableRbacAuthorization: true
        enablePurgeProtection: true        
	}
}

resource sqlAdminPasswordSecret 'Microsoft.KeyVault/vaults/secrets@2023-07-01' = {
  name: 'SqlAdminPassword'
  parent: keyVault
  properties: {
    value: sqlAdminPassword
  }
}

resource ssoClientSecret 'Microsoft.KeyVault/vaults/secrets@2023-07-01' = {
  name: 'RockITATS-SSO-ClientSecret'
  parent: keyVault
  properties: {
    value: ssoClientSecretValue
  }
}

//
// Azure App Config:
//
resource appConfigService 'Microsoft.AppConfiguration/configurationStores@2023-03-01' = {
  name: appConfigServiceName
  location: location
  sku: {
    name: appConfigServiceSku
  }
  // properties: {
  //   softDeleteRetentionInDays: appConfigSoftDeleteRetentionInDays
  //   enablePurgeProtection: appConfigSoftEnable
  // }
  identity: {
    type: 'SystemAssigned'
  }
}

//
// Role assignments
//
var roleStorageBlobDataContributor = subscriptionResourceId('Microsoft.Authorization/roleDefinitions', 'ba92f5b4-2d11-453d-a403-e96b0029c9fe')
var roleAppConfigDataReader = subscriptionResourceId('Microsoft.Authorization/roleDefinitions', '516239f1-63e1-4d78-a4de-a74fb236a071')
var roleKeyVaultSecretsUser = subscriptionResourceId('Microsoft.Authorization/roleDefinitions', '4633458b-17de-408a-b874-0445c86b69e6')
var roleKeyVaultAdministrator = subscriptionResourceId('Microsoft.Authorization/roleDefinitions', '00482a5a-887f-4fb3-b363-3b7fe8e74483')
var roleKeyVaultCertificatesUser = subscriptionResourceId('Microsoft.Authorization/roleDefinitions', 'db79e9a7-68ee-4b58-9aeb-b90e7c24fcba')

// Storage
resource appserviceStorageBlobAccess 'Microsoft.Authorization/roleAssignments@2022-04-01' = {
  name: guid(appService.id, storageAccount.id, roleStorageBlobDataContributor)
  scope: storageAccount
  properties: {
    roleDefinitionId: roleStorageBlobDataContributor
    principalId: appService.identity.principalId
  }
}

// resource functionStorageBlobAccess 'Microsoft.Authorization/roleAssignments@2022-04-01' = {
//   name: guid(functionApp.id, keyVault.id, roleStorageBlobDataContributor)
//   scope: storageAccount
//   properties: {
//     roleDefinitionId: roleStorageBlobDataContributor
//     principalId: functionApp.identity.principalId
//   }
// }

// Keyvault Secret User reader role assignment
resource managedIdentityKeyVaultSecretUserRoleAccess 'Microsoft.Authorization/roleAssignments@2022-04-01' = {
  name: guid(appService.id, keyVault.id, roleKeyVaultSecretsUser)
  scope: keyVault
  properties: {
    roleDefinitionId: roleKeyVaultSecretsUser
    principalId: appService.identity.principalId
  }
}

@description('Optional list of object IDs to assign Key Vault Secrets Officer role.')
param extraKeyVaultSecretsOfficerPrincipals array = []

resource extraSecretsOfficerAssignments 'Microsoft.Authorization/roleAssignments@2022-04-01' = [for principalId in extraKeyVaultSecretsOfficerPrincipals: {
  name: guid(principalId, keyVault.id, roleKeyVaultSecretsUser)
  scope: keyVault
  properties: {
    roleDefinitionId: roleKeyVaultSecretsUser
    principalId: principalId
  }
}]

// resource functionKeyVaultSecretUserRoleAccess 'Microsoft.Authorization/roleAssignments@2022-04-01' = {
//   name: guid(functionApp.id, keyVault.id, roleKeyVaultSecretsUser)
//   scope: keyVault
//   properties: {
//     roleDefinitionId: roleKeyVaultSecretsUser
//     principalId: functionApp.identity.principalId
//   }
// }

// Keyvault Certificate User role assignment
resource managedIdentityKeyVaultCertificatesUserRoleAccess 'Microsoft.Authorization/roleAssignments@2022-04-01' = {
  name: guid(appService.id, keyVault.id, roleKeyVaultCertificatesUser)
  scope: keyVault
  properties: {
    roleDefinitionId: roleKeyVaultCertificatesUser
    principalId: appService.identity.principalId
  }
}

// Grant ROCKITSpecialists-ROCK ATS-509bd417-90f3-4522-9e5c-acca191c99c Key Vault Administrator role
resource servicePrincipalKeyVaultAdminAccess 'Microsoft.Authorization/roleAssignments@2022-04-01' = {
  name: guid('e9bda821-8423-4d5f-b5fa-038ad60ed8c5', keyVault.id, roleKeyVaultAdministrator)
  scope: keyVault
  properties: {
    roleDefinitionId: roleKeyVaultAdministrator
    principalId: 'e9bda821-8423-4d5f-b5fa-038ad60ed8c5'
  }
}
// resource functionKeyVaultCertificatesUserRoleAccess 'Microsoft.Authorization/roleAssignments@2022-04-01' = {
//   name: guid(functionApp.id, keyVault.id, roleKeyVaultCertificatesUser)
//   scope: keyVault
//   properties: {
//     roleDefinitionId: roleKeyVaultCertificatesUser
//     principalId: functionApp.identity.principalId
//   }
// }

// Appconfig reader role assignment
resource managedIdentityAppConfigServiceRoleAccess 'Microsoft.Authorization/roleAssignments@2022-04-01' = {
  name: guid(appService.id, appConfigService.id, roleAppConfigDataReader)
  scope: appConfigService
  properties: {
    roleDefinitionId: roleAppConfigDataReader
    principalId: appService.identity.principalId
  }
}

// resource functionAppConfigServiceRoleAccess 'Microsoft.Authorization/roleAssignments@2022-04-01' = {
//   name: guid(functionApp.id, appConfigService.id, roleAppConfigDataReader)
//   scope: appConfigService
//   properties: {
//     roleDefinitionId: roleAppConfigDataReader
//     principalId: functionApp.identity.principalId
//   }
// }

// // Service Bus Data Reader Assignment
// resource managedIdentityServiceBusReaderServiceRoleAccess 'Microsoft.Authorization/roleAssignments@2022-04-01' = {
//   name: guid(appService.id, serviceBus.id, roleKeyServiceBusDataReader)
//   scope: serviceBus
//   properties: {
//     roleDefinitionId: roleKeyServiceBusDataReader
//     principalId: appService.identity.principalId
//   }
// }

// resource functionServiceBusReaderServiceRoleAccess 'Microsoft.Authorization/roleAssignments@2022-04-01' = {
//   name: guid(functionApp.id, serviceBus.id, roleKeyServiceBusDataReader)
//   scope: serviceBus
//   properties: {
//     roleDefinitionId: roleKeyServiceBusDataReader
//     principalId: functionApp.identity.principalId
//   }
// }

// // Service Bus Data Sender Assignment
// resource managedIdentityServiceBusSenderServiceRoleAccess 'Microsoft.Authorization/roleAssignments@2022-04-01' = {
//   name: guid(appService.id, serviceBus.id, roleKeyServiceBusDataSender)
//   scope: serviceBus
//   properties: {
//     roleDefinitionId: roleKeyServiceBusDataSender
//     principalId: appService.identity.principalId
//   }
// }

// resource functionServiceBusSenderServiceRoleAccess 'Microsoft.Authorization/roleAssignments@2022-04-01' = {
//   name: guid(functionApp.id, serviceBus.id, roleKeyServiceBusDataSender)
//   scope: serviceBus
//   properties: {
//     roleDefinitionId: roleKeyServiceBusDataSender
//     principalId: functionApp.identity.principalId
//   }
// }

//
// Map required roles on the service connection, so it has acces to required resources:
//
// resource serviceConnectionKeyVaultRoleAccess 'Microsoft.Authorization/roleAssignments@2022-04-01' = {
//   name: guid(serviceConnectionPrincipalObjectId, keyVault.id, roleKeyVaultSecretsUser)
//   scope: keyVault
//   properties: {
//     roleDefinitionId: roleKeyVaultSecretsUser
//     principalId: serviceConnectionPrincipalObjectId
//   }
// }

// resource serviceConnectionAppConfigServiceRoleAccess 'Microsoft.Authorization/roleAssignments@2022-04-01' = {
//   name: guid(serviceConnectionPrincipalObjectId, appConfigService.id, roleAppConfigDataOwner)
//   scope: appConfigService
//   properties: {
//     roleDefinitionId: roleAppConfigDataOwner
//     principalId: serviceConnectionPrincipalObjectId
//   }
// }

module hostEnable 'SNI.bicep' = if (!empty(customDomain)) {
  name: 'enableSNI'
  params: {
    appHostname: resolvedCustomDomain
    appName: appService.name
    certificateThumbprint: certificate.properties.thumbprint
  }
}

// module functionHostEnable 'SNI.bicep' = {
//   name: 'enableFunctionSNI'
//   params: {
//     appHostname: functionCustomDomain
//     appName: functionApp.name
//     certificateThumbprint: functionCertificate.properties.thumbprint
//   }
// }
