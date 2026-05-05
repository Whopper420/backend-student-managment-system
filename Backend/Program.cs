using SimpleApp.Shared.Models;
using MySql.Data.MySqlClient;
using Backend.Data;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
        policy
            .WithOrigins("http://localhost:5188")
            .AllowAnyHeader()
            .AllowAnyMethod());
});
var app = builder.Build();

app.UseRouting();
app.UseCors("AllowFrontend");


var db = new MySqlConnectionFactory();

app.MapGet("/", () => "Edu system running");


app.MapGet("/users", () =>
{
    var list = new List<User>();

    using var conn = db.CreateConnection();
    conn.Open();

    var cmd = new MySqlCommand("SELECT id, name, role FROM users", conn);
    var reader = cmd.ExecuteReader();

    while (reader.Read())
    {
        list.Add(new User
        {
            Id = reader.GetInt32(0),
            Name = reader.GetString(1),
            Role = reader.GetString(2)
        });
    }

    return list;
});

app.MapPost("/users", (User user) =>
{
    using var conn = db.CreateConnection();
    conn.Open();

    var cmd = new MySqlCommand(
        "INSERT INTO users(name, role) VALUES(@name, @role)", conn);

    cmd.Parameters.AddWithValue("@name", user.Name);
    cmd.Parameters.AddWithValue("@role", user.Role);

    cmd.ExecuteNonQuery();

    return "User added";
});


app.MapGet("/courses", () =>
{
    var list = new List<Course>();

    using var conn = db.CreateConnection();
    conn.Open();

    var cmd = new MySqlCommand("SELECT id, title, teacher_id FROM courses", conn);
    var reader = cmd.ExecuteReader();

    while (reader.Read())
    {
        list.Add(new Course
        {
            Id = reader.GetInt32(0),
            Title = reader.GetString(1),
            TeacherId = reader.GetInt32(2)
        });
    }

    return list;
});

app.MapPost("/courses", (Course course) =>
{
    using var conn = db.CreateConnection();
    conn.Open();

    var cmd = new MySqlCommand(
        "INSERT INTO courses(title, teacher_id) VALUES(@title, @teacher_id)", conn);

    cmd.Parameters.AddWithValue("@title", course.Title);
    cmd.Parameters.AddWithValue("@teacher_id", course.TeacherId);

    cmd.ExecuteNonQuery();

    return "Course added";
});


app.MapGet("/assignments", () =>
{
    var list = new List<Assignment>();

    using var conn = db.CreateConnection();
    conn.Open();

    var cmd = new MySqlCommand("SELECT id, title, course_id FROM assignments", conn);
    var reader = cmd.ExecuteReader();

    while (reader.Read())
    {
        list.Add(new Assignment
        {
            Id = reader.GetInt32(0),
            Title = reader.GetString(1),
            CourseId = reader.GetInt32(2)
        });
    }

    return list;
});

app.MapPost("/assignments", (Assignment a) =>
{
    using var conn = db.CreateConnection();
    conn.Open();

    var cmd = new MySqlCommand(
        "INSERT INTO assignments(title, course_id) VALUES(@title, @course_id)", conn);

    cmd.Parameters.AddWithValue("@title", a.Title);
    cmd.Parameters.AddWithValue("@course_id", a.CourseId);

    cmd.ExecuteNonQuery();

    return "Assignment added";
});

app.MapPost("/enroll", (EnrollmentRequest req) =>
{
    using var conn = db.CreateConnection();
    conn.Open();

    var userCmd = new MySqlCommand("SELECT COUNT(*) FROM users WHERE id=@id", conn);
    userCmd.Parameters.AddWithValue("@id", req.UserId);

    var userExists = Convert.ToInt32(userCmd.ExecuteScalar()) > 0;

    var courseCmd = new MySqlCommand("SELECT COUNT(*) FROM courses WHERE id=@id", conn);
    courseCmd.Parameters.AddWithValue("@id", req.CourseId);

    var courseExists = Convert.ToInt32(courseCmd.ExecuteScalar()) > 0;

    if (!userExists || !courseExists)
        return Results.BadRequest("Invalid user or course ID");

    var insert = new MySqlCommand(
        "INSERT INTO enrollments(user_id, course_id) VALUES(@u, @c)", conn);

    insert.Parameters.AddWithValue("@u", req.UserId);
    insert.Parameters.AddWithValue("@c", req.CourseId);

    insert.ExecuteNonQuery();

    return Results.Ok("Enrolled");
});
app.MapGet("/users/{id}/courses", (int id) =>
{
    var list = new List<Course>();

    using var conn = db.CreateConnection();
    conn.Open();

    var cmd = new MySqlCommand(@"
        SELECT c.id, c.title, c.teacher_id
        FROM courses c
        JOIN enrollments e ON e.course_id = c.id
        WHERE e.user_id = @id
    ", conn);

    cmd.Parameters.AddWithValue("@id", id);

    var reader = cmd.ExecuteReader();

    while (reader.Read())
    {
        list.Add(new Course
        {
            Id = reader.GetInt32(0),
            Title = reader.GetString(1),
            TeacherId = reader.GetInt32(2)
        });
    }

    return list;
});

app.MapGet("/enrollments", () =>
{
    var list = new List<object>();

    using var conn = db.CreateConnection();
    conn.Open();

    var cmd = new MySqlCommand(@"
        SELECT e.id, u.name, c.title
        FROM enrollments e
        JOIN users u ON e.user_id = u.id
        JOIN courses c ON e.course_id = c.id
    ", conn);

    var reader = cmd.ExecuteReader();

    while (reader.Read())
    {
        list.Add(new
        {
            Id = reader.GetInt32(0),
            User = reader.GetString(1),
            Course = reader.GetString(2)
        });
    }

    return list;
});

app.Run();