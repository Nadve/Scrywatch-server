using Scrywatch.Core.Cards;
using Scrywatch.Core.ValueObjects;
using Scrywatch.Persistence.Data;

namespace Scrywatch.Infrastructure.Cards;

public static class PriceDataExtension
{
    public static Price Filter(this IEnumerable<PriceData> prices,
        Finish finish, Currency currency) => new()
        {
            Finish = finish,
            Currency = currency,
            DateValuePairs = prices.Where(p =>
                p.Currency == currency &&
                p.Finish == finish)
                .ToDictionary(
                    p => DateOnly.FromDateTime(p.Date),
                    p => p.Value)
        };
}
