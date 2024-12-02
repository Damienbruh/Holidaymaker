using System.Data.Common;
using Npgsql;
namespace app.Queries;

public class HotellQueries
{
    private NpgsqlDataSource _database;
    
    List<string?> _streetNames;
    List<string?> _postalCodes;
    List<string?> _cities;
    List<string?> _region;
    List<string?> _country;
    List<string?> _distancesToSkiSlope;
    List<string?> _distancesToTownCenter;
    List<string?> _ratings;

    public HotellQueries(NpgsqlDataSource database)
    {
        _database = database;
        _streetNames = new List<string?>();   
        _postalCodes = new List<string?>();
        _cities = new List<string?>();
        _region = new List<string?>();
        _country = new List<string?>();
        _distancesToSkiSlope = new List<string?>();
        _distancesToTownCenter = new List<string?>();
        _ratings = new List<string?>();
    }

    public async void SpecificHotell()
    {
        try
        {
            await using (var cmd = _database.CreateCommand("SELECT * FROM hotels")) // Skapa vårt kommand/query
                await using (var reader = await cmd.ExecuteReaderAsync()) // Kör vår kommando/query och inväntar resultatet.
                    while ( await reader.ReadAsync()) // Läser av 1 rad/objekt i taget ifrån resultatet och kommer avsluta loopen när det inte finns fler rader att läsa. 
                    {
                        Dictionary<int, List<string>> columnMappings = new Dictionary<int, List<string>>()
                        {
                            { 1, _streetNames },
                            { 2, _postalCodes },
                            { 3, _cities },
                            { 4, _region },
                            { 5, _country },
                            { 6, _distancesToSkiSlope },
                            { 7, _distancesToTownCenter },
                            { 8, _ratings }
                        };
                        while (reader.Read())
                        {
                            foreach (var entry in columnMappings)
                            {
                                int columnIndex = entry.Key;
                                List<string> targetList = entry.Value;
                                
                                if (reader.IsDBNull(columnIndex))
                                {
                                    targetList.Add("null");
                                }
                                else
                                {
                                    targetList.Add(reader.GetValue(columnIndex).ToString());
                                }
                            }
                        }
                        /*if (reader.IsDBNull(2))
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
                        }*/
                    }

            _postalCodes.ForEach(delegate(string postalcode)
            {
                Console.WriteLine(postalcode);
            });
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        
    }
}