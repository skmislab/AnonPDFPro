@echo off
setlocal EnableExtensions
cd /d "%~dp0"
echo AnonPDF Pro - Package Restore
where nuget >nul 2>&1
if errorlevel 1 (
    if not exist nuget.exe powershell -NoProfile -Command "[Net.ServicePointManager]::SecurityProtocol = [Net.SecurityProtocolType]::Tls12; Invoke-WebRequest -UseBasicParsing -Uri 'https://dist.nuget.org/win-x86-commandline/v6.14.0/nuget.exe' -OutFile 'nuget.exe'"
    if not exist nuget.exe exit /b 1
    set "NUGET_CMD=%~dp0nuget.exe"
) else (
    set "NUGET_CMD=nuget"
)
"%NUGET_CMD%" restore packages.config -PackagesDirectory packages -NonInteractive
if errorlevel 1 exit /b 1
dotnet restore tests\AnonPDFPro.Tests\AnonPDFPro.Tests.csproj
if errorlevel 1 exit /b 1
powershell -NoProfile -ExecutionPolicy Bypass -File "%~dp0build\Restore-CustomPdfSweep.ps1"
if errorlevel 1 exit /b 1
echo Packages and custom pdfSweep restored successfully.
exit /b 0
