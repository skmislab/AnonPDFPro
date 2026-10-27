using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AnonPDF;
using iText.IO.Font.Constants;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf.Canvas.Parser;
using Newtonsoft.Json;
using NUnit.Framework;

namespace AnonPDFPro.Tests;

[TestFixture]
public class PageOrganizationTests
{
    internal static byte[] Document(string prefix, int count = 3)
    {
        return PdfRegressionTests.CreatePdf(pdf => {
            for (var i = 1; i <= count; i++)
            {
                var page = pdf.AddNewPage(i == 2 ? PageSize.A5 : PageSize.A4);
                if (i == 3) page.SetRotation(90);
                new PdfCanvas(page).BeginText().SetFontAndSize(PdfFontFactory.CreateFont(StandardFonts.HELVETICA), 16)
                    .MoveText(30, 200).ShowText(prefix + i).EndText();
            }
        });
    }

    [Test]
    public void NewPlanDoesNotChangeTheDocument()
    {
        var plan = PageOrganizationPlan.Open(Document("A"));
        Assert.That(plan.IsChanged, Is.False);
        Assert.That(plan.ExistingPageMap().Values, Is.EqualTo(new[] { 1, 2, 3 }));
    }

    [Test]
    public void OriginalPagesAreMarkedAndRestoredWithoutChangingThePdfLayout()
    {
        var plan = PageOrganizationPlan.Open(Document("A"));
        var ids = plan.Pages.Select(p => p.Id).ToArray();
        Assert.That(plan.RemoveOrRestore(new[] { 0 }), Is.True);
        Assert.That(plan.IsChanged, Is.True);
        Assert.That(plan.IsLayoutChanged, Is.False);
        Assert.That(plan.Pages.Select(p => p.Id), Is.EqualTo(ids));
        Assert.That(plan.Pages[0].MarkedForDeletion, Is.True);
        Assert.That(plan.RemapProject(new ProjectData()).PagesToRemove, Is.EquivalentTo(new[] { 1 }));
        using (var composed = new PdfDocument(new PdfReader(new MemoryStream(plan.Compose()))))
            Assert.That(composed.GetNumberOfPages(), Is.EqualTo(3), "Deletion marks must not physically remove original pages from the working PDF.");
        Assert.That(plan.RemoveOrRestore(new[] { 0 }), Is.True);
        Assert.That(plan.Pages[0].MarkedForDeletion, Is.False);
        Assert.That(plan.IsChanged, Is.False);
    }

    [Test]
    public void ExistingMenuDeletionMarksAreInitializedAndCanBeRestoredTransactionally()
    {
        var removed = new HashSet<int> { 2 };
        var project = new ProjectData { PagesToRemove = removed };
        var plan = PageOrganizationPlan.Open(Document("A"), removedPages: removed);
        Assert.That(plan.Pages[1].MarkedForDeletion, Is.True);
        Assert.That(plan.IsChanged, Is.False);
        Assert.That(plan.RemoveOrRestore(new[] { 1 }), Is.True);
        Assert.That(plan.RemapProject(project).PagesToRemove, Is.Empty);
        Assert.That(removed, Is.EquivalentTo(new[] { 2 }), "Cancelling the dialog must leave the application model unchanged.");
        Assert.That(plan.RemoveOrRestore(new[] { 1 }), Is.True);
        Assert.That(plan.IsChanged, Is.False);
        Assert.That(plan.RemapProject(project).PagesToRemove, Is.EquivalentTo(new[] { 2 }));
    }

    [Test]
    public void NewlyAddedBlankAndImportedPagesReallyDisappear()
    {
        var plan = PageOrganizationPlan.Open(Document("A"));
        var originalIds = plan.Pages.Select(p => p.Id).ToArray();
        plan.AddBlank(1, 595, 842);
        plan.AddPdf(Document("B", 1), "", new[] { 1 }, 2);
        Assert.That(plan.RemoveOrRestore(new[] { 1, 2 }), Is.True);
        Assert.That(plan.Pages.Select(p => p.Id), Is.EqualTo(originalIds));
        Assert.That(plan.Pages.Any(p => p.MarkedForDeletion), Is.False);
        Assert.That(plan.IsChanged, Is.False);
    }

