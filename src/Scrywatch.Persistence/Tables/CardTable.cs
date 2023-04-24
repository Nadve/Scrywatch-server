namespace Scrywatch.MergeService.Database.Tables;

public sealed class CardTable : ScrywatchTable
{
    public CardTable() : base("CardType", 1) { }

    public override IDictionary<string, Type> Columns => new Dictionary<string, Type>
    {
        { "Guid", typeof(string) },
        { "Name", typeof(string) },
        { "SetCode", typeof(string) },
        { "Rarity", typeof(string) },
        { "CollectiorNumber", typeof(string) }
    };
}
