@echo off
setlocal EnableExtensions
echo ============================================
echo AnonPDF Pro - Package Restore
echo ============================================
echo.

echo Checking for nuget.exe...
where nuget >nul 2>&1
if %ERRORLEVEL% NEQ 0 (
    echo NuGet.exe not found in PATH.
    echo.
    echo Downloading nuget.exe...
    powershell -Command "Invoke-WebRequest -Uri 'https://dist.nuget.org/win-x86-commandline/latest/nuget.exe' -OutFile 'nuget.exe'"
    if exist nuget.exe (
        echo Downloaded nuget.exe successfully.
        set NUGET_CMD=nuget.exe
    ) else (
        echo Failed to download nuget.exe
        echo Please install NuGet manually or use Visual Studio Package Manager
        pause
        exit /b 1
    )
) else (
    set NUGET_CMD=nuget
)

echo.
echo Restoring packages to local 'packages' folder...
set "CUSTOM_PDFSWEEP_SRC=vendor\itext.pdfsweep.5.0.6\lib\net461"
set "CUSTOM_PDFSWEEP_DST=packages\itext.pdfsweep.5.0.6\lib\net461"
%NUGET_CMD% restore AnonPDFPro.sln -PackagesDirectory packages

if %ERRORLEVEL% EQU 0 (
    echo.
    echo Applying custom itext.pdfsweep JPEG 2000 overlay...

    if not exist "%CUSTOM_PDFSWEEP_SRC%\itext.cleanup.dll" (
        echo Missing custom overlay file: %CUSTOM_PDFSWEEP_SRC%\itext.cleanup.dll
        pause
        exit /b 1
    )
    if not exist "%CUSTOM_PDFSWEEP_SRC%\openjp2.dll" (
        echo Missing custom overlay file: %CUSTOM_PDFSWEEP_SRC%\openjp2.dll
        pause
        exit /b 1
    )
    if not exist "%CUSTOM_PDFSWEEP_DST%" mkdir "%CUSTOM_PDFSWEEP_DST%"

    copy /Y "%CUSTOM_PDFSWEEP_SRC%\itext.cleanup.dll" "%CUSTOM_PDFSWEEP_DST%\itext.cleanup.dll" >nul
    if %ERRORLEVEL% NEQ 0 (
        echo Failed to copy custom itext.cleanup.dll
        pause
        exit /b 1
    )
    if exist "%CUSTOM_PDFSWEEP_SRC%\itext.cleanup.xml" copy /Y "%CUSTOM_PDFSWEEP_SRC%\itext.cleanup.xml" "%CUSTOM_PDFSWEEP_DST%\itext.cleanup.xml" >nul
    copy /Y "%CUSTOM_PDFSWEEP_SRC%\openjp2.dll" "%CUSTOM_PDFSWEEP_DST%\openjp2.dll" >nul
    if %ERRORLEVEL% NEQ 0 (
        echo Failed to copy openjp2.dll
        pause
        exit /b 1
    )

    echo.
    echo ============================================
    echo Packages restored successfully!
    echo ============================================
    echo.
    echo You can now build the project in Visual Studio or use:
    echo   msbuild AnonPDFPro.sln /p:Configuration=Release
    echo.
) else (
    echo.
    echo ============================================
    echo Package restore failed!
    echo ============================================
    echo Please check your internet connection and try again.
    echo.
)

pause