    [Test]
    public void MixedSelectionRemovesAddedPagesAndMarksOriginalsBeforeRemapping()
    {
        var plan = PageOrganizationPlan.Open(Document("A"), removedPages: new[] { 2 });
        plan.AddBlank(1, 595, 842); // old 1, blank, old 2, old 3
        Assert.That(plan.RemoveOrRestore(new[] { 0, 1 }), Is.True);
        plan.Move(new[] { 2 }, 0); // old 3, old 1, old 2
        var project = plan.RemapProject(new ProjectData { PagesToRemove = new HashSet<int> { 2 } });
        Assert.That(plan.Pages.Count, Is.EqualTo(3));
        Assert.That(plan.Pages.Select(p => p.MarkedForDeletion), Is.EqualTo(new[] { false, true, true }));
        Assert.That(project.PagesToRemove, Is.EquivalentTo(new[] { 2, 3 }));
        var restored = JsonConvert.DeserializeObject<ProjectData>(JsonConvert.SerializeObject(project));
        Assert.That(restored.PagesToRemove, Is.EquivalentTo(new[] { 2, 3 }));
    }

    [Test]
    public void PartlyMarkedSelectionIsFirstMarkedAsAGroupAndThenRestoredAsAGroup()
    {
        var plan = PageOrganizationPlan.Open(Document("A"), removedPages: new[] { 2 });
        Assert.That(plan.RemoveOrRestore(new[] { 0, 1 }), Is.True);
        Assert.That(plan.Pages.Take(2).All(p => p.MarkedForDeletion), Is.True);
        Assert.That(plan.RemoveOrRestore(new[] { 0, 1 }), Is.True);
        Assert.That(plan.Pages.Take(2).Any(p => p.MarkedForDeletion), Is.False);
        Assert.That(plan.RemapProject(new ProjectData { PagesToRemove = new HashSet<int> { 2 } }).PagesToRemove, Is.Empty);
    }

    [Test]
    public void RemovingEveryOutputPageIsRejectedWithoutPartialChanges()
    {
        var plan = PageOrganizationPlan.Open(Document("A", 1));
        Assert.That(plan.RemoveOrRestore(new[] { 0 }), Is.False);
        plan.AddBlank(1, 595, 842);
        var ids = plan.Pages.Select(p => p.Id).ToArray();
        Assert.That(plan.RemoveOrRestore(new[] { 0, 1 }), Is.False);
        Assert.That(plan.Pages.Select(p => p.Id), Is.EqualTo(ids));
        Assert.That(plan.Pages.Any(p => p.MarkedForDeletion), Is.False);
    }

    [Test]
    public void InvalidRemovalSelectionDoesNotChangeAnyPage()
    {
        var plan = PageOrganizationPlan.Open(Document("A"));
        Assert.That(() => plan.RemoveOrRestore(new[] { 0, 3 }), Throws.ArgumentException);
        Assert.That(plan.IsChanged, Is.False);
        Assert.That(plan.Pages.Any(p => p.MarkedForDeletion), Is.False);
    }

    [TestCase(0)]
    [TestCase(90)]
    [TestCase(180)]
    [TestCase(270)]
    public void LazyPreviewOfBlankPage800IncludesAddedContentWithoutModifyingTheSource(int rotation)
    {
        var bytes = PdfRegressionTests.CreatePdf(pdf => {
            for (int i = 0; i < 800; i++) pdf.AddNewPage(new PageSize(200, 100));
        });
        using var source = new PdfDocument(new PdfReader(new MemoryStream(bytes)));
        Assert.That(PageOrganizationPlan.IsEmptyPage(source.GetPage(800)), Is.True);
        byte[] preview = PageOrganizationPlan.CreatePagePreview(source, 800, rotation, (target, page) => {
            new PdfCanvas(page).Rectangle(20, 20, 50, 30).Fill().BeginText()
                .SetFontAndSize(PdfFontFactory.CreateFont(StandardFonts.HELVETICA), 12)
                .MoveText(80, 30).ShowText("Added object").EndText();
        });
        Assert.That(PageOrganizationPlan.IsEmptyPage(source.GetPage(800)), Is.True);
        Assert.That(source.GetNumberOfPages(), Is.EqualTo(800));
        using var result = new PdfDocument(new PdfReader(new MemoryStream(preview)));
        Assert.That(result.GetNumberOfPages(), Is.EqualTo(1));
        Assert.That(result.GetPage(1).GetRotation(), Is.EqualTo(rotation));
        Assert.That(PdfTextExtractor.GetTextFromPage(result.GetPage(1)), Does.Contain("Added object"));
        using var image = PdfRegressionTests.Render(preview);
        int dark = 0;
        for (int y = 0; y < image.Height; y += 5)
            for (int x = 0; x < image.Width; x += 5)
                if (image.GetPixel(x, y).R < 40) dark++;
        Assert.That(dark, Is.GreaterThan(40), "Objects on a previously blank page must be visible in the thumbnail.");
    }

