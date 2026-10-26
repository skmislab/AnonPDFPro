# Regression tests

The NUnit project targets .NET Framework 4.8 / x64 and uses the application's
actual dependencies. It does not start the UI, load PII plugins, contact licence
servers, or use client PDFs. PDF fixtures are generated in memory.

## Prerequisites

- Windows, Visual Studio 2022 (.NET desktop build tools), .NET Framework 4.8
  targeting pack, and .NET SDK 9 or newer.
- Run `restore-packages.bat` from the complete corresponding-source package
  or the developer workspace. Do not replace custom pdfSweep with the stock DLL.
- The OCR integration test needs `bin/<Configuration>/tessdata/pol.traineddata`,
  the same Polish OCR model used by the application.
  Its location can be overridden during build with `/p:OcrTestDataPath=<full-path>`.
- Native codec rebuilding additionally requires the C++ and CMake build tools.

## Run

From a Visual Studio Developer PowerShell:

```powershell
msbuild tests\AnonPDFPro.Tests\AnonPDFPro.Tests.csproj /t:Build /p:Configuration=Debug
dotnet test tests\AnonPDFPro.Tests\AnonPDFPro.Tests.csproj --no-build --no-restore
```

`dotnet test` alone cannot build the legacy WinForms/WPF application reliably;
build with Visual Studio MSBuild first. Test results can be saved with
`--logger trx --results-directory TestResults`.

## Coverage

- Command-line parsing, template paths, overwrite guards and geometry bypass.
- Whole-word selection, document margin settings and Unicode project/cache data.
- Layered images sharing a CTM, nested graphics states, A5/Letter/A4 and rotations.
- Physical removal of pixels from both underlying images, plus PDFium rendering
  and character coordinates on rotated pages (Cursor/Marker dependency).
- Redaction of ActualText/Alt in marked content, structure elements and ancestors.
- Native Polish OCR initialization and recognition.
- HTML objects, PDF/A-2b output intent, embedded fonts and searchable Unicode text.
- Certificate/XML signature verification, including tampering rejection.

The initial 5.0.6.4 baseline passed 29 tests and failed the two new accessibility
metadata tests. These failures demonstrated the issue fixed by pdfSweep 5.0.8.
All tests, including those two, must pass on the updated dependencies.

Private client-document checks live in `private/scripts/Test-PdfSweepImageCtm.py`,
`Test-PdfSweepCodecMigration.py` and `Test-BatchTemplateValidation.py`. Client
documents and generated artifacts must never be committed to the public repo.
