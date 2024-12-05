using app.Queries.TableObjects;
using Npgsql;
using NpgsqlTypes;

namespace app.Queries;

public class BookingsViewQueries
{
    private NpgsqlDataSource _database;

    public BookingsViewQueries(NpgsqlDataSource database)
    {
        _database = database;
    }


    public async Task<List<BookingsView>> GetAllBookings()
    {
        List<BookingsView> bookings = new List<BookingsView>();
        await using (var cmd = _database.CreateCommand("SELECT * FROM all_bookings_view"))
        await using (var reader = await cmd.ExecuteReaderAsync())
            while 
                (await reader.ReadAsync())
            {
                bookings.Add(new BookingsView
                {
                    BookingsId = reader.GetInt32(0),
                    StartDate = reader.GetString(1),
                    EndDate = reader.GetString(2),
                    Status = reader.GetString(3),
                    Addons = reader.GetString(4),
                    AddonPrice = reader.GetInt32(5),
                    RoomId = reader.GetInt32(6),
                    Size = reader.GetString(7),
                    Price = reader.GetInt32(8),
                    RoomNumber = reader.GetInt32(9),
                    CustomerID = reader.GetInt32(10),
                    CustomerName = reader.GetString(11),
                    CustomerEmail = reader.GetString(12),
                    CustomerPhone = reader.GetString(13),
                    Birthyear = reader.GetString(14),
                    
                    
                });
            }
        
        return bookings;

    }
}
    
  