    [TestCase(new[] { 1, 2 }, 0, new[] { 2, 3, 1 })]
    [TestCase(new[] { 0, 2 }, 3, new[] { 2, 1, 3 })]
    [TestCase(new[] { 0, 1 }, 1, new[] { 1, 2, 3 })]
    public void GroupMovePreservesRelativeOrder(int[] selected, int gap, int[] expected)
    {
        var plan = PageOrganizationPlan.Open(Document("A"));
        plan.Move(selected, gap);
        Assert.That(plan.Pages.Select(p => p.ExistingPageNumber), Is.EqualTo(expected));
    }

    [TestCase(new[] { 0 }, 4, false, new[] { 2, 3, 1, 4, 5 })]
    [TestCase(new[] { 0 }, 5, true, new[] { 2, 3, 4, 5, 1 })]
    [TestCase(new[] { 3, 4 }, 1, false, new[] { 4, 5, 1, 2, 3 })]
    [TestCase(new[] { 1, 3 }, 5, false, new[] { 1, 3, 2, 4, 5 })]
    [TestCase(new[] { 1, 2 }, 2, false, new[] { 1, 2, 3, 4, 5 })]
    [TestCase(new[] { 1, 2 }, 3, true, new[] { 1, 2, 3, 4, 5 })]
    [TestCase(new[] { 3, 0 }, 3, true, new[] { 2, 3, 1, 4, 5 })]
    public void MoveToPageUsesTheSpecifiedPageAsAnInsertionAnchor(int[] selected, int target, bool after, int[] expected)
    {
        var plan = PageOrganizationPlan.Open(Document("A", 5));
        plan.MoveToPage(selected, target, after);
        Assert.That(plan.Pages.Select(p => p.ExistingPageNumber), Is.EqualTo(expected));
    }

    [Test]
    public void MoveToPage800KeepsTheSelectionOrderAndDeletionMarks()
    {
        var bytes = PdfRegressionTests.CreatePdf(document => {
            for (int i = 0; i < 800; i++) document.AddNewPage();
        });
        var plan = PageOrganizationPlan.Open(bytes, removedPages: new[] { 2 });
        plan.MoveToPage(new[] { 1, 0 }, 800, true);
        Assert.That(plan.Pages.Skip(798).Select(p => p.ExistingPageNumber), Is.EqualTo(new[] { 1, 2 }));
        Assert.That(plan.Pages[799].MarkedForDeletion, Is.True);
        var project = plan.RemapProject(new ProjectData {
            PagesToRemove = new HashSet<int> { 2 },
            RedactionBlocks = new List<RedactionBlock> { new RedactionBlock { PageNumber = 1 } }
        });
        Assert.That(project.RedactionBlocks[0].PageNumber, Is.EqualTo(799));
        Assert.That(project.PagesToRemove, Is.EquivalentTo(new[] { 800 }));
    }

    [TestCase(0)]
    [TestCase(-1)]
    [TestCase(6)]
    public void InvalidMoveDestinationDoesNotChangeTheDocument(int destination)
    {
        var plan = PageOrganizationPlan.Open(Document("A", 5));
        Assert.That(() => plan.MoveToPage(new[] { 0 }, destination, false), Throws.TypeOf<System.ArgumentOutOfRangeException>());
        Assert.That(plan.IsChanged, Is.False);
    }

