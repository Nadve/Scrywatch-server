using Dapper;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Scrywatch.Infrastructure;

public sealed class DbConnection : Persistence.IDbConnection
{
    private readonly IConfiguration _config;

    public DbConnection(IConfiguration configuration) =>
        _config = configuration;

    public async Task<IEnumerable<T>> QueryAsync<T>(string storedProcedure, string connectionId = "Default")
        => await QueryAsync<T>(storedProcedure, new { }, connectionId);

    public async Task<IEnumerable<T>> QueryAsync<T>(string storedProcedure, object parameters,
        string connectionId = "Default")
    {
        using var connection = new SqlConnection(_config.GetConnectionString(connectionId));
        return await connection.QueryAsync<T>(storedProcedure, parameters,
            commandType: CommandType.StoredProcedure);
    }

    public async Task<T> QueryFirstOrDefaultAsync<T>(string storedProcedure, object parameters,
        string connectionId = "Default")
    {
        using var connection = new SqlConnection(_config.GetConnectionString(connectionId));
        return await connection.QueryFirstOrDefaultAsync<T>(storedProcedure, parameters,
            commandType: CommandType.StoredProcedure);
    }

    public async Task<int> ExecuteAsync(string storedProcedure, object parameters,
        string connectionId = "Default")
    {
        using var connection = new SqlConnection(_config.GetConnectionString(connectionId));
        return await connection.ExecuteAsync(storedProcedure, parameters,
            commandType: CommandType.StoredProcedure);
    }
}
