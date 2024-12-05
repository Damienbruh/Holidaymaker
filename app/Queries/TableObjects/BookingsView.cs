namespace app.Queries.TableObjects;

public class BookingsView
{
    public int BookingsId { get; set; }
    public string StartDate { get; set; }
    public string EndDate { get; set; }
    public string? Status { get; set; } 
    public string? Addons { get; set; }
    public int AddonPrice { get; set; }
    public int RoomId { get; set; }
    public string? Size { get; set; }
    public int Price { get; set; }
    public int RoomNumber { get; set; }
    public int CustomerID { get; set; }
    public string? CustomerName { get; set; }
    public string? CustomerEmail { get; set; }
    public string? CustomerPhone { get; set; }
    public string? Birthyear { get; set; }
}