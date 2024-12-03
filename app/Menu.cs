using System.Xml.Xsl;
using app.Queries;
using app.Queries.TableObjects;
using app.Menus;
namespace app;
public class Menu
{
    
    // if time allows maybe break out different menus into different classes and maybe call them via interface
    
    
    //här specifierar vi vilka olika states meny kan vara i
    public enum MenuStateEnum
    {
        LoggedOut,
        Main,
        ManageCustomers,
        TestingMenu,
        ResultMenu,
        BookingMenu
    }

    //här specifierar vi vad varje menystate ska visa
    private readonly Dictionary<MenuStateEnum, String[]> _menuOptions = new()
    {
        { MenuStateEnum.LoggedOut, new []{"please input you username and password"} },
        { MenuStateEnum.Main, new []{"1. customer management","2. create booking", "3. logOut","4. quit", "5. testing menu"}},
        { MenuStateEnum.ManageCustomers, new []{"1. add customer","2. find customer by name","3.edit customer by id","4. remove customer by id", "5. return"}},
        { MenuStateEnum.TestingMenu, new []{"1. damien test command", "2. david test command", "3. kasper test command", "4. noel test command", "5. return"}},
        { MenuStateEnum.ResultMenu, new []{"arrow keys to navigate", "enter to confirm", "backspace to return"}},
        { MenuStateEnum.BookingMenu, new []{"1. search by city", "2. search by distance"}}
    };
    private readonly Dictionary<MenuStateEnum, Func<Task>> _menuHandlers;
    private MenuStateEnum _menuState;
    private string _menuMessage = "please select one of the menu options by typing";
    private QueryHandler _queryHandler;
    private bool _menuLoop = true;

    private ResultsMenu _resultsMenu;
    
    public Menu(QueryHandler queryHandler)
    {
        //här lägger vi till vilken function som skall callas vid vilket state, viktigt att functionen är en async task. ska leta efter bättre lösning
        _menuHandlers = new Dictionary<MenuStateEnum, Func<Task>>
        {
            { MenuStateEnum.LoggedOut, HandleLoggedOutMenu},
            { MenuStateEnum.Main, HandleMainMenu},
            { MenuStateEnum.ManageCustomers, HandleManageCustomersMenu},
            { MenuStateEnum.TestingMenu, TestingMenuHandler},
            { MenuStateEnum.ResultMenu, ResultMenuHandler},
            { MenuStateEnum.BookingMenu, BookingMenuHandler}
        };
        _menuState = MenuStateEnum.Main; //säger var vi startar menu state
        _queryHandler = queryHandler;
        _resultsMenu = new ResultsMenu(_queryHandler);
    }

