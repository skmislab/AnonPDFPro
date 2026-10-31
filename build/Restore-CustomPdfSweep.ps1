param(
    [string]$SourceRoot = "",
    [switch]$RebuildNative,
    [string]$CMakeGenerator = "Visual Studio 17 2022"
)

Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"
$root = Split-Path -Parent $PSScriptRoot
$version = "5.0.8"
$package = Join-Path $root "packages\itext.pdfsweep.$version\lib\net461"
if (-not (Test-Path -LiteralPath $package)) { throw "Restore NuGet packages first." }

if (-not $SourceRoot) {
    foreach ($candidate in @(
        "third-party-sources\itext-pdfsweep-dotnet-$version-custom",
        "private\test\itext-pdfsweep-dotnet-$version-custom"
    )) {
        $path = Join-Path $root $candidate
        if (Test-Path -LiteralPath $path) { $SourceRoot = $path; break }
    }
}
if (-not $SourceRoot) {
    throw "Custom pdfSweep sources are required. Use the complete corresponding-source package; the standard NuGet DLL does not contain our JPEG2000/JBIG2 fixes."
}
$SourceRoot = (Resolve-Path -LiteralPath $SourceRoot).Path
$artifacts = Join-Path $SourceRoot "artifacts"
$files = @("itext.cleanup.dll", "openjp2.dll", "jbig2bridge.dll")
$missing = @($files | Where-Object { -not (Test-Path -LiteralPath (Join-Path $artifacts $_)) })
if (-not $RebuildNative -and $missing.Count -eq 0) {
    $info = [Diagnostics.FileVersionInfo]::GetVersionInfo((Join-Path $artifacts "itext.cleanup.dll"))
    if ($info.FileVersion -ne "5.0.8.2") { throw "Unexpected custom pdfSweep artifact version: $($info.FileVersion)" }
    foreach ($file in $files) { Copy-Item -LiteralPath (Join-Path $artifacts $file) -Destination $package -Force }
    $xml = Join-Path $artifacts "itext.cleanup.xml"
    if (Test-Path -LiteralPath $xml) { Copy-Item -LiteralPath $xml -Destination $package -Force }
    Write-Host "Installed existing custom pdfSweep 5.0.8.2 artifacts (signatures preserved)."
    return
}

$vswhere = Join-Path ${env:ProgramFiles(x86)} "Microsoft Visual Studio\Installer\vswhere.exe"
if (-not (Test-Path -LiteralPath $vswhere)) { throw "Visual Studio Build Tools with MSBuild and C++ are required." }
$installation = & $vswhere -latest -products '*' -requires Microsoft.Component.MSBuild -property installationPath
if (-not $installation) { throw "MSBuild installation not found." }
$msbuild = Join-Path $installation "MSBuild\Current\Bin\MSBuild.exe"
$hash = [Security.Cryptography.SHA256]::Create()
try { $sourceId = ([BitConverter]::ToString($hash.ComputeHash([Text.Encoding]::UTF8.GetBytes($SourceRoot)))).Replace('-', '').Substring(0, 12) }
finally { $hash.Dispose() }
$nativeBuildRoot = Join-Path $root "obj\pdfsweep\$sourceId"
$cmake = Get-Command cmake -ErrorAction SilentlyContinue
if (-not $cmake) {
    $cmakePath = Join-Path $installation "Common7\IDE\CommonExtensions\Microsoft\CMake\CMake\bin\cmake.exe"
    if (-not (Test-Path -LiteralPath $cmakePath)) { throw "Install the C++ CMake tools for Windows." }
} else { $cmakePath = $cmake.Source }

function Build-Native([string]$Directory, [string]$Dll, [string[]]$Options = @()) {
    $source = Join-Path $SourceRoot "sources\$Directory"
    # Keep MSVC tracking files below MAX_PATH even for deeply extracted source packages.
    $output = Join-Path $nativeBuildRoot $Directory
    & $cmakePath -S $source -B $output -G $CMakeGenerator -A x64 @Options
    if ($LASTEXITCODE -ne 0) { throw "CMake configuration failed: $Directory" }
    & $cmakePath --build $output --config Release --parallel
    if ($LASTEXITCODE -ne 0) { throw "Native build failed: $Directory" }
    $binary = @(Get-ChildItem -LiteralPath $output -Recurse -File -Filter $Dll)
    if ($binary.Count -ne 1) { throw "Expected exactly one $Dll under $output" }
    Copy-Item -LiteralPath $binary[0].FullName -Destination $package -Force
}

Build-Native "openjpeg-custom" "openjp2.dll" @("-DBUILD_CODEC=OFF", "-DBUILD_TESTING=OFF", "-DBUILD_SHARED_LIBS=ON")
Build-Native "jbig2bridge-custom" "jbig2bridge.dll"
$project = Join-Path $SourceRoot "sources\itext-pdfsweep-dotnet-develop-custom\itext\itext.cleanup\itext.cleanup.localbuild.csproj"
& $msbuild $project /t:Rebuild /nologo /v:minimal "/p:DependencyPackagesPath=$root\packages" "/p:OpenJpegDllPath=$package\openjp2.dll" "/p:Jbig2BridgeDllPath=$package\jbig2bridge.dll"
if ($LASTEXITCODE -ne 0) { throw "Custom pdfSweep managed build failed." }
$built = Join-Path (Split-Path -Parent $project) "bin\local"
foreach ($file in @("itext.cleanup.dll", "itext.cleanup.xml")) {
    Copy-Item -LiteralPath (Join-Path $built $file) -Destination $package -Force
}
Write-Host "Built custom pdfSweep 5.0.8.2 and both native codecs from corresponding sources."
