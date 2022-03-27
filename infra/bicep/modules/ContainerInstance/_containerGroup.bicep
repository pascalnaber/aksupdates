param containerName string 
param location string = 'westeurope'
param imageName string 
param cpuCores int = 1
param memoryInGb int = 1
param dnsName string 
param acrName string

resource acr 'Microsoft.ContainerRegistry/registries@2019-12-01-preview' existing = {
  scope: resourceGroup('aksupdates-common') 
  name: acrName  
}

resource containerGroup 'Microsoft.ContainerInstance/containerGroups@2019-12-01' = {
  name: containerName
  location: location
  properties: {
    imageRegistryCredentials: [
      {
        server: acr.properties.loginServer
        username: acr.listCredentials().username
        password: acr.listCredentials().passwords[0].value
      }
    ]
    containers: [
      {
        name: containerName
        properties: {
          image: imageName
          resources: {
            requests: {
              cpu: cpuCores
              memoryInGB: memoryInGb
            }
          }
          ports: [
            {
              protocol: 'TCP'
              port: 8080
            }
          ]
        }
      }
      {
        name: 'caddy'
        properties: {
          image: 'caddy:latest'
          command: [
            'caddy'
            'reverse-proxy'
            '--from'
            'akslatestversiondev.techdriven.nl'
            '--to'
            'localhost:8080'
          ]
          ports: [
          {
            protocol: 'TCP'
            port: 80
          }
          {
            protocol: 'TCP'
            port: 443
          }
          ]
          resources: {
            requests: {
              cpu: 1
              memoryInGB: 1
            }
          }
      }
    }
    ]
    restartPolicy: 'OnFailure'
    osType: 'Linux'
    ipAddress: {
      type: 'Public'
      dnsNameLabel: dnsName
      ports: [
        {
          protocol: 'TCP'
          port: 80
        }
        {
          protocol: 'TCP'
          port: 443
        }
      ]
    }
  }
}


output containerIpv4Address string = containerGroup.properties.ipAddress.ip
output containerDnsName string = containerGroup.properties.ipAddress.fqdn
