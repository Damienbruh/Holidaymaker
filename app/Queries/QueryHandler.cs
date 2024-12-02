using Npgsql;

namespace app.Queries;

public class QueryHandler
{
    private NpgsqlDataSource _database;
    public TestQueries TestQueries { get; }
    public VerifyLoginHandler VerifyLoginHandler { get; }
    public HotellQueries HotellQueries { get; }
    public CustomerQueries CustomerQueries { get; }
    
    public BookingQueries BookingQueries { get; }
    public QueryHandler(NpgsqlDataSource database)
    
    
    {
        _database = database;
        TestQueries = new(_database);
        VerifyLoginHandler = new(_database);
        HotellQueries = new(_database);
        CustomerQueries = new(_database);
        BookingQueries = new BookingQueries(_database);
    }
}