@description('Specifies a name that is used to make the role assignment id unique with.')
param roleAssignmentName string

@description('Specifies the ACR container registry including azurecr.io suffix.')
param registryName string

@description('Specifies Service Principal identifier to assign the role towards (could be id for MI).')
param principalId string

// this is the role id for pull rights from ACR:
var azureContainerRegPullRoleId = '7f951dda-4ed3-4680-a7ca-43fe172d538d'

resource containerReg 'Microsoft.ContainerRegistry/registries@2022-02-01-preview' existing = {
  name: registryName
}

resource acrPullRoleAssignment 'Microsoft.Authorization/roleAssignments@2020-10-01-preview' = {
  name: guid(azureContainerRegPullRoleId, registryName, roleAssignmentName)
  scope: containerReg
  properties: {
    roleDefinitionId: subscriptionResourceId('Microsoft.Authorization/roleDefinitions', azureContainerRegPullRoleId)
    principalId: principalId
    principalType: 'ServicePrincipal'
  }
}
