#nullable disable
using System.Text.Json.Serialization;

namespace Scrywatch.MergeService.Scryfall;

public sealed record Set : IEquatable<Set>
{
    [JsonPropertyName("id")]
    public Guid Id { get; init; }

    [JsonPropertyName("icon_svg_uri")]
    public string Svg { get; init; }

    [JsonPropertyName("name")]
    public string Name { get; init; }

    [JsonPropertyName("code")]
    public string Code { get; init; }

    public bool Equals(Set set)
        => Id.Equals(set.Id);

    public override int GetHashCode()
        => base.GetHashCode() * Code.GetHashCode() + Svg.GetHashCode();
}
