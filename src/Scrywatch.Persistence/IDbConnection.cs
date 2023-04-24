namespace Scrywatch.Persistence;

public interface IDbConnection
{
    Task<IEnumerable<T>> QueryAsync<T>(string storedProcedure, string connectionId = "Default");

    Task<IEnumerable<T>> QueryAsync<T>(string storedProcedure, object parameters, string connectionId = "Default");

    Task<T> QueryFirstOrDefaultAsync<T>(string storedProcedure, object parameters, string connectionId = "Default");

    Task<int> ExecuteAsync(string storedProcedure, object parameters, string connectionId = "Default");
}