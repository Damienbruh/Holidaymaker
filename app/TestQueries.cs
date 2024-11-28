using Npgsql;
namespace app;

public class TestQueries
{
    private NpgsqlDataSource _database;

    public TestQueries(NpgsqlDataSource database)
    {
        _database = database;
    }
    

    public async void AllCustomers()
    {
        await using (var cmd = _database.CreateCommand("SELECT * FROM customers")) // Skapa vårt kommand/query
        await using (var reader = await cmd.ExecuteReaderAsync()) // Kör vår kommando/query och inväntar resultatet.
            while ( await reader.ReadAsync()) // Läser av 1 rad/objekt i taget ifrån resultatet och kommer avsluta loopen när det inte finns fler rader att läsa. 
            {
                Console.WriteLine($"Id: {reader.GetInt32(0)}," +
                                  $"Name: {reader.GetString(1)}," +
                                  $"email: {reader.GetString(2)}," +
                                  $"phone_number: {reader.GetString(3)}," +
                                  $"birthyear: {reader.GetInt32(4)}");
            }
    }
}