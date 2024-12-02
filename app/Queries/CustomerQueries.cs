using Npgsql;
using NpgsqlTypes;

namespace app.Queries;

public class CustomerQueries
{
    private NpgsqlDataSource _database;

    public CustomerQueries(NpgsqlDataSource database)
    {
        _database = database;
    }


    public async Task AllCustomers()
    {
        try
        {
            await using (var cmd = _database.CreateCommand("SELECT * FROM customers"))
            await using (var reader = await cmd.ExecuteReaderAsync())
                while
                    (await reader
                        .ReadAsync())
                {
                    Console.WriteLine($"Id: {reader.GetInt32(0)}," +
                                      $"Name: {reader.GetString(1)}," +
                                      $"email: {reader.GetString(2)}," +
                                      $"phone_number: {reader.GetString(3)}," +
                                      $"birthyear: {reader.GetInt32(4)}");
                }

            Console.WriteLine("done");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
            Console.WriteLine(ex.StackTrace);
        }
    }

    public async Task SearchCustomer(string columnName, String searchTerm)
    {
        try
        {
            var query = $"SELECT * FROM customers WHERE {columnName} ILIKE @searchTerm";
            await using (var cmd = _database.CreateCommand(query))
            {
                cmd.Parameters.AddWithValue("searchTerm", $"%{searchTerm}%");

                await using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        Console.WriteLine($"Id: {reader.GetInt32(0)}, " +
                                          $"Name: {reader.GetString(1)}, " +
                                          $"Email: {reader.GetString(2)}, " +
                                          $"Phone: {reader.GetString(3)}, " +
                                          $"BirthYear: {reader.GetInt32(4)}");
                    }
                }
            }
        } 
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }

    public async Task DeleteCustomer(int customerId)
    {
        try
        {
            var query = $"DELETE FROM customers WHERE customer_id = {customerId}";
            await using (var cmd = _database.CreateCommand(query))
            {
                cmd.Parameters.AddWithValue("customer_id", customerId);

                var rowsAffected = await cmd.ExecuteNonQueryAsync();
                Console.WriteLine(rowsAffected > 0
                    ? "Customer successfully deleted."
                    : "Customer does not exist.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }

    public async Task InsertCustomer(string name, string email, string phoneNumber, string birthyear)
    {
        try
        {
            var query = @"INSERT INTO customers (name, email, phone_number, birthyear)
                          VALUES (@name, @email, @phone_number, @birthyear)";
            await using (var cmd = _database.CreateCommand(query))
            {
                cmd.Parameters.AddWithValue("name", name);
                cmd.Parameters.AddWithValue("email", email);
                cmd.Parameters.AddWithValue("phone_number", phoneNumber);
                cmd.Parameters.AddWithValue("birthyear", birthyear);

                var rowsAffected = await cmd.ExecuteNonQueryAsync();
                Console.WriteLine(rowsAffected > 0
                    ? "Customer successfully added."
                    : "Customer does not exist.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }



 public async Task UpdateCustomer(int customerId, string? name = null, string? email = null,
     string? phoneNumber = null, string? birthyear = null)
 {

 }
}

