using ProtoBuf.Grpc;
using System.ServiceModel;

namespace ABDDB.Shared.Store
{
    [ServiceContract]
    public interface IStoreService
    {
        [OperationContract]
        Task<GetResponse> GetAsync(GetRequest request, CallContext context = default);

        [OperationContract]
        Task PutAsync(PutRequest request, CallContext context = default);
        
        [OperationContract]
        Task DeleteAsync(DeleteRequest request, CallContext context = default);
    }
}