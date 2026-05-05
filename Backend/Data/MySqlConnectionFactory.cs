using MySql.Data.MySqlClient;

namespace Backend.Data;

public class MySqlConnectionFactory
{
    private readonly string _connectionString;

    public MySqlConnectionFactory()
    {
        _connectionString =
            "server=172.18.64.1;port=3306;database=edu_db;user=edu_dev;password=password";
    }

    public MySqlConnection CreateConnection()
    {
        return new MySqlConnection(_connectionString);
    }
}