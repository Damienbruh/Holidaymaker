﻿
namespace app;


class Program
{
    static void Main()
    {
        Database db = new();
        TestQueries queries = new(db.Connection());
        
        queries.AllCustomers();
        //program.loginMenu();
        
    }
    /*public void loginMenu()
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
        
    
    }*/
}




