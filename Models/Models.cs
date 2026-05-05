namespace SimpleApp.Models;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Role { get; set; } = "";
}

public class Course
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public int TeacherId { get; set; }
}

public class Assignment
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public int CourseId { get; set; }
}