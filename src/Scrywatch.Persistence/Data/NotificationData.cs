using Scrywatch.Core.Interests;
using Scrywatch.Core.ValueObjects;

namespace Scrywatch.Persistence.Data;

public sealed class NotificationData
{
    public int InterestId { get; init; }

    public string Email { get; init; } = null!;

    public int CardId { get; init; }

    public Finish Finish { get; init; } = null!;

    public Currency Currency { get; init; } = null!;

    public Intention Intention { get; init; } = null!;

    public int Goal { get; init; }
}
