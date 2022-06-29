//see:; https://github.com/Azure/azure-quickstart-templates/blob/master/quickstarts/microsoft.app/container-app-create/main.bicep

@description('Specifies the name of the container app.')
param containerAppName string = 'containerapp-${uniqueString(resourceGroup().id)}'

@description('Specifies the name of the container app environment.')
param containerAppEnvName string = 'containerapp-env-${uniqueString(resourceGroup().id)}'

@description('Specifies the location for all resources.')
param location string = resourceGroup().location

@description('Specifies the ACR container registry from where to pull - full name is expected including azurecr.io suffix.')
param registryName string

@description('Specifies the docker container image to deploy.')
param containerImage string = 'mcr.microsoft.com/azuredocs/containerapps-helloworld:latest'

@description('Specifies the container port.')
param targetPort int = 80

@description('Number of CPU cores the container can use. Can be with a maximum of two decimals.')
param cpuCore string = '0.5'

@description('Amount of memory (in gibibytes, GiB) allocated to the container up to 4GiB. Can be with a maximum of two decimals. Ratio with CPU cores must be equal to 2.')
param memorySize string = '1'

@description('Minimum number of replicas that will be deployed')
@minValue(0)
@maxValue(25)
param minReplicas int = 0

@description('Maximum number of replicas that will be deployed')
@minValue(0)
@maxValue(25)
param maxReplicas int = 3

// this is the role id for pull rights from ACR:
var azureContainerRegPullRoleId = '7f951dda-4ed3-4680-a7ca-43fe172d538d'

resource containerReg 'Microsoft.ContainerRegistry/registries@2022-02-01-preview' existing = {
  name: registryName
}

resource containerAppEnv 'Microsoft.App/managedEnvironments@2022-03-01' existing = {
  name: containerAppEnvName
}

resource userIdentity 'Microsoft.ManagedIdentity/userAssignedIdentities@2021-09-30-preview' = {
  name: 'bidding-api-user-identity'
  location: location
}

resource containerApp 'Microsoft.App/containerApps@2022-03-01' = {
  name: containerAppName
  identity: {
    type: 'UserAssigned'
    userAssignedIdentities: {
      '${userIdentity.id}': {}
    }
  }
  location: location
  properties: {
    managedEnvironmentId: containerAppEnv.id
    configuration: {
      registries: [
        {
          server: registryName
          identity: 'system'
        }
      ]
      ingress: {
        external: true
        targetPort: targetPort
        allowInsecure: false
        traffic: [
          {
            latestRevision: true
            weight: 100
          }
        ]
      }
    }
    template: {
      revisionSuffix: 'firstrevision'
      containers: [
        {
          name: containerAppName
          image: containerImage
          resources: {
            cpu: json(cpuCore)
            memory: '${memorySize}Gi'
          }
        }
      ]
      scale: {
        minReplicas: minReplicas
        maxReplicas: maxReplicas
      }
    }
  }
}

resource acrPullRoleAssignment 'Microsoft.Authorization/roleAssignments@2020-10-01-preview' = {
  name: guid(containerApp.name, azureContainerRegPullRoleId, containerReg.name)
  scope: containerReg
  properties: {
    roleDefinitionId: subscriptionResourceId('Microsoft.Authorization/roleDefinitions', azureContainerRegPullRoleId)
    principalId: userIdentity.properties.principalId
    principalType: 'ServicePrincipal'
  }
}

output containerAppFQDN string = containerApp.properties.configuration.ingress.fqdn
