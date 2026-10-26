using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using AnonPDF;
using MessagePack;
using Newtonsoft.Json;
using NUnit.Framework;
using PdfRectangle = iText.Kernel.Geom.Rectangle;

namespace AnonPDFPro.Tests;

[TestFixture]
public class CommandLineTests
{
    [TestCase("--help")]
    [TestCase("-h")]
    [TestCase("/?")]
    public void HelpAliasesAreRecognized(string argument)
    {
        Assert.That(Program.ParseStartupOptions(new[] { argument }).ShowHelp, Is.True);
    }

    [Test]
    public void TemplateProcessingKeepsExplicitInputAndOutput()
    {
        var options = Program.ParseStartupOptions(new[] {
            "source.pdf", "--headless", "--template", "layout.app",
            "--pdfout", "result.pdf", "--overwrite", "--skip-template-validation"
        });
        Assert.Multiple(() => {
            Assert.That(options.InputPath, Is.EqualTo(Path.GetFullPath("source.pdf")));
            Assert.That(options.TemplatePath, Is.EqualTo(Path.GetFullPath("layout.app")));
            Assert.That(options.PdfOutputPath, Is.EqualTo(Path.GetFullPath("result.pdf")));
            Assert.That(options.Headless && options.OverwriteOutput && options.SkipTemplateValidation, Is.True);
            Assert.That(options.RunOcr || options.IndexText, Is.False);
        });
    }

    [Test]
    public void PdfAEnablesTheRequiredTextPreparation()
    {
        var options = Program.ParseStartupOptions(new[] { "--format", "pdfa" });
        Assert.That(options.PdfAOutput && options.RunOcr && options.IndexText, Is.True);
    }

    [Test]
    public void NativeTextIndexingDoesNotEnableOcr()
    {
        var options = Program.ParseStartupOptions(new[] { "--text" });
        Assert.That(options.IndexText, Is.True);
        Assert.That(options.RunOcr, Is.False);
    }

    [TestCase("--template")]
    [TestCase("--format")]
    [TestCase("--lang")]
    [TestCase("--unknown")]
    public void MissingOrUnknownOptionsReturnAnError(string argument)
    {
        Assert.That(Program.ParseStartupOptions(new[] { argument }).ParseError, Is.Not.Null.And.Not.Empty);
    }

    [Test]
    public void GeometryBypassRequiresHeadlessMode()
    {
        var options = Program.ParseStartupOptions(new[] { "--skip-template-validation" });
        Assert.That(Program.ValidateHeadlessOptions(options, out var error, out var code), Is.False);
        Assert.That(error, Is.Not.Null.And.Not.Empty);
        Assert.That(code, Is.EqualTo(2));
    }

    [Test]
    public void ExistingOutputRequiresExplicitOverwrite()
    {
        var directory = Path.Combine(Path.GetTempPath(), "AnonPDFPro-tests-" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(directory);
        try
        {
            var input = Path.Combine(directory, "input.pdf");
            var template = Path.Combine(directory, "template.app");
            var output = Path.Combine(directory, "output.pdf");
            File.WriteAllText(input, "input");
            File.WriteAllText(template, "{}");
            File.WriteAllText(output, "keep-existing-output");
            var options = Program.ParseStartupOptions(new[] {
                input, "--headless", "--template", template, "--pdfout", output
            });
            Assert.That(Program.ValidateHeadlessOptions(options, out _, out var code), Is.False);
            Assert.That(code, Is.EqualTo(4));
            Assert.That(File.ReadAllText(output), Is.EqualTo("keep-existing-output"));
            options.OverwriteOutput = true;
            Assert.That(Program.ValidateHeadlessOptions(options, out _, out _), Is.True);
            options.PdfOutputPath = input;
            Assert.That(Program.ValidateHeadlessOptions(options, out _, out _), Is.False);
        }
        finally { Directory.Delete(directory, true); }
    }
}

[TestFixture]
public class DocumentContractTests
{
    [TestCase(0, 0, 595, 842, false)]
    [TestCase(-0.5f, 20, 10, 10, false)]
    [TestCase(-2, 20, 10, 10, true)]
    [TestCase(590, 20, 10, 10, true)]
    [TestCase(20, 840, 10, 10, true)]
    public void OutsidePageDetectionRespectsTheVisiblePage(float x, float y, float w, float h, bool expected)
    {
        Assert.That(PdfTextSearcher.IsOutOfPageBounds(new PdfRectangle(x, y, w, h), 0, 0, 595, 842), Is.EqualTo(expected));
    }

