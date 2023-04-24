using Scrywatch.Core.ValueObjects;

namespace Scrywatch.Persistence.Data;

public sealed record PriceData
{
    public Finish Finish { get; init; } = null!;

    public Currency Currency { get; init; } = null!;

    public DateTime Date { get; init; }

    public float Value { get; init; }

    public PriceData() { }

    public PriceData(Finish finish, Currency currency, DateTime date, float value)
    {
        Finish = finish;
        Currency = currency;
        Date = date;
        Value = value;
    }

}
