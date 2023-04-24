namespace Scrywatch.MergeService.Database.Tables;

public sealed class NameTable : ScrywatchTable
{
    public NameTable() : base("NameType", 6) { }

    public override IDictionary<string, Type> Columns => new Dictionary<string, Type>
    {
        { "Value", typeof(string) }
    };
}
