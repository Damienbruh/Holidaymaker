using app.Queries;
namespace app;
public class Menu
{
    //här specifierar vi vilka olika states meny kan vara i
    enum MenuStateEnum
    {
        LoggedOut,
        Main,
        ViewBookings,
        CreateBookings,
        ViewCustomers,
        ManageCustomers,
    }

    //här specifierar vi vad varje menystate ska visa
    private readonly Dictionary<MenuStateEnum, String[]> _menuOptions = new()
    {
        //{ MenuStateEnum.LoggedOut, new []{""} },
        { MenuStateEnum.Main, new []{"1. view bookings","2. create new booking", "3. customer management","4. logOut","5. quit"}},
        // { MenuStateEnum.ViewBookings, new []{""}}, 
        // { MenuStateEnum.CreateBookings, new []{""}},
         { MenuStateEnum.ViewCustomers, new []{"1. view all customers","2. find customer by id","3. find customer by name", "4. edit customer by id", "5. remove customer by id","6. return"}},
         { MenuStateEnum.ManageCustomers, new []{"1. view customers", "2. add customer", "3. return"}}
    };
    private readonly Dictionary<MenuStateEnum, Func<Task>> _menuHandlers;
    private MenuStateEnum _menuState;
    private string _menuMessage = "please select one of the menu options by typing";
    private QueryHandler _queryHandler;
    private bool _menuLoop = true;

    public Menu(QueryHandler queryHandler)
    {
        //här lägger vi till vilken function som skall callas vid vilket state, viktigt att functionen är en async task. skall leta efter bättre lösning
        _menuHandlers = new Dictionary<MenuStateEnum, Func<Task>>
        {
            { MenuStateEnum.LoggedOut, HandleLoggedOutMenu},
            { MenuStateEnum.Main, HandleMainMenu},
            { MenuStateEnum.ViewBookings, HandleViewBookingsMenu}, 
            { MenuStateEnum.CreateBookings, HandleCreateBookingsMenu},
            { MenuStateEnum.ViewCustomers, HandleViewCustomersMenu},
            { MenuStateEnum.ManageCustomers, HandleManageCustomersMenu}
        };
        _menuState = MenuStateEnum.ViewCustomers; //säger var vi startar menustate
        _queryHandler = queryHandler;
    }

    public async Task MenuMain()
    {
        while (_menuLoop)
        {
            PrintMenu();
            await GetInput();
        }
    }

    private async Task GetInput()
    {
        if (_menuHandlers.TryGetValue(_menuState, out var handler))
        {
            await handler(); // can i get response first and pass in here?
        }
        else
        {
            throw new Exception("no handler for menu");
        }
    }

    private void PrintMenu()
    {
        if (_menuOptions.TryGetValue(_menuState, out string[]? options))
        {
            Console.WriteLine(_menuMessage);
            foreach (var option in options)
            {
                Console.WriteLine(option);
            }
        }
        else
        {
            Console.WriteLine("no options for this state");
        }
    }

    private async Task HandleLoggedOutMenu()
    {
        VerifyLogIn();
        _menuState = MenuStateEnum.Main;
        
    }
    private async Task HandleMainMenu()
    {
        string? response = Console.ReadLine();

        while (String.IsNullOrEmpty(response))
        {
            Console.WriteLine("invalid option try again");
            response = Console.ReadLine();
        }

        switch (response)
        {
            case "1": //view bookings
                _menuState = MenuStateEnum.ViewBookings;
                break;
            case "2": //create booking
                _menuState = MenuStateEnum.CreateBookings;
                break;
            case "3": //customer management
                _menuState = MenuStateEnum.ManageCustomers;
                break;
            case "4": //logout
                _menuState = MenuStateEnum.LoggedOut;
                break;
            case "5": //quit
                _menuLoop = false;
                break;
        }
    }
    private async Task HandleViewBookingsMenu() {}
    private async Task HandleCreateBookingsMenu() {}
    private async Task HandleViewCustomersMenu()
    {
        string? response = Console.ReadLine();

        while (String.IsNullOrEmpty(response))
        {
            Console.WriteLine("invalid option try again");
            response = Console.ReadLine();
        }

        switch (response)
        {
            case "1": //view all customers
                await _queryHandler.CustomerQueries.AllCustomers();
                break;
            case "2": //find customer by id
                Console.WriteLine("not implemented");
                break;
            case "3": //find customer by name
                Console.WriteLine("not implemented");
                break;
            case "4": //edit customer by id
                Console.WriteLine("not implemented");
                break;
            case "5": //remove customer by id
                _menuLoop = false;
                break;
            case "6":
                _menuState = MenuStateEnum.ManageCustomers;
                break;
        }
    }

    private async Task HandleManageCustomersMenu()
    {
        string? response = Console.ReadLine();

        while (String.IsNullOrEmpty(response))
        {
            Console.WriteLine("invalid option try again");
            response = Console.ReadLine();
        }

        switch (response)
        {
            case "1": //view customer
                _menuState = MenuStateEnum.ViewCustomers;
                break;
            case "2": //add customer
                Console.WriteLine("added customer 100% big true");
                break;
            case "3": //return
                _menuState = MenuStateEnum.Main;
                break;
        }
    }

    private void VerifyLogIn()
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

            if (username == correctUsername &&
                password ==
                correctPassword) // ifall denna boolean är sann skriv ut att man är utloggad och gå vidare till general menu. pst inte implementerat general menu.
            {
                Console.WriteLine("\nCorrect credentials! You are now logged in!");
                isLoggedIn = true;
            }
            else
            {
                Console.WriteLine("\nIncorrect credentials! Try again!");
            }

        }
    }
}