namespace Scrywatch.MergeService.Database.Tables;

public sealed class PriceTable : ScrywatchTable
{
    public PriceTable() : base("CardPriceType", 3) { }

    public override IDictionary<string, Type> Columns => new Dictionary<string, Type>
    {
        { "CardGuid", typeof(string) },
        { "Finish", typeof(string) },
        { "Currency", typeof(string) },
        { "Date", typeof(DateTime) },
        { "Value", typeof(float) }
    };
}
