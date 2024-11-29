using Npgsql;
namespace app.Queries;

public class CustomerQueries
{
    private NpgsqlDataSource _database;

    public CustomerQueries(NpgsqlDataSource database)
    {
        _database = database;
    }
    

    public async Task AllCustomers()
    {
        try
        {
            await using (var cmd = _database.CreateCommand("SELECT * FROM customers")) 
            await using (var reader = await cmd.ExecuteReaderAsync()) 
                while
                    (await reader
                        .ReadAsync())  
                {
                    Console.WriteLine($"Id: {reader.GetInt32(0)}," +
                                      $"Name: {reader.GetString(1)}," +
                                      $"email: {reader.GetString(2)}," +
                                      $"phone_number: {reader.GetString(3)}," +
                                      $"birthyear: {reader.GetInt32(4)}");
                }

            Console.WriteLine("done");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
            Console.WriteLine(ex.StackTrace);
        }
    }
}