    [Test]
    public void InsertAndMovePreserveContentDimensionsRotationAndSourceBytes()
    {
        var source = Document("A");
        var original = source.ToArray();
        var plan = PageOrganizationPlan.Open(source);
        plan.AddPdf(Document("B"), "", new[] { 3, 1 }, 1);
        plan.AddBlank(0, 300, 450);
        plan.Move(new[] { 5 }, 1);
        using var pdf = new PdfDocument(new PdfReader(new MemoryStream(plan.Compose())));
        var texts = Enumerable.Range(1, pdf.GetNumberOfPages()).Select(p => PdfTextExtractor.GetTextFromPage(pdf.GetPage(p)).Trim());
        Assert.That(texts, Is.EqualTo(new[] { "", "A3", "A1", "B3", "B1", "A2" }));
        Assert.That(pdf.GetPage(1).GetPageSize().GetWidth(), Is.EqualTo(300));
        Assert.That(pdf.GetPage(2).GetRotation(), Is.EqualTo(90));
        Assert.That(pdf.GetPage(6).GetPageSize().GetWidth(), Is.EqualTo(PageSize.A5.GetWidth()));
        Assert.That(source, Is.EqualTo(original));
    }

    [Test]
    public void AllProjectObjectsRotationsDeletionMarksAndAltEditsFollowTheirPage()
    {
        var plan = PageOrganizationPlan.Open(Document("A"));
        plan.Move(new[] { 2 }, 0); // old 3, 1, 2
        plan.AddBlank(1, 595, 842); // old 3, blank, old 1, old 2
        var original = new ProjectData {
            RedactionBlocks = new List<RedactionBlock> { new RedactionBlock { PageNumber = 1 } },
            TextAnnotations = new List<TextAnnotation> { new TextAnnotation { PageNumber = 2 } },
            RasterObjects = new List<RasterObject> { new RasterObject { PageNumber = 3 } },
            ArrowObjects = new List<ArrowObject> { new ArrowObject { PageNumber = 1 } },
            VectorShapes = new List<VectorShapeObject> { new VectorShapeObject { PageNumber = 2 } },
            CommentAnnotations = new List<CommentAnnotation> { new CommentAnnotation { PageNumber = 3 } },
            PagesToRemove = new HashSet<int> { 2 }, PageRotationOffsets = new Dictionary<int, int> { [3] = 90 },
            AltTextEdits = new Dictionary<string, string> { ["1:12.25:40"] = "ALT \u017b\u00f3\u0142\u0107" },
            CurrentPage = 1, PagesListTopPage = 2, ThumbnailsTopPage = 3
        };
        var remapped = plan.RemapProject(original);
        Assert.Multiple(() => {
            Assert.That(remapped.RedactionBlocks[0].PageNumber, Is.EqualTo(3));
            Assert.That(remapped.TextAnnotations[0].PageNumber, Is.EqualTo(4));
            Assert.That(remapped.RasterObjects[0].PageNumber, Is.EqualTo(1));
            Assert.That(remapped.ArrowObjects[0].PageNumber, Is.EqualTo(3));
            Assert.That(remapped.VectorShapes[0].PageNumber, Is.EqualTo(4));
            Assert.That(remapped.CommentAnnotations[0].PageNumber, Is.EqualTo(1));
            Assert.That(remapped.PagesToRemove, Is.EquivalentTo(new[] { 4 }));
            Assert.That(remapped.PageRotationOffsets[1], Is.EqualTo(90));
            Assert.That(remapped.AltTextEdits["3:12.25:40"], Is.EqualTo("ALT \u017b\u00f3\u0142\u0107"));
            Assert.That(remapped.CurrentPage, Is.EqualTo(3));
            Assert.That(remapped.PagesListTopPage, Is.EqualTo(4));
            Assert.That(remapped.ThumbnailsTopPage, Is.EqualTo(1));
            Assert.That(original.RedactionBlocks[0].PageNumber, Is.EqualTo(1));
        });
    }

