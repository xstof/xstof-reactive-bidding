//see:; https://github.com/Azure/azure-quickstart-templates/blob/master/quickstarts/microsoft.app/container-app-create/main.bicep

@description('Specifies the name of the container app.')
param containerAppName string = 'containerapp-${uniqueString(resourceGroup().id)}'

@description('Specifies the name of the container app environment.')
param containerAppEnvName string = 'containerapp-env-${uniqueString(resourceGroup().id)}'

@description('Specifies the location for all resources.')
param location string = resourceGroup().location

@description('Specifies the ACR container registry from where to pull - leaving off azurecr.io suffix.')
param registryName string

@description('Specifies the name of the Resource Group the ACR container registry is in.')
param registryResourceGroupName string

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

var fullRegistryName = '${registryName}.azurecr.io'

resource containerReg 'Microsoft.ContainerRegistry/registries@2022-02-01-preview' existing = {
  name: registryName
  scope: resourceGroup(registryResourceGroupName)
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
  dependsOn: [
    acrPullRoleAssignment
  ]
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
          server: fullRegistryName
          identity: userIdentity.id
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

module acrPullRoleAssignment './acr-pull-role-assignment.bicep' = {
  name: 'acr-pull-role-assignment'
  scope: resourceGroup(registryResourceGroupName)
  params: {
    registryName: registryName
    principalId: userIdentity.properties.principalId
    roleAssignmentName: containerAppName
  }
}

output containerAppFQDN string = containerApp.properties.configuration.ingress.fqdn
