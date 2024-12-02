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
    

    public async Task<List<Customer>> AllCustomers()
    {
        List<Customer> customers = new List<Customer>();
        await using (var cmd = _database.CreateCommand("SELECT * FROM customers")) // Skapa vårt kommand/query
        await using (var reader = await cmd.ExecuteReaderAsync()) // Kör vår kommando/query och inväntar resultatet.
            while
                (await reader
                    .ReadAsync()) // Läser av 1 rad/objekt i taget ifrån resultatet och kommer avsluta loopen när det inte finns fler rader att läsa. 
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