namespace Scrywatch.MergeService.Scryfall;

/// <summary>
/// System.Text.Json.JsonSerializer.DeserializeAsync<TCLass> returns TCLass?,
/// which is annoying since it's forcing us to do a null check.
/// Null is only returned if TClass in null though.
/// If we don't try to deserialize null object null won't get returned,
/// in conclusion this wraper method saves us from pointless null checking.
/// </summary>
public static class JsonSerializer
{
    public async static Task<TClass> DeserializeAsync<TClass>(
        Stream stream, CancellationToken cancellationToken)
        where TClass : class
        => await System.Text.Json.JsonSerializer.DeserializeAsync<TClass>(
            stream, cancellationToken: cancellationToken)
        ?? throw new ArgumentNullException(nameof(stream));
}
