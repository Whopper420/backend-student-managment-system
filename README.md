# SimpleApp

A full-stack education management application built with .NET 8, featuring a PostgreSQL-backed REST API and a Blazor WebAssembly frontend.

## Project Structure

| Directory           | Description                             |
| ------------------- | --------------------------------------- |
| `Backend/`          | Minimal API with Dapper + PostgreSQL    |
| `Frontend/`         | Blazor WebAssembly SPA                  |
| `SimpleApp.Shared/` | Shared models between client and server |
| `Data/`             | Data access utilities                   |

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

- sudo apt update
- sudo apt install -y dotnet-sdk-8.0
- dotnet restore

### Database Setup

Create a PostgreSQL database and update the connection string in `Backend/Program.cs`:

```
Host=localhost;Port=5432;Database=edu_db;Username=edu_dev;Password=password
```

### Database Schema

```
-- USERS
CREATE TABLE Users (
    Id SERIAL PRIMARY KEY,
    Name TEXT NOT NULL,
    Role TEXT NOT NULL
);

-- COURSES
CREATE TABLE Courses (
    Id SERIAL PRIMARY KEY,
    Title TEXT NOT NULL,
    TeacherId INT NOT NULL,
    FOREIGN KEY (TeacherId) REFERENCES Users(Id)
);

-- ASSIGNMENTS
CREATE TABLE Assignments (
    Id SERIAL PRIMARY KEY,
    Title TEXT NOT NULL,
    CourseId INT NOT NULL,
    FOREIGN KEY (CourseId) REFERENCES Courses(Id)
);

-- ENROLLMENT REQUESTS
CREATE TABLE EnrollmentRequests (
    UserId INT NOT NULL,
    CourseId INT NOT NULL,
    PRIMARY KEY (UserId, CourseId),
    FOREIGN KEY (UserId) REFERENCES Users(Id),
    FOREIGN KEY (CourseId) REFERENCES Courses(Id)
);
```

### Running the App (requires 2 terminals one for backend api and one for frontend)

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

| Method | Endpoint              | Description                    |
| ------ | --------------------- | ------------------------------ |
| GET    | `/users`              | List all users                 |
| POST   | `/users`              | Create a user                  |
| GET    | `/courses`            | List all courses               |
| POST   | `/courses`            | Create a course                |
| GET    | `/assignments`        | List all assignments           |
| POST   | `/assignments`        | Create an assignment           |
| POST   | `/enroll`             | Enroll a user in a course      |
| GET    | `/enrollments`        | List all enrollments           |
| GET    | `/users/{id}/courses` | Get a user's enrolled courses  |
| GET    | `/courses/{id}/users` | Get users enrolled in a course |

## Frontend Pages

| Route           | Description                           |
| --------------- | ------------------------------------- |
| `/`             | Dashboard with stats                  |
| `/users`        | User list with add form               |
| `/courses`      | Course list with add form             |
| `/enrollment`   | Enroll a student (dropdown selectors) |
| `/enrollments`  | View all enrollments                  |
| `/student/{id}` | View a student's courses              |
| `/course/{id}`  | View students in a course             |
