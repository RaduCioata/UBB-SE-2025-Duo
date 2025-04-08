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
Write-Host "Running model tests with coverage..." -ForegroundColor Cyan
dotnet test ModelTests\ModelTests.csproj `
    --configuration Release `
    /p:CollectCoverage=true `
    /p:CoverletOutputFormat=cobertura `
    /p:CoverletOutput=.\TestResults\model-coverage.cobertura.xml `
    /p:Include="[DuoModels]*" `
    --logger "console;verbosity=detailed"

if ($LASTEXITCODE -ne 0) {
    Write-Host "Model tests failed with exit code $LASTEXITCODE" -ForegroundColor Red
    exit $LASTEXITCODE
}

# Check if the coverage file exists
$coverageFile = "TestResults\model-coverage.cobertura.xml"

if (Test-Path $coverageFile) {
    Write-Host "Coverage file found at $coverageFile" -ForegroundColor Green

    # Use the standalone ReportGenerator tool directly if available
    if (Get-Command "reportgenerator" -ErrorAction SilentlyContinue) {
        Write-Host "Generating HTML coverage report..." -ForegroundColor Cyan
        
        # Use a simpler command line that's less likely to have issues
        & reportgenerator "-reports:$coverageFile" "-targetdir:coveragereport" "-reporttypes:Html" "-sourcedirs:DuoModels"
        
        if ($LASTEXITCODE -eq 0) {
            Write-Host "Coverage report available at: coveragereport\index.html" -ForegroundColor Green
        } else {
            Write-Host "Failed to generate report. Trying alternative method..." -ForegroundColor Yellow
            
            # Alternative approach using dotnet tool
            dotnet reportgenerator "-reports:$coverageFile" "-targetdir:coveragereport" "-reporttypes:Html" "-sourcedirs:DuoModels"
        }
    } else {
        # Try with dotnet tool
        Write-Host "ReportGenerator not found as standalone command. Trying with dotnet tool..." -ForegroundColor Yellow
        dotnet reportgenerator "-reports:$coverageFile" "-targetdir:coveragereport" "-reporttypes:Html" "-sourcedirs:DuoModels"
    }
    
    Write-Host "Coverage report available at: coveragereport\index.html" -ForegroundColor Green
} else {
    Write-Host "Coverage file not found at $coverageFile. Searching for alternatives..." -ForegroundColor Yellow
    
    $coverageFiles = Get-ChildItem -Path . -Filter *.xml -Recurse | Where-Object { $_.Name -like '*coverage*.xml' } | Sort-Object LastWriteTime -Descending
    
    if ($coverageFiles.Count -gt 0) {
        $coverageFile = $coverageFiles[0].FullName
        Write-Host "Using most recent coverage file: $coverageFile" -ForegroundColor Green
        
        reportgenerator "-reports:$coverageFile" "-targetdir:coveragereport" "-reporttypes:Html" "-sourcedirs:DuoModels"
        
        Write-Host "Coverage report available at: coveragereport\index.html" -ForegroundColor Green
    } else {
        Write-Host "No coverage files found." -ForegroundColor Red
        exit 1
    }
} 