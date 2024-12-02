using Npgsql;
namespace app.Queries;

public class VerifyLoginHandler
{
    private NpgsqlDataSource _database;

    public VerifyLoginHandler(NpgsqlDataSource database)
    {
        _database = database;
    }

    public async Task VerifyLogin(string username, string password)
    {
        await using (var cmd = _database.CreateCommand("SELECT * FROM customers")) 
        await using (var reader = await cmd.ExecuteReaderAsync())
           
}