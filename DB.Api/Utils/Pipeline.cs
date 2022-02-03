using ABDDB.Api.Grpc;
using ABDDB.Replication.Actors;

namespace ABDDB.Api.Utils
{
    public static class Pipeline
    {
        public static void ConfigurePipeline(this WebApplication app)
        {
            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapGrpcService<Replica>();
            app.MapGrpcService<StoreService>();
            app.MapGrpcService<ClientService>();
        }
    }
}
