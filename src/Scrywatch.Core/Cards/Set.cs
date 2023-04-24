namespace Scrywatch.Core.Cards;

public sealed record Set
{
    public string Name { get; init; } = null!;

    public string Code { get; init; } = null!;

    public string Svg { get; init; } = null!;
}
