using Microsoft.AspNetCore.Identity;

namespace Scrywatch.Infrastructure.Auth;

public class RoleStore : IRoleStore<UserRole>
{
    public void Dispose() { }

    Task<IdentityResult> IRoleStore<UserRole>.CreateAsync(UserRole role, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    Task<IdentityResult> IRoleStore<UserRole>.DeleteAsync(UserRole role, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    Task<UserRole?> IRoleStore<UserRole>.FindByIdAsync(string roleId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    Task<UserRole?> IRoleStore<UserRole>.FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    Task<string?> IRoleStore<UserRole>.GetNormalizedRoleNameAsync(UserRole role, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    Task<string> IRoleStore<UserRole>.GetRoleIdAsync(UserRole role, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    Task<string?> IRoleStore<UserRole>.GetRoleNameAsync(UserRole role, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    Task IRoleStore<UserRole>.SetNormalizedRoleNameAsync(UserRole role, string? normalizedName, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    Task IRoleStore<UserRole>.SetRoleNameAsync(UserRole role, string? roleName, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    Task<IdentityResult> IRoleStore<UserRole>.UpdateAsync(UserRole role, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
