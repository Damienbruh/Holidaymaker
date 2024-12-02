using System.Collections.Generic;

namespace app
{
    public class Booking
    {
        public string Name { get; set; }
        public string Destination { get; set; }
        public string Date { get; set; }

        public Booking(string name, string destination, string date)
        {
            Name = name;
            Destination = destination;
            Date = date;
        }
    }

    public class BookingManager
    {
        private List<Booking> bookings = new List<Booking>();

        public void CreateBookingMethod()
        {
            Console.WriteLine("Book your flight!");

            Console.WriteLine("Enter your name:");
            string name = Console.ReadLine();

            Console.WriteLine("Enter your destination:");
            string destination = Console.ReadLine();

            Console.WriteLine("Enter the date (yyyy-mm-dd):");
            string date = Console.ReadLine();

            Console.WriteLine($"\nBooking confirmed for {name} to {destination} on {date}.");

            Booking newBooking = new Booking(name, destination, date);
            bookings.Add(newBooking);
        }

        public void ViewBookings()
        {
            if (bookings.Count == 0)
            {
                Console.WriteLine("No bookings available.");
                return;
            }

            Console.WriteLine("\nAll Bookings:");

            foreach (var booking in bookings)
            {
                Console.WriteLine($"Name: {booking.Name}, Destination: {booking.Destination}, Date: {booking.Date}");
            }
        }
    }
}