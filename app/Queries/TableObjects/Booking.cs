namespace app.Queries.TableObjects;

public class Booking
{
    public int Id { get; set; }
    public int StartDate { get; set; }
    public int EndDate { get; set; }
    public string? Status { get; set; }
}