using Npgsql;
namespace app.Queries;

public class BookingJoinRoomsQueryHandler
{
    
    private NpgsqlDataSource _database;

    public BookingJoinRoomsQueryHandler(NpgsqlDataSource database)
    {
        _database = database;
    }
    
    
    public async Task InsertBookingJoinRoom(int bookingId, int roomId)
    {

            var query = @"INSERT INTO bookings_join_rooms (rooms_fk, booking_fk)
                      VALUES ($1, $2)";
            await using (var cmd = _database.CreateCommand(query))
            {
                cmd.Parameters.AddWithValue(roomId);
                cmd.Parameters.AddWithValue(bookingId);
                
                await cmd.ExecuteNonQueryAsync();
            }
    } 
}