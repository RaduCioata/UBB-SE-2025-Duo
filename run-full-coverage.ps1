# Script to run full test coverage for the entire application

# First build the main Duo project to ensure the DLL is available
Write-Host "Building Duo main project..." -ForegroundColor Cyan
dotnet build Duo\Duo.csproj

if ($LASTEXITCODE -ne 0) {
    Write-Host "Main project build failed with exit code $LASTEXITCODE. Continuing with available components..." -ForegroundColor Yellow
}

# Build the DuoModels project
Write-Host "Building DuoModels project..." -ForegroundColor Cyan
dotnet build DuoModels\DuoModels.csproj

if ($LASTEXITCODE -ne 0) {
    Write-Host "DuoModels build failed with exit code $LASTEXITCODE" -ForegroundColor Red
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
Write-Host "Updating namespace in model files..." -ForegroundColor Cyan
Get-ChildItem -Path DuoModels\*.cs | ForEach-Object { 
    $content = Get-Content $_.FullName
    $content = $content -replace 'namespace Duo\.Models', 'namespace DuoModels'
    Set-Content -Path $_.FullName -Value $content
    Write-Host "  Updated namespace in $($_.Name)" -ForegroundColor Green
}

# Clear old coverage files
Write-Host "Cleaning up previous test results..." -ForegroundColor Cyan
if (Test-Path "TestResults") {
    Remove-Item "TestResults" -Recurse -Force
}
if (Test-Path "coveragereport") {
    Remove-Item "coveragereport" -Recurse -Force
}

# Create TestResults directory
New-Item -Path "TestResults" -ItemType Directory -Force | Out-Null

# Run model tests
Write-Host "Running model tests..." -ForegroundColor Cyan
dotnet test ModelTests\ModelTests.csproj `
    --configuration Release `
    /p:CollectCoverage=true `
    /p:CoverletOutputFormat=cobertura `
    /p:CoverletOutput=.\TestResults\model-coverage.cobertura.xml `
    /p:Include="[DuoModels]*" `
    --logger "console;verbosity=detailed"

# Run SimpleServiceTests 
Write-Host "Running simple service tests..." -ForegroundColor Cyan
if (-not (Test-Path "SimpleServiceTests\TestResults")) {
    New-Item -ItemType Directory -Path "SimpleServiceTests\TestResults" -Force | Out-Null
}
dotnet test SimpleServiceTests\SimpleServiceTests.csproj `
    --configuration Release `
    --logger "console;verbosity=detailed"

# Check for simple service coverage file
$simpleServiceCoverageFile = "SimpleServiceTests\TestResults\coverage.cobertura.xml"
if (Test-Path $simpleServiceCoverageFile) {
    $coverageFiles += $simpleServiceCoverageFile
}

# Run MockServiceTests
Write-Host "Running mock service tests..." -ForegroundColor Cyan
dotnet test ServiceTests\MockServicesTests.csproj `
    --configuration Release `
    /p:CollectCoverage=true `
    /p:CoverletOutputFormat=cobertura `
    /p:CoverletOutput=.\TestResults\mock-service-coverage.cobertura.xml `
    /p:Include="[DuoModels]*" `
    --logger "console;verbosity=detailed"

# Check for model coverage file
$modelCoverageFile = "TestResults\model-coverage.cobertura.xml"
if (-not (Test-Path $modelCoverageFile)) {
    $modelCoverageFile = Get-ChildItem -Path . -Filter "*coverage*.xml" -Recurse | 
                        Where-Object { $_.Name -like '*model-coverage*.xml' } | 
                        Sort-Object LastWriteTime -Descending | 
                        Select-Object -First 1 -ExpandProperty FullName
    
    if ([string]::IsNullOrEmpty($modelCoverageFile)) {
        Write-Host "Model coverage file not found. Cannot continue." -ForegroundColor Red
        exit 1
    }
    
    Write-Host "Using model coverage file: $modelCoverageFile" -ForegroundColor Yellow
}

# Create a temporary copy of the repo test project without WindowsAppSDK dependency
Write-Host "Setting up repository tests..." -ForegroundColor Cyan
$originalProjFile = "TestsDuo2\TestsDuo2.NoWindowsSDK.csproj"
$tempProjFile = "TestsDuo2\TestsDuo2.NoWindowsSDK.Temp.csproj"

# Copy the file and modify it to remove WindowsAppSDK dependencies
if (Test-Path $originalProjFile) {
    $projContent = Get-Content $originalProjFile -Raw
    $projContent = $projContent -replace '<PackageReference Include="Microsoft\.WindowsAppSDK".*?\/>', ''
    $projContent = $projContent -replace '<PackageReference Include="Microsoft\.Windows\.SDK\.BuildTools".*?\/>', ''
    Set-Content -Path $tempProjFile -Value $projContent
    Write-Host "  Created temporary project file without Windows SDK dependencies" -ForegroundColor Green
}

# Try to run the repository tests with the temporary project file
Write-Host "Running repository tests..." -ForegroundColor Cyan
dotnet test $tempProjFile `
    --configuration Release `
    /p:CollectCoverage=true `
    /p:CoverletOutputFormat=cobertura `
    /p:CoverletOutput=.\TestResults\repo-coverage.cobertura.xml `
    --logger "console;verbosity=detailed"

# Clean up the temporary file
if (Test-Path $tempProjFile) {
    Remove-Item $tempProjFile -Force
}

# Check for repository coverage file
$repoCoverageFile = "TestResults\repo-coverage.cobertura.xml"
$coverageFiles = @()

if (Test-Path $modelCoverageFile) {
    $coverageFiles += $modelCoverageFile
}

if (Test-Path $repoCoverageFile) {
    $coverageFiles += $repoCoverageFile
} else {
    $repoCoverageFile = Get-ChildItem -Path . -Filter "*coverage*.xml" -Recurse | 
                        Where-Object { $_.Name -like '*repo-coverage*.xml' } | 
                        Sort-Object LastWriteTime -Descending | 
                        Select-Object -First 1 -ExpandProperty FullName
    
    if (-not [string]::IsNullOrEmpty($repoCoverageFile)) {
        $coverageFiles += $repoCoverageFile
        Write-Host "Using repository coverage file: $repoCoverageFile" -ForegroundColor Yellow
    } else {
        Write-Host "Repository coverage file not found. Report will include only model coverage." -ForegroundColor Yellow
    }
}

# Check for mock service coverage file
$mockServiceCoverageFile = "TestResults\mock-service-coverage.cobertura.xml"
if (Test-Path $mockServiceCoverageFile) {
    $coverageFiles += $mockServiceCoverageFile
}

# Generate combined HTML report
Write-Host "Generating HTML coverage report..." -ForegroundColor Cyan
if ($coverageFiles.Count -gt 0) {
    reportgenerator `
        "-reports:$($coverageFiles -join ';')" `
        "-targetdir:coveragereport" `
        "-reporttypes:Html" `
        "-sourcedirs:DuoModels;Duo\Repositories;Duo\Services;Duo\Helpers;Duo\Validators;SimpleServiceTests\Services;ServiceTests\Services" 
    
    Write-Host "Coverage report available at: coveragereport\index.html" -ForegroundColor Green
} else {
    Write-Host "No coverage files found to generate report from." -ForegroundColor Red
    exit 1
} 