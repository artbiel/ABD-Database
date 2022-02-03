using ABDDB.LocalStorage;
using ADBDB.Api.Configuration.Models;
using ProtoBuf.Grpc.Server;

namespace ABDDB.Api.Utils
{
    public static class BuilderExtensions
    {
        public static void ApplyConfiguration(this WebApplicationBuilder builder, ConfigurationModel configuration) =>
            builder.WebHost.UseUrls(configuration.ClusterConfiguration.CurrentNode.Uri);

        public static void AddServices(this WebApplicationBuilder builder, ConfigurationModel configuration) =>
            builder.Services
                .AddLocalStorage()
                .AddReplication(configuration)
                .AddApi()
                .AddEndpointsApiExplorer()
                .AddSwaggerGen()
                .AddCodeFirstGrpc();
    }
}
