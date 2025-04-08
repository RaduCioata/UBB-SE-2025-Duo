# Script to generate code coverage report

# Build the project first to ensure binaries are up to date
Write-Host "Building TestsDuo2 project..." -ForegroundColor Cyan
dotnet build TestsDuo2\TestsDuo2.NoWindowsSDK.csproj

if ($LASTEXITCODE -ne 0) {
    Write-Host "Build failed with exit code $LASTEXITCODE" -ForegroundColor Red
    exit $LASTEXITCODE
}

# Run tests with coverage
Write-Host "Running tests with coverage..." -ForegroundColor Cyan
dotnet test TestsDuo2\TestsDuo2.NoWindowsSDK.csproj `
    /p:CollectCoverage=true `
    /p:CoverletOutputFormat=cobertura `
    /p:CoverletOutput=./TestResults/coverage.cobertura.xml

if ($LASTEXITCODE -ne 0) {
    Write-Host "Tests failed with exit code $LASTEXITCODE" -ForegroundColor Red
    exit $LASTEXITCODE
}

# Check if ReportGenerator is installed
$reportGeneratorInstalled = $null -ne (Get-Command "reportgenerator" -ErrorAction SilentlyContinue)

if (-not $reportGeneratorInstalled) {
    Write-Host "Installing ReportGenerator..." -ForegroundColor Yellow
    dotnet tool install -g dotnet-reportgenerator-globaltool
    
    if ($LASTEXITCODE -ne 0) {
        Write-Host "Failed to install ReportGenerator. Please install it manually with: dotnet tool install -g dotnet-reportgenerator-globaltool" -ForegroundColor Red
        exit $LASTEXITCODE
    }
}

# Generate HTML report
Write-Host "Generating HTML coverage report..." -ForegroundColor Cyan
reportgenerator "-reports:TestResults/coverage.cobertura.xml" "-targetdir:coveragereport" "-reporttypes:Html"

if ($LASTEXITCODE -ne 0) {
    Write-Host "Failed to generate coverage report. Exit code: $LASTEXITCODE" -ForegroundColor Red
    exit $LASTEXITCODE
}

# Open report in default browser
$reportPath = Resolve-Path "coveragereport\index.html"
Write-Host "Coverage report generated at: $reportPath" -ForegroundColor Green
Write-Host "Opening report in browser..." -ForegroundColor Cyan
Start-Process $reportPath 