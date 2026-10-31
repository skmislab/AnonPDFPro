using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using AnonPDF;
using iText.IO.Image;
using iText.IO.Font.Constants;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Geom;
using iText.Kernel.Pdf.Canvas;
using iText.PdfCleanup;
using NUnit.Framework;
using PdfRectangle = iText.Kernel.Geom.Rectangle;
using DrawingRectangle = System.Drawing.Rectangle;
using Color = System.Drawing.Color;
using Path = System.IO.Path;

namespace AnonPDFPro.Tests;

[TestFixture, NonParallelizable]
public class OcrRedactionGeometryTests
{
    [TestCase(true, 0)]
    [TestCase(true, 90)]
    [TestCase(true, 180)]
    [TestCase(true, 270)]
    [TestCase(false, 0)]
    [TestCase(false, 90)]
    [TestCase(false, 180)]
    [TestCase(false, 270)]
    public void OcrCleanupRemovesWholeRasterWordAndPreservesAdjacentRows(bool cursor, int rotation)
    {
        using var bitmap = new Bitmap(180, 100);
        using (var g = Graphics.FromImage(bitmap))
        {
            g.Clear(Color.White);
            for (int x = 28; x < 138; x += 10)
                g.FillRectangle(Brushes.Black, x, 28, 7, 24);
            g.FillRectangle(Brushes.Black, 28, 12, 107, 5);
            g.FillRectangle(Brushes.Black, 28, 65, 107, 5);
        }
        using var png = new MemoryStream();
        bitmap.Save(png, ImageFormat.Png);
        byte[] source = PdfRegressionTests.CreatePdf(document => {
            var page = document.AddNewPage(new PageSize(180, 100));
            page.SetRotation(rotation);
            new PdfCanvas(page).AddImageFittedIntoRectangle(
                ImageDataFactory.Create(png.ToArray()), new PdfRectangle(0, 0, 180, 100), false);
        });
        var area = cursor ? new PdfRectangle(30, 50, 100, 20) : new PdfRectangle(30, 58, 100, 2);
        var hit = cursor ? new PdfRectangle(30, 53, 100, 14) : area;
        var word = new PdfRectangle(27, 47, 110, 26);
        var block = new RedactionBlock { IsCursorSelection = cursor, IsMarkerSelection = !cursor };
        using var output = new MemoryStream();
        using (var document = new iText.Kernel.Pdf.PdfDocument(
            new iText.Kernel.Pdf.PdfReader(new MemoryStream(source)),
            new iText.Kernel.Pdf.PdfWriter(output)))
        {
            var cleanup = new PdfCleanUpTool(document);
            foreach (var rect in RedactionCleanupGeometry.Build(block, area, hit, new[] { word }, false))
                cleanup.AddCleanupLocation(new PdfCleanUpLocation(1, rect, ColorConstants.WHITE)
                    .SetTextRegion(RedactionCleanupGeometry.GetOcrTextHitArea(rect)));
            cleanup.CleanUp();
        }
        using var result = PdfRegressionTests.Render(output.ToArray());
        if (rotation == 90) result.RotateFlip(RotateFlipType.Rotate270FlipNone);
        if (rotation == 180) result.RotateFlip(RotateFlipType.Rotate180FlipNone);
        if (rotation == 270) result.RotateFlip(RotateFlipType.Rotate90FlipNone);
        Assert.That(CountInk(result, new DrawingRectangle(27, 27, 111, 27)), Is.Zero,
            "All edge pixels of the raster word must be physically removed, not only its middle.");
        Assert.Multiple(() => {
            Assert.That(CountInk(result, new DrawingRectangle(28, 12, 107, 5)), Is.EqualTo(535));
            Assert.That(CountInk(result, new DrawingRectangle(28, 65, 107, 5)), Is.EqualTo(535));
        });
    }

