using Grpc.Net.Client;

namespace ABDDB.Client.Services
{
    public interface IChannelPool : IAsyncDisposable
    {
        GrpcChannel GetActiveChannel();
        Task Execute(Func<GrpcChannel, Task> task);
        Task<T> Execute<T>(Func<GrpcChannel, Task<T>> task);
    }
}
