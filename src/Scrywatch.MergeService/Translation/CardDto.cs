using Scrywatch.Core.ValueObjects;
using Scrywatch.Persistence.Data;

namespace Scrywatch.MergeService.Translation;

public sealed record CardDto(
    Guid Id,
    string Name,
    string Rarity,
    string SetCode,
    string CollectorNumber,
    string? FrontFaceUrl,
    string? BackFaceUrl,
    IEnumerable<Finish> Finishes,
    IEnumerable<PriceData> Prices);
