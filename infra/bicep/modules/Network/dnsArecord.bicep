param ipAddress string
param dnsZone string
param subdomain string

resource dnsArecord 'Microsoft.Network/dnsZones/A@2018-05-01' = {
  
  name: '${dnsZone}/${subdomain}'
  properties: {
    TTL: 60
    ARecords: [
      {
        ipv4Address: ipAddress
      }
    ]
  }
}
