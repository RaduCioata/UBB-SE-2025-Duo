# Script to run service tests only
Write-Host "Building and running service tests..."

# Create output directory if it doesn't exist
if (-not (Test-Path "TestResults")) {
    New-Item -ItemType Directory -Path "TestResults"
}

# Build the ServiceTests project in Release mode
dotnet build ServiceTests/ServiceTests.csproj -c Release

# Run the tests with coverage
dotnet test ServiceTests/ServiceTests.csproj --no-build -c Release 

# Generate coverage report
Write-Host "Generating coverage report..."
reportgenerator -reports:TestResults/service-coverage.cobertura.xml -targetdir:service-coverage-report -reporttypes:Html -sourcedirs:DuoModels;Duo/Services

Write-Host "Coverage report available at: service-coverage-report\index.html" 