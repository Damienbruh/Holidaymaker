using app.Queries.TableObjects;
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

    public async Task<List<Customer>> SearchCustomer(string columnName, string searchTerm)
    {
       
            List<Customer> customers = new List<Customer>();
            var query = $"SELECT * FROM customers WHERE {columnName} ILIKE @searchTerm";
            await using (var cmd = _database.CreateCommand(query))
            {
                cmd.Parameters.AddWithValue("searchTerm", $"%{searchTerm}%");

                await using (var reader = await cmd.ExecuteReaderAsync())
                {
                    if (!reader.HasRows)
                    {
                        Console.WriteLine("Customer does not exist");
                        
                    }
                    customers.Add(new Customer
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.IsDBNull(1) ? null : reader.GetString(1),
                        Email = reader.IsDBNull(2) ? null : reader.GetString(2),
                        PhoneNumber = reader.IsDBNull(3) ? null : reader.GetString(3),
                        Birthyear = reader.GetInt32(4)
                    });
                }
                
                Console.WriteLine("done");
                return customers;
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

    public async Task InsertCustomer(string name, string email, string phoneNumber, int birthyear)
    {
        try
        {
            const string query = @"INSERT INTO customers (name, email, phone_number, birthyear)
                                   VALUES ($1, $2, $3, $4)";
            await using var cmd = _database.CreateCommand(query);
            cmd.Parameters.AddWithValue(name);
            cmd.Parameters.AddWithValue(email);
            cmd.Parameters.AddWithValue(phoneNumber);
            cmd.Parameters.AddWithValue(birthyear);

            var rowsAffected = await cmd.ExecuteNonQueryAsync();
            Console.WriteLine(rowsAffected > 0
                ? "Customer successfully added."
                : "Customer does not exist.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    } 
    
    public async Task<int> InsertCustomerReturn(string name, string email, string phoneNumber, int birthyear)
    {
   
        const string query = @"INSERT INTO customers (name, email, phone_number, birthyear)
                               VALUES ($1, $2, $3, $4)
                               RETURNING customer_id";
        await using var cmd = _database.CreateCommand(query);
        cmd.Parameters.AddWithValue(name);
        cmd.Parameters.AddWithValue(email);
        cmd.Parameters.AddWithValue(phoneNumber);
        cmd.Parameters.AddWithValue(birthyear);

        var newId = await cmd.ExecuteScalarAsync();
            
        return Convert.ToInt32(newId);
    }
    
    public async Task UpdateCustomer(int customerId, string? name = null, string? email = null, string? phoneNumber = null, int? birthYear = null)
    {
        try
        {
           
            var updateParts = new List<string>();

            if (name != null) updateParts.Add("name = @name");
            if (email != null) updateParts.Add("email = @Email");
            if (phoneNumber != null) updateParts.Add("phone_number = @phoneNumber");
            if (birthYear.HasValue) updateParts.Add("birthyear = @birthYear");

            if (updateParts.Count == 0)
            {
                Console.WriteLine("No fields to update.");
                return;
            }

            var updateQuery = $"UPDATE customers SET {string.Join(", ", updateParts)} WHERE customer_id = @id";

            await using (var cmd = _database.CreateCommand(updateQuery))
            {
                cmd.Parameters.AddWithValue("id", customerId);

                if (name != null) cmd.Parameters.AddWithValue("name", name);
                if (email != null) cmd.Parameters.AddWithValue("email", email);
                if (phoneNumber != null) cmd.Parameters.AddWithValue("phoneNumber", phoneNumber);
                if (birthYear.HasValue) cmd.Parameters.AddWithValue("birthYear", birthYear.Value);

                var rowsAffected = await cmd.ExecuteNonQueryAsync();
                Console.WriteLine(rowsAffected > 0
                    ? "Customer updated successfully."
                    : "No customer found with the specified ID.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}


