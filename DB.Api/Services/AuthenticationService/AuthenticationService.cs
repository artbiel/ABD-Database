using ABDDB.Api.RequestCoordinator;
using ABDDB.Api.Services.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text.Json;

namespace ABDDB.Api.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly HttpContext _httpContext;
        private readonly IRequestCoordinator _requestCoordinator;

        public AuthenticationService(IHttpContextAccessor httpContextAccessor, IRequestCoordinator requestCoordinator)
        {
            _httpContext = httpContextAccessor?.HttpContext ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            _requestCoordinator = requestCoordinator ?? throw new ArgumentNullException(nameof(requestCoordinator));
        }

        public async Task<bool> CreateUser(string userName, string password, Role role)
        {
            var userSerial = await _requestCoordinator.GetAsync(userName);

            if (userSerial is not null)
                return false;

            byte[] salt = RandomNumberGenerator.GetBytes(128 / 8);

            string passwordHash = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));

            var userInfo = new UserInfo(passwordHash, salt, role);
            await _requestCoordinator.PutAsync(userName, JsonSerializer.Serialize(userInfo));
            return true;
        }

        public async Task<bool> SignIn(string userName, string password)
        {
            var userSerial = await _requestCoordinator.GetAsync(userName);

            if (userSerial is null)
                return false;

            var user = JsonSerializer.Deserialize<UserInfo>(userSerial);

            var passwordHash = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                    password: password,
                    salt: user.Salt,
                    prf: KeyDerivationPrf.HMACSHA256,
                    iterationCount: 100000,
                    numBytesRequested: 256 / 8));

            if (user.PasswordHash != passwordHash)
                return false;

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, userName),
                new Claim(ClaimTypes.Role, user.Role.ToString()),
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                AllowRefresh = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(1),
                IsPersistent = true,
            };

            await _httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            return true;
        }

        public Task SignOut() => _httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    }
}
