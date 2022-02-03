using ABDDB.LocalStorage;
using ABDDB.LocalStorage.Models;
using ABDDB.Replication.Contracts;
using Microsoft.AspNetCore.Authorization;
using ProtoBuf.Grpc;

namespace ABDDB.Replication.Actors
{
    [Authorize(AuthenticationSchemes = "Certificate")]
    public class Replica : IReplica
    {
        private readonly ILocalStorage _localStorage;

        public Replica(ILocalStorage localStorage)
        {
            _localStorage = localStorage ?? throw new ArgumentNullException(nameof(localStorage));
        }

        public async Task<ReadResponse> ReadAsync(ReadRequest request, CallContext context = default)
        {
            var (value, timestamp) = await _localStorage.GetAsync(request.Key);
            return new ReadResponse { TimestampModel = timestamp, Value = value };
        }

        public Task WriteAsync(WriteRequest request, CallContext context = default) =>
             _localStorage.PutAsync(request.Key, new ValueModel(request.Value, request.TimestampModel));
    }
}