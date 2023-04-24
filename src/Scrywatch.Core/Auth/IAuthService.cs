using System.Security.Claims;

namespace Scrywatch.Core.Auth;

public interface IAuthService
{
    Task<Confirmation> ConfirmEmail(string email, string token);

    Task<Registration> Register(string email, string password);

    Task<Signing> SignIn(string email, string password);

    Task Logout();

    Task<string?> GetUserId(ClaimsPrincipal claimsPrincipal);
}