    public async Task MenuMain()
    {
        PrintMenuOptions();
        while (_menuLoop)
        {
            await CallHandler();
            PrintMenuOptions();
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

    private void PrintMenuOptions()
    {
        if (_menuOptions.TryGetValue(_menuState, out string[]? options))
        {
            Console.WriteLine(_menuMessage);
            foreach (var option in options)
            {
                Console.Write(option + "   ");
            }
            Console.WriteLine();
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
            case "1": //customer management
                _menuState = MenuStateEnum.ManageCustomers;
                break;
            case "2": //create booking
                _menuState = MenuStateEnum.BookingMenu;
                break;
            case "3": //logout
                _menuState = MenuStateEnum.LoggedOut;
                break;
            case "4": //quit
                _menuLoop = false;
                break;
            case "5": //testing
                _menuState = MenuStateEnum.TestingMenu;
                break;
        }
    }

    private async Task HandleManageCustomersMenu()
    {
        switch (GetInput())
        {
            case "1": //add customer
                Console.WriteLine("added customer 100% big true");
                break;
            case "2": //find customer by name
                break;
            case "3": //edit customer by id
                break;
            case "4": //remove customer by id
                break;
            case "5": //return
                _menuState = MenuStateEnum.Main;
                break;
        }
    }

    private async Task BookingMenuHandler()
    {
        switch (GetInput())
        {
            case "1": //search by city
                
                break;
            case "2": //search by distance to ski slope
                break;
            case "3"://return
                _menuState = MenuStateEnum.Main;
                break;
        }
    }

    private async Task TestingMenuHandler()
    {
        switch (GetInput())
        {
            case "1": //damien testing
              // SEARCH await _queryHandler.CustomerQueries.SearchCustomer("name", "Thom");
             // INSERT  await _queryHandler.CustomerQueries.InsertCustomer("David maguy", "Davidmaguy123@gmail.com", "070-418-9995", 1999);
             // DELETE await _queryHandler.CustomerQueries.DeleteCustomer(201);
             // UPDATE await _queryHandler.CustomerQueries.UpdateCustomer(200, name: "deez nuticus", email: "deez@gmail.com");
              
                break;
            case "2": //david testing
                _menuState = MenuStateEnum.ResultMenu;
                break;
            case "3": //kasper testing
                await _queryHandler.HotellQueries.AllHotels();
                break;
            case "4": //noel testing
                //await _queryHandler.BookingToHotelQueryHandler.GetAvailableRoomsForHotel(9,new DateTime(2024, 12,14), new DateTime(2024, 12, 21));
                //await _queryHandler.BookingToHotelQueryHandler.GetAvailableRooms(new DateTime(2024, 12,14), new DateTime(2024, 12, 21));
                //await _queryHandler.HotelAndFeaturesQueries.AllHotels();
                //await _queryHandler.HotelAndFeaturesQueries.SearchBy();
                //await _queryHandler.BookingJoinRoomsQueryHandler.InsertBookingJoinRoom(3, new List<int>{ 4, 5, 6 });
                await _queryHandler.BookingQueries.InsertBooking(new DateTime(2024, 12, 14), new DateTime(2024, 12, 21));
                break;
            case "5":
                _menuState = MenuStateEnum.Main;
                break;
        }
        
    }

    private string PadBoth(string text, int totalWidth)
    {
        int spaces = totalWidth - text.Length;
        int padLeft = spaces / 2 + text.Length;
        return text.PadLeft(padLeft).PadRight(totalWidth);
    }
    
    private async Task ResultMenuHandler()
    {
        List<Customer> customers = await _queryHandler.TestQueries.TestQuery();

        // foreach(var prop in customers[1].GetType().GetProperties()) {
        //     Console.WriteLine("{0}-{1}", prop.Name, prop.Name.GetType());
        // }


        //Console.ReadLine();
        int maxIdLength = customers.Max(c => c.Id.ToString().Length);
        int maxNameLength = customers.Max(c => (c.Name ?? "").Length);
        int maxEmailLength = customers.Max(c => (c.Email ?? "").Length);
        int maxPhonenumberLength = customers.Max(c => (c.PhoneNumber ?? "").Length);
        int maxBirthyearLength = customers.Max(c => c.Birthyear.ToString().Length);
        int extraTextLength = "Id:   |  Name:   |  Email:   |  PhoneNumber:   |  BirthYear:   |".Length;
        int totalWidth = maxIdLength + maxNameLength + maxEmailLength + maxPhonenumberLength + maxBirthyearLength + extraTextLength;
        int customersStart = 0;
        int customersEnd = 10;
        ConsoleKeyInfo key;
        bool test = true;
        int row = 0;
        Console.Clear();
        Console.WriteLine(new string('-', totalWidth));
        Console.WriteLine("|" + PadBoth("viewing customers", totalWidth - 2) + "|");
        Console.WriteLine(new string('-', totalWidth));
        
        (int left, int top) = Console.GetCursorPosition();
        
        

        Console.CursorVisible = false;
        
        while (test)
        {
            Console.SetCursorPosition(left, top);
            
            for (int i = customersStart; i < customersEnd && i < customers.Count; i++)
            {
                Customer customer = customers[i];

                Console.ForegroundColor = (i == row) ? ConsoleColor.Green : ConsoleColor.Gray;

                if (i == row)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"Id: {customer.Id.ToString().PadRight(maxIdLength)}  |  " +
                                      $"Name: {(customer.Name ?? "null").PadRight(maxNameLength)}  |  " +
                                      $"Email: {(customer.Email ?? "null").PadRight(maxEmailLength)}  |  " +
                                      $"PhoneNumber: {(customer.PhoneNumber ?? "null").PadRight(maxPhonenumberLength)}  |  " +
                                      $"BirthYear: {customer.Birthyear.ToString().PadRight(maxBirthyearLength)}" + "  <--");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkMagenta;
                    Console.Write("Id: ");
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    Console.Write(customer.Id.ToString().PadRight(maxIdLength));
                    Console.ForegroundColor = ConsoleColor.DarkMagenta;
                    Console.Write("  |  ");
                    
                    Console.Write("Name: ");
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    Console.Write((customer.Name ?? "null").PadRight(maxNameLength));
                    Console.ForegroundColor = ConsoleColor.DarkMagenta;
                    Console.Write("  |  ");
                    
                    Console.Write("Email: ");
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    Console.Write((customer.Email ?? "null").PadRight(maxEmailLength));
                    Console.ForegroundColor = ConsoleColor.DarkMagenta;
                    Console.Write("  |  ");
                    
                    Console.Write("PhoneNumber: ");
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    Console.Write((customer.PhoneNumber ?? "null").PadRight(maxPhonenumberLength));
                    Console.ForegroundColor = ConsoleColor.DarkMagenta;
                    Console.Write("  |  ");
                    
                    Console.Write("BirthYear: ");
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    Console.Write(customer.Birthyear.ToString().PadRight(maxBirthyearLength));
                    Console.ForegroundColor = ConsoleColor.DarkMagenta;
                    Console.WriteLine("  |  ");
                }
            }
            Console.ResetColor();
            Console.WriteLine(new string('-', totalWidth));
            if (_menuOptions.TryGetValue(_menuState, out string[]? options))
            {
                string optionsText = "";
                foreach (var option in options)
                {
                    optionsText = optionsText + option + "   ";
                }
                Console.WriteLine("|" + PadBoth(optionsText, totalWidth - 2) + "|");
            }
            else
            {
                Console.WriteLine("no options for this state");
            }
            Console.WriteLine(new string('-', totalWidth));
            
            
            key = Console.ReadKey(true);
            
            switch (key.Key)
            {
                case ConsoleKey.DownArrow:
                    if (row == customers.Count - 1)
                    {
                        row = 0;
                        customersStart = 0;
                        customersEnd = 10;
                    }
                    else
                    {
                        if (row >= customers.Count - 5)
                        {
                            customersStart = customers.Count - 10;
                            customersEnd = customers.Count;
                        }
                        else if (row >= 4)
                        {
                            customersStart++;
                            customersEnd++;
                        }

                        row++;
                    }
                    break;
                case ConsoleKey.UpArrow:
                    if (row == 0)
                    {
                        row = customers.Count - 1;
                        customersStart = customers.Count - 10;
                        customersEnd = customers.Count;
                    }
                    else
                    {
                        if (row <= 4)
                        {
                            customersStart = 0;
                            customersEnd = 10;
                        }
                        else if (row < customers.Count - 5)
                        {
                            customersStart--;
                            customersEnd--;
                        }
                        
                        row --;
                    }
                    
                    break;
                case ConsoleKey.Enter:
                    test = false;
                    break;
                    
            }
            
        }
    }
}