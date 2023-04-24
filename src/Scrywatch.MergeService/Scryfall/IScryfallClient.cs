namespace Scrywatch.MergeService.Scryfall
{
    public interface IScryfallClient
    {
        Task<Bulk> GetBulkData(CancellationToken cancellationToken);
        Task<IEnumerable<Card>> GetCardsAsync(CancellationToken cancellationToken);
        Task<IEnumerable<Set>> GetSetsAsync(CancellationToken cancellationToken);
    }
}