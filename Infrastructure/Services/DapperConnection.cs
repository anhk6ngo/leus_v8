using System.Data;
using Dapper;
using Npgsql;

namespace LeUs.Infrastructure.Services;

public class DapperConnection(IConfiguration configuration): IDapperConnection
{
    public async Task ExecuteAsync(string funName, DynamicParameters parameters)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        await using var connection = new NpgsqlConnection(connectionString);
        await connection.ExecuteAsync(funName, parameters, commandType: CommandType.StoredProcedure);
    }
}