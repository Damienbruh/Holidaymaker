using System.ComponentModel.DataAnnotations;

namespace app.Queries.TableObjects;

public class Booking
{
    public int Id { get; set; }
    public string StartDate { get; set; }
    public string EndDate { get; set; }
    public string? Status { get; set; }
}