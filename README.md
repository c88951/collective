# Collective Art Collection API

A .NET API for managing art collections with a comprehensive tagging system.

## Project Structure

The solution consists of two projects:

1. **CollectiveData** - Data layer containing:
   - Models
   - DTOs (Data Transfer Objects)
   - Repositories
   - Data configurations
   - Database context

2. **CollectiveAPI** - API layer containing:
   - Controllers
   - API endpoints
   - Swagger UI

## Features

- **Comprehensive Tagging System**:
  - Structured tags organized by categories
  - User-defined custom tags
  - Multilingual support for tag categories

- **Art Collection Management**:
  - Track artwork details (title, artist, medium, etc.)
  - Organize artworks into collections
  - Associate tags with artworks

## Getting Started

### Prerequisites

- .NET 8.0 SDK

### Running the API

1. Clone the repository
2. Navigate to the project directory
3. Run the API:

```bash
cd CollectiveAPI
dotnet run
```

4. Access the Swagger UI at http://localhost:5000

## API Documentation

The API provides endpoints for:

- Managing tag categories
- Creating and managing tags
- Associating tags with artworks
- Managing art collections

For detailed API documentation, refer to the Swagger UI.

## Development

The project uses:

- Entity Framework Core for data access
- In-memory database for development and testing
- Repository pattern for data access abstraction
