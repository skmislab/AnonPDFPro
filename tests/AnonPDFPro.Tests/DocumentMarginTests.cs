using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using AnonPDF;
using iText.IO.Font.Constants;
using iText.IO.Image;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf.Xobject;
using NUnit.Framework;
using PdfRectangle = iText.Kernel.Geom.Rectangle;

namespace AnonPDFPro.Tests;

[TestFixture, NonParallelizable]
public class DocumentMarginTests
{
    private static readonly DocumentMarginSettings Margins20 = new() {
        TopMillimeters = 20, BottomMillimeters = 20, LeftMillimeters = 20, RightMillimeters = 20
    };

    private static IEnumerable<TestCaseData> VisiblePageCases()
    {
        foreach (int rotation in new[] { 0, 90, 180, 270 })
            foreach (bool cropped in new[] { false, true })
                foreach (bool offset in new[] { false, true })
                    yield return new TestCaseData(rotation, cropped, offset);
    }

    private static byte[] Apply(byte[] source, DocumentMarginSettings margins)
    {
        using var output = new MemoryStream();
        using (var pdf = new PdfDocument(new PdfReader(new MemoryStream(source)), new PdfWriter(output)))
            PDFForm.ApplyDocumentMarginsToPdf(pdf, margins);
        return output.ToArray();
    }

    private static byte[] HiddenContent(int rotation, bool cropped, bool offset, bool oversizedCrop = false)
    {
        return PdfRegressionTests.CreatePdf(pdf => {
            float x = offset ? -60 : 0;
            float y = offset ? 35 : 0;
            var page = pdf.AddNewPage(new PageSize(400, 400));
            page.SetMediaBox(new PdfRectangle(x, y, 400, 400)).SetRotation(rotation);
            if (cropped) page.SetCropBox(new PdfRectangle(x + 40, y + 50, 300, 280));
            if (oversizedCrop) page.SetCropBox(new PdfRectangle(x - 20, y - 20, 440, 440));
            var visible = page.GetCropBox().GetIntersection(page.GetMediaBox());
            var canvas = new PdfCanvas(page);
            canvas.SetFillColor(ColorConstants.GREEN).Rectangle(x + 170, y + 170, 60, 60).Fill();
            canvas.SetFillColor(ColorConstants.RED)
                .Rectangle(visible.GetLeft() - 18, y + 190, 10, 20)
                .Rectangle(visible.GetRight() + 8, y + 190, 10, 20)
                .Rectangle(x + 190, visible.GetBottom() - 18, 20, 10)
                .Rectangle(x + 190, visible.GetTop() + 8, 20, 10).Fill()
                .BeginText().SetFontAndSize(PdfFontFactory.CreateFont(StandardFonts.HELVETICA), 10)
                .MoveText(x + 100, visible.GetBottom() - 15).ShowText("HIDDEN AUTHOR DATA").EndText();

            var form = new PdfFormXObject(new PdfRectangle(0, 0, 20, 10));
            new PdfCanvas(form, pdf).SetFillColor(ColorConstants.RED).Rectangle(0, 0, 20, 10).Fill();
            var after = new PdfCanvas(page.NewContentStreamAfter(), page.GetResources(), pdf);
            after.AddXObjectAt(form, x + 150, visible.GetTop() + 5);
            using var image = new Bitmap(10, 20);
            using (var graphics = Graphics.FromImage(image)) graphics.Clear(System.Drawing.Color.Red);
            using var png = new MemoryStream();
            image.Save(png, ImageFormat.Png);
            after.AddImageFittedIntoRectangle(ImageDataFactory.Create(png.ToArray()),
                new PdfRectangle(visible.GetLeft() - 18, y + 130, 10, 20), false);
        });
    }

    private static void AssertNoHiddenContent(byte[] result)
    {
        using var image = PdfRegressionTests.Render(result);
        int red = 0;
        int green = 0;
        for (int y = 0; y < image.Height; y++)
            for (int x = 0; x < image.Width; x++)
            {
                var color = image.GetPixel(x, y);
                if (color.R > 170 && color.G < 80 && color.B < 80) red++;
                if (color.G > 170 && color.R < 80 && color.B < 80) green++;
            }
        Assert.Multiple(() => {
            Assert.That(red, Is.Zero, "Originally hidden text, vectors, images and forms must not enter the page after scaling.");
            Assert.That(green, Is.GreaterThan(1000), "Visible content must remain on the page.");
        });
    }

    [TestCaseSource(nameof(VisiblePageCases))]
    public void MarginsKeepContentOutsideTheOriginalVisiblePageHidden(int rotation, bool cropped, bool offset)
    {
        var source = HiddenContent(rotation, cropped, offset);
        var result = Apply(source, Margins20);
        AssertNoHiddenContent(result);
        using var before = new PdfDocument(new PdfReader(new MemoryStream(source)));
        using var after = new PdfDocument(new PdfReader(new MemoryStream(result)));
        Assert.Multiple(() => {
            Assert.That(after.GetNumberOfPages(), Is.EqualTo(1));
            Assert.That(after.GetPage(1).GetRotation(), Is.EqualTo(rotation));
            Assert.That(after.GetPage(1).GetMediaBox().ToString(), Is.EqualTo(before.GetPage(1).GetMediaBox().ToString()));
            Assert.That(after.GetPage(1).GetCropBox().ToString(), Is.EqualTo(before.GetPage(1).GetCropBox().ToString()));
        });
    }

    [TestCase(0)]
    [TestCase(90)]
    [TestCase(180)]
    [TestCase(270)]
    public void CropBoxCannotExposeContentOutsideTheOriginalMediaBox(int rotation)
    {
        AssertNoHiddenContent(Apply(HiddenContent(rotation, false, true, oversizedCrop: true), Margins20));
    }

    [Test]
    public void FullBleedContentIsClippedAtTheScaledPageEdgeNotTheUnscaledEdge()
    {
        var source = PdfRegressionTests.CreatePdf(pdf => {
            var page = pdf.AddNewPage(new PageSize(400, 400));
            new PdfCanvas(page).SetFillColor(ColorConstants.GREEN).Rectangle(-100, -100, 600, 600).Fill();
        });
        using var result = PdfRegressionTests.Render(Apply(source, Margins20));
        Assert.Multiple(() => {
            Assert.That(result.GetPixel(20, 200).R, Is.EqualTo(255));
            Assert.That(result.GetPixel(380, 200).R, Is.EqualTo(255));
            Assert.That(result.GetPixel(200, 20).R, Is.EqualTo(255));
            Assert.That(result.GetPixel(200, 380).R, Is.EqualTo(255));
            Assert.That(result.GetPixel(60, 200).R, Is.Zero, "The visible portion of edge-crossing content must be retained.");
        });
    }

    [TestCase(false)]
    [TestCase(true)]
    public void NoMarginsLeaveTheContentStreamsUnchanged(bool nullSettings)
    {
        var source = HiddenContent(0, false, false);
        using var output = new MemoryStream();
        using var pdf = new PdfDocument(new PdfReader(new MemoryStream(source)), new PdfWriter(output));
        var page = pdf.GetPage(1);
        var content = page.GetContentBytes();
        int streamCount = page.GetContentStreamCount();
        PDFForm.ApplyDocumentMarginsToPdf(pdf, nullSettings ? null : new DocumentMarginSettings());
        Assert.That(page.GetContentStreamCount(), Is.EqualTo(streamCount));
        Assert.That(page.GetContentBytes(), Is.EqualTo(content));
    }
}
