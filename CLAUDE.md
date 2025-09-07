# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Structure

This is a C# .NET 8.0 project with Test-Driven Development (TDD) practices. The main solution is located in the `Wordle/` directory.

**Key Components:**
- `Wordle.Api/` - ASP.NET Core Web API project using traditional Program.cs/Startup.cs pattern
- `Wordle.Api.Tests/` - xUnit integration tests with custom test fixtures
- The API runs with `/api` path base and includes health check endpoints

**Architecture:**
- Uses traditional ASP.NET Core startup pattern with separate `Startup.cs` class
- Domain-driven structure with controllers organized in `Domains/` folders
- Integration testing setup using `Microsoft.AspNetCore.Mvc.Testing` and custom test fixtures
- JSON serialization configured with enum string conversion

## Development Commands

**Working Directory:** Always run commands from the `Wordle/` directory.

```bash
# Build the solution
dotnet build

# Run all tests
dotnet test

# Run the API locally
dotnet run --project Wordle.Api

# Run specific test project
dotnet test Wordle.Api.Tests

# Clean and rebuild
dotnet clean && dotnet build
```

## Testing Framework

- Uses xUnit for testing with FluentAssertions
- Integration tests use `ApiTestFixture` with `TestApplicationClient`
- Test collection defined as "ApiTest" for shared fixtures
- Tests are organized to mirror the API structure

## Key Dependencies

- .NET 8.0 with nullable reference types enabled
- ASP.NET Core for web API
- xUnit + FluentAssertions for testing
- Microsoft.AspNetCore.Mvc.Testing for integration tests