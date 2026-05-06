using System.Net.Http.Json;
using SimpleApp.Shared.Models;

namespace Frontend.Services;

public class ApiService
{
    private readonly HttpClient _http;

    public ApiService(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<User>> GetUsers()
    {
        return (await _http.GetFromJsonAsync<List<User>>("users")) ?? new();
    }

    public async Task AddUser(User user)
    {
        await _http.PostAsJsonAsync("users", user);
    }

    public async Task DeleteUser(int id)
    {
        await _http.DeleteAsync($"users/{id}");
    }

    public async Task<List<Course>> GetCourses()
    {
        return (await _http.GetFromJsonAsync<List<Course>>("courses")) ?? new();
    }

    public async Task AddCourse(Course course)
    {
        await _http.PostAsJsonAsync("courses", course);
    }

    public async Task<List<Assignment>> GetAssignments()
    {
        return (await _http.GetFromJsonAsync<List<Assignment>>("assignments")) ?? new();
    }

    public async Task AddAssignment(Assignment assignment)
    {
        await _http.PostAsJsonAsync("assignments", assignment);
    }

    public async Task<string> EnrollUser(EnrollmentRequest request)
    {
        var response = await _http.PostAsJsonAsync("enroll", request);
        return await response.Content.ReadAsStringAsync();
    }

    public async Task<List<EnrollmentDto>> GetEnrollments()
    {
        return (await _http.GetFromJsonAsync<List<EnrollmentDto>>("enrollments")) ?? new();
    }

    public async Task<List<Course>> GetUserCourses(int userId)
    {
        return (await _http.GetFromJsonAsync<List<Course>>($"users/{userId}/courses")) ?? new();
    }

    public async Task<List<User>> GetCourseUsers(int courseId)
    {
        return (await _http.GetFromJsonAsync<List<User>>($"courses/{courseId}/users")) ?? new();
    }
}

public class EnrollmentDto
{
    public int Id { get; set; }
    public string? User { get; set; }
    public string? Course { get; set; }
}
