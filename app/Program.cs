using app.Queries;
using DotNetEnv;
namespace app;


class Program
{
    static void Main()
    {
        Env.TraversePath().Load();

        Database db = new();
        
        //HotellQueries hotellQueries = new(db.Connection());
        QueryHandler queryHandler = new(db.Connection());
        //Menu menu = new(queryHandler);
        
        
    }
}



    





