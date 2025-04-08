# Script to run tests and report coverage by avoiding Windows App SDK issues

# First rebuild the Domain Models assembly to get the updated models
Write-Host "Building DuoModels project..." -ForegroundColor Cyan
dotnet build DuoModels\DuoModels.csproj

if ($LASTEXITCODE -ne 0) {
    Write-Host "Build failed with exit code $LASTEXITCODE" -ForegroundColor Red
    exit $LASTEXITCODE
}

# Copy model files if needed
Write-Host "Ensuring model files are up to date..." -ForegroundColor Cyan
Get-ChildItem -Path Duo\Models\*.cs | ForEach-Object {
    $targetPath = "DuoModels\$($_.Name)"
    Copy-Item -Path $_.FullName -Destination $targetPath -Force
    Write-Host "  Copied $($_.Name) to DuoModels\" -ForegroundColor Green
}

# Build and run tests specifically targeting the models
Write-Host "Building and running model tests..." -ForegroundColor Cyan

# Use the newer ModelTests project that only tests models
dotnet test ModelTests\ModelTests.csproj `
    /p:CollectCoverage=true `
    /p:CoverletOutputFormat=cobertura `
    /p:CoverletOutput=./TestResults/coverage.cobertura.xml `
    /p:Include="[DuoModels]*" `
    --logger "console;verbosity=detailed"

if ($LASTEXITCODE -ne 0) {
    Write-Host "Model tests failed with exit code $LASTEXITCODE" -ForegroundColor Red
}

# Generate HTML report with explicit source inclusion
Write-Host "Generating HTML coverage report..." -ForegroundColor Cyan
reportgenerator `
    "-reports:TestResults/coverage.cobertura.xml" `
    "-targetdir:coveragereport" `
    "-reporttypes:Html" `
    "-sourcedirs:DuoModels"

Write-Host "Coverage report available at: coveragereport\index.html" -ForegroundColor Green 