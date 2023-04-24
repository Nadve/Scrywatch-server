namespace Scrywatch.MergeService.IO;
public class MergeResult
{
    public int Delete;
    public int Insert;
    public int Update;

    public MergeResult(IEnumerable<MergeOutput> merges)
    {
        Insert = merges.Where(m => m.Action.Equals("INSERT"))
            .FirstOrDefault()?.Count ?? 0;

        Delete = merges.Where(m => m.Action.Equals("DELETE"))
            .FirstOrDefault()?.Count ?? 0;

        Update = merges.Where(m => m.Action.Equals("UPDATE"))
            .FirstOrDefault()?.Count ?? 0;
    }

    private string DeleteStr => Delete != 0
        ? $"{Delete} rows deleted"
        : string.Empty;

    private string InsertStr => Insert != 0
        ? $"{Insert} rows inserted"
        : string.Empty;

    private string UpdateStr => Update != 0
        ? $"{Update} rows updated"
        : string.Empty;

    public override string ToString() =>
        $"\t{InsertStr} {UpdateStr} {DeleteStr}";
}
