using Dapper;
using Npgsql;
using SimpleApp.Shared.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod());
});

var app = builder.Build();

app.UseRouting();
app.UseCors("AllowAll");

app.MapGet("/", () => "OK");

string connString =
    "Host=localhost;Port=5432;Database=edu_db;Username=edu_dev;Password=password";



app.MapGet("/users", () =>
{
    using var conn = new NpgsqlConnection(connString);
    return conn.Query<User>(
        "SELECT id, name, role FROM users"
    ).ToList();
});

app.MapPost("/users", (User user) =>
{
    using var conn = new NpgsqlConnection(connString);

    conn.Execute(@"
        INSERT INTO users(name, role)
        VALUES (@Name, @Role)
    ", user);

    return Results.Ok("User added");
});


app.MapGet("/courses", () =>
{
    using var conn = new NpgsqlConnection(connString);

    return conn.Query<Course>(@"
        SELECT id, title, teacher_id AS TeacherId
        FROM courses
    ").ToList();
});

app.MapPost("/courses", (Course course) =>
{
    using var conn = new NpgsqlConnection(connString);

    conn.Execute(@"
        INSERT INTO courses(title, teacher_id)
        VALUES (@Title, @TeacherId)
    ", course);

    return Results.Ok("Course added");
});

app.MapGet("/assignments", () =>
{
    using var conn = new NpgsqlConnection(connString);

    return conn.Query<Assignment>(@"
        SELECT id, title, course_id AS CourseId
        FROM assignments
    ").ToList();
});

app.MapPost("/assignments", (Assignment a) =>
{
    using var conn = new NpgsqlConnection(connString);

    conn.Execute(@"
        INSERT INTO assignments(title, course_id)
        VALUES (@Title, @CourseId)
    ", a);

    return Results.Ok("Assignment added");
});


app.MapPost("/enroll", (EnrollmentRequest req) =>
{
    using var conn = new NpgsqlConnection(connString);

    var userExists = conn.ExecuteScalar<int>(
        "SELECT COUNT(1) FROM users WHERE id = @Id",
        new { Id = req.UserId });

    var courseExists = conn.ExecuteScalar<int>(
        "SELECT COUNT(1) FROM courses WHERE id = @Id",
        new { Id = req.CourseId });

    if (userExists == 0 || courseExists == 0)
        return Results.BadRequest("Invalid user or course ID");

    conn.Execute(@"
        INSERT INTO enrollments(user_id, course_id)
        VALUES (@UserId, @CourseId)
    ", req);

    return Results.Ok("Enrolled");
});


app.MapGet("/users/{id}/courses", (int id) =>
{
    using var conn = new NpgsqlConnection(connString);

    return conn.Query<Course>(@"
        SELECT c.id, c.title, c.teacher_id AS TeacherId
        FROM courses c
        JOIN enrollments e ON e.course_id = c.id
        WHERE e.user_id = @id
    ", new { id }).ToList();
});

app.MapGet("/enrollments", () =>
{
    using var conn = new NpgsqlConnection(connString);

    return conn.Query(@"
        SELECT e.id, u.name AS User, c.title AS Course
        FROM enrollments e
        JOIN users u ON e.user_id = u.id
        JOIN courses c ON e.course_id = c.id
    ").ToList();
});


app.MapGet("/courses/{id}/users", (int id) =>
{
    using var conn = new NpgsqlConnection(connString);

    return conn.Query<User>(@"
        SELECT u.id, u.name, u.role
        FROM users u
        JOIN enrollments e ON e.user_id = u.id
        WHERE e.course_id = @id
    ", new { id }).ToList();
});

app.Run();