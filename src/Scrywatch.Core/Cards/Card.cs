using Scrywatch.Core.ValueObjects;

namespace Scrywatch.Core.Cards;

public sealed record Card
{
    public int Id { get; init; }

    public string Name { get; init; } = null!;

    public string Language { get; init; } = null!;

    public string Rarity { get; init; } = null!;

    public string CollectorNumber { get; init; } = null!;

    public Set Set { get; init; } = null!;

    public Face Face { get; init; } = null!;

    public IEnumerable<Finish> Finishes { get; init; } = null!;

    public IEnumerable<Price> Prices { get; init; } = null!;
}