    [Test]
    public void EmbeddedDocumentSurvivesProjectRoundTripWithoutExternalSources()
    {
        var plan = PageOrganizationPlan.Open(Document("A", 1));
        plan.AddPdf(Document("B", 1), "", new[] { 1 }, 1);
        var data = new ProjectData { EmbeddedDocument = new EmbeddedPdfDocument {
            Id = "revision", OriginalPath = "missing.pdf", PdfBytes = plan.Compose()
        }, DocumentRevisionId = "revision" };
        var restored = JsonConvert.DeserializeObject<ProjectData>(JsonConvert.SerializeObject(data));
        using var pdf = new PdfDocument(new PdfReader(new MemoryStream(restored.EmbeddedDocument.PdfBytes)));
        Assert.That(pdf.GetNumberOfPages(), Is.EqualTo(2));
        Assert.That(PdfTextExtractor.GetTextFromPage(pdf.GetPage(2)), Does.Contain("B1"));
    }

    [Test]
    public void InvalidOperationsDoNotChangeThePlan()
    {
        var plan = PageOrganizationPlan.Open(Document("A"));
        Assert.That(() => plan.AddBlank(0, 0, 842), Throws.ArgumentException);
        Assert.That(() => plan.AddPdf(Document("B"), "", new[] { 4 }, 0), Throws.ArgumentException);
        Assert.That(() => plan.Move(new[] { -1 }, 0), Throws.ArgumentException);
        Assert.That(plan.Pages.Count, Is.EqualTo(3));
        Assert.That(plan.IsChanged, Is.False);
    }

    [Test]
    public void ImportKeepsFormValuesSeparateWhenFieldNamesMatch()
    {
        byte[] WithField(string value) => PdfRegressionTests.CreatePdf(document => {
            var page = document.AddNewPage();
            var field = new iText.Forms.Fields.TextFormFieldBuilder(document, "shared")
                .SetWidgetRectangle(new Rectangle(30, 100, 150, 30)).CreateText().SetValue(value);
            iText.Forms.PdfAcroForm.GetAcroForm(document, true).AddField(field, page);
        });
        var plan = PageOrganizationPlan.Open(WithField("FIRST"));
        plan.AddPdf(WithField("SECOND"), "", new[] { 1 }, 1);
        using var pdf = new PdfDocument(new PdfReader(new MemoryStream(plan.Compose())));
        var values = iText.Forms.PdfAcroForm.GetAcroForm(pdf, false).GetAllFormFields().Values.Select(f => f.GetValueAsString());
        Assert.That(values, Is.EquivalentTo(new[] { "FIRST", "SECOND" }));
    }

    [Test]
    public void ExistingInternalLinksFollowMovedPages()
    {
        var source = PdfRegressionTests.CreatePdf(document => {
            var first = document.AddNewPage();
            document.AddNewPage();
            var target = document.AddNewPage();
            first.AddAnnotation(new iText.Kernel.Pdf.Annot.PdfLinkAnnotation(new Rectangle(30, 100, 60, 20))
                .SetAction(iText.Kernel.Pdf.Action.PdfAction.CreateGoTo(iText.Kernel.Pdf.Navigation.PdfExplicitDestination.CreateFit(target))));
        });
        var plan = PageOrganizationPlan.Open(source);
        plan.Move(new[] { 2 }, 0);
        using var result = new PdfDocument(new PdfReader(new MemoryStream(plan.Compose())));
        var destination = result.GetPage(2).GetAnnotations()[0].GetPdfObject().GetAsDictionary(PdfName.A).GetAsArray(PdfName.D).GetAsDictionary(0);
        Assert.That(destination.GetIndirectReference(), Is.EqualTo(result.GetPage(1).GetPdfObject().GetIndirectReference()));
    }

    [Test]
    public void AddedBlankPageDoesNotRequireAnOcrTextLayer()
    {
        var plan = PageOrganizationPlan.Open(Document("A", 1));
        plan.AddBlank(1, 595, 842);
        using var document = new PdfDocument(new PdfReader(new MemoryStream(plan.Compose())));
        Assert.That(PageOrganizationPlan.IsEmptyPage(document.GetPage(1)), Is.False);
        Assert.That(PageOrganizationPlan.IsEmptyPage(document.GetPage(2)), Is.True);
    }
}
