using Npgsql;
namespace app.Queries;

public class BookingsJoinAddons
{
    private NpgsqlDataSource _database;

    public BookingsJoinAddons(NpgsqlDataSource database)
    {
        _database = database;
    }

    public async Task InsertBookingJoinAddons(int bookingId, int addonsId)
    {

            var query = @"INSERT INTO bookings_join_addons (addon_fk, booking_fk)
                      VALUES ($1, $2)";
            await using (var cmd = _database.CreateCommand(query))
            {
                cmd.Parameters.AddWithValue(addonsId);
                cmd.Parameters.AddWithValue(bookingId);

                await cmd.ExecuteNonQueryAsync();
            }
    }
}