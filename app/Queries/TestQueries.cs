using System.IO.Pipelines;
using Npgsql;
using app.Queries.TableObjects;
namespace app.Queries;

public class TestQueries
{
    private NpgsqlDataSource _database;
    
    public TestQueries(NpgsqlDataSource database)
    {
        _database = database;
    }


    public async Task GetAddons()
    {
        await using (var cmd = _database.CreateCommand("SELECT * FROM addons"))
        await using (var reader = await cmd.ExecuteReaderAsync())
            while (await reader.ReadAsync())
            {
                Console.WriteLine($"addon id: {reader.GetInt32(0)}" +
                                  $"addon: {reader.GetString(1)}" +
                                  $"price: {reader.GetInt32(2)}");
            }
    }

    public async Task<List<Customer>> TestQuery()
    {
        List<Customer> customers = new List<Customer>();
        await using (var cmd = _database.CreateCommand("SELECT * FROM customers"))
        await using (var reader = await cmd.ExecuteReaderAsync())
            while
                (await reader
                    .ReadAsync()) 
            {
                customers.Add(new Customer
                {
                    Id = reader.GetInt32(0),
                    Name = reader.IsDBNull(1) ? null : reader.GetString(1),
                    Email = reader.IsDBNull(2) ? null : reader.GetString(2),
                    PhoneNumber = reader.IsDBNull(3) ? null : reader.GetString(3),
                    Birthyear = reader.GetInt32(4)
                });
            }

        Console.WriteLine("done");
        return customers;
    }
}