using Scrywatch.Core.Cards;
using Scrywatch.Core.ValueObjects;
using Scrywatch.Persistence;
using Scrywatch.Persistence.Data;

namespace Scrywatch.Infrastructure.Cards;

public sealed class CardRepository : ICardRepository
{
    private readonly IDbConnection _db;

    public CardRepository(IDbConnection db) => _db = db;

    public async Task<Card> GetCard(int id, Finish finish, Currency currency)
    {
        var cardData = await _db.QueryFirstOrDefaultAsync<CardData>(StoredProcedure.GetCard, new { id });
        var prices = await GetCardPrices(id, finish, currency);
        return new Card
        {
            Id = cardData.Id,
            Name = cardData.Name,
            Language = cardData.Language,
            Rarity = cardData.Rarity,
            Set = new Set
            {
                Name = cardData.Set,
                Code = cardData.SetCode,
                Svg = cardData.SetSvg
            },
            CollectorNumber = cardData.CollectorNumber,
            Face = new Face
            {
                Front = cardData.FrontFaceUrl,
                Back = cardData.BackFaceUrl
            },
            Finishes = new[] { finish },
            Prices = new[] { new Price { Finish = finish, Currency = currency, DateValuePairs = prices } }
        };
    }

    public async Task<IEnumerable<string>> GetCardNames() => await
        _db.QueryAsync<string>(StoredProcedure.GetCardNames);

    public async Task<IEnumerable<Card>> GetCardsByName(string name)
    {
        var cardData = await _db.QueryAsync<CardData>(StoredProcedure.GetCards, new { name });

        List<Card> cards = new();
        foreach (var card in cardData)
        {
            var finishes = await GetCardFinishes(card.Id);
            var prices = await GetAllCardPrices(card.Id);
            cards.Add(new Card
            {
                Id = card.Id,
                Name = card.Name,
                Language = card.Language,
                Rarity = card.Rarity,
                Set = new Set
                {
                    Name = card.Set,
                    Code = card.SetCode,
                    Svg = card.SetSvg
                },
                CollectorNumber = card.CollectorNumber,
                Face = new Face
                {
                    Front = card.FrontFaceUrl,
                    Back = card.BackFaceUrl
                },
                Finishes = finishes,
                Prices = prices
            });
        }

        return cards;
    }

    private async Task<IEnumerable<Finish>> GetCardFinishes(int id) =>
        await _db.QueryAsync<Finish>(StoredProcedure.GetCardFinishes, new { id });

    private async Task<IEnumerable<Price>> GetAllCardPrices(int id)
    {
        var cardPriceData = await _db.QueryAsync<PriceData>(StoredProcedure.GetAllCardPrices, new { id });
        return new Price[]
        {
            cardPriceData.Filter(Finish.NonFoil, Currency.Usd),
            cardPriceData.Filter(Finish.Foil, Currency.Usd),
            cardPriceData.Filter(Finish.Etched, Currency.Usd),
            cardPriceData.Filter(Finish.NonFoil, Currency.Eur),
            cardPriceData.Filter(Finish.Foil, Currency.Eur),
            cardPriceData.Filter(Finish.NonFoil, Currency.Tix)
        }.Where(p => p.DateValuePairs.Count > 1);
    }

    private async Task<IDictionary<DateOnly, float>> GetCardPrices(int id, Finish finish, Currency currency)
    {
        var prices = await _db.QueryAsync<DateValuePair>(StoredProcedure.GetCardPrices, new { id, finish, currency });
        return prices.ToDictionary(p => DateOnly.FromDateTime(p.Date), p => p.Value);
    }
}
