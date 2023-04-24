namespace Scrywatch.MergeService.Database.Tables;

public sealed class SetTable : ScrywatchTable
{
    public SetTable() : base("SetType", 2) { }

    public override IDictionary<string, Type> Columns => new Dictionary<string, Type>
    {
        { "Name", typeof(string) },
        { "Code", typeof(string) },
        { "Svg", typeof(string) }
    };
}
