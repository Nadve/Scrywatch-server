using Scrywatch.Core.ValueObjects;

namespace Scrywatch.Core.Cards;

public interface ICardRepository
{
    Task<IEnumerable<string>> GetCardNames();

    Task<IEnumerable<Card>> GetCardsByName(string name);

    Task<Card> GetCard(int id, Finish finish, Currency currency);
}
