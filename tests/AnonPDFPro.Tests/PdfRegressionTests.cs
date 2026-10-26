using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using iText.IO.Font.Constants;
using iText.IO.Image;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Data;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using iText.Kernel.Pdf.Tagging;
using iText.Kernel.Pdf.Tagutils;
using iText.Kernel.Pdf.Xobject;
using iText.PdfCleanup;
using NUnit.Framework;
using PdfRectangle = iText.Kernel.Geom.Rectangle;
using Color = System.Drawing.Color;
using Path = System.IO.Path;

namespace AnonPDFPro.Tests;

[TestFixture, NonParallelizable]
public class PdfRegressionTests
{
    // The application also bypasses the wrapper's incorrect char return marshalling on net48.
    [DllImport("pdfium_x64.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "FPDFText_GetUnicode")]
    private static extern uint GetUnicode(PDFiumSharp.Types.FPDF_TEXTPAGE textPage, int index);

    internal static byte[] CreatePdf(Action<PdfDocument> populate)
    {
        using var output = new MemoryStream();
        using (var document = new PdfDocument(new PdfWriter(output))) populate(document);
        return output.ToArray();
    }

    internal static byte[] Clean(byte[] source, PdfRectangle rectangle)
    {
        using var output = new MemoryStream();
        using (var document = new PdfDocument(new PdfReader(new MemoryStream(source)), new PdfWriter(output)))
        {
            var tool = new PdfCleanUpTool(document);
            tool.AddCleanupLocation(new PdfCleanUpLocation(1, rectangle, ColorConstants.WHITE));
            tool.CleanUp();
        }
        return output.ToArray();
    }

    internal static Bitmap Render(byte[] bytes)
    {
        using var document = new PDFiumSharp.PdfDocument(data: bytes, password: string.Empty);
        var page = document.Pages[0];
        using var bitmap = new PDFiumSharp.PDFiumBitmap((int)page.Width, (int)page.Height, true);
        bitmap.FillRectangle(0, 0, bitmap.Width, bitmap.Height, 0xffffffff);
        page.Render(bitmap, flags: PDFiumSharp.Enums.RenderingFlags.Annotations);
        using var stream = new MemoryStream();
        bitmap.Save(stream);
        stream.Position = 0;
        using var image = Image.FromStream(stream);
        return new Bitmap(image);
    }

    private static PdfImageXObject ImageObject(PdfDocument document, Color color)
    {
        using var bitmap = new Bitmap(100, 100);
        using (var graphics = Graphics.FromImage(bitmap)) graphics.Clear(color);
        using var stream = new MemoryStream();
        bitmap.Save(stream, ImageFormat.Png);
        return new PdfImageXObject(ImageDataFactory.Create(stream.ToArray()));
    }

    private sealed class Images : IEventListener
    {
        public readonly List<(PdfImageXObject Image, float[] Ctm)> Items = new();
        public void EventOccurred(IEventData data, EventType type)
        {
            var info = (ImageRenderInfo)data;
            var matrix = info.GetImageCtm();
            Items.Add((info.GetImage(), Enumerable.Range(0, 9).Select(matrix.Get).ToArray()));
        }
        public ICollection<EventType> GetSupportedEvents() => new[] { EventType.RENDER_IMAGE };
    }

    [TestCase("shared", 0, 400, 300)]
    [TestCase("nested", 0, 400, 300)]
    [TestCase("shared", 90, 420, 595)]
    [TestCase("shared", 180, 612, 792)]
    [TestCase("shared", 270, 595, 842)]
    public void SharedImageTransformAndPixelsOutsideRedactionArePreserved(string mode, int rotation, int width, int height)
    {
        var source = CreatePdf(document => {
            var page = document.AddNewPage(new PageSize(width, height));
            page.SetRotation(rotation);
            var first = page.GetResources().AddImage(ImageObject(document, Color.Red));
            var second = page.GetResources().AddImage(ImageObject(document, Color.Blue));
            var prefix = mode == "nested" ? "q 2 0 0 2 0 0 cm q 80 0 0 80 10 40 cm " : "q 160 0 0 160 20 80 cm ";
            new PdfCanvas(page).WriteLiteral(prefix + first + " Do " + second + " Do Q" + (mode == "nested" ? " Q" : ""));
        });
        var cleaned = Clean(source, new PdfRectangle(60, 120, 20, 20));
        using var before = new PdfDocument(new PdfReader(new MemoryStream(source)));
        using var after = new PdfDocument(new PdfReader(new MemoryStream(cleaned)));
        var originalImages = new Images();
        var cleanedImages = new Images();
        new PdfCanvasProcessor(originalImages).ProcessPageContent(before.GetPage(1));
        new PdfCanvasProcessor(cleanedImages).ProcessPageContent(after.GetPage(1));
        Assert.That(cleanedImages.Items.Count, Is.EqualTo(2));
        for (var i = 0; i < 2; i++)
        {
            Assert.That(cleanedImages.Items[i].Ctm, Is.EqualTo(originalImages.Items[i].Ctm).Within(0.001));
            using var stream = new MemoryStream(cleanedImages.Items[i].Image.GetImageBytes());
            using var image = new Bitmap(stream);
            Assert.That(image.GetPixel(30, 70).R, Is.GreaterThan(250), "Underlying image pixels, not just an overlay, must be cleared");
            Assert.That(image.GetPixel(30, 70).G, Is.GreaterThan(250));
            Assert.That(image.GetPixel(30, 70).B, Is.GreaterThan(250));
            var untouched = image.GetPixel(80, 20);
            Assert.That(i == 0 ? untouched.R : untouched.B, Is.GreaterThan(250));
            Assert.That(untouched.G, Is.LessThan(5));
        }
        Assert.That(after.GetPage(1).GetRotation(), Is.EqualTo(rotation));
        Assert.That(after.GetPage(1).GetPageSize().GetWidth(), Is.EqualTo(width));
        Assert.That(after.GetPage(1).GetPageSize().GetHeight(), Is.EqualTo(height));
        using var renderedBefore = Render(source);
        using var renderedAfter = Render(cleaned);
        Assert.That(renderedAfter.Size, Is.EqualTo(renderedBefore.Size));
        var coloredBefore = 0;
        var coloredAfter = 0;
        for (var y = 0; y < renderedAfter.Height; y++)
            for (var x = 0; x < renderedAfter.Width; x++)
            {
                if (renderedBefore.GetPixel(x, y).G < 100) coloredBefore++;
                if (renderedAfter.GetPixel(x, y).G < 100) coloredAfter++;
            }
        Assert.That(coloredBefore, Is.GreaterThan(25000));
        Assert.That(coloredAfter, Is.InRange(coloredBefore - 500, coloredBefore - 350));
    }

    [TestCase(false)]
    [TestCase(true)]
    [Category("UpgradeRequired")]
    public void RedactedTextCannotSurviveInAccessibilityMetadata(bool structureTree)
    {
        const string secret = "PRIVATE-ALT-DATA";
        var source = CreatePdf(document => {
            var page = document.AddNewPage(new PageSize(400, 300));
            var canvas = new PdfCanvas(page);
            if (structureTree)
            {
                document.SetTagged();
                var tag = new TagTreePointer(document).SetPageForTagging(page).AddTag(StandardRoles.P);
                tag.GetProperties().SetActualText(secret).SetAlternateDescription(secret);
                tag.AddTag(StandardRoles.SPAN);
                tag.GetProperties().SetActualText(secret).SetAlternateDescription(secret);
                canvas.OpenTag(tag.GetTagReference());
            }
            else
            {
                var properties = new PdfDictionary();
                properties.Put(PdfName.ActualText, new PdfString(secret));
                properties.Put(PdfName.Alt, new PdfString(secret));
                canvas.BeginMarkedContent(PdfName.Span, properties);
            }
            canvas.BeginText().SetFontAndSize(PdfFontFactory.CreateFont(StandardFonts.HELVETICA), 12)
                .MoveText(20, 220).ShowText("PUBLIC").MoveText(180, 0).ShowText("SECRET").EndText().EndMarkedContent();
        });
        var cleaned = Clean(source, new PdfRectangle(198, 215, 60, 20));
        using var document = new PdfDocument(new PdfReader(new MemoryStream(cleaned)));
        var text = PdfTextExtractor.GetTextFromPage(document.GetPage(1), new SimpleTextExtractionStrategy());
        Assert.That(text, Does.Contain("PUBLIC").And.Not.Contain("SECRET"));
        Assert.That(System.Text.Encoding.ASCII.GetString(document.GetPage(1).GetContentBytes()), Does.Not.Contain(secret));
        for (var i = 1; i < document.GetNumberOfPdfObjects(); i++)
        {
            if (document.GetPdfObject(i) is PdfDictionary dictionary)
                foreach (var key in new[] { PdfName.ActualText, PdfName.Alt })
                    Assert.That(dictionary.GetAsString(key)?.ToUnicodeString(), Is.Not.EqualTo(secret), "Sensitive " + key + " retained on object " + i);
        }
    }

    [Test]
    public void PdfiumRendersContentAndPreservesSearchableText()
    {
        var pdf = CreatePdf(document => {
            var canvas = new PdfCanvas(document.AddNewPage(new PageSize(400, 300)));
            canvas.SetFillColor(ColorConstants.BLACK).Rectangle(50, 60, 100, 80).Fill();
            canvas.BeginText().SetFontAndSize(PdfFontFactory.CreateFont(StandardFonts.HELVETICA), 16)
                .MoveText(50, 240).ShowText("SEARCHABLE 123456789").EndText();
        });
        using var rendered = Render(pdf);
        Assert.That(rendered.GetPixel(75, 200).R, Is.LessThan(5));
        Assert.That(rendered.GetPixel(10, 10).R, Is.GreaterThan(250));
        using var document = new PdfDocument(new PdfReader(new MemoryStream(pdf)));
        Assert.That(PdfTextExtractor.GetTextFromPage(document.GetPage(1)), Does.Contain("SEARCHABLE 123456789"));
    }

    [Test, Category("Ocr")]
    public void NativeOcrLoadsTheShippedPolishModelAndRecognizesText()
    {
        var data = Path.Combine(TestContext.CurrentContext.TestDirectory, "tessdata");
        Assert.That(File.Exists(Path.Combine(data, "pol.traineddata")), Is.True, "Restore the application's OCR language data first");
        using var image = new Bitmap(1000, 160);
        using (var graphics = Graphics.FromImage(image))
        using (var font = new Font("Arial", 48))
        {
            graphics.Clear(Color.White);
            graphics.DrawString("TEST DOKUMENTU 12345", font, Brushes.Black, 20, 35);
        }
        using var stream = new MemoryStream();
        image.Save(stream, ImageFormat.Png);
        using var pix = TesseractOCR.Pix.Image.LoadFromMemory(stream.ToArray());
        using var engine = new TesseractOCR.Engine(data, "pol", TesseractOCR.Enums.EngineMode.LstmOnly, null, null, false, null);
        using var page = engine.Process(pix, TesseractOCR.Enums.PageSegMode.SingleLine);
        Assert.That(page.Text, Does.Contain("DOKUMENTU").And.Contain("12345"));
    }

    [TestCase(0)]
    [TestCase(90)]
    [TestCase(270)]
    public void PdfiumTextGeometryRemainsInPdfCoordinatesAfterPageRotation(int rotation)
    {
        var bytes = CreatePdf(document => {
            var page = document.AddNewPage(new PageSize(400, 300)).SetRotation(rotation);
            new PdfCanvas(page).BeginText().SetFontAndSize(PdfFontFactory.CreateFont(StandardFonts.HELVETICA), 20)
                .MoveText(50, 200).ShowText("WORD").EndText();
        });
        using var document = new PDFiumSharp.PdfDocument(data: bytes, password: string.Empty);
        var text = PDFiumSharp.PDFium.FPDFText_LoadPage(document.Pages[0].Handle);
        try
        {
            Assert.That(PDFiumSharp.PDFium.FPDFText_CountChars(text), Is.EqualTo(4));
            Assert.That(GetUnicode(text, 0), Is.EqualTo((uint)'W'));
            Assert.That(PDFiumSharp.PDFium.FPDFText_GetCharBox(text, 0, out var left, out var right, out var bottom, out var top), Is.True);
            Assert.That(left, Is.InRange(49, 52));
            Assert.That(right, Is.InRange(65, 72));
            Assert.That(bottom, Is.InRange(198, 202));
            Assert.That(top, Is.InRange(212, 217));
        }
        finally { PDFiumSharp.PDFium.FPDFText_ClosePage(text); }
    }
}
