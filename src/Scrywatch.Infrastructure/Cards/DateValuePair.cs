namespace Scrywatch.Infrastructure.Cards;

public sealed record DateValuePair
{
    public DateTime Date { get; init; }

    public float Value { get; init; }
}
