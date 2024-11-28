using app.Queries;

namespace app;

public class Menu
{
    private string[] _menuOptionsMain = {"1. view bookings","2. create new booking", "3. customer management","4. logOut","5. quit"};

    private string _optionMessage = "please select what you want to do by typing the number associated";

    enum MenuStateEnum
    {
        LoggedOut,
        Main,
        ViewBookings,
        CreateBookings,
        ViewCustomers,
        ManageCustomers,
    }

    private MenuStateEnum _menuState = MenuStateEnum.LoggedOut;
    
    private QueryHandler _queryHandler;
    public Menu(QueryHandler queryHandler)
    {
        _queryHandler = queryHandler;
        //_queryHandler.AllCustomersTest();
        PrintMenu();
    }

    private void PrintMenu()
    {
        switch (_menuState)
        {
            case MenuStateEnum.LoggedOut:
                if (VerifyLogIn())
                {
                    _menuState = MenuStateEnum.Main;
                    PrintMenu();
                }
                break;
            case MenuStateEnum.Main:
                Console.WriteLine(_optionMessage);
                foreach (string options in _menuOptionsMain)
                {
                    Console.WriteLine(options);
                }
                GetInput();
                break;
            case MenuStateEnum.ViewBookings:
                break;
            case MenuStateEnum.CreateBookings:
                break;
            case MenuStateEnum.ViewCustomers:
                break;
            case MenuStateEnum.ManageCustomers:
                break;
        }   
    }

    private void GetInput()
    {
        string? response = Console.ReadLine();
        if (String.IsNullOrEmpty(response)) 
        {
            
        }
        
        switch (_menuState)
        {
            case MenuStateEnum.LoggedOut:
                break;
            case MenuStateEnum.Main:
                
                break;
            case MenuStateEnum.ViewBookings:
                break;
            case MenuStateEnum.CreateBookings:
                break;
            case MenuStateEnum.ViewCustomers:
                break;
            case MenuStateEnum.ManageCustomers:
                break;
        } 
    }

    
    
    
    private bool VerifyLogIn()
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
    
            if (username == correctUsername && password == correctPassword) // ifall denna boolean är sann skriv ut att man är utloggad och gå vidare till general menu. pst inte implementerat general menu.
            {
                Console.WriteLine("\nCorrect credentials! You are now logged in!");
                isLoggedIn = true;
            }
            else
            {
                Console.WriteLine("\nIncorrect credentials! Try again!");
            }
            
        }

        return isLoggedIn;
    }
}