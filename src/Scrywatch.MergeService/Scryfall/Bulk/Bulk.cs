#nullable disable
using System.Text.Json.Serialization;

namespace Scrywatch.MergeService.Scryfall;
public sealed record Bulk
{
    [JsonPropertyName("data")]
    public IEnumerable<BulkData> Data { get; init; }

    public string AllCardsUri =>
        Data.First(d => d.Type.Equals("all_cards")).Uri;

    public DateTime AllCardsUpdated =>
        Data.First(d => d.Type.Equals("all_cards")).Updated;
}
