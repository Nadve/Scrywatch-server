#nullable disable
using System.Text.Json.Serialization;

namespace Scrywatch.MergeService.Scryfall;
public record CardSets
{
    [JsonPropertyName("data")]
    public IEnumerable<Set> Data { get; init; }
}
