# Build and run tests with a non-Windows SDK project file
Write-Host "Building and running tests..." -ForegroundColor Cyan

# Build the main Duo project first
dotnet build Duo\Duo.csproj

if ($LASTEXITCODE -ne 0) {
    Write-Host "Build of main project failed with exit code $LASTEXITCODE" -ForegroundColor Red
    exit $LASTEXITCODE
}

# Build tests using the special project file
dotnet build TestsDuo2\TestsDuo2.NoWindowsSDK.csproj

if ($LASTEXITCODE -ne 0) {
    Write-Host "Build of tests failed with exit code $LASTEXITCODE" -ForegroundColor Red
    exit $LASTEXITCODE
}

# Run tests using the special project file
dotnet test TestsDuo2\TestsDuo2.NoWindowsSDK.csproj --no-build

if ($LASTEXITCODE -ne 0) {
    Write-Host "Tests failed with exit code $LASTEXITCODE" -ForegroundColor Red
    exit $LASTEXITCODE
}

Write-Host "Tests completed successfully" -ForegroundColor Green 