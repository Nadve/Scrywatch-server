using Scrywatch.Core.Cards;
using Scrywatch.Core.Interests;
using Scrywatch.Persistence;
using Scrywatch.Persistence.Data;

namespace Scrywatch.Infrastructure.Interests;

public sealed class InterestRepository : IInterestRepository
{
    private readonly IDbConnection _db;
    private readonly ICardRepository _cardRepository;

    public InterestRepository(IDbConnection db, ICardRepository cardRepository)
    {
        _db = db;
        _cardRepository = cardRepository;
    }

    public async Task<InterestDto?> FindById(int id)
    {
        return await _db.QueryFirstOrDefaultAsync<InterestDto>(
            StoredProcedure.FindInterestById,
            new { id });
    }

    public async Task<IEnumerable<Interest>> Get(string userId)
    {
        var interestData = await _db.QueryAsync<InterestData>(
            StoredProcedure.GetInterest,
            new { userId });

        List<Interest> interests = new();
        foreach (var interest in interestData)
        {
            interests.Add(new Interest
            {
                Id = interest.Id,
                Intention = interest.Intention,
                Goal = interest.Goal,
                Card = await _cardRepository.GetCard(
                    interest.CardId, interest.Finish, interest.Currency)
            });
        }

        return interests;
    }

    public async Task<Interest> Create(InterestDto interest)
    {
        await _db.ExecuteAsync(StoredProcedure.CreateInterest, interest.AsDynamic);

        var interestData = await Find(interest)
            ?? throw new ApplicationException("Failed to create interest, but how and why?");

        return new Interest
        {
            Id = interestData.Id,
            Intention = interestData.Intention,
            Goal = interestData.Goal,
            Card = await _cardRepository.GetCard(
                interestData.CardId, interestData.Finish, interestData.Currency)
        };
    }

    public async Task Delete(int id)
    {
        await _db.ExecuteAsync(StoredProcedure.DeleteInterest, new { id });
    }

    public async Task Update(int id, int goal)
    {
        await _db.ExecuteAsync(StoredProcedure.UpdateInterest, new { id, goal });
    }

    public async Task<bool> Exists(InterestDto interest)
    {
        return await Find(interest) != null;
    }

    private async Task<InterestData?> Find(InterestDto interest)
    {
        return await _db.QueryFirstOrDefaultAsync<InterestData>(
            StoredProcedure.FindInterest,
            interest.AsDynamic);
    }
}
