# Squares API

A backend API that stores 2D points and finds all the squares you can make from them. Built for a coding assignment.

## Project Structure

I split this into 4 projects to keep things organized (probably overkill for this size, but it's good practice):

- **Squares.Api** - The web API with controllers and endpoints
- **Squares.Domain** - The core models (Point, PointList, Square) and interfaces
- **Squares.Infrastructure** - Database stuff (EF Core + SQLite) and the square detection algorithm
- **Squares.Tests** - Unit tests for the square detection

## Tech Stack

- .NET 8
- ASP.NET Core (using controllers, not minimal APIs - I'm more familiar with controllers)
- EF Core with SQLite
- xUnit for testing
- Swagger for API documentation

## API Endpoints

All endpoints are under `/api/point-lists`:

- `POST /api/point-lists` - Create a new list of points

  - Send: `{ "name": "My Points", "points": [{ "x": -1, "y": -1 }, { "x": 1, "y": 1 }] }`
  - Returns the created list with an ID

- `GET /api/point-lists` - Get all point lists

- `GET /api/point-lists/{id}` - Get a specific list by ID

- `POST /api/point-lists/{id}/points` - Add a point to an existing list

  - Send: `{ "x": 3, "y": -2 }`

- `DELETE /api/point-lists/{id}/points/{pointId}` - Delete a point from a list

- `GET /api/point-lists/{id}/squares?countOnly=true` - Find squares in a list
  - If `countOnly=true`, just returns the number
  - Otherwise returns all squares with their points

You can test everything in Swagger at `https://localhost:5001/swagger` when the API is running.

## How Square Detection Works

I used a diagonal-based algorithm I found online. The idea is:

1. For every pair of points, calculate the midpoint and the distance between them (this could be a diagonal of a square)
2. Group pairs that have the same midpoint and same distance - these might form opposite corners of a square
3. Check if the diagonals are perpendicular (using dot product = 0) to make sure it's actually a square and not just a rectangle
4. Also skip pairs that share the same endpoint

The algorithm finds both axis-aligned squares and rotated ones, which is cool. I had to look up the math for checking if diagonals are perpendicular - turns out you use the dot product.

**Performance:** It's O(n²) which is fine for small to medium datasets (hundreds or thousands of points). If you had millions of points, you'd probably want to optimize this or use a database query, but for this assignment it should be fine.

The code has two methods: `CountSquares` (just counts, faster) and `GetSquares` (returns the actual square objects).

## Database

I'm using SQLite with EF Core. The database file (`squares.db`) gets created automatically in the API folder when you first run it. EF Core migrations are in `Squares.Infrastructure/Persistence/Migrations` - the API runs migrations automatically on startup so you don't have to worry about it.

## Getting Started

**What you need:**

- .NET 8 SDK installed

**Steps to run:**

1. Build the solution:

   ```
   dotnet build Squares.sln
   ```

2. Run the API:

   ```
   dotnet run --project Squares.Api --launch-profile https
   ```

   Note: If you get HTTPS errors, you might need to trust the dev certificate first:

   ```
   dotnet dev-certs https --trust
   ```

3. Open Swagger in your browser:
   ```
   https://localhost:5001/swagger
   ```

**Running tests:**

```
dotnet test Squares.sln
```

## Design Decisions & What I Learned

**Why multiple point lists?** I thought it would be more useful - you could have different datasets. Also makes the API more flexible. Probably overkill for this assignment but it's a common pattern.

**Why SQLite?** Easy to set up, no need for a separate database server. Good for learning EF Core. If this was production, I'd probably use SQL Server or PostgreSQL, but SQLite works fine for this.

**The square detection algorithm:** I googled "how to find squares in set of points" and found the diagonal-based approach. It seemed like the most straightforward way. I had to add a check to make sure diagonals are perpendicular (dot product = 0) to avoid counting rectangles as squares. The math was a bit tricky but I got it working.

**Project structure:** I separated things into Domain, Infrastructure, and Api layers. This is probably more than needed for a small project, but I wanted to practice clean architecture. It makes testing easier too.

**What I'd do differently:**

- Maybe add validation for duplicate points (right now you can add the same point twice)
- Add pagination if lists get really big
- Write more tests, especially for the API endpoints
- Add better error messages

**Time spent:** 8 hours.
