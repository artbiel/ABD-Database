using ABDDB.Shared.Client.Messages;
using ProtoBuf.Grpc;
using System.ServiceModel;

namespace ABDDB.Shared.Client
{
    [ServiceContract]
    public interface IClientService
    {
        [OperationContract]
        Task<ConnectResponse> Connect(ConnectRequest request, CallContext context = default);

        [OperationContract]
        Task<ConnectResponse> Disconnect(CallContext context = default);
    }
}
