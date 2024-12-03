namespace app.Queries.TableObjects;

public class BookingToHotell
{
    //hotell
    public int HotelId { get; set; }
    // public string? StreetName { get; set; }
    // public string? PostalCode { get; set; }
    // public string? City { get; set; }
    // public string? Region { get; set; }
    // public string? Country { get; set; }
    // public string? DistanceToSkiSlope { get; set; }
    // public string? DistanceToTownCenter { get; set; }
    // public int Rating { get; set; }
    
    //rooms
    public int RoomId { get; set; }
    public string? Size { get; set; }
    public int Price { get; set; }
    public int RoomNumber { get; set; }
    
    //bookings
    // public int BookingId { get; set; }
    // public string? StartDate { get; set; }
    // public string? EndDate { get; set; }
    // public string? Status { get; set; }
}