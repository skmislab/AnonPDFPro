# Regression tests

The NUnit project targets .NET Framework 4.8 / x64 and uses the application's
actual dependencies. It does not start the UI, load PII plugins, contact licence
servers, or use client PDFs. PDF fixtures are generated in memory.

## Prerequisites

- Windows, Visual Studio 2022 (.NET desktop build tools), .NET Framework 4.8
  targeting pack, and .NET SDK 9 or newer.
- Run `restore-packages.bat` from the complete corresponding-source package
  or the developer workspace. Do not replace custom pdfSweep with the stock DLL.
- The native Tesseract integration test needs
  `bin/<Configuration>/tessdata/pol.traineddata`. The current desktop OCR pipeline
  uses Windows OCR; this legacy codec test does not replace its interactive test.
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
- Complete removal of scanned word edges for Cursor and Marker, all four page
  rotations, local ink refinement on grey paper and faint text, and preservation
  of adjacent rows/table rules, including an invisible OCR line with overlapping
  font metrics (independent pdfSweep text/image regions). Exact-area Box remains unexpanded. Windows OCR
  also completes without a message loop on its calling synchronization context.
- Margin scaling keeps text, vectors, images and forms outside the original
  MediaBox/CropBox hidden, including rotated pages and offset page origins.
- Layered images sharing a CTM, nested graphics states, A5/Letter/A4 and rotations.
- Physical removal of pixels from both underlying images, plus PDFium rendering
  and character coordinates on rotated pages (Cursor/Marker dependency).
- Redaction of ActualText/Alt in marked content, structure elements and ancestors.
- Native Polish OCR initialization and recognition.
- HTML objects, PDF/A-2b output intent, embedded fonts and searchable Unicode text.
- Certificate/XML signature verification, including tampering rejection.
- Page insertion/reordering, object/ALT remapping, embedded project PDF data,
  form-field name collisions, internal links and DPI-scaled thumbnail sizes.
- Minimal drag-marker repaint regions and throttled page-list edge scrolling,
  including marker geometry at 100-200% DPI, without displaying any UI.
- Row-major thumbnail layout, precise hit testing, multi-selection and bounded
  bitmap retention while scrolling through an 800-page document.
- Lazy single-page previews with added content on a previously blank page,
  including page 800 and all four rotations, without changing the source PDF.
- Transactional page deletion marks, group restore, removal of newly inserted
  pages, deletion-mark remapping and protection against an empty output PDF.
- Moving a selection before/after a specified page, including page 800, and
  two-location navigation with scroll restoration after DPI/layout changes.
- Strict destination-number validation using the current page count, including
  inserted pages, oversized pasted values and page removal.

The normal suite does not show windows. The attended desktop-controller test
`PageOrganizationIntegrationTests` is marked `Explicit` and is excluded from
automatic runs. Do not automate showing/reparenting the page dialog inside a
test-host form: that strategy produced native ListView lifecycle exceptions.
Actual dialog appearance, DPI layout and mouse interactions require manual QA.

## Public releases

Every public release also requires interactive regression testing of the actual
Release binaries. Follow the release rule in `../AGENTS.md` and the private
`private/release-qa` corpus and checklist. Keep client documents and all resulting
evidence private. Record untested or blocked cases explicitly; passing this NUnit
suite alone must not be presented as a completed desktop release test.

The initial 5.0.6.4 baseline passed 29 tests and failed the two new accessibility
metadata tests. These failures demonstrated the issue fixed by pdfSweep 5.0.8.
All tests, including those two, must pass on the updated dependencies.

Private client-document checks live in `private/scripts/Test-PdfSweepImageCtm.py`,
`Test-PdfSweepCodecMigration.py` and `Test-BatchTemplateValidation.py`. Client
documents and generated artifacts must never be committed to the public repo.
