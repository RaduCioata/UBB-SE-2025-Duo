# Check if the test DLL exists and run tests directly
$testDll = "TestsDuo2\bin\Debug\net8.0-windows10.0.19041\TestsDuo2.dll"

if (-not (Test-Path $testDll)) {
    Write-Host "Test DLL not found at: $testDll" -ForegroundColor Yellow
    Write-Host "Building test project first..." -ForegroundColor Cyan
    dotnet build TestsDuo2\TestsDuo2.csproj
    
    if ($LASTEXITCODE -ne 0) {
        Write-Host "Build failed with exit code $LASTEXITCODE" -ForegroundColor Red
        exit $LASTEXITCODE
    }
}

# Look for vstest.console.exe in common locations
$vspath = "C:\Program Files\Microsoft Visual Studio"
$vstestPaths = @(
    # Visual Studio 2022
    "$vspath\2022\Enterprise\Common7\IDE\CommonExtensions\Microsoft\TestWindow\vstest.console.exe",
    "$vspath\2022\Professional\Common7\IDE\CommonExtensions\Microsoft\TestWindow\vstest.console.exe",
    "$vspath\2022\Community\Common7\IDE\CommonExtensions\Microsoft\TestWindow\vstest.console.exe",
    # Visual Studio 2019
    "$vspath\2019\Enterprise\Common7\IDE\CommonExtensions\Microsoft\TestWindow\vstest.console.exe",
    "$vspath\2019\Professional\Common7\IDE\CommonExtensions\Microsoft\TestWindow\vstest.console.exe",
    "$vspath\2019\Community\Common7\IDE\CommonExtensions\Microsoft\TestWindow\vstest.console.exe"
)

$vstestPath = $null
foreach ($path in $vstestPaths) {
    if (Test-Path $path) {
        $vstestPath = $path
        break
    }
}

if (Test-Path $testDll) {
    if ($vstestPath) {
        Write-Host "Running tests directly with $vstestPath..." -ForegroundColor Cyan
        & $vstestPath $testDll
    
        if ($LASTEXITCODE -ne 0) {
            Write-Host "Tests failed with exit code $LASTEXITCODE" -ForegroundColor Red
            exit $LASTEXITCODE
        }
    
        Write-Host "Tests completed successfully" -ForegroundColor Green
    } else {
        Write-Host "Could not find vstest.console.exe. Trying with dotnet test directly..." -ForegroundColor Yellow
        dotnet test TestsDuo2\TestsDuo2.csproj --no-build
        
        if ($LASTEXITCODE -ne 0) {
            Write-Host "Tests failed with exit code $LASTEXITCODE" -ForegroundColor Red
            exit $LASTEXITCODE
        }
        
        Write-Host "Tests completed successfully" -ForegroundColor Green
    }
    
    # Generate code coverage report
    Write-Host "Generating code coverage report..." -ForegroundColor Cyan
    dotnet test TestsDuo2\TestsDuo2.NoWindowsSDK.csproj /p:CollectCoverage=true /p:CoverletOutputFormat=cobertura /p:CoverletOutput=./TestResults/coverage.cobertura.xml
    
    # Generate HTML report if ReportGenerator is installed
    if (Get-Command "reportgenerator" -ErrorAction SilentlyContinue) {
        Write-Host "Generating HTML coverage report..." -ForegroundColor Cyan
        reportgenerator "-reports:TestResults/coverage.cobertura.xml" "-targetdir:coveragereport" "-reporttypes:Html"
        Write-Host "Coverage report generated at: $(Resolve-Path 'coveragereport/index.html')" -ForegroundColor Green
    } else {
        Write-Host "ReportGenerator not found. Install it with: dotnet tool install -g dotnet-reportgenerator-globaltool" -ForegroundColor Yellow
    }
} else {
    Write-Host "Test DLL still not found after build. Something went wrong." -ForegroundColor Red
    exit 1
} 