using ABDDB.Api.Services;
using ABDDB.Shared.Client;
using ABDDB.Shared.Client.Messages;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using ProtoBuf.Grpc;

namespace ABDDB.Api.Grpc
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class ClientService : IClientService
    {
        private IAuthenticationService _authenticationService;

        public ClientService(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService ?? throw new ArgumentNullException(nameof(authenticationService));
        }

        public async Task<ConnectResponse> Connect(ConnectRequest request, CallContext context = default)
        {
            var authenticated = await _authenticationService.SignIn(request.UserName, request.Password);
            return new ConnectResponse { Authenticated = authenticated };
        }

        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public async Task<ConnectResponse> Disconnect(CallContext context = default)
        {
            await _authenticationService.SignOut();
            return new ConnectResponse { Authenticated = false };
        }
    }
}
