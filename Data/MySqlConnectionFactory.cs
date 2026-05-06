using Npgsql;
namespace SimpleApp.Data;

var conn = new NpgsqlConnection("Host=localhost;Port=5432;Database=edu_db;Username=edu_dev;Password=password;");
conn.Open();

var cmd = new NpgsqlCommand("SELECT * FROM users", conn);
var reader = cmd.ExecuteReader();