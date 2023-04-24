using Ardalis.SmartEnum;
using System.Data;

namespace Scrywatch.MergeService.Database.Tables;

public abstract class ScrywatchTable : SmartEnum<ScrywatchTable>
{
    public static readonly ScrywatchTable Card = new CardTable();
    public static readonly ScrywatchTable Set = new SetTable();
    public static readonly ScrywatchTable CardPrice = new PriceTable();
    public static readonly ScrywatchTable CardFinish = new FinishTable();
    public static readonly ScrywatchTable CardFace = new FaceTable();
    public static readonly ScrywatchTable CardName = new NameTable();

    protected ScrywatchTable(string name, int value) : base(name, value) { }

    public abstract IDictionary<string, Type> Columns { get; }

    public DataTable Create()
    {
        var dataTable = new DataTable(Name);
        foreach (var column in Columns)
        {
            dataTable.Columns.Add(column.Key, column.Value);
        }

        return dataTable;
    }
}
