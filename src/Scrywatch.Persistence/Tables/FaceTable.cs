namespace Scrywatch.MergeService.Database.Tables;

public sealed class FaceTable : ScrywatchTable
{
    public FaceTable() : base("CardFaceType", 5) { }

    public override IDictionary<string, Type> Columns => new Dictionary<string, Type>
    {
        { "CardGuid", typeof(string) },
        { "Face", typeof(string) },
        { "Url", typeof(string) }
    };
}
