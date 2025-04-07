# UBB-SE-2025-Duo

A language learning application inspired by Duolingo, built as part of the Software Engineering course at Babeș-Bolyai University.

## Project Description

Duo is a Windows desktop application built using WinUI 3 and .NET 8, designed to help users learn languages through interactive lessons and quizzes. The application includes features like user authentication, friend management, leaderboards, achievements, and profile customization.

## Main Features

- **User Authentication**: Register, login, and password reset functionality
- **Profile Management**: Customize profile picture and privacy settings
- **Friends System**: Add friends, view their activity, and sort them by different criteria
- **Achievements**: Track learning progress with achievements and badges
- **Leaderboards**: Global and friends-based leaderboards sorted by different metrics
- **Course System**: Learn through structured courses and quizzes (coming soon)

## Architecture

The application is built using the MVVM (Model-View-ViewModel) pattern and follows clean architecture principles:

- **Models**: Represent the data entities and business objects
- **Views**: WinUI 3 XAML pages for the user interface
- **ViewModels**: Handle the business logic and data binding
- **Services**: Provide functionality for various features
- **Repositories**: Handle data access and persistence
- **Interfaces**: Define contracts for dependency injection

## Technologies Used

- **WinUI 3**: Modern UI framework for Windows applications
- **Microsoft.Extensions.DependencyInjection**: For dependency injection
- **Microsoft.Data.SqlClient**: For database access
- **StyleCop**: For code style enforcement

## Setup Instructions

### Prerequisites

- Visual Studio 2022
- .NET 8.0 SDK
- Windows App SDK
- SQL Server Express or higher

### Database Setup

1. Create a new SQL Server database
2. Run the database scripts provided in the `Database` folder
3. Update the connection string in `appsettings.json`

### Running the Application

1. Clone the repository
2. Open the solution in Visual Studio
3. Restore NuGet packages
4. Build the solution
5. Run the application

## Project Structure

- **Duo**: Main application project
  - **Models**: Data models
  - **ViewModels**: View models for MVVM
  - **Views**: XAML UI pages
  - **Services**: Business logic services
  - **Repositories**: Data access
  - **Interfaces**: Contracts for DI
  - **Data**: Database connection
  - **Helpers**: Utility classes
- **Duo.Tests**: Unit and integration tests

## Contributors

- Radu Cioata

## License

This project is part of the Software Engineering course at Babeș-Bolyai University. 