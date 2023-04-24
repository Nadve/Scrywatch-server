using Scrywatch.Core.ValueObjects;

namespace Scrywatch.Core.Interests;

public sealed class InterestDto
{
    public string UserId { get; init; } = null!;

    public int CardId { get; init; }

    public int Goal { get; init; }

    public Finish Finish { get; init; } = null!;

    public Currency Currency { get; init; } = null!;

    public Intention Intention { get; init; } = null!;

    public dynamic AsDynamic => new
    {
        UserId,
        CardId,
        Goal,
        Finish = Finish.Name,
        Currency = Currency.Name,
        Intention = Intention.Name
    };
}
