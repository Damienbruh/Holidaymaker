using Npgsql;
namespace app.Queries;

public class BookingQueries
{
    private NpgsqlDataSource _database;



    public BookingQueries(NpgsqlDataSource database)
    {
        _database = database;
    }

    
    public async Task<int> InsertBooking(DateTime startDate, DateTime endDate)
    {
 

        var query = @"INSERT INTO bookings (start_date, end_date, status)
                  VALUES ($1, $2, 'active')
                  RETURNING bookings_id";
        await using (var cmd = _database.CreateCommand(query))
        {
            cmd.Parameters.AddWithValue(startDate);
            cmd.Parameters.AddWithValue(endDate);
            
            var newId = await cmd.ExecuteScalarAsync();
            
            return Convert.ToInt32(newId);

        }
    } 
    
    
    public async Task UpdateBooking(int bookingId, DateTime? start_date = null, DateTime? end_date = null, string? status = null)
    {
        try
        {
           
            var updateParts = new List<string>();

            if (start_date != null) updateParts.Add(" start_date = @start_date");
            if (end_date != null) updateParts.Add("end_date = @end_date");
            if (status != null) updateParts.Add("status= @status");

            if (updateParts.Count == 0)
            {
                Console.WriteLine("No fields to update.");
                return;
            }

            var updateQuery = $"UPDATE bookings SET {string.Join(", ", updateParts)} WHERE bookings_id = @id";

            await using (var cmd = _database.CreateCommand(updateQuery))
            {
                cmd.Parameters.AddWithValue("id", bookingId);

                if (start_date != null) cmd.Parameters.AddWithValue("start_date", start_date);
                if (end_date != null) cmd.Parameters.AddWithValue("end_date", end_date);
                if (status != null) cmd.Parameters.AddWithValue("status", status);

                var rowsAffected = await cmd.ExecuteNonQueryAsync();
                Console.WriteLine(rowsAffected > 0
                    ? "Booking updated successfully."
                    : "No Booking found with the specified ID.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    } 
    
    public async void SpecificBookings()
    {
        try
        {
            await using (var cmd = _database.CreateCommand("SELECT * FROM Bookings")) // Skapa vårt kommand/query
            await using (var reader = await cmd.ExecuteReaderAsync()) // Kör vår kommando/query och inväntar resultatet.
                while ( await reader.ReadAsync()) // Läser av 1 rad/objekt i taget ifrån resultatet och kommer avsluta loopen när det inte finns fler rader att läsa. 
                {
                    // Console.WriteLine($"bookings_id: {reader.GetString(2)}," +
                    //                   $"start_date: {reader.GetString(1)}," +
                    //                   $"end_date: {reader.GetString(2)}," +
                    //                   $"status: {reader.GetString(3)}," +
                }
            
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        
    }
}