using app.Queries;
using DotNetEnv;
namespace app;


class Program
{
    static void Main()
    {
        Env.TraversePath().Load();

        Database database = new();
        
        QueryHandler queryHandler = new(database.Connection());
        Menu menu = new(queryHandler);

        menu.MenuMain();

    }
}



    





