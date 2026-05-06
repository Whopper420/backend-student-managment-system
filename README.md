# SimpleApp

A full-stack education management application built with .NET 8, featuring a PostgreSQL-backed REST API and a Blazor WebAssembly frontend.

## Project Structure

| Directory | Description |
|---|---|
| `Backend/` | Minimal API with Dapper + PostgreSQL |
| `Frontend/` | Blazor WebAssembly SPA |
| `SimpleApp.Shared/` | Shared models between client and server |
| `Data/` | Data access utilities |

## Features

- **User Management** - Create and view users with roles
- **Course Management** - Create and browse courses with teacher assignments
- **Student Enrollment** - Enroll users in courses with validation
- **Dashboard** - Overview stats for users, courses, and enrollments
- **Relational Queries** - View students per course and courses per student

## Tech Stack

- **.NET 8** - Framework
- **Blazor WebAssembly** - Frontend SPA
- **Dapper** - Micro-ORM for data access
- **PostgreSQL** - Database
- **Bootstrap 5** - UI styling

## Getting Started

### Prerequisites

- .NET 8 SDK
- PostgreSQL

### Database Setup

Create a PostgreSQL database and update the connection string in `Backend/Program.cs`:

```
Host=localhost;Port=5432;Database=edu_db;Username=edu_dev;Password=password
```

### Running the App

1. Start the backend:
   ```bash
   cd Backend
   dotnet run
   ```

2. Start the frontend:
   ```bash
   cd Frontend
   dotnet run
   ```

The API runs on `http://localhost:5000` and the frontend connects to it via `HttpClient`.

## API Endpoints

| Method | Endpoint | Description |
|---|---|---|
| GET | `/users` | List all users |
| POST | `/users` | Create a user |
| GET | `/courses` | List all courses |
| POST | `/courses` | Create a course |
| GET | `/assignments` | List all assignments |
| POST | `/assignments` | Create an assignment |
| POST | `/enroll` | Enroll a user in a course |
| GET | `/enrollments` | List all enrollments |
| GET | `/users/{id}/courses` | Get a user's enrolled courses |
| GET | `/courses/{id}/users` | Get users enrolled in a course |

## Frontend Pages

| Route | Description |
|---|---|
| `/` | Dashboard with stats |
| `/users` | User list with add form |
| `/courses` | Course list with add form |
| `/enrollment` | Enroll a student (dropdown selectors) |
| `/enrollments` | View all enrollments |
| `/student/{id}` | View a student's courses |
| `/course/{id}` | View students in a course |
