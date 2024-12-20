using DotNetEnv;
using Npgsql;

namespace app
{
    public class Database
    {

        private NpgsqlDataSource _connection;

        public NpgsqlDataSource Connection()
        {
            return _connection;
        }

        public Database()
        {
            _connection = NpgsqlDataSource.Create(Env.GetString("connectString"));
            Console.WriteLine("Connection established");
            using var conn = _connection.OpenConnection();
            Console.WriteLine("Connected successfully");
        }

    }
}