    [Test]
    public void OffsetPageOriginIsRespected()
    {
        Assert.That(PdfTextSearcher.IsOutOfPageBounds(new PdfRectangle(5, 25, 10, 10), 10, 20, 420, 595), Is.True);
        Assert.That(PdfTextSearcher.IsOutOfPageBounds(new PdfRectangle(15, 25, 10, 10), 10, 20, 420, 595), Is.False);
    }

    [Test]
    public void OldProjectsKeepWholeWordSelectionAsTheirDefault()
    {
        var block = JsonConvert.DeserializeObject<RedactionBlock>("{\"PageNumber\":1}");
        Assert.That(block.IncludeWholeWords, Is.True);
    }

    [Test]
    public void PreciseBoxBoundsAndWholeWordPreferenceSurviveProjectSerialization()
    {
        var block = new RedactionBlock(new RectangleF(12.125f, 27.375f, 81.625f, 15.875f), 2) {
            IncludeWholeWords = false, ScopeId = "scope", MatchedTag = "Za\u017c\u00f3\u0142\u0107 g\u0119\u015bl\u0105 ja\u017a\u0144"
        };
        var copy = JsonConvert.DeserializeObject<RedactionBlock>(JsonConvert.SerializeObject(block));
        Assert.Multiple(() => {
            Assert.That(copy.Bounds, Is.EqualTo(block.Bounds));
            Assert.That(copy.PageNumber, Is.EqualTo(2));
            Assert.That(copy.IncludeWholeWords, Is.False);
            Assert.That(copy.MatchedTag, Is.EqualTo(block.MatchedTag));
        });
    }

    [Test]
    public void MarginCloneDoesNotChangeTheOriginalDocumentSettings()
    {
        var original = new DocumentMarginSettings { TopMillimeters = 20, LeftMillimeters = 5 };
        var copy = original.Clone();
        copy.TopMillimeters = 30;
        Assert.That(original.TopMillimeters, Is.EqualTo(20));
        Assert.That(copy.LeftMillimeters, Is.EqualTo(5));
        Assert.That(original.HasMargins, Is.True);
        Assert.That(new DocumentMarginSettings().HasMargins, Is.False);
    }

    [Test]
    public void CompressedCachePreservesUnicodeOcrGeometryAndMissingOptionalData()
    {
        var cache = new PdfCacheFile {
            Version = 4, FileSize = 1234, LastWriteTimeUtcTicks = 987654321,
            NerCacheIdentity = "PII_PL:test",
            Lines = new List<CachedLineDto> { new CachedLineDto {
                PageNumber = 2, PageRotation = 90, Text = "Ziela\u0144ski Stra\u00dfe \u017b\u00f3\u0142\u0107",
                IsOcr = true, PageWidth = 420, PageHeight = 595, PageLeft = 12.25f,
                OcrWordBoundsFlat = new[] { 13.125f, 25.25f, 62.5f, 9.875f },
                Characters = new List<CharInfoDto> { new CharInfoDto { Ch = '\u0144', X = 13.125f, Y = 25.25f, W = 5.5f, H = 9.875f } }
            } }
        };
        var options = MessagePackSerializerOptions.Standard.WithCompression(MessagePackCompression.Lz4BlockArray);
        var copy = MessagePackSerializer.Deserialize<PdfCacheFile>(MessagePackSerializer.Serialize(cache, options), options);
        Assert.Multiple(() => {
            Assert.That(copy.Version, Is.EqualTo(cache.Version));
            Assert.That(copy.PersonalData, Is.Null);
            Assert.That(copy.Lines[0].Text, Is.EqualTo(cache.Lines[0].Text));
            Assert.That(copy.Lines[0].OcrWordBoundsFlat, Is.EqualTo(cache.Lines[0].OcrWordBoundsFlat));
            Assert.That(copy.Lines[0].Characters[0].Ch, Is.EqualTo('\u0144'));
            Assert.That(copy.Lines[0].PageRotation, Is.EqualTo(90));
        });
    }
}
