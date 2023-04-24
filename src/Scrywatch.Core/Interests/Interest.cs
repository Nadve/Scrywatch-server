using Scrywatch.Core.Cards;

namespace Scrywatch.Core.Interests;

public sealed class Interest
{
    public int Id { get; init; }

    public Intention Intention { get; init; } = null!;

    public int Goal { get; init; }

    public Card Card { get; init; } = null!;
}
