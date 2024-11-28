using app.Queries;

namespace app;


class Program
{
    static void Main()
    {
        Database db = new();
        TestQueries queries = new(db.Connection());
        QueryHandler queryHandler = new(db.Connection());
        Menu menu = new(queryHandler);
        
        queries.AllCustomers();
        
    }
}



    





