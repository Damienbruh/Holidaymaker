using Npgsql;
using app.Queries.TableObjects;
namespace app.Queries;

public class HotellQueries
{
    private NpgsqlDataSource _database;
    
    public HotellQueries(NpgsqlDataSource database)
    {
        _database = database;
    }
    
    
    public async Task<List<Hotel>> AllHotels()
    {
        List<Hotel> hotels = new List<Hotel>();
        await using (var cmd = _database.CreateCommand("SELECT * FROM hotels")) // Skapa vårt kommand/query
        await using (var reader = await cmd.ExecuteReaderAsync()) // Kör vår kommando/query och inväntar resultatet.
            while
                (await reader
                    .ReadAsync()) // Läser av 1 rad/objekt i taget ifrån resultatet och kommer avsluta loopen när det inte finns fler rader att läsa. 
            {
                hotels.Add(new Hotel
                {
                    Id = reader.GetInt32(0),
                    StreetName = reader.IsDBNull(1) ? null : reader.GetString(1),
                    PostalCode = reader.IsDBNull(2) ? null : reader.GetString(2),
                    City = reader.IsDBNull(3) ? null : reader.GetString(3),
                    Region = reader.IsDBNull(4) ? null : reader.GetString(4),
                    Country = reader.IsDBNull(5) ? null : reader.GetString(5),
                    DistanceToSkiSlope = reader.IsDBNull(6) ? null : reader.GetString(6),
                    DistanceToTownCenter = reader.IsDBNull(7) ? null : reader.GetString(7),
                    Rating = reader.GetInt32(8)
                });
            }

        Console.WriteLine("done");
        return hotels;
    }
}