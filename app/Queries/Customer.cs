using System.Collections.Generic;
using System.Threading.Tasks;
using Npgsql;

namespace app.Queries;

public class Customer
{
    public int CustomerId { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public int BirthYear { get; set; }
}

public class CustomerQuery
{
    private readonly string _connectionString;

    public CustomerQuery(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<List<Customer>> GetCustomersAsync()
    {
        var customers = new List<Customer>();
        string query = @"SELECT id, name, email, phone_number, birthyear FROM customers";

        using (var connection = new NpgsqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (var command = new NpgsqlCommand(query, connection))
            {
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        customers.Add(new Customer
                        {
                            CustomerId = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Email = reader.GetString(2),
                            PhoneNumber = reader.GetString(3),
                            BirthYear = reader.GetInt32(4)
                        });
                    }
                }
            }
        }

        return customers;
    }
}