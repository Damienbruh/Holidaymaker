using Npgsql;
namespace app.Queries;

public class BookingsJoinCustomer
{
    
    private NpgsqlDataSource _database;

    public BookingsJoinCustomer(NpgsqlDataSource database)
    {
        _database = database;
    }
    
    
    public async Task InsertBookingsJoinCustomer(int customerId, int bookingId)
    {

        var query = @"INSERT INTO bookings_join_customer (customer_fk, booking_fk)
                  VALUES ($1, $2)";
        await using (var cmd = _database.CreateCommand(query))
        {
            cmd.Parameters.AddWithValue(customerId);
            cmd.Parameters.AddWithValue(bookingId);
            
            await cmd.ExecuteNonQueryAsync();

        }
    } 
}