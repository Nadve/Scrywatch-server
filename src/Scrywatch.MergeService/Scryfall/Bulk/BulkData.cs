#nullable disable
using System.Text.Json.Serialization;

namespace Scrywatch.MergeService.Scryfall;

public sealed record BulkData
{
    [JsonPropertyName("type")]
    public string Type { get; init; }

    [JsonPropertyName("download_uri")]
    public string Uri { get; init; }

    [JsonPropertyName("updated_at")]
    public DateTime Updated { get; init; }
}
