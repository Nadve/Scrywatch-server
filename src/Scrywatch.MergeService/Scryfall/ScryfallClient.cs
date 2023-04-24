namespace Scrywatch.MergeService.Scryfall;

public class ScryfallClient : IScryfallClient
{
    private readonly HttpClient _httpClient;

    public ScryfallClient(HttpClient httpClient)
        => _httpClient = httpClient;

    public async Task<Bulk> GetBulkData(CancellationToken cancellationToken)
    {
        using Stream bulkStream = await GetStreamAsync(ScryfallEndpoint.Bulk, cancellationToken);
        return await JsonSerializer.DeserializeAsync<Bulk>(bulkStream, cancellationToken);
    }

    public async Task<IEnumerable<Card>> GetCardsAsync(CancellationToken cancellationToken)
    {
        using Stream cardStream = await GetCardStreamAsync(cancellationToken);
        return await JsonSerializer.DeserializeAsync<IEnumerable<Card>>(cardStream, cancellationToken);
    }

    public async Task<IEnumerable<Set>> GetSetsAsync(CancellationToken cancellationToken)
    {
        using Stream setStream = await GetStreamAsync(ScryfallEndpoint.Sets, cancellationToken);
        return (await JsonSerializer.DeserializeAsync<CardSets>(setStream, cancellationToken)).Data;
    }

    private async Task<Stream> GetCardStreamAsync(CancellationToken cancellationToken)
    {
        Bulk bulk = await GetBulkData(cancellationToken);
        return await GetStreamAsync(bulk.AllCardsUri, cancellationToken);
    }

    private async Task<Stream> GetStreamAsync(string url, CancellationToken cancellationToken)
        => await _httpClient.GetStreamAsync(url, cancellationToken)
        ?? throw new ArgumentNullException(nameof(url));
}
