using Dapper;
using Npgsql;
using SimpleApp.Shared.Models;

namespace Backend.Data;

public class UserRepository
{
    private readonly string _connString =
        "Host=localhost;Port=5432;Database=edu_db;Username=edu_dev;Password=password;";

    public List<User> GetUsers()
    {
        using var conn = new NpgsqlConnection(_connString);

        return conn.Query<User>("SELECT id, name, role FROM users").ToList();
    }
}