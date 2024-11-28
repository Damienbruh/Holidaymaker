using Npgsql;

namespace app.Queries;

public class QueryHandler
{
    private NpgsqlDataSource _database;
    public TestQueries TestQueries { get; }
    public HotellQueries HotellQueries { get; }
    //public Customer CustomerQueries { get; }
    public QueryHandler(NpgsqlDataSource database)
    {
        _database = database;
        TestQueries = new(_database);
        HotellQueries = new(_database);
        //CustomerQueries = new(_database);
    }
}