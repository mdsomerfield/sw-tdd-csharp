# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Structure

This is a C# .NET 8.0 solution implementing a Wordle game with Test-Driven Development (TDD) practices. The main solution is located in the `Wordle/` directory.

**Solution Components:**
- `Wordle.Api/` - ASP.NET Core Web API project using traditional Program.cs/Startup.cs pattern
- `Wordle.Web/` - ASP.NET Core MVC Web application with Razor views
- `Wordle.Api.Tests/` - xUnit integration tests for the API
- `Wordle.E2E.Tests/` - End-to-end tests with real server instances and Playwright support
- `Wordle.Web.Tests/` - Web application tests (currently empty, placeholder for future tests)

**Architecture:**
- Both API and Web projects use traditional ASP.NET Core startup pattern with separate `Startup.cs` classes
- API uses domain-driven structure with controllers organized in `Domains/` folders (e.g., `Domains/System/`)
- Web application follows MVC pattern with Controllers, Views (Razor), Models, and Services
- E2E tests run both API and Web servers on dynamic ports for isolated testing
- JSON serialization configured with enum string conversion and camelCase naming

## Development Commands

**Working Directory:** Always run commands from the `Wordle/` directory.

```bash
# Build the solution
dotnet build

# Run all tests (includes API, E2E tests)
dotnet test

# Run the API locally
dotnet run --project Wordle.Api

# Run the Web application locally
dotnet run --project Wordle.Web

# Run specific test project
dotnet test Wordle.Api.Tests
dotnet test Wordle.E2E.Tests

# Clean and rebuild
dotnet clean && dotnet build

# Run API and Web in parallel (for development)
dotnet run --project Wordle.Api & dotnet run --project Wordle.Web
```

## Testing Framework

**Test Types:**
- **Integration Tests** (`Wordle.Api.Tests`): Use `ApiTestFixture` with `TestApplicationClient` for in-memory API testing
- **E2E Tests** (`Wordle.E2E.Tests`): Use `RealServerTestFixture` to run actual API and Web servers on dynamic ports
- **Playwright Support**: E2E tests include Playwright (v1.55.0) for browser automation capabilities

**Test Organization:**
- xUnit as the test framework with FluentAssertions for readable assertions
- Test collections: "ApiTest" for API tests, "E2ETest" for end-to-end tests
- Base test classes provide common HTTP client functionality and JSON serialization
- Tests organized to mirror the application structure

## API Endpoints

**System/Health:**
- `GET /api/system/health` - Returns health check status with timestamp

## Key Dependencies

**Core Framework:**
- .NET 8.0 with nullable reference types enabled
- ASP.NET Core for both API and Web applications

**Testing:**
- xUnit 2.9.2 - Test framework
- FluentAssertions 7.0.0 - Assertion library
- Microsoft.AspNetCore.Mvc.Testing - Integration testing
- Microsoft.Playwright 1.55.0 - Browser automation for E2E tests

**Web Application:**
- Razor views for server-side rendering
- Microsoft.Extensions.Http for HTTP client factory support

## Configuration

- Both API and Web projects support environment-specific configuration (appsettings.{Environment}.json)
- E2E tests dynamically configure the Web application to point to the test API instance
- API runs with `/api` path base by default