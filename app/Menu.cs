using System.Xml.Xsl;
using app.Queries;
using app.Queries.TableObjects;
using app.Menus;
using Sprache;

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
        { MenuStateEnum.TestingMenu, new []{"1. test 1", "2. result menu test", "3. test 3", "4. test 4", "5. return"}},
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
        _menuState = MenuStateEnum.LoggedOut; //säger var vi startar menu state
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

    private string GetInput(string message ="")
    {
        Console.WriteLine(message);
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
            Console.WriteLine("invalid login credentials");
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
                await AddCustomer();
                break;
            case "2": //find customer by name
                await SearchCustomerByName();
                break;
            case "3": //edit customer by id
                await EditCustomerById();
                break;
            case "4": //remove customer by id
                await RemoveCustomerById();
                break;
            case "5": //return
                _menuState = MenuStateEnum.Main;
                break;
        }
    }

    private async Task AddCustomer()
    {
        Console.WriteLine("Please enter your customer name");
            string? name = GetInput();

            Console.WriteLine("please input your customer email");
            string? email = GetInput();
            
        Console.WriteLine("please input your customer phonenumber");
            string? phone = GetInput();
            
        Console.WriteLine("please input your birth year");
        if (int.TryParse(GetInput(), out int birthYear))
        {
            await _queryHandler.CustomerQueries.InsertCustomer(name, email, phone, birthYear);
            Console.WriteLine("customer added"); 
        }
        else
        {
            Console.WriteLine("invalid customer birth year, try again");
        }
    }

    private async Task SearchCustomerByName()
    {
        Console.WriteLine("Please enter the name you want to search for");
        string? name = GetInput();
        
        await _queryHandler.CustomerQueries.SearchCustomer("name", name);
    }

    private async Task EditCustomerById()
    {
        Console.WriteLine("Please enter the customer ID you want to edit");
        if (int.TryParse(GetInput(), out int customerId))
        {
            Console.WriteLine("Enter new name (leave blank for no input)");
            string name = Console.ReadLine();
            
            Console.WriteLine("Enter new email (leave blank for no input)");
            string email = Console.ReadLine();
            
            Console.WriteLine("Enter new phone number (leave blank for no input)");
            string phone = Console.ReadLine();
            
            Console.WriteLine("Enter new birth year (leave blank for no input)");
            string birthYearInput = Console.ReadLine();
            int? birthYear = string.IsNullOrWhiteSpace(birthYearInput) ? null : int.Parse(birthYearInput);
            Console.WriteLine("Customer updated");
        }
        else
        {
            Console.WriteLine("invalid customer id");
        }
    }
    
    private async Task RemoveCustomerById()
    {
        Console.WriteLine("Please enter the customer ID you want to remove");
        if (int.TryParse(GetInput(), out int customerId))
        {
            await _queryHandler.CustomerQueries.DeleteCustomer(customerId);
            Console.WriteLine("customer removed");
        }
        else
        {
            Console.WriteLine("invalid customer ID you want to remove");
        }
    }
    
    private async Task BookingMenuHandler()
    {
        switch (GetInput())
        {
            case "1": //search by city
                List<HotelAndFeatures> hotels = await _queryHandler.HotelAndFeaturesQueries.SearchBy("city", GetInput("enter city to search by: "));
                foreach (var hotel in hotels)
                {
                    Console.Write("hotel id: " + hotel.HotelId + "  |  ");
                    Console.Write("rating: " + hotel.Rating + "  |  ");
                    Console.Write("distance to ski: " + hotel.DistanceToSkiSlope + "  |  ");
                    Console.Write("distance to center: " + hotel.DistanceToTownCenter + "  |  ");
                    Console.Write("feature: " + hotel.Feature + "  |  ");
                    Console.Write("street address: " + hotel.StreetName + "  |  ");
                    Console.Write("city: " + hotel.City + "  |  ");
                    Console.WriteLine("country: " + hotel.Country);
                }
                break;
            case "2": //search by distance to ski slope
                break;
            case "3"://return
                _menuState = MenuStateEnum.Main;
                return;
        }

        string input = GetInput("please select a hotel by typing its id");
        int hotelId;
        while (!int.TryParse(input, out hotelId))
        {
            input = GetInput("please select a hotel by typing its id");
        }

        input = GetInput("please input start date in format yyyy-mm-dd");
        DateTime startDate;
        while (!DateTime.TryParse(input, out startDate))
        {
            input = GetInput("please input start date in format yyyy-mm-dd");
        }
        input = GetInput("please input start date in format yyyy-mm-dd");
        DateTime endDate;
        while (!DateTime.TryParse(input, out endDate))
        {
            input = GetInput("please input end date in format yyyy-mm-dd");
        }

        List<BookingToHotell> rooms =
            await _queryHandler.BookingToHotelQueryHandler.GetAvailableRoomsForHotel(hotelId, startDate, endDate);
        
        int bookingId = await _queryHandler.BookingQueries.InsertBooking(startDate, endDate);

        bool roomJoin = true;
        
        while (roomJoin)
        {
            foreach (var room in rooms)
            {
                Console.Write("room id: " + room.RoomId + "  |  ");
                Console.Write("price: " + room.Price + "  |  ");
                Console.Write("room size: " + room.Size + "  |  ");
                Console.WriteLine("room number: " + room.RoomNumber + "  |  ");
            }
            
            input = GetInput("please select a room to add to booking by typing its id");
            int roomId;
            while (!int.TryParse(input, out roomId))
            {
                input = GetInput("please select a room to add to booking by typing its id");
            }

            switch (GetInput("would you like to add more rooms or exit? 1. add 2. exit"))
            {
                case "1":
                    await _queryHandler.BookingJoinRoomsQueryHandler.InsertBookingJoinRoom(bookingId, roomId);
                    break;
                case"2":
                    await _queryHandler.BookingJoinRoomsQueryHandler.InsertBookingJoinRoom(bookingId, roomId);
                    roomJoin = false;
                    break;
            }
        }

        

        bool customerJoin = true;

        while (customerJoin)
        {
            switch (GetInput(
                        "1.search for customer by name  2. add customer to booking by id  3. add customer to booking by creating new  4. continue"))
            {
                case "1": // search name
                    await _queryHandler.CustomerQueries.SearchCustomer("name", GetInput("type name to search by"));
                    break;
                case "2": //add by id
                    input = GetInput("input id");
                    int customerid1;
                    while (!int.TryParse(input, out customerid1))
                    {
                        input = GetInput("wrong input id");
                    }
                    await _queryHandler.BookingsJoinCustomer.InsertBookingsJoinCustomer(customerid1, bookingId);;
                    break;
                case "3": //add by create
                    int customerId = await _queryHandler.CustomerQueries.InsertCustomerReturn(GetInput("inputname"),
                        GetInput("input email"),GetInput("input phonenumber"), Convert.ToInt32(GetInput("input birthyear")));
                    await _queryHandler.BookingsJoinCustomer.InsertBookingsJoinCustomer(customerId, bookingId);
                    break;
                case "4": //continue
                    customerJoin = false;
                    break;
            }
            
        }
        

        bool addAddons = true;


        while (addAddons)
        {
            await _queryHandler.TestQueries.GetAddons();

            switch (GetInput("1. add addon by id  2. complete booking"))
            {
                case "1": //add addon
                    input = GetInput("input id");
                    int addonId;
                    while (!int.TryParse(input, out addonId))
                    {
                        input = GetInput("wrong input id");
                    }
                    await _queryHandler.BookingsJoinAddons.InsertBookingJoinAddons(bookingId, addonId);
                    break;
                case "2": // done
                    addAddons = false;
                    break;
            }
        }

        GetInput("we are don?");
        
        _menuState = MenuStateEnum.Main;
    }

    private async Task TestingMenuHandler()
    {
        switch (GetInput())
        {
            case "1": //damien testing
                await _queryHandler.CustomerQueries.AllCustomers(); 
              // SEARCH: await _queryHandler.CustomerQueries.SearchCustomer("name", "Thom");
             // INSERT: await _queryHandler.CustomerQueries.InsertCustomer("David maguy", "Davidmaguy123@gmail.com", "070-418-9995", 1999);
             // DELETE: await _queryHandler.CustomerQueries.DeleteCustomer(201);
             // UPDATE: await _queryHandler.CustomerQueries.UpdateCustomer(200, name: "Stinky Carl", email: "orb@gmail.com");
              
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
                //await _queryHandler.BookingQueries.InsertBooking(new DateTime(2024, 12, 14), new DateTime(2024, 12, 21));
                await _queryHandler.BookingsJoinCustomer.InsertBookingsJoinCustomer(202, 12);
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