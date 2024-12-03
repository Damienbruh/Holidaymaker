using Npgsql;
namespace app.Queries;

public class BookingsJoinAddons
{
    private NpgsqlDataSource _database;

    public BookingsJoinAddons(NpgsqlDataSource database)
    {
        _database = database;
    }

    public async Task<int> InsertBookingJoinAddons(int bookingId, List<int> addonsId)
    {
        var rowsAffected = 0;
        foreach (var roomId in addonsId)
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