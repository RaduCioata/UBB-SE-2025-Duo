# Script to run model tests and generate coverage reports

# First rebuild the Domain Models assembly to get the updated models
Write-Host "Building DuoModels project..." -ForegroundColor Cyan
dotnet build DuoModels\DuoModels.csproj

if ($LASTEXITCODE -ne 0) {
    Write-Host "Build failed with exit code $LASTEXITCODE" -ForegroundColor Red
    exit $LASTEXITCODE
}

# Ensure model files are up to date
Write-Host "Ensuring model files are up to date..." -ForegroundColor Cyan
Get-ChildItem -Path Duo\Models\*.cs | ForEach-Object {
    $targetPath = "DuoModels\$($_.Name)"
    Copy-Item -Path $_.FullName -Destination $targetPath -Force
    Write-Host "  Copied $($_.Name) to DuoModels\" -ForegroundColor Green
}

# Update all files to have DuoModels namespace
Write-Host "Updating namespace in all model files..." -ForegroundColor Cyan
Get-ChildItem -Path DuoModels\*.cs | ForEach-Object { 
    $content = Get-Content $_.FullName
    $content = $content -replace 'namespace Duo\.Models', 'namespace DuoModels'
    Set-Content -Path $_.FullName -Value $content
    Write-Host "  Updated namespace in $($_.Name)" -ForegroundColor Green
}

# Clear old coverage files
Write-Host "Cleaning up previous test results..." -ForegroundColor Cyan
if (Test-Path "ModelTests\TestResults") {
    Remove-Item "ModelTests\TestResults" -Recurse -Force
}

# Build and run model tests with coverage
Write-Host "Building and running model tests..." -ForegroundColor Cyan

# Clean the project first
dotnet clean ModelTests\ModelTests.csproj

# Run the tests with coverage
dotnet test ModelTests\ModelTests.csproj `
    --configuration Release `
    /p:CollectCoverage=true `
    /p:CoverletOutputFormat=cobertura `
    /p:CoverletOutput=.\ModelTests\TestResults\coverage.cobertura.xml `
    /p:Include="[DuoModels]*" `
    --logger "console;verbosity=detailed"

if ($LASTEXITCODE -ne 0) {
    Write-Host "Model tests failed with exit code $LASTEXITCODE" -ForegroundColor Red
    exit $LASTEXITCODE
}

# Find the generated coverage file
$coverageFile = "ModelTests\TestResults\coverage.cobertura.xml"
if (-not (Test-Path $coverageFile)) {
    Write-Host "Coverage file not found at expected location. Searching for alternatives..." -ForegroundColor Yellow
    $coverageFiles = Get-ChildItem -Path . -Filter *.xml -Recurse | Where-Object { $_.Name -like '*coverage*.xml' } | Sort-Object LastWriteTime -Descending
    if ($coverageFiles.Count -gt 0) {
        $coverageFile = $coverageFiles[0].FullName
        Write-Host "Using most recent coverage file: $coverageFile" -ForegroundColor Green
    } else {
        Write-Host "No coverage files found." -ForegroundColor Red
        exit 1
    }
}

# Generate HTML report
Write-Host "Generating HTML coverage report..." -ForegroundColor Cyan
reportgenerator `
    "-reports:$coverageFile" `
    "-targetdir:coveragereport" `
    "-reporttypes:Html" `
    "-classfilters:+DuoModels.*" `
    "-sourcedirs:DuoModels"

Write-Host "Coverage report available at: coveragereport\index.html" -ForegroundColor Green

# Run standalone tests with coverage
Write-Host "Running standalone tests with coverage..." -ForegroundColor Cyan
dotnet test StandaloneTests/StandaloneTests.csproj /p:CollectCoverage=true /p:CoverletOutputFormat=cobertura /p:Include="[*]StandaloneTests.Services.*" /p:CoverletOutput="./TestResults/standalone-coverage.cobertura.xml"

if ($LASTEXITCODE -ne 0) {
    Write-Host "Standalone tests failed with exit code $LASTEXITCODE" -ForegroundColor Red
    exit $LASTEXITCODE
}

# Find the standalone coverage file
$standaloneCoverageFile = "./TestResults/standalone-coverage.cobertura.xml"
if (-not (Test-Path $standaloneCoverageFile)) {
    Write-Host "Standalone coverage file not found at expected location. Searching for alternatives..." -ForegroundColor Yellow
    $standaloneCoverageFiles = Get-ChildItem -Path . -Filter *.xml -Recurse | Where-Object { $_.Name -like '*coverage*.xml' } | Sort-Object LastWriteTime -Descending
    if ($standaloneCoverageFiles.Count -gt 0) {
        $standaloneCoverageFile = $standaloneCoverageFiles[0].FullName
        Write-Host "Using most recent standalone coverage file: $standaloneCoverageFile" -ForegroundColor Green
    } else {
        Write-Host "No standalone coverage files found." -ForegroundColor Red
        exit 1
    }
}

# Generate standalone HTML report
Write-Host "Generating standalone HTML coverage report..." -ForegroundColor Cyan
reportgenerator `
    "-reports:$standaloneCoverageFile" `
    "-targetdir:standalone-coverage-report" `
    "-reporttypes:Html" `
    "-classfilters:+StandaloneTests.Services.*" `
    "-sourcedirs:./StandaloneTests"

Write-Host "Standalone coverage report available at: standalone-coverage-report\index.html" -ForegroundColor Green 