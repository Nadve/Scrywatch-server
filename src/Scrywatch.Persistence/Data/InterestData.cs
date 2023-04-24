using Scrywatch.Core.Interests;
using Scrywatch.Core.ValueObjects;

namespace Scrywatch.Persistence.Data;

public sealed record InterestData
{
    public int Id { get; init; }

    public int CardId { get; init; }

    public int Goal { get; init; }

    public Finish Finish { get; init; } = null!;

    public Currency Currency { get; init; } = null!;

    public Intention Intention { get; init; } = null!;
}