    [Test]
    public void ExactBoxDoesNotExpandToOcrWords()
    {
        var box = new RedactionBlock { IncludeWholeWords = false };
        var area = new PdfRectangle(20.125f, 40.25f, 8.5f, 3.75f);
        var result = RedactionCleanupGeometry.Build(box, area, area,
            new[] { new PdfRectangle(10, 30, 100, 20) }, false);
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result[0], Is.SameAs(area));
    }

    [Test]
    public void NativeCursorKeepsInteriorHitArea()
    {
        var area = new PdfRectangle(20, 30, 100, 20);
        var hit = new PdfRectangle(20, 33, 100, 14);
        var result = RedactionCleanupGeometry.Build(new RedactionBlock { IsCursorSelection = true },
            area, hit, Array.Empty<PdfRectangle>(), false);
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result[0], Is.SameAs(hit));
    }

    [Test]
    public void ScannedWordCleanupPreservesInvisibleNeighbourWithOverlappingFontMetrics()
    {
        byte[] source = PdfRegressionTests.CreatePdf(document => {
            var page = document.AddNewPage(new PageSize(180, 100));
            var canvas = new PdfCanvas(page);
            var font = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);
            canvas.BeginText().SetFontAndSize(font, 18).SetTextRenderingMode(PdfCanvasConstants.TextRenderingMode.INVISIBLE)
                .MoveText(28, 53).ShowText("TARGET").EndText();
            canvas.BeginText().SetFontAndSize(font, 18).SetTextRenderingMode(PdfCanvasConstants.TextRenderingMode.INVISIBLE)
                .MoveText(28, 74).ShowText("NEIGHBOUR").EndText();
        });
        using var output = new MemoryStream();
        using (var document = new iText.Kernel.Pdf.PdfDocument(
            new iText.Kernel.Pdf.PdfReader(new MemoryStream(source)), new iText.Kernel.Pdf.PdfWriter(output)))
        {
            var cleanup = new PdfCleanUpTool(document);
            foreach (var rect in RedactionCleanupGeometry.Build(new RedactionBlock { IsCursorSelection = true },
                new PdfRectangle(30, 50, 100, 20), new PdfRectangle(30, 53, 100, 14),
                new[] { new PdfRectangle(27, 47, 110, 26) }, false))
                cleanup.AddCleanupLocation(new PdfCleanUpLocation(1, rect, ColorConstants.WHITE)
                    .SetTextRegion(RedactionCleanupGeometry.GetOcrTextHitArea(rect)));
            cleanup.CleanUp();
        }
        using var result = new iText.Kernel.Pdf.PdfDocument(new iText.Kernel.Pdf.PdfReader(new MemoryStream(output.ToArray())));
        string text = PdfTextExtractor.GetTextFromPage(result.GetPage(1));
        Assert.That(text, Does.Contain("NEIGHBOUR"));
        Assert.That(text, Does.Not.Contain("TARGET"));
    }

    [Test]
    public void GraphicCursorDoesNotInsetRasterArea()
    {
        var area = new PdfRectangle(20, 30, 100, 20);
        var hit = new PdfRectangle(20, 33, 100, 14);
        var result = RedactionCleanupGeometry.Build(new RedactionBlock { IsCursorSelection = true },
            area, hit, Array.Empty<PdfRectangle>(), true);
        Assert.That(result[0], Is.SameAs(area));
    }

    [TestCase(false)]
    [TestCase(true)]
    public void OcrTextHitAreaPreservesBothEndsAndOnlyInsetsAcrossTheText(bool vertical)
    {
        var area = new PdfRectangle(20, 30, 60, 12);
        var hit = RedactionCleanupGeometry.GetOcrTextHitArea(area, vertical);
        if (vertical)
        {
            Assert.That(hit.GetBottom(), Is.EqualTo(area.GetBottom()));
            Assert.That(hit.GetTop(), Is.EqualTo(area.GetTop()));
            Assert.That(hit.GetWidth(), Is.EqualTo(area.GetWidth() * 0.5f));
        }
        else
        {
            Assert.That(hit.GetLeft(), Is.EqualTo(area.GetLeft()));
            Assert.That(hit.GetRight(), Is.EqualTo(area.GetRight()));
            Assert.That(hit.GetHeight(), Is.EqualTo(area.GetHeight() * 0.5f));
        }
    }

    [Test]
    public void DefaultPdfSweepLocationsKeepTheirOriginalTextRegion()
    {
        var area = new PdfRectangle(20, 30, 60, 12);
        var location = new PdfCleanUpLocation(1, area, ColorConstants.WHITE);
        Assert.That(location.GetTextRegion(), Is.SameAs(area));
        var hit = RedactionCleanupGeometry.GetOcrTextHitArea(area);
        location.SetTextRegion(hit);
        Assert.That(location.GetRegion(), Is.SameAs(area));
        Assert.That(location.GetTextRegion(), Is.SameAs(hit));
        var derived = new OverriddenLocation(area, hit);
        Assert.That(derived.GetTextRegion(), Is.SameAs(hit));
    }

    private sealed class OverriddenLocation : PdfCleanUpLocation
    {
        private readonly PdfRectangle overridden;
        internal OverriddenLocation(PdfRectangle area, PdfRectangle overridden) : base(1, area)
        {
            this.overridden = overridden;
        }
        public override PdfRectangle GetRegion() => overridden;
    }

    [TestCase(true)]
    [TestCase(false)]
    public void PdfATextLayoutDoesNotUseExtraInkPaddingWhenRawGeometryIsAvailable(bool hasRawGeometry)
    {
        var raw = new PdfRectangle(20, 30, 60, 12);
        var padded = new PdfRectangle(18, 28, 64, 16);
        var line = new PdfTextSearcher.CachedLine();
        line.OcrWords.Add(new PdfTextSearcher.OcrWordInfo { Text = "TEST", BoundingBox = padded });
        if (hasRawGeometry) line.RawOcrWordBounds.Add(raw);
        Assert.That(PdfTextSearcher.GetOcrTextLayoutBounds(line, 0), Is.SameAs(hasRawGeometry ? raw : padded));
        Assert.That(PdfTextSearcher.GetOcrTextLayoutBounds(line, 1), Is.Null);
    }

    [Test]
    public void InkRefinementIncludesMissingEdgeGlyphsButNotNeighbouringTextOrGrid()
    {
        using var bitmap = new Bitmap(220, 100);
        using (var g = Graphics.FromImage(bitmap))
        {
            g.Clear(Color.White);
            g.FillRectangle(Brushes.Black, 28, 28, 7, 22);
            g.FillRectangle(Brushes.Black, 45, 28, 75, 22);
            g.FillRectangle(Brushes.Black, 132, 28, 5, 22);
            g.FillRectangle(Brushes.Black, 28, 15, 107, 5);
            g.FillRectangle(Brushes.Black, 28, 55, 107, 5);
            g.FillRectangle(Brushes.Black, 150, 28, 30, 22);
            g.DrawLine(Pens.Black, 0, 25, 219, 25);
        }
        var word = new RectangleF(30, 30, 100, 16);
        var refiner = new OcrInkBoundsRefiner(bitmap, new[] {
            word, new RectangleF(28, 15, 107, 5), new RectangleF(28, 55, 107, 5),
            new RectangleF(150, 28, 30, 22)
        });
        var refined = refiner.Refine(word);
        Assert.Multiple(() => {
            Assert.That(refined.Left, Is.LessThanOrEqualTo(28));
            Assert.That(refined.Right, Is.GreaterThanOrEqualTo(137));
            Assert.That(refined.Top, Is.InRange(26, 28));
            Assert.That(refined.Bottom, Is.InRange(50, 53));
            Assert.That(refined.Right, Is.LessThan(150));
        });
    }

    [Test]
    public void EmptyOrWhiteOcrWordDoesNotGrowIntoAdjacentInk()
    {
        using var bitmap = new Bitmap(120, 80);
        using (var g = Graphics.FromImage(bitmap)) g.Clear(Color.White);
        var word = new RectangleF(20, 30, 50, 15);
        Assert.That(new OcrInkBoundsRefiner(bitmap, new[] { word }).Refine(word), Is.EqualTo(word));
    }

    [TestCase(200, 60)]
    [TestCase(255, 230)]
    public void InkRefinementUsesLocalContrastRatherThanTreatingGreyPaperAsInk(int background, int foreground)
    {
        using var bitmap = new Bitmap(120, 80);
        using (var g = Graphics.FromImage(bitmap))
        using (var brush = new SolidBrush(Color.FromArgb(foreground, foreground, foreground)))
        {
            g.Clear(Color.FromArgb(background, background, background));
            g.FillRectangle(brush, 29, 29, 42, 18);
        }
        var word = new RectangleF(30, 30, 40, 16);
        var refined = new OcrInkBoundsRefiner(bitmap, new[] { word }).Refine(word);
        Assert.That(refined, Is.EqualTo(new RectangleF(28, 28, 44, 20)));
    }

    [Test, Category("Ocr")]
    public void WindowsOcrCompletesWithoutPumpingTheCallingSynchronizationContext()
    {
        var flags = BindingFlags.NonPublic | BindingFlags.Static;
        object engine = typeof(PdfTextSearcher).GetMethod("CreateWindowsOcrEngine", flags)
            .Invoke(null, new object[] { null });
        if (engine == null) Assert.Ignore("Windows OCR language is not installed.");
        string path = Path.Combine(Path.GetTempPath(), "AnonPDFPro-ocr-test-" + Guid.NewGuid().ToString("N") + ".png");
        try
        {
            using (var bitmap = new Bitmap(500, 100))
            using (var g = Graphics.FromImage(bitmap))
            using (var font = new Font("Arial", 24))
            {
                g.Clear(Color.White);
                g.DrawString("TEST 123456789", font, Brushes.Black, 20, 20);
                bitmap.Save(path, ImageFormat.Png);
            }
            Task recognition;
            var previous = SynchronizationContext.Current;
            try
            {
                SynchronizationContext.SetSynchronizationContext(new NonPumpingContext());
                recognition = (Task)typeof(PdfTextSearcher).GetMethod("RecognizeWindowsOcrAsync", flags)
                    .Invoke(null, new[] { engine, path });
            }
            finally { SynchronizationContext.SetSynchronizationContext(previous); }
            Assert.That(recognition.Wait(TimeSpan.FromSeconds(15)), Is.True,
                "Headless OCR must not depend on the blocked WinForms synchronization context.");
            object result = recognition.GetType().GetProperty("Result").GetValue(recognition);
            Assert.That(result.GetType().GetProperty("Text").GetValue(result).ToString(), Does.Contain("TEST"));
        }
        finally { File.Delete(path); }
    }

    private sealed class NonPumpingContext : SynchronizationContext
    {
        public override void Post(SendOrPostCallback callback, object state) { }
    }

    private static int CountInk(Bitmap bitmap, DrawingRectangle area)
    {
        int count = 0;
        for (int y = area.Top; y < area.Bottom; y++)
            for (int x = area.Left; x < area.Right; x++)
                if (bitmap.GetPixel(x, y).R < 128) count++;
        return count;
    }
}
