#nullable disable
using System.Text.Json.Serialization;

namespace Scrywatch.MergeService.Scryfall;

public sealed record ImageUri
{
    [JsonPropertyName("normal")]
    public string Normal { get; init; }
}
