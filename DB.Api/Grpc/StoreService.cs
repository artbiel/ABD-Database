using ABDDB.Api.RequestCoordinator;
using ABDDB.Shared.Store;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using ProtoBuf.Grpc;

namespace ABDDB.Api.Grpc
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class StoreService : IStoreService
    {
        private readonly IRequestCoordinator _requestCoordinator;

        public StoreService(IRequestCoordinator requestCoordinator)
        {
            _requestCoordinator = requestCoordinator ?? throw new ArgumentNullException(nameof(requestCoordinator));
        }

        public async Task<GetResponse> GetAsync(GetRequest request, CallContext context = default)
        {
            var value = await _requestCoordinator.GetAsync(request.Key, context.CancellationToken);
            return new GetResponse { Value = value };
        }

        public Task PutAsync(PutRequest request, CallContext context = default) =>
            _requestCoordinator.PutAsync(request.Key, request.Value, context.CancellationToken);

        public Task DeleteAsync(DeleteRequest request, CallContext context = default) =>
            _requestCoordinator.DeleteAsync(request.Key, context.CancellationToken);
    }
}
