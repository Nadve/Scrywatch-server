namespace Scrywatch.Persistence.Data;

public sealed record CardData
{
    public int Id { get; init; }

    public string Name { get; init; } = null!;

    public string Language { get; init; } = null!;

    public string Rarity { get; init; } = null!;

    public string Set { get; init; } = null!;

    public string SetCode { get; init; } = null!;

    public string SetSvg { get; init; } = null!;

    public string CollectorNumber { get; init; } = null!;

    public string FrontFaceUrl { get; init; } = null!;

    public string? BackFaceUrl { get; init; }
}
