using Npgsql;
namespace app.Queries;

public class HotellQueries
{
    private NpgsqlDataSource _database;
    
    

    public HotellQueries(NpgsqlDataSource database)
    {
        _database = database;
        SpecificHotell();
        
        
    }

    public async void SpecificHotell()
    {
        try
        {
            await using (var cmd = _database.CreateCommand("SELECT * FROM hotels")) // Skapa vårt kommand/query
                await using (var reader = await cmd.ExecuteReaderAsync()) // Kör vår kommando/query och inväntar resultatet.
                    while ( await reader.ReadAsync()) // Läser av 1 rad/objekt i taget ifrån resultatet och kommer avsluta loopen när det inte finns fler rader att läsa. 
                    {
                        Console.WriteLine($"hotel_id: {reader.GetInt32(0)}," +
                                          $"street_name: {reader.GetString(1)}," +
                                          $"postal_code: {reader.GetString(2)}," +
                                          $"city: {reader.GetString(3)}," +
                                          $"region: {reader.GetInt32(4)}" +
                                          $"country: {reader.GetString(3)}," +
                                          $"distance_to_ski_slope: {reader.GetInt32(3)},");
                                            
                    }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        
    }
}