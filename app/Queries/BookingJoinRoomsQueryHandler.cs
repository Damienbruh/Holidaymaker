using Npgsql;
namespace app.Queries;

public class BookingJoinRoomsQueryHandler
{
    
    private NpgsqlDataSource _database;

    public BookingJoinRoomsQueryHandler(NpgsqlDataSource database)
    {
        _database = database;
    }
    
    
    public async Task<int> InsertBookingJoinRoom(int bookingId, List<int> roomIds)
    {
        var rowsAffected = 0;
        foreach (var roomId in roomIds)
        {
            var query = @"INSERT INTO bookings_join_rooms (rooms_fk, booking_fk)
                      VALUES ($1, $2)";
            await using (var cmd = _database.CreateCommand(query))
            {
                cmd.Parameters.AddWithValue(bookingId);
                cmd.Parameters.AddWithValue(roomId);
                
                await cmd.ExecuteNonQueryAsync();
                rowsAffected++; 
            }
        }
        
        Console.WriteLine(rowsAffected);
        Console.ReadLine();
        return rowsAffected;

    } 
}