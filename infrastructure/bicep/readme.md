# Commands to run in order

## Build bicep file

This step is used to valdate the bicep file.

```bash
bicep build .\steer73.rock.bicep
```

## Login to the tenant

```bash
az login --tenant steer73.com
```

## Set the subscription to the correct one (Rock ATS DevTest)

```bash
az account set --subscription 3db64f22-f271-4b99-be77-6b4263b4d270
```

## Verify your account is logged into the correct place

```bash
az account show
```

## Parameters File Setup

1. **Copy** the `example.parameters.json` file to a new file named `prod.parameters.json`.
2. **Update** the values in `prod.parameters.json` as required:

| Parameter | Description |
| :-------- | :----------- |
| `environmentSuffix` | The environment name (e.g., `dev`, `prod`, `test`). |
| `location` | The location of the resource group (e.g., `uksouth`). |
| `sqlAdminPassword` | The password for the SQL admin user. |
| `customDomain` | (Optional) The custom domain for the app. |
| `sqlAdminLogin` | The UPN (User Principal Name) of the SQL admin account. |
| `sqlAdminTenantId` | The tenant ID the SQL admin account belongs to. |
| `sqlAdminSid` | The SID (Security Identifier) of the SQL admin account. |
| `ssoClientSecretValue` | The secret value for the SSO client. |
| `extraKeyVaultSecretsOfficerPrincipals` | A list of principals to be granted the `KeyVaultSecretsOfficer` role. |

---

**Note:** Ensure all sensitive values (like passwords and secrets) are securely stored and handled appropriately.


## Set variables for resource group and parameter file

```powershell
$env:ATS_RESOURCE_GROUP="ATS-Prod"
$env:ATS_PARAM_FILE="prod.parameters.json"
```

## Create the environment

This step will create the resource group and deploy the bicep file to it.

```powershell

(-w is the whatif attribute and doesn't actually update anything until you remove the -w).

```powershell
az group create --name $env:ATS_RESOURCE_GROUP --location uksouth
az deployment group create --template-file .\steer73.rock.bicep --resource-group $env:ATS_RESOURCE_GROUP --mode Incremental --parameters "@$env:ATS_PARAM_FILE" -w
```

## Post Deployment Steps

### SQL Permissions

1. Sign into Azure using the account specified in the parameter files sqlAdminLogin parameter.
2. Connect to the SQL database using Entra authentication.
3. Run the CreateAppUser.sql script to give the app access to the database.
   *Make sure to update AppName to the correct environment.*
4. Run the CreateDevOpsUser.sql script to give the Azure DevOps service account access to the database.

### App Configuration Settings

1. Grant yourself [App Configuration Data Owner] permissions
2. Run the following command to import required settings into App Configuration
   (update the APP_CONFIG_NAME to be the name of the App Configuration resource to be imported into and the APP_CONFIG_SETTINGS_FILE to be the name of the JSON file to import):

   ```powershell
   $env:APP_CONFIG_NAME="rockats-azureappconfig-neu-dev"
   $env:APP_CONFIG_SETTINGS_FILE="defaultAzureAppSettings.Dev.json"

   az appconfig kv import --name $env:APP_CONFIG_NAME --source file --format json --path ../appsettings/$env:APP_CONFIG_SETTINGS_FILE --profile appconfig/kvset --auth-mode login --yes
   ```

3. Update the newly created values in the App Configuration resource with the correct values for the environment

### Key Vault

1. Grant yourself the [Key Vault Certificates Officer] role
2. Create 2 certificates in the Key Vault:
   - `EncryptionCert` with a subject of 'CN=encryptioncert'
   - `SigningCert` with a subject of 'CN=signingcert'

## Release pipeline

If customising the current release pipeline the following should be updated:

1. Variables
   - Create a new SQL Admin connection string and populate it's value with the new SQL databases connection string
2. Azure Key Vault
   - Update to the newly created key vault
3. Set connection string value
   - Update the connection string value with the variable created in step 1
4. Start Azure App Service
   - Update the app service name to the new app service name
   - Update the resource group to the new resource group name
5. Azure App Service Deploy
   - Update the display name
   - Update the app service name to the new app service name
   - Update the resource group to the new resource group name

### Test App

1. Open the test app in a browser
2. Sign in using the following credentials:
   - Username: admin
   - Password: 1q2w3E*
3. Change the admin password to something secure
