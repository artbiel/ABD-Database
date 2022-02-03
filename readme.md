# ADB Database
____
### It's not production-ready solution!
C# implementation ADB Replication Algorithm.
## Features
____
1. In-memory local storage.
2. Configuration using ```appsettings.json```.
3. gRPC transport. 
4. Async client API
5. "Smart" client library encapsulating reconnection.
6. Secure messaging within the cluster 

## Getting started
____
1. Clone repository.
2. Build project in release mode.
3. Generate certificate using this [guide](https://www.yogihosting.com/certificate-authentication/).
4. Congfigure appsettings.json.
5. Run run.bat.

## Connecting to the cluster
____
### Connecting using Swagger
1. Open in browser ```https://{node_uri}/swagger/index.html```.
2. Connect using default user ("admin", "12345").
3. Perform put or get operations.

### Connecting using client library
1. Use ```AddABDDB()``` extension method to register dependecies.
```csharp
    services.AddADBDB(connectionString:"https://localhost:5001;https://localhost:5002;https://localhost:5003",
        userName: "admin",
        password: "12345",
        options => { options.UseLoggerFactory(...); })
```
2. Get ```IDBClient``` service from IoC container.
```csharp
    var client = provider.GetRequiredService<IDBClient>();
```
3. Perform requests asynchronously.
```csharp
    var key = Random.Shared.Next(100);
    var value = Random.Shared.Next(1000);
    await client.PutAsync(key, value);
    var result = await client.GetAsync<int, int>(key);
```
You can find complete example within ```ABDDB.ClientExample``` project.
## Configuration
____
DB is configuring using appsettings.json
#### Cluster configuration
DB doesn't use any DNS, so you should explicitly provide all URIs and Node IDs. Node ID should be unique within the cluster.
```json
{
    "ClusterConfig": {
        "Nodes": [
          {
            "Id": 1,
            "Uri": "https://localhost:5001"
          },
          ...
        ]
    }
}
 ```
### Security Configuration
PFX sertificate is using for internal authentication within the cluster.
```json
{
    "SecurityConfig": 
      {
        "Certificate": {
          "Path": "certificate.pfx",
          "Password": "12345"
        },
        "AllowedThumbprints": [
          "211FCD2A8241FEFBB9C1FD1A205E14A22B6C2380"
        ]
      }
}
 ```
#### Transport configuration
This section is unnecessary. If you don't specify values, they will be set by default, as shown below (initial and max backoff are measured in seconds).

```json
{
    "TransportConfig": 
      {
        "MaxRetryAttempts": 3,
        "InitialBackoff": 2,
        "MaxBackoff": 8,
        "BackoffMultiplier": 2
      }
}
```

### Full config example:
```json
   {
      "ClusterConfig": {
        "Nodes": [
          {
            "Id": 1,
            "Uri": "https://localhost:5001"
          },
          {
            "Id": 2,
            "Uri": "https://localhost:5002"
          },
          {
            "Id": 3,
            "Uri": "https://localhost:5003"
          }
        ]
      },
      "SecurityConfig": 
      {
        "Certificate": {
          "Path": "certificate.pfx",
          "Password": "12345"
        },
        "AllowedThumbprints": [
          "211FCD2A8241FEFBB9C1FD1A205E14A22B6C2380"
        ]
      },
      "TransportConfig": 
      {
        "MaxRetryAttempts": 3,
        "InitialBackoff": 2,
        "MaxBackoff": 8,
        "BackoffMultiplier": 2
      }
    }
```