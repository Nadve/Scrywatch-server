namespace Scrywatch.MergeService.Database.Tables;

public sealed class FinishTable : ScrywatchTable
{
    public FinishTable() : base("CardFinishType", 4) { }

    public override IDictionary<string, Type> Columns => new Dictionary<string, Type>
    {
        { "CardGuid", typeof(string) },
        { "Finish", typeof(string) }
    };
}
