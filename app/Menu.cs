using app.Queries;
namespace app;
public class Menu
{
    
    // if time allows maybe break out different menus into different classes and maybe call them via interface
    
    
    //här specifierar vi vilka olika states meny kan vara i
    enum MenuStateEnum
    {
        LoggedOut,
        Main,
        ViewBookings,
        CreateBookings,
        ViewCustomers,
        ManageCustomers,
        TestingMenu,
    }

    //här specifierar vi vad varje menystate ska visa
    private readonly Dictionary<MenuStateEnum, String[]> _menuOptions = new()
    {
        { MenuStateEnum.LoggedOut, new []{"please input you username and password"} },
        { MenuStateEnum.Main, new []{"1. view bookings","2. create new booking", "3. customer management","4. logOut","5. quit", "6. testing menu"}},
        // { MenuStateEnum.ViewBookings, new []{""}}, 
        // { MenuStateEnum.CreateBookings, new []{""}},
        { MenuStateEnum.ViewCustomers, new []{"1. view all customers","2. find customer by id","3. find customer by name", "4. edit customer by id", "5. remove customer by id","6. return"}},
        { MenuStateEnum.ManageCustomers, new []{"1. view customers", "2. add customer", "3. return"}},
        { MenuStateEnum.TestingMenu, new []{"1. damien test command", "2. david test command", "3. kasper test command", "4. noel test command"}}
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
            { MenuStateEnum.ManageCustomers, HandleManageCustomersMenu},
            { MenuStateEnum.TestingMenu, TestingMenuHandler}
        };
        _menuState = MenuStateEnum.LoggedOut; //säger var vi startar menu state
        _queryHandler = queryHandler;
    }

    public async Task MenuMain()
    {
        while (_menuLoop)
        {
            PrintMenu();
            await CallHandler();
        }
    }

    private async Task CallHandler()
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

    private string GetInput()
    {
        string? response = Console.ReadLine();

        while (String.IsNullOrEmpty(response))
        {
            Console.WriteLine("invalid option try again");
            response = Console.ReadLine();
        }
        return response;
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
        Console.WriteLine("please input your username");
        string? username = GetInput();
        
       
        Console.WriteLine("please input your password");
        string? password = GetInput();
        

        if (await _queryHandler.VerifyLoginHandler.VerifyLogin(username, password))
        {
            Console.WriteLine("login successfull, press any key to continue");
            Console.ReadLine();
            _menuState = MenuStateEnum.Main;
        }
        else
        {
            Console.WriteLine("invalid login credentials, press any key to continue");
            Console.ReadLine();
        }
        
        
    }
    private async Task HandleMainMenu()
    {
        switch (GetInput())
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
            case "6":
                _menuState = MenuStateEnum.TestingMenu;
                break;
            case "7": //testing
                _queryHandler.HotellQueries.SpecificHotell();
                break;
        }
    }
    private async Task HandleViewBookingsMenu() {}
    private async Task HandleCreateBookingsMenu() {}
    private async Task HandleViewCustomersMenu()
    {
        switch (GetInput())
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
        switch (GetInput())
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

    private async Task TestingMenuHandler()
    {
        switch (GetInput())
        {
            case "1": //damien testing
                
                break;
            case "2": //david testing
                
                break;
            case "3": //kasper testing
                
                break;
            case "4": //noel testing
                
                break;
        }
        
    }
}