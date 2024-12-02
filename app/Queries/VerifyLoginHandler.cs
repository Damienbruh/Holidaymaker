using System.Data;
using Npgsql;
namespace app.Queries;

public class VerifyLoginHandler
{
    private NpgsqlDataSource _database;

    public VerifyLoginHandler(NpgsqlDataSource database)
    {
        _database = database;
    }

    public async Task<bool> VerifyLogin(string username, string password)
    {
        Boolean verifiedLogin = false;
        await using (var cmd = _database.CreateCommand($"SELECT verify_login('{username}', '{password}')"))
        await using (var reader = await cmd.ExecuteReaderAsync())
            while
                (await reader
                    .ReadAsync())
            {
                verifiedLogin = reader.GetBoolean(0);
            }
        return verifiedLogin;
    }
}