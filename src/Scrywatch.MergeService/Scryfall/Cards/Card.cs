using System.Text.Json.Serialization;

namespace Scrywatch.MergeService.Scryfall;

public sealed class Card : IEquatable<Card>
{
    [JsonPropertyName("id")]
    public Guid Id { get; init; }

    [JsonPropertyName("set_Id")]
    public Guid SetId { get; init; }

    [JsonPropertyName("image_uris")]
    public ImageUri? ImageUris { get; init; }

    [JsonPropertyName("card_faces")]
    public CardFace[]? CardFaces { get; init; }

    [JsonPropertyName("name")]
    public string Name { get; init; } = null!;

    [JsonPropertyName("lang")]
    public string Language{ get; init; } = null!;

    [JsonPropertyName("finishes")]
    public IEnumerable<string> Finishes { get; init; } = null!;

    [JsonPropertyName("prices")]
    public Price Prices { get; init; } = null!;

    [JsonPropertyName("collector_number")]
    public string CollectorNumber { get; init; } = null!;

    [JsonPropertyName("rarity")]
    public string Rarity { get; init; } = null!;

    [JsonPropertyName("set")]
    public string SetCode { get; init; } = null!;

    public string? FrontFace => ImageUris?.Normal
        ?? CardFaces?[0]?.ImageUris?.Normal;

    public string? BackFace => CardFaces?[1]?.ImageUris?.Normal;

    public override bool Equals(object? obj) =>
        obj is Card card && Id.Equals(card.Id);

    public bool Equals(Card? other) =>
        other != null && Id.Equals(other.Id);

    public override int GetHashCode() =>
        CollectorNumber.GetHashCode() ^ Name.GetHashCode();
}
