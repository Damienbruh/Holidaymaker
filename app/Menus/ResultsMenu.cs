using System.Security.AccessControl;

namespace app.Menus;
using app.Queries;
using app.Queries.TableObjects;

public class ResultsMenu
{
    private QueryHandler _queryHandler;
    public  ResultsMenu(QueryHandler queryHandler)
    {
        _queryHandler = queryHandler;
    }

    public async Task DisplayResults(List<object> dataList)
    {
       // List<> properties;
        foreach(var prop in dataList[1].GetType().GetProperties()) {
            Console.WriteLine("{0}-{1}", prop.Name, prop.GetType());
            
        }
        
    }
    
}