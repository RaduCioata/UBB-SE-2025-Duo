# Script to run simple service tests only
Write-Host "Building and running simple service tests..."

# Create output directory if it doesn't exist
if (-not (Test-Path "SimpleServiceTests\TestResults")) {
    New-Item -ItemType Directory -Path "SimpleServiceTests\TestResults" -Force
}

# Build and run the SimpleServiceTests project
dotnet test SimpleServiceTests\SimpleServiceTests.csproj `
    --logger "console;verbosity=detailed"

# Generate coverage report
Write-Host "Generating coverage report..."
reportgenerator -reports:SimpleServiceTests\TestResults\coverage.cobertura.xml -targetdir:simple-service-coverage-report -reporttypes:Html -sourcedirs:SimpleServiceTests\Services

Write-Host "Coverage report available at: simple-service-coverage-report\index.html" 