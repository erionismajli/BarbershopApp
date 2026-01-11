# Barbershop Scheduler Application

A full-stack barbershop scheduling application built with Angular frontend and .NET 8 backend using Onion Architecture.

## Features

- ✅ Light/Dark mode theme switching
- ✅ Create, view, update, and delete appointments
- ✅ Filter appointments by barber and date
- ✅ Real-time statistics (Total, Today, This Week)
- ✅ Responsive design
- ✅ Comprehensive error handling
- ✅ Input validation
- ✅ Clean architecture (Onion Architecture)

## Project Structure

```
barbershop-app/
├── frontend/          # Angular 18 application
│   ├── src/
│   │   ├── app/
│   │   │   ├── components/    # UI components
│   │   │   ├── services/      # API services
│   │   │   ├── models/        # TypeScript models
│   │   │   └── app.component.*
│   │   └── styles.css
│   └── package.json
│
└── backend/           # .NET 10 Web API
    └── src/
        ├── Barbershop.Domain/        # Domain entities and interfaces
        ├── Barbershop.Application/   # Business logic, DTOs, services
        ├── Barbershop.Infrastructure/ # Data access, repositories
        └── Barbershop.API/           # Web API controllers
```

## Prerequisites

- Node.js 18+ and npm
- .NET 8 SDK
- Angular CLI 18+

## Getting Started

### Backend Setup

1. Navigate to the backend directory:
```bash
cd backend
```

2. Restore packages:
```bash
dotnet restore
```

3. Run the API:
```bash
dotnet run --project src/Barbershop.API/Barbershop.API.csproj
```

The API will be available at:
- HTTP: http://localhost:5000
- HTTPS: https://localhost:7000
- Swagger UI: https://localhost:7000/swagger

### Frontend Setup

1. Navigate to the frontend directory:
```bash
cd frontend
```

2. Install dependencies:
```bash
npm install
```

3. Install Tailwind CSS:
```bash
npm install -D tailwindcss postcss autoprefixer
npx tailwindcss init
```

4. Run the development server:
```bash
npm start
```

The frontend will be available at http://localhost:4200

## Architecture

### Onion Architecture Layers

1. **Domain Layer** (`Barbershop.Domain`)
   - Entities (Booking)
   - Interfaces (IRepository, IBookingRepository)

2. **Application Layer** (`Barbershop.Application`)
   - DTOs (Data Transfer Objects)
   - Services (Business logic)
   - Validators (FluentValidation)
   - Mappings (AutoMapper)

3. **Infrastructure Layer** (`Barbershop.Infrastructure`)
   - DbContext (Entity Framework Core)
   - Repositories (Data access implementation)
   - Database configuration

4. **Presentation Layer** (`Barbershop.API`)
   - Controllers (API endpoints)
   - Middleware configuration
   - CORS setup

## API Endpoints

- `GET /api/bookings` - Get all bookings
- `GET /api/bookings/{id}` - Get booking by ID
- `POST /api/bookings` - Create new booking
- `PUT /api/bookings/{id}` - Update booking
- `DELETE /api/bookings/{id}` - Delete booking
- `GET /api/bookings/stats` - Get booking statistics

## Technologies Used

### Frontend
- Angular 18
- TypeScript
- Tailwind CSS
- RxJS

### Backend
- .NET 8
- Entity Framework Core
- AutoMapper
- FluentValidation
- Swagger/OpenAPI

## Development Notes

- The backend uses an in-memory database for development
- CORS is configured to allow requests from `http://localhost:4200`
- All API responses follow a consistent `ApiResponse<T>` format
- Error handling is implemented at all layers
- Input validation is performed using FluentValidation

## License

MIT License
