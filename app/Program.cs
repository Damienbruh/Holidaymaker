using app.Queries;

namespace app;


class Program
{
    static async Task Main(string[] args)
    {
        string connectionString = "Host=localhost;Port=5432;Username=postgres;Password=1234;Database=postgres";

        var customerQuery = new CustomerQuery(connectionString);

        try
        {
            var customers = await customerQuery.GetCustomersAsync();
            foreach (var customer in customers)
            {
                Console.WriteLine($"ID: {customer.CustomerId}, Name: {customer.Name}, Email: {customer.Email}, " +
                                  $"Phone: {customer.PhoneNumber}, Birth Year: {customer.BirthYear}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    } 
}
    
   /* static void Main()
    {
        Database db = new();
        TestQueries queries = new(db.Connection());
        QueryHandler queryHandler = new(db.Connection());
        Menu menu = new(queryHandler);
        
        queries.AllCustomers();
        
    }
}
*/


    





