namespace app.Queries.TableObjects;

public class HotelAndFeatures
{
    public int HotelId { get; set; }
    public string? StreetName { get; set; }
    public string? PostalCode { get; set; }
    public string? City { get; set; }
    public string? Region { get; set; }
    public string? Country { get; set; }
    public string? DistanceToSkiSlope { get; set; }
    public string? DistanceToTownCenter { get; set; }
    public int Rating { get; set; }
    public string? HotelNames { get; set; }
    public string? DistanceToLinusHouse { get; set; }
    public int FeatureId { get; set; }
    public string? Feature { get; set; }
}