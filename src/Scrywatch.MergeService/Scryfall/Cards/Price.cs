using System.Text.Json.Serialization;

namespace Scrywatch.MergeService.Scryfall;

public sealed record Price
{
    [JsonPropertyName("usd")]
    public string? Usd { get; init; }

    [JsonPropertyName("usd_foil")]
    public string? UsdFoil { get; init; }

    [JsonPropertyName("usd_etched")]
    public string? UsdEtched { get; init; }

    [JsonPropertyName("eur")]
    public string? Eur { get; init; }

    [JsonPropertyName("eur_foil")]
    public string? EurFoil { get; init; }

    [JsonPropertyName("tix")]
    public string? Tix { get; init; }
}
