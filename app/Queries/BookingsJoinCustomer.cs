using Npgsql;
namespace app.Queries;

public class BookingsJoinCustomer
{
    
    private NpgsqlDataSource _database;

    public BookingsJoinCustomer(NpgsqlDataSource database)
    {
        _database = database;
    }
    
    
    public async Task<int> InsertBookingsJoinCustomer(int bookingId, List<int> customerIds)
    {
        var rowsAffected = 0;
        foreach (var customerId in customerIds)
        {
            var query = @"INSERT INTO bookings_join_customer (rooms_fk, customer_fk)
                      VALUES ($1, $2)";
            await using (var cmd = _database.CreateCommand(query))
            {
                cmd.Parameters.AddWithValue(bookingId);
                cmd.Parameters.AddWithValue(customerId);
                
                await cmd.ExecuteNonQueryAsync();
                rowsAffected++; 
            }
        }
        
        Console.WriteLine(rowsAffected);
        Console.ReadLine();
        return rowsAffected;

    } 
}