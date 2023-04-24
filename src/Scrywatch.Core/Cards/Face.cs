namespace Scrywatch.Core.Cards;

public sealed record Face
{
    public string Front { get; init; } = null!;

    public string? Back { get; init; }
}
