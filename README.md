# TripPlannerBlazorApp
"A travel planning web app built with ASP.NET Core 9 &amp; Blazor Server. Features trip management, destination tracking with maps, expense monitoring with budget comparison, booking management, and real-time weather via OpenWeatherMap API. Uses SQLite, EF Core, and ASP.NET Identity."
✈️ TripPlanner App
A full-stack travel planning web application built with ASP.NET Core 9 and Blazor Server. Plan trips, track expenses, manage bookings, and get real-time weather for your destinations — all in one place.

📸 Overview
TripPlanner is a semester project that solves a common traveler problem: managing everything across multiple apps. This application combines trip itinerary planning, budget tracking, booking management, and live weather data into a single platform.

🚀 Features

User Authentication — Secure register/login/logout using ASP.NET Core Identity with hashed passwords
Trip Management — Create, edit, and delete trips with cover images, travel dates, budget, and status tracking
Destination Planning — Add multiple destinations per trip with GPS coordinates, arrival/departure dates, and map display
Expense Tracking — Log expenses by category (Food, Transport, Accommodation, Entertainment) with receipt uploads and budget vs. spent comparison
Booking Manager — Track hotel, flight, and car bookings with confirmation reference numbers and status (Pending / Confirmed / Cancelled)
Live Weather — Real-time weather for each destination via OpenWeatherMap API using stored coordinates
File Uploads — Upload trip cover images and expense receipts stored on the server
Demo Account — Pre-seeded demo account for instant access without registration


🛠️ Tech Stack
LayerTechnologyFrontendBlazor Server (ASP.NET Core 9)BackendASP.NET Core 9 Minimal APIDatabaseSQLite via Entity Framework Core 9AuthenticationASP.NET Core IdentityORMEntity Framework Core 9Weather APIOpenWeatherMap REST APILocation APIGeocoding / Location Search APILanguageC# (.NET 9)

🗄️ Database Schema
AspNetUsers     → User accounts (managed by ASP.NET Identity)
Trips           → Id, Name, Description, StartDate, EndDate, Budget, Status, CoverImageUrl, UserId
Destinations    → Id, Name, Country, Latitude, Longitude, ArrivalDate, DepartureDate, Order, TripId
Expenses        → Id, Title, Amount, Category, Date, Currency, ReceiptUrl, TripId, UserId
Bookings        → Id, Title, Type, Status, CheckIn, CheckOut, Amount, Currency, Location, BookingRef, TripId, UserId
Relationships: One User → Many Trips → Many Destinations / Expenses / Bookings

⚙️ Getting Started
Prerequisites

.NET 9 SDK
Visual Studio 2022 or VS Code
EMU8086 (not required — this is a .NET project)

Installation

Clone the repository

bash   git clone https://github.com/YOUR_USERNAME/TripPlannerApp.git
   cd TripPlannerApp

Set your API keys in appsettings.Development.json

json   {
     "OpenWeatherMap": {
       "ApiKey": "YOUR_OPENWEATHERMAP_API_KEY"
     }
   }
Get a free API key at openweathermap.org

Run the application

bash   dotnet run
Or press F5 in Visual Studio.

The database is created automatically on first run with all tables and a seeded demo account.

Demo Credentials
Email:    demo@tripplanner.com
Password: Demo@1234!

⚠️ If you see "no such table: Bookings", delete tripplanner.db and restart. The app will recreate it with all tables.


📁 Project Structure
TripPlannerApp/
├── Components/           # Blazor UI components and pages
│   ├── Pages/            # Routable pages (Dashboard, Trips, Expenses, etc.)
│   └── Layout/           # Shared layout and navigation
├── Data/
│   ├── ApplicationDbContext.cs   # EF Core database context
│   └── SeedData.cs               # Demo data seeder
├── Models/               # C# entity classes (Trip, Expense, Booking, etc.)
├── Services/             # Business logic layer
│   ├── TripService.cs
│   ├── ExpenseService.cs
│   ├── BookingService.cs
│   ├── WeatherService.cs
│   ├── LocationService.cs
│   └── FileUploadService.cs
├── appsettings.json               # Production config
├── appsettings.Development.json   # Development config (API keys here)
├── Program.cs                     # App entry point, DI, middleware, auth endpoints
└── TripPlannerApp.csproj

🔐 Authentication Flow
POST /account/login    → SignInManager.PasswordSignInAsync → redirect /dashboard
POST /account/register → UserManager.CreateAsync → auto sign-in → redirect /dashboard
POST /account/logout   → SignInManager.SignOutAsync → redirect /
Passwords are hashed using PBKDF2 with salt — never stored as plain text.

📦 NuGet Packages
xmlMicrosoft.AspNetCore.Identity.EntityFrameworkCore  v9.0.0
Microsoft.EntityFrameworkCore.Sqlite               v9.0.0
Microsoft.EntityFrameworkCore.Tools                v9.0.0

🙋 Author
Sajjad Ejaz
Student ID: 241932
Air University, Islamabad
BS Computer Science — Semester Project

📄 License
This project is for academic purposes. Feel free to use it as a learning reference.
