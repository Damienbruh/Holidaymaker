using System.Data;
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
        await using (var cmd = _database.CreateCommand("SELECT * FROM booking_view"))
        await using (var reader = await cmd.ExecuteReaderAsync())
            while
                (await reader.ReadAsync())
            {
                bookings.Add(new BookingsView
                {
                    BookingsId = reader.GetInt32(0),
                    StartDate = reader.GetDateTime(1).ToString(),
                    EndDate = reader.GetDateTime(2).ToString(),
                    Status = reader.GetString(3),
                    AddonsId = reader.IsDBNull(4) ? null : reader.GetInt32(4),
                    Addons = reader.IsDBNull(5) ? null : reader.GetString(5),
                    AddonPrice = reader.IsDBNull(6) ? null : reader.GetInt32(6),
                    RoomId = reader.GetInt32(7),
                    Size = reader.GetString(8),
                    Price = reader.GetInt32(9),
                    RoomNumber = reader.GetInt32(10),
                    CustomerID = reader.GetInt32(11),
                    CustomerName = reader.GetString(12),
                    CustomerEmail = reader.GetString(13),
                    CustomerPhone = reader.GetString(14),
                    Birthyear = reader.GetInt32(15),


                });
            }

        return bookings;
    }

    public async Task<List<BookingsView>> SearchBookingByName(string customerName)
    {

        List<BookingsView> bookings = new List<BookingsView>();
        var query = $"SELECT * FROM booking_view WHERE name ILIKE @customerName";
        
        await using (var cmd = _database.CreateCommand(query))
        {
            cmd.Parameters.AddWithValue("customerName", $"%{customerName}%");

            await using (var reader = await cmd.ExecuteReaderAsync())
            {
                if (!reader.HasRows)
                {
                    Console.WriteLine("Booking does not exist");
                    return bookings;

                }

                while (await reader.ReadAsync())
                {
                    bookings.Add(new BookingsView
                    {
                        BookingsId = reader.GetInt32(0),
                        StartDate = reader.GetDateTime(1).ToString(),
                        EndDate = reader.GetDateTime(2).ToString(),
                        Status = reader.GetString(3),
                        AddonsId = reader.IsDBNull(4) ? null : reader.GetInt32(4),
                        Addons = reader.IsDBNull(5) ? null : reader.GetString(5),
                        AddonPrice = reader.IsDBNull(6) ? null : reader.GetInt32(6),
                        RoomId = reader.GetInt32(7),
                        Size = reader.GetString(8),
                        Price = reader.GetInt32(9),
                        RoomNumber = reader.GetInt32(10),
                        CustomerID = reader.GetInt32(11),
                        CustomerName = reader.GetString(12),
                        CustomerEmail = reader.GetString(13),
                        CustomerPhone = reader.GetString(14),
                        Birthyear = reader.GetInt32(15),
                    });
                }
            }

            Console.WriteLine("Bookings connected to that name: ");
            
        }

        return bookings;
    }

} 

