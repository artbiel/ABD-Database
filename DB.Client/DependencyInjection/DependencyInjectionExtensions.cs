using ABDDB.Client.DBClient;
using ABDDB.Client.Models;
using ABDDB.Client.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ABDDB.Client
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddABDDB(this IServiceCollection services,
                string connectionString, UserCredentials credentials, Action<DBOptions>? configureOptions = default)
        {
            var options = new DBOptions();
            if (configureOptions is not null)
                configureOptions(options);
            var loggerFactory = options.LoggerFactory;

            return services
                  .AddSingleton<IDBClient, DBClient.DBClient>()
                  .AddSingleton<IChannelPool, ChannelPool>(s => new ChannelPool(loggerFactory, connectionString, credentials));
        }

        public static IServiceCollection AddABDDB(this IServiceCollection services,
                string connectionString, string userName, string password, Action<DBOptions>? configureOptions = default)
            => services.AddABDDB(connectionString, new UserCredentials(userName, password), configureOptions);

    }
}