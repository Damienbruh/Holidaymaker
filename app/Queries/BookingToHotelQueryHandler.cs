using app.Queries.TableObjects;

namespace app.Queries;
using Npgsql;

public class BookingToHotelQueryHandler
{
    /*
     example query
     select * from booking_to_hotels_view 
         where
             hotel_id = '9' and 
             room_id in(
                 select room_id from booking_to_hotels_view
                          where(start_date < '2024-12-21' and end_date < '2024-12-14')
                 );
     */
    private NpgsqlDataSource _database;
    
    public BookingToHotelQueryHandler(NpgsqlDataSource database)
    {
        _database = database;
    }


    public async Task<List<BookingToHotell>> GetAvailableRoomsForHotel(int hotel_id, DateTime startDate, DateTime endDate)
    {
        List<BookingToHotell> availableRooms = new List<BookingToHotell>();

        await using var cmd = _database.CreateCommand(@"SELECT * FROM booking_to_hotels_view WHERE hotel_id = $1
                                                                    AND room_id IN(SELECT room_id FROM booking_to_hotels_view 
                                                                    where(($2 NOT BETWEEN start_date and end_date)
                                                                    AND ($3 NOT BETWEEN start_date and end_date))
                                                                    AND (($2 NOT BETWEEN end_date and start_date)
                                                                    AND ($3 NOT BETWEEN end_date and start_date)))");
        cmd.Parameters.AddWithValue(hotel_id);
        cmd.Parameters.AddWithValue(endDate);
        cmd.Parameters.AddWithValue(startDate);
        await using var reader = await cmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
             availableRooms.Add(new BookingToHotell
             {
                 HotelId = reader.GetInt32(0),
                 RoomId = reader.GetInt32(9),
                 Size = reader.GetString(10),
                 Price = reader.GetInt32(11),
                 RoomNumber = reader.GetInt32(12),
                 StartDate = reader.GetDateTime(14).ToString(),
                 EndDate = reader.GetDateTime(15).ToString(),
             });   
        }

        foreach (var room in availableRooms)
        {
            Console.Write(room.HotelId + "  |  ");
            Console.Write(room.RoomId  + "  |  ");
            Console.Write(room.Size  + "  |  ");
            Console.Write(room.Price  + "  |  ");
            Console.WriteLine(room.RoomNumber  + "  |  ");
            Console.Write(room.StartDate + "  |  ");
            Console.WriteLine(room.EndDate + "  |  ");
        }

        Console.ReadLine();
        return availableRooms;
    }
    public async Task<List<BookingToHotell>> GetAvailableRooms(DateTime startDate, DateTime endDate)
    {
        List<BookingToHotell> availableRooms = new List<BookingToHotell>();

        await using var cmd = _database.CreateCommand(@"SELECT * FROM booking_to_hotels_view 
                                                                     WHERE room_id IN(SELECT room_id FROM booking_to_hotels_view 
                                                                     where(($1 NOT BETWEEN start_date and end_date) AND ($2 NOT BETWEEN start_date and end_date))
                                                                     AND (($1 NOT BETWEEN end_date and start_date) AND ($2 NOT BETWEEN end_date and start_date)))");
        cmd.Parameters.AddWithValue(endDate);
        cmd.Parameters.AddWithValue(startDate);
        await using var reader = await cmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            availableRooms.Add(new BookingToHotell
            {
                HotelId = reader.GetInt32(0),
                RoomId = reader.GetInt32(9),
                Size = reader.GetString(10),
                Price = reader.GetInt32(11),
                RoomNumber = reader.GetInt32(12),
                StartDate = reader.GetDateTime(14).ToString(),
                EndDate = reader.GetDateTime(15).ToString()
            });   
        }

        foreach (var room in availableRooms)
        {
            Console.Write(room.HotelId + "  |  ");
            Console.Write(room.RoomId  + "  |  ");
            Console.Write(room.Size  + "  |  ");
            Console.Write(room.Price  + "  |  ");
            Console.Write(room.RoomNumber  + "  |  ");
            Console.Write(room.StartDate + "  |  ");
            Console.WriteLine(room.EndDate + "  |  ");
        }

        Console.ReadLine();
        return availableRooms;
    }
    
}