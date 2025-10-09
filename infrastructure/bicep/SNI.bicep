// SNI.bicep

param appName string // from webApp.name
param appHostname string // the full endpoint for this web app (xyz.domain.com)
param certificateThumbprint string // from certificate.properties.thumbprint

resource hostBinding 'Microsoft.Web/sites/hostNameBindings@2023-01-01' = {
  name: '${appName}/${appHostname}'
  properties: {
    sslState: 'SniEnabled'
    thumbprint: certificateThumbprint
  }
}