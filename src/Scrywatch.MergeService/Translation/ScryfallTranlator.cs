using Scrywatch.Core.ValueObjects;
using Scrywatch.MergeService.Scryfall;
using Scrywatch.Persistence.Data;

namespace Scrywatch.MergeService.Translation;

public static class ScryfallTranlator
{
    public static IEnumerable<CardDto> Translate(this IEnumerable<Card> cards, DateTime date)
    {
        List<CardDto> translatedCards = new();
        foreach (var card in cards)
        {
            translatedCards.Add(new CardDto(
                card.Id,
                card.Name,
                card.Rarity,
                card.SetCode,
                card.CollectorNumber,
                card.FrontFace,
                card.BackFace,
                GetCardFinishes(card),
                GetPrices(card, date)));
        }

        return translatedCards;
    }

    private static IEnumerable<Finish> GetCardFinishes(Card card)
    {
        List<Finish> finishes = new();

        if (card.Finishes.Contains(Finish.NonFoil.Name, StringComparer.OrdinalIgnoreCase))
            finishes.Add(Finish.NonFoil);

        if (card.Finishes.Contains(Finish.Foil.Name, StringComparer.OrdinalIgnoreCase))
            finishes.Add(Finish.Foil);

        if (card.Finishes.Contains(Finish.Etched.Name, StringComparer.OrdinalIgnoreCase))
            finishes.Add(Finish.Etched);

        if (card.Finishes.Contains(Finish.Glossy.Name, StringComparer.OrdinalIgnoreCase))
            finishes.Add(Finish.Glossy);

        return finishes;
    }

    private static IEnumerable<PriceData> GetPrices(Card card, DateTime date)
        => new List<PriceData>
    {
        new PriceData(
            Finish.NonFoil,
            Currency.Usd,
            date,
            NullToZeroParse(card.Prices.Usd)),
        new PriceData(
            Finish.Foil,
            Currency.Usd,
            date,
            NullToZeroParse(card.Prices.UsdFoil)),
        new PriceData(
            Finish.Etched,
            Currency.Usd,
            date,
            NullToZeroParse(card.Prices.UsdEtched)),
        new PriceData(
            Finish.NonFoil,
            Currency.Eur,
            date,
            NullToZeroParse(card.Prices.Eur)),
        new PriceData(
            Finish.Foil,
            Currency.Eur,
            date,
            NullToZeroParse(card.Prices.EurFoil)),
        new PriceData(
            Finish.NonFoil,
            Currency.Tix,
            date,
            NullToZeroParse(card.Prices.Tix))
    };

    public static float NullToZeroParse(string? value) =>
        value is null ? 0 : float.Parse(value);
}
