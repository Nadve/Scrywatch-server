#nullable disable
using System.Text.Json.Serialization;

namespace Scrywatch.MergeService.Scryfall;

public sealed record CardFace
{
    [JsonPropertyName("image_uris")]
    public ImageUri ImageUris { get; init; }
}
