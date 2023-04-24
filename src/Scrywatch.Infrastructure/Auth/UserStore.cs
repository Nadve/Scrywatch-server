using Microsoft.AspNetCore.Identity;
using Scrywatch.Core.Auth;
using Scrywatch.Persistence;

namespace Scrywatch.Infrastructure.Auth;

public class UserStore : IUserStore<User>, IUserPasswordStore<User>, IUserEmailStore<User>
{
    private readonly IDbConnection _db;

    public UserStore(IDbConnection db) => _db = db;

    public async Task<IdentityResult> CreateAsync(User user) => await
        _db.ExecuteAsync(StoredProcedure.CreateUser, user.AsDynamic) > 0
        ? IdentityResult.Success
        : IdentityResult.Failed(new IdentityError() { Code = "120", Description = "Cannot Create User!" });

    public async Task<IdentityResult> DeleteAsync(User user) => await
        _db.ExecuteAsync(StoredProcedure.DeleteUser, user.AsDynamic) > 0
        ? IdentityResult.Success
        : IdentityResult.Failed(new IdentityError() { Code = "120", Description = "Cannot Delete User!" });

    public async Task<IdentityResult> UpdateAsync(User user) => await
        _db.ExecuteAsync(StoredProcedure.UpdateUser, user.AsDynamic) > 0
        ? IdentityResult.Success
        : IdentityResult.Failed(new IdentityError() { Code = "120", Description = "Cannot Update User!" });

    public Task<IdentityResult> CreateAsync(User user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return CreateAsync(user);
    }

    public Task<IdentityResult> DeleteAsync(User user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return DeleteAsync(user);
    }

    public Task<IdentityResult> UpdateAsync(User user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return UpdateAsync(user);
    }

    public Task<User?> FindByIdAsync(string userId, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return _db.QueryFirstOrDefaultAsync<User?>(StoredProcedure.GetUser, new { id = userId });
    }

    public Task<User?> FindByNameAsync(string userName, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return _db.QueryFirstOrDefaultAsync<User?>(StoredProcedure.FindUser, new { userName });
    }

    public Task<User?> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return _db.QueryFirstOrDefaultAsync<User?>(StoredProcedure.FindUserByEmail, new { normalizedEmail });
    }

    public Task<string?> GetNormalizedUserNameAsync(User user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return Task.Run(() => user?.UserName?.ToUpper());
    }

    public Task<string?> GetPasswordHashAsync(User user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return Task.Run(() => user?.PasswordHash);
    }

    public Task<string> GetUserIdAsync(User user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return Task.Run(() => user.Id.ToString());
    }

    public Task<string?> GetUserNameAsync(User user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return Task.Run(() => user?.UserName);
    }

    public Task<bool> HasPasswordAsync(User user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return Task.FromResult(!string.IsNullOrWhiteSpace(user?.PasswordHash));
    }

    public Task SetNormalizedUserNameAsync(User user, string? normalizedName, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        if (normalizedName is null) throw new ArgumentNullException(normalizedName);
        return Task.FromResult(user.NormalizedUserName = normalizedName);
    }

    public Task SetPasswordHashAsync(User user, string? passwordHash, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        if (passwordHash is null) throw new ArgumentNullException(passwordHash);
        return Task.FromResult(user.PasswordHash = passwordHash);
    }

    public Task SetUserNameAsync(User user, string? userName, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        if (userName == null) throw new ArgumentNullException(nameof(userName));
        return Task.FromResult(user.UserName = userName);
    }

    public void Dispose() { }

    public Task SetEmailAsync(User user, string? email, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        if (email == null) throw new ArgumentNullException(nameof(email));
        return Task.FromResult(user.Email = email);
    }

    public Task<string?> GetEmailAsync(User user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return Task.FromResult(user?.Email);
    }

    public Task<bool> GetEmailConfirmedAsync(User user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return Task.FromResult(user.EmailConfirmed);
    }

    public Task SetEmailConfirmedAsync(User user, bool confirmed, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return Task.FromResult(user.EmailConfirmed = confirmed);
    }

    public Task<string?> GetNormalizedEmailAsync(User user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return Task.FromResult(user?.NormalizedEmail);
    }

    public Task SetNormalizedEmailAsync(User user, string? normalizedEmail, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        if (normalizedEmail == null)
            throw new ArgumentNullException(nameof(normalizedEmail));
        return Task.FromResult(user.NormalizedEmail = normalizedEmail);
    }
}
