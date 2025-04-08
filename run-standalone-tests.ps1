# Script to run standalone tests without dependencies on the Duo project
Write-Host "Building and running standalone tests..."

# Create output directory if it doesn't exist
if (-not (Test-Path "TestResults")) {
    New-Item -ItemType Directory -Path "TestResults"
}

# Build and run the standalone tests
dotnet test StandaloneTests/StandaloneTests.csproj

# Generate coverage report if the coverage file exists
$coverageFile = "TestResults/standalone-coverage.cobertura.xml"
if (Test-Path $coverageFile) {
    Write-Host "Generating coverage report..."
    reportgenerator `
        -reports:$coverageFile `
        -targetdir:standalone-coverage-report `
        -reporttypes:Html `
        -sourcedirs:StandaloneTests/Services

    Write-Host "Coverage report available at: standalone-coverage-report\index.html"
} else {
    Write-Host "Coverage file not found: $coverageFile"
} 