using System;
namespace app;
/*View, skapa, avboka, ändra


{
    public class Booking
    {
        public void CreateBookingMethod()
        {
            Console.WriteLine("Book your flight!");

            // Get user information
            Console.WriteLine("Enter your name:");
            string name = Console.ReadLine();

            Console.WriteLine("Enter your destination:");
            string destination = Console.ReadLine();

            Console.WriteLine("Enter the date (yyyy-mm-dd):");
            string date = Console.ReadLine();

            // Confirm Booking
            Console.WriteLine($"\nBooking confirmed for {name} to {destination} on {date}.");

            booking = new Booking(name, destination, date);
        }



        public void ViewBoooking()
        {
            
        }
    }
}    
*/
public class ViewBooking
{
    public string name { get; set; }
    public string Destination { get; set; }
    public string Date { get; set; }
}
