namespace Scrywatch.Core.Auth;

public sealed class User
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string UserName { get; set; } = null!;
    public string NormalizedUserName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string NormalizedEmail { get; set; } = null!;
    public bool EmailConfirmed { get; set; }
    public string PasswordHash { get; set; } = null!;
    public string SecurityStamp { get; set; } = null!;
    public string ConcurrencyStamp { get; set; } = Guid.NewGuid().ToString();
    public DateTimeOffset? LockoutEnd { get; set; }
    public bool LockoutEnabled { get; set; }
    public int AccessFailedCount { get; set; }
    public string? PhoneNumber { get; set; }
    public bool PhoneNumberConfirmed { get; set; }
    public bool TwoFactorEnabled { get; set; }

    public dynamic AsDynamic => new
    {
        Id,
        UserName,
        NormalizedUserName,
        Email,
        NormalizedEmail,
        EmailConfirmed,
        PasswordHash,
        SecurityStamp,
        ConcurrencyStamp,
        PhoneNumber,
        PhoneNumberConfirmed,
        TwoFactorEnabled,
        LockoutEnd,
        LockoutEnabled,
        AccessFailedCount
    };
}
