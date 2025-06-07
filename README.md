# Locker Room Vibes CMS

Locker Room Vibes CMS is a RESTful backend API built with ASP.NET Core MVC and Entity Framework Core that enables football teams to curate and manage music playlists tailored to their team’s mood and atmosphere. Whether the team needs motivation, focus, or relaxation, staff can create playlists that fit those moods and associate them with specific teams.

This system provides full management of:

- **Teams** — Football teams within the system.
- **Playlists** — Collections of music tracks designed to evoke specific moods for a team.
- **Tracks** — Individual songs or audio pieces.
- **PlaylistTracks** — The many-to-many relationship connecting tracks to playlists.

By organizing music in this way, the API helps football clubs build an engaging and supportive environment through mood-based music curation.

---

## Technologies Used

- ASP.NET Core MVC (Web API)
- Entity Framework Core (Code First)
- SQL Server (or any EF Core supported database)
- Dependency Injection and Service Layer pattern
- JSON-based RESTful API endpoints

---

## Features

- Full CRUD operations for Teams, Playlists, Tracks, and PlaylistTracks
- Validation and error handling for robust API responses
- Relational data support with entity relationships (e.g., Teams to Playlists, Playlists to Tracks)
- Designed for internal club use to manage team music moods efficiently

---

## Setup Instructions

1. Clone the repository:

   ```bash
   git clone https://github.com/YourUsername/LockerRoomVibesCms.git

Navigate to the project folder:

cd LockerRoomVibesCms

Configure your database connection string in appsettings.json.

Run database migrations:
dotnet ef database update

Run the project:
dotnet run

Access the API via https://localhost:{port}/api

API Endpoints (Summary)
GET /api/teams — List all teams

GET /api/teams/{id} — Get a specific team

POST /api/teams — Create a new team

PUT /api/teams/{id} — Update a team

DELETE /api/teams/{id} — Delete a team

Similar CRUD endpoints exist for playlists, tracks, and playlisttracks



