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
        
        queries.AllCustomers();
        
        loginMenu();
    }

    public static void loginMenu()
    {
        Console.WriteLine("Login with Username and Password!");
    
        string correctUsername = "Kasper"; //Här får vi callea på admin usernames samt admin passwords
        string correctPassword = "Kasper123";
    
        bool isLoggedIn = false; 
    
        while (!isLoggedIn)
        {
            Console.WriteLine("Enter your username: ");
            
            String username = Console.ReadLine();
    
            Console.WriteLine("Enter your password ");
            String password = Console.ReadLine();
    
            if (correctLogin(username, password, correctUsername, correctPassword)) // ifall denna boolean är sann skriv ut att man är utloggad och gå vidare till general menu. pst inte implementerat general menu.
            {
                Console.WriteLine("\nCorrect credentials! You are now logged in!");
                isLoggedIn = true;
            }
            else
            {
                Console.WriteLine("\nIncorrect credentials! Try again!");
            }
            
        }
    
        bool correctLogin(string username, string password, string correctUsername, string correctPassword)
        {
            return username == correctUsername && password == correctPassword;
        }
        
    
    }
}
*/


    





