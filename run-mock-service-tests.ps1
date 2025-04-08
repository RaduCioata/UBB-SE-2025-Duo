# Script to run mock service tests only
Write-Host "Building and running mock service tests..."

# Create output directory if it doesn't exist
if (-not (Test-Path "TestResults")) {
    New-Item -ItemType Directory -Path "TestResults"
}

# Build the DuoModels project first
dotnet build DuoModels\DuoModels.csproj -c Release

# Build and run the MockServicesTests project
dotnet test ServiceTests\MockServicesTests.csproj -c Release `
    /p:CollectCoverage=true `
    /p:CoverletOutputFormat=cobertura `
    /p:CoverletOutput=.\TestResults\mock-services-coverage.cobertura.xml `
    /p:Include="[DuoModels]*" `
    --logger "console;verbosity=detailed"

# Generate coverage report
Write-Host "Generating coverage report..."
reportgenerator -reports:TestResults\mock-services-coverage.cobertura.xml -targetdir:mock-services-coverage-report -reporttypes:Html -sourcedirs:DuoModels;ServiceTests\Services

Write-Host "Coverage report available at: mock-services-coverage-report\index.html" 