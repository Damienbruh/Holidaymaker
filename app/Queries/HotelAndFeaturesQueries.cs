using Npgsql;
using app.Queries.TableObjects;
namespace app.Queries;

public class HotelAndFeaturesQueries
{
    private NpgsqlDataSource _database;
    
    public HotelAndFeaturesQueries(NpgsqlDataSource database)
    {
        _database = database;
    }
    
    
    public async Task<List<HotelAndFeatures>> AllHotels()
    {
        List<HotelAndFeatures> hotels = new List<HotelAndFeatures>();
        await using (var cmd = _database.CreateCommand("SELECT * FROM feature_and_hotel_view")) // Skapa vårt kommand/query
        await using (var reader = await cmd.ExecuteReaderAsync()) // Kör vår kommando/query och inväntar resultatet.
            while
                (await reader
                    .ReadAsync()) // Läser av 1 rad/objekt i taget ifrån resultatet och kommer avsluta loopen när det inte finns fler rader att läsa. 
            {
                hotels.Add(new HotelAndFeatures
                {
                    HotelId = reader.GetInt32(0),
                    StreetName = reader.IsDBNull(1) ? null : reader.GetString(1),
                    PostalCode = reader.IsDBNull(2) ? null : reader.GetString(2),
                    City = reader.IsDBNull(3) ? null : reader.GetString(3),
                    Region = reader.IsDBNull(4) ? null : reader.GetString(4),
                    Country = reader.IsDBNull(5) ? null : reader.GetString(5),
                    DistanceToSkiSlope = reader.IsDBNull(6) ? null : reader.GetString(6),
                    DistanceToTownCenter = reader.IsDBNull(7) ? null : reader.GetString(7),
                    Rating = reader.GetInt32(8),
                    FeatureId = reader.GetInt32(9),
                    Feature = reader.IsDBNull(10) ? null : reader.GetString(10),
                });
            }

        // foreach (var hotel in hotels)
        // {
        //     Console.Write(hotel.HotelId + "  |  ");
        //     Console.Write(hotel.StreetName  + "  |  ");
        //     Console.Write(hotel.City  + "  |  ");
        //     Console.Write(hotel.Country  + "  |  ");
        //     Console.Write(hotel.Rating  + "  |  ");
        //     Console.Write(hotel.FeatureId + "  |  ");
        //     Console.WriteLine(hotel.Feature + "  |  ");
        // }

        Console.ReadLine();
        return hotels;
    }

    
    public async Task<List<HotelAndFeatures>> SearchBy(string specificColumn, string searchValue)
    {
        //Skapa lista av kolumner som finns innuti hotels table
        string[] acceptableColumns =
        {
            "street_name", "postal_code", "city", "region", "country", "distance_to_ski_slope",
            "distance_to_town_center", "rating", "hotel_features_id", "feature"
        };
        //Ifall input inte stämmer överens med någon av kolumnerna i listan, ge fel medelande.
        if (!acceptableColumns.Contains(specificColumn.ToLower()))
        {
            throw new ArgumentException("Invalid column name!");
        }
        // Gör en ny lista som håller i de nya hotellerna som matchar med queryn
        List<HotelAndFeatures> hotels = new List<HotelAndFeatures>();
        
        // Definera queryn
        var searchByQuery = $"SELECT * FROM feature_and_hotel_view WHERE {specificColumn} ILIKE @searchValue";
        //Skapa command till databas som använder Queryn
        await using (var cmd = _database.CreateCommand(searchByQuery))
        {
            //Skicka in parameter åt SQL commandet genom att skapa en ny parameter "SearchValue och sätt in user input inuti
            cmd.Parameters.Add(new NpgsqlParameter("searchValue", $"%{searchValue}%"));
            //Kör queryn och hämtar tillbaka data från databasen
            await using (var reader = await cmd.ExecuteReaderAsync())
            {
                // Läser av 1 rad/objekt i taget ifrån resultatet och kommer avsluta loopen när det inte finns fler rader att läsa. 
                while (await reader.ReadAsync())
                {
                    hotels.Add(new HotelAndFeatures
                    {
                        HotelId = reader.GetInt32(0),
                        StreetName = reader.IsDBNull(1) ? null : reader.GetString(1), 
                        PostalCode = reader.IsDBNull(2) ? null : reader.GetString(2), 
                        City = reader.IsDBNull(3) ? null : reader.GetString(3),
                        Region = reader.IsDBNull(4) ? null : reader.GetString(4),
                        Country = reader.IsDBNull(5) ? null : reader.GetString(5),
                        DistanceToSkiSlope = reader.IsDBNull(6) ? null : reader.GetString(6),
                        DistanceToTownCenter = reader.IsDBNull(7) ? null : reader.GetString(7),
                        Rating = reader.GetInt32(8),
                        FeatureId = reader.GetInt32(9),
                        Feature = reader.IsDBNull(10) ? null : reader.GetString(10)
                    });
                }
            }
        }
        // foreach (var hotel in hotels)
        // {
        //     Console.Write(hotel.HotelId + "  |  ");
        //     Console.Write(hotel.StreetName  + "  |  ");
        //     Console.Write(hotel.City  + "  |  ");
        //     Console.Write(hotel.Country  + "  |  ");
        //     Console.Write(hotel.Rating  + "  |  ");
        //     Console.Write(hotel.FeatureId + "  |  ");
        //     Console.WriteLine(hotel.Feature + "  |  ");
        // }

        
        Console.WriteLine("done");
        Console.ReadLine();
        return hotels;
    }
}