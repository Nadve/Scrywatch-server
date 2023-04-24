using Scrywatch.Core.Interests;

namespace Scrywatch.Core.Notifications;

public sealed class Notification
{
    public string Email { get; init; } = null!;

    public IEnumerable<Interest> Interests { get; init; } = null!;
}
