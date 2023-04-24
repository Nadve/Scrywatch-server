using Scrywatch.Core.ValueObjects;

namespace Scrywatch.Core.Cards;

public sealed record Price
{
    public Finish Finish { get; init; } = null!;

    public Currency Currency { get; init; } = null!;

    public IDictionary<DateOnly, float> DateValuePairs { get; init; } = null!;
}
