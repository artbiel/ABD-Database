using ProtoBuf.Grpc;
using System.ServiceModel;

namespace ABDDB.Replication.Contracts
{
    [ServiceContract]
    public interface IReplica
    {
        [OperationContract]
        Task<ReadResponse> ReadAsync(ReadRequest request, CallContext context = default);

        [OperationContract]
        Task WriteAsync(WriteRequest request, CallContext context = default);
    }
}