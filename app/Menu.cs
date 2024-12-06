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
        BookingMenu,
        ViewBookingMenu
    }

    //här specifierar vi vad varje menystate ska visa
    private readonly Dictionary<MenuStateEnum, String[]> _menuOptions = new()
    {
        { MenuStateEnum.LoggedOut, new []{"please input you username and password"} },
        { MenuStateEnum.Main, new []{"1. customer management","2. create booking", "3. logOut","4. quit", "5. testing menu", "6. view bookings"}},
        { MenuStateEnum.ManageCustomers, new []{"1. add customer","2. find customer by name","3.edit customer by id","4. remove customer by id", "5. return"}},
        { MenuStateEnum.TestingMenu, new []{"1. test 1", "2. result menu test", "3. test 3", "4. test 4", "5. return"}},
        { MenuStateEnum.ResultMenu, new []{"arrow keys to navigate", "enter to confirm", "backspace to return", "testing text", "testing text", "testing text", "testing text"}},
        { MenuStateEnum.BookingMenu, new []{"1. search by city", "2. search by distance"}},
        { MenuStateEnum.ViewBookingMenu, new []{"1. view all bookings", "2. view bookings by customer name"}}
    };
    private readonly Dictionary<MenuStateEnum, Func<Task>> _menuHandlers;
    private MenuStateEnum _menuState;
    private string _menuMessage = "please select one of the menu options by typing";
    private QueryHandler _queryHandler;
    private bool _menuLoop = true;
    
    
    public Menu(QueryHandler queryHandler)
    {
        //här lägger vi till vilken function som skall callas vid vilket state, viktigt att functionen är en async task. ska leta efter bättre lösning
        _menuHandlers = new Dictionary<MenuStateEnum, Func<Task>>
        {
            { MenuStateEnum.LoggedOut, HandleLoggedOutMenu},
            { MenuStateEnum.Main, HandleMainMenu},
            { MenuStateEnum.ManageCustomers, HandleManageCustomersMenu},
            { MenuStateEnum.TestingMenu, TestingMenuHandler},
            //{ MenuStateEnum.ResultMenu, ResultMenuHandler},
            { MenuStateEnum.BookingMenu, BookingMenuHandler},
            {MenuStateEnum.ViewBookingMenu, ViewBookingHandler}
        };
        _menuState = MenuStateEnum.Main; //säger var vi startar menu state
        _queryHandler = queryHandler;

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

    #region menus
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
            case "6": //view all bookings
                _menuState = MenuStateEnum.ViewBookingMenu;
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
        
        await ResultMenuHandler(await _queryHandler.CustomerQueries.SearchCustomer("name", name));
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

    private async Task ViewBookingHandler()
    {
        switch (GetInput())
        {
           case "1": //view all bookings 
               await AllBookings();
               break;
           case "2": //find booking by customer name
               await SearchBookingsByName();
               break;
           case "3":
               _menuState = MenuStateEnum.Main;
               break;
        }
        
    }

    private async Task AllBookings()
    {
        List<BookingsView> AllBookings = await _queryHandler.BookingViewQueries.GetAllBookings();
        foreach (var bookings in AllBookings)
        {
            Console.Write("Bookings id: " + bookings.BookingsId + "  |  ");
            Console.Write("StartDate: " + bookings.StartDate + "  |  ");
            Console.Write("EndDate: " + bookings.EndDate + "  |  ");
            Console.Write("Status: " + bookings.Status + "  |  ");
            Console.Write("Addons: " + bookings.Addons + "  |  ");
            Console.Write("Room size: " + bookings.EndDate + "  |  ");
            Console.Write("Room number: " + bookings.Status + "  |  ");
            Console.Write("Customer name: " + bookings.CustomerName + "  |  ");
            Console.WriteLine("Price: " + bookings.Price + "  |  ");
        }
    }

    private async Task SearchBookingsByName()
    {
        
        Console.WriteLine("Please enter the name you want to search for:");
        string? inputName = Console.ReadLine(); 

        if (string.IsNullOrWhiteSpace(inputName))
        {
            Console.WriteLine("Invalid input. Please provide a valid name.");
            return;
        }

        try
        {
            List<BookingsView> searchResults = await _queryHandler.BookingViewQueries.SearchBookingByName(inputName);

            if (searchResults.Count == 0)
            {
                Console.WriteLine($"No bookings found for the name: {inputName}");
            }
            else
            {
                Console.WriteLine($"Bookings found for the name: {inputName}");
                foreach (var booking in searchResults)
                {
                    Console.WriteLine($"Booking ID: {booking.BookingsId}, " +
                                      $"Start Date: {booking.StartDate}, " +
                                      $"End Date: {booking.EndDate}, " +
                                      $"Status: {booking.Status}, " +
                                      $"Room Size: {booking.Size}, " +
                                      $"Room Number: {booking.RoomNumber}, " +
                                      $"Customer Name: {booking.CustomerName}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while searching for bookings: {ex.Message}");
        }
    }
    
    private async Task BookingMenuHandler()
    {
        Console.WriteLine("we have hotels in these cities Innsbruck, Chamonix, Aspen, Zermatt, Hakuba"); //i vas lazy on a friday :)
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
                /*string searchName = "Lainey Tuffield"; 
                List<BookingsView> searchBookingsName = await _queryHandler.BookingViewQueries.SearchBookingByName(searchName);

                if (searchBookingsName.Count == 0)
                {
                    Console.WriteLine("No bookings found for the specified customer name.");
                }
                else
                {
                    foreach (var bookings in searchBookingsName)
                    {
                        Console.Write("Bookings id: " + bookings.BookingsId + "  |  ");
                        Console.Write("Start Date: " + bookings.StartDate + "  |  ");
                        Console.Write("End Date: " + bookings.EndDate + "  |  ");
                        Console.Write("Status: " + bookings.Status + "  |  ");
                        Console.Write("Room size: " + bookings.Size + "  |  ");
                        Console.WriteLine("Room number: " + bookings.RoomNumber);
                    }
                }*/
                    
                /* List<BookingsView> AllBookings = await _queryHandler.BookingViewQueries.GetAllBookings();
                 foreach (var bookings in allBookings)
                 {
                     Console.Write("Bookings id: " + bookings.BookingsId + "  |  ");
                     Console.Write("StartDate: " + bookings.StartDate + "  |  ");
                     Console.Write("Room size: " + bookings.EndDate + "  |  ");
                     Console.WriteLine("Room number: " + bookings.Status + "  |  ");
                 } */
                // ALL: await _queryHandler.CustomerQueries.AllCustomers(); 
                // SEARCH: await _queryHandler.CustomerQueries.SearchCustomer("name", "Thom");
                // INSERT: await _queryHandler.CustomerQueries.InsertCustomer("David maguy", "Davidmaguy123@gmail.com", "070-418-9995", 1999);
                // DELETE: await _queryHandler.CustomerQueries.DeleteCustomer(201);
                // UPDATE: await _queryHandler.CustomerQueries.UpdateCustomer(200, name: "Stinky Carl", email: "orb@gmail.com");
              
                break;
            case "2": //david testing
                await ResultMenuHandler(await _queryHandler.TestQueries.TestQuery());
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
    #endregion
    
    private async Task ResultMenuHandler(List<Customer> customers)
    {
        
        //MenuHelpers.CalculateMaxWidthOfAllProperties(customers); // test



        Console.ReadLine();
        int maxIdLength = customers.Max(c => c.Id.ToString().Length);
        int maxNameLength = customers.Max(c => (c.Name ?? "").Length);
        int maxEmailLength = customers.Max(c => (c.Email ?? "").Length);
        int maxPhonenumberLength = customers.Max(c => (c.PhoneNumber ?? "").Length);
        int maxBirthyearLength = customers.Max(c => c.Birthyear.ToString().Length);
        int extraTextLength = "|  Id:   |  Name:   |  Email:   |  PhoneNumber:   |  BirthYear:   |".Length;
        int totalWidth = maxIdLength + maxNameLength + maxEmailLength + maxPhonenumberLength + maxBirthyearLength + extraTextLength;
        int customersStart = 0;
        int customersEnd = 10;
        ConsoleKeyInfo key;
        int row = 0;
        Console.Clear();

        string[] headerStrings = MenuHelpers.CreateHeaderStrings("Contrary to popular belief, Lorem Ipsum is not " +
                                                                 "simply random text. It has roots in a piece of " +
                                                                 "classical Latin literature from 45 BC, making " +
                                                                 "it over 2000 years old. Richard McClintock, a " +
                                                                 "Latin professor at Hampden-Sydney College in Virginia, " +
                                                                 "looked up one of the more obscure Latin words, " +
                                                                 "consectetur, from a Lorem Ipsum passage, and going " +
                                                                 "through the cites of the word in classical literature, " +
                                                                 "discovered the undoubtable source. Lorem Ipsum comes from " +
                                                                 "sections 1.10.32 and 1.10.33 of de Finibus Bonorum et Malorum " +
                                                                 "(The Extremes of Good and Evil) by Cicero, written in 45 BC. This book " +
                                                                 "is a treatise on the theory of ethics, very popular during the Renaissance. " +
                                                                 "The first line of Lorem Ipsum, Lorem ipsum dolor sit amet.., " +
                                                                 "comes from a line in section 1.10.32", totalWidth);

        foreach (var text in headerStrings)
        {
            Console.WriteLine(text);
        }
        
        (int left, int top) = Console.GetCursorPosition();
        
        Console.CursorVisible = false;
        bool test = true;
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
                    Console.WriteLine($"|  ID: {customer.Id.ToString().PadRight(maxIdLength)}  |  " +
                                      $"Name: {(customer.Name ?? "null").PadRight(maxNameLength)}  |  " +
                                      $"Email: {(customer.Email ?? "null").PadRight(maxEmailLength)}  |  " +
                                      $"PhoneNumber: {(customer.PhoneNumber ?? "null").PadRight(maxPhonenumberLength)}  |  " +
                                      $"BirthYear: {customer.Birthyear.ToString().PadRight(maxBirthyearLength)}" + "  <--");
                }
                else
                {
                    ConsoleColor color1 = ConsoleColor.DarkMagenta;
                    ConsoleColor color2 = ConsoleColor.DarkCyan;
                    
                    
                    Console.Write("|  ");
                    
                    MenuHelpers.PrintWithColor(customer.Id.ToString().PadRight(maxIdLength), 
                        color2, 
                        "ID: ",
                        color1,
                        "  |  ",
                        color1);
                    
                    MenuHelpers.PrintWithColor((customer.Name ?? "null").PadRight(maxNameLength), 
                        color2,
                        "Name: ",
                        color1,
                        "  |  ",
                        color1);

                    MenuHelpers.PrintWithColor((customer.Email ?? "null").PadRight(maxEmailLength),
                        color2,
                        "Email: ",
                        color1,
                        "  |  ",
                        color1);

                    MenuHelpers.PrintWithColor((customer.PhoneNumber ?? "null").PadRight(maxPhonenumberLength),
                        color2,
                        "PhoneNumber: ",
                        color1,
                        "  |  ",
                        color1);

                    MenuHelpers.PrintWithColor(customer.Birthyear.ToString().PadRight(maxBirthyearLength),
                        color2,
                        "BirthYear: ",
                        color1);
                    
                    Console.WriteLine("  |  ");
                }
            }
            Console.ResetColor();

            string[] optionStrings = MenuHelpers.createOptionFooterStrings(_menuOptions[_menuState].ToList(), totalWidth);

            foreach (var text in optionStrings)
            {
                Console.WriteLine(text);
            }
   
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
                        if (row >= customers.Count - 6)
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