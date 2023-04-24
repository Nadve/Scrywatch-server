namespace Scrywatch.Core.Interests;

public interface IInterestRepository
{
    Task<bool> Exists(InterestDto interest);

    Task<InterestDto?> FindById(int id);

    Task<IEnumerable<Interest>> Get(string userId);

    Task<Interest> Create(InterestDto interest);

    Task Delete(int id);

    Task Update(int id, int goal);
}
