using ABDDB.Client;
using ABDDB.Client.DBClient;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

IServiceCollection services = new ServiceCollection();
services.AddABDDB(connectionString: "https://localhost:5001;https://localhost:5002;https://localhost:5003",
        userName: "admin",
        password: "12345")
    .AddLogging(b => b.AddConsole());
var provider = services.BuildServiceProvider();

var client = provider.GetRequiredService<IDBClient>();

while (true)
{
    var key = Random.Shared.Next(100);
    var value = Random.Shared.Next(1000);
    await client.PutAsync(key, value);
    var result = await client.GetAsync<int, int>(key);
    await Task.Delay(3000);
}
