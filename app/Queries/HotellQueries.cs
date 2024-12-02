using Npgsql;
namespace app.Queries;

public class HotellQueries
{
    private NpgsqlDataSource _database;
    
    List<string?> _postalCodes;
    List<string?> _region;

    public HotellQueries(NpgsqlDataSource database)
    {
        _database = database;
        _postalCodes = new List<string?>();
        _region = new List<string?>();
    }

    public async void SpecificHotell()
    {
        try
        {
            await using (var cmd = _database.CreateCommand("SELECT * FROM hotels")) // Skapa vårt kommand/query
                await using (var reader = await cmd.ExecuteReaderAsync()) // Kör vår kommando/query och inväntar resultatet.
                    while ( await reader.ReadAsync()) // Läser av 1 rad/objekt i taget ifrån resultatet och kommer avsluta loopen när det inte finns fler rader att läsa. 
                    {
                        if (reader.IsDBNull(2))
                        {
                            _postalCodes.Add("null");
                        }
                        else
                        {
                           _postalCodes.Add(reader.GetString(2)); 
                        }
                        if (reader.IsDBNull(4))
                        {
                            _region.Add("null");
                        }
                        else
                        {
                            _region.Add(reader.GetString(4)); 
                        }
                        /*Console.WriteLine($"hotel_id: {reader.GetInt32(0)}," +
                                          $"street_name: {reader.GetString(1)}," +
                                          $"postal_code: {reader.GetString(2)}," +
                                          $"city: {reader.GetString(3)}," +
                                          $"region: {reader.GetInt32(4)}" +
                                          $"country: {reader.GetString(3)}," +
                                          $"distance_to_ski_slope: {reader.GetInt32(3)},");*/
                    }

            _postalCodes.ForEach(delegate(string postalcode)
            {
                Console.WriteLine(postalcode);
            });
            /*foreach (var VARIABLE in _postalCodes)
            {
                Console.WriteLine(VARIABLE);
                /*if (VARIABLE == null){
                    Console.WriteLine(VARIABLE);
                }#1#

            }*/
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        
    }
}