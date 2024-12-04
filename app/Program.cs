using app.Queries;
using DotNetEnv;
namespace app;


class Program
{
    static async Task Main()
    {
        Env.TraversePath().Load();

        Database database = new();
        
        QueryHandler queryHandler = new(database.Connection());
        Menu menu = new(queryHandler);

        await menu.MenuMain();
        
        
        Console.WriteLine("program will now exit press any key to continue");
        Console.ReadLine();
        
    }
}



    





