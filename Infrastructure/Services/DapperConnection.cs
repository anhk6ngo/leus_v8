using System.Data;
using Dapper;
using Npgsql;

namespace LeUs.Infrastructure.Services;

public class DapperConnection(IConfiguration configuration) : IDapperConnection
{
    public async Task ExecuteAsync(string funName, DynamicParameters parameters)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        await using var connection = new NpgsqlConnection(connectionString);
        await connection.ExecuteAsync(funName, parameters, commandType: CommandType.StoredProcedure);
    }

    public async Task<double> GetBalanceAsync(Guid id)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@id", id);
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        await using var connection = new NpgsqlConnection(connectionString);
        return connection.ExecuteScalar<double>("SELECT getuserbalance(@user_id)", parameters);
    }
}