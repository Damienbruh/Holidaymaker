namespace app.Queries.TableObjects;

public class Addons
{
    public int Id { get; set; }
    public string? Addon { get; set; }
    public int Price { get; set; }
    public int HotelFK{ get; set; }
}