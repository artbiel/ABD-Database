
using ABDDB.Api.Services.Models;

namespace ABDDB.Api.Services
{
    public interface IAuthenticationService
    {
        Task<bool> CreateUser(string userName, string password, Role role);
        Task<bool> SignIn(string userName, string password);
        Task SignOut();
    }
}