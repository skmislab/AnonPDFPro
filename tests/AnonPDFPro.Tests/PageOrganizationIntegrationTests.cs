using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using AnonPDF;
using Newtonsoft.Json;
using NUnit.Framework;

namespace AnonPDFPro.Tests;

[TestFixture, NonParallelizable, Apartment(ApartmentState.STA)]
[Explicit("Run the desktop-controller integration test only in an attended local test session.")]
public class PageOrganizationIntegrationTests
{
    private static object Call(PDFForm form, string method, params object[] arguments) => typeof(PDFForm)
        .GetMethod(method, BindingFlags.NonPublic | BindingFlags.Instance).Invoke(form, arguments);
    private static void Set(PDFForm form, string field, object value) => typeof(PDFForm)
        .GetField(field, BindingFlags.NonPublic | BindingFlags.Instance).SetValue(form, value);
    private static T Get<T>(PDFForm form, string field) => (T)typeof(PDFForm)
        .GetField(field, BindingFlags.NonPublic | BindingFlags.Instance).GetValue(form);
    private static PDFForm Form()
    {
        var form = new PDFForm();
        Set(form, "suppressDocumentBackgroundProcessing", true);
        Set(form, "headlessBatchExecution", true);
        Set(form, "isApplicationClosing", true);
        Call(form, "PDFForm_Load", form, EventArgs.Empty);
        return form;
    }

    [Test]
    public void DeletionMarksUseTheExistingPdfAndSupportUndoRedo()
    {
        string original = Path.Combine(Path.GetTempPath(), "AnonPDFPro-removal-" + Guid.NewGuid().ToString("N") + ".pdf");
        try
        {
            byte[] bytes = PageOrganizationTests.Document("A");
            File.WriteAllBytes(original, bytes);
            using var form = Form();
            Set(form, "inputPdfPath", original);
            Call(form, "LoadPdf", new object[] { null });
            var document = Get<PDFiumSharp.PdfDocument>(form, "pdf");
            var plan = PageOrganizationPlan.Open(bytes);
            Assert.That(plan.RemoveOrRestore(new[] { 0 }), Is.True);
            form.ApplyPageOrganization(plan, null);
            Assert.That(Get<PDFiumSharp.PdfDocument>(form, "pdf"), Is.SameAs(document));
            Assert.That(Get<string>(form, "inputPdfPath"), Is.EqualTo(original));
            Assert.That(Get<HashSet<int>>(form, "pagesToRemove"), Is.EquivalentTo(new[] { 1 }));
            Assert.That(Call(form, "UndoLastAction"), Is.True);
            Assert.That(Get<HashSet<int>>(form, "pagesToRemove"), Is.Empty);
            Assert.That(Call(form, "RedoLastAction"), Is.True);
            Assert.That(Get<HashSet<int>>(form, "pagesToRemove"), Is.EquivalentTo(new[] { 1 }));
            Assert.That(File.ReadAllBytes(original), Is.EqualTo(bytes));
        }
        finally { try { File.Delete(original); } catch (IOException ex) { TestContext.Error.WriteLine("Cleanup: " + ex.Message); } }
    }

    [Test]
    public void ApplicationCanReorganizeUndoRedoAndReopenEmbeddedProject()
    {
        string directory = Path.Combine(Path.GetTempPath(), "AnonPDFPro-organization-" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(directory);
        try
        {
            string original = Path.Combine(directory, "original.pdf");
            var bytes = PageOrganizationTests.Document("A");
            File.WriteAllBytes(original, bytes);
            using var form = Form();
            Set(form, "inputPdfPath", original);
            Call(form, "LoadPdf", new object[] { null });
            Assert.That(Get<string>(form, "headlessBatchError"), Is.Null);
            Assert.That(Get<int>(form, "numPages"), Is.EqualTo(3));
            var blocks = Get<List<RedactionBlock>>(form, "redactionBlocks");
            blocks.Add(new RedactionBlock { PageNumber = 1 });
            var plan = PageOrganizationPlan.Open(bytes);
            plan.Move(new[] { 2 }, 0);
            plan.AddBlank(1, 300, 450);
            form.ApplyPageOrganization(plan, plan.Compose());
            Assert.That(Get<int>(form, "numPages"), Is.EqualTo(4));
            Assert.That(Get<List<RedactionBlock>>(form, "redactionBlocks")[0].PageNumber, Is.EqualTo(3));
            var signature = (string)Call(form, "BuildProjectStateSignature");
            Assert.That(JsonConvert.DeserializeObject<ProjectData>(signature).EmbeddedDocument, Is.Null,
                "Undo entries must not duplicate the full PDF bytes");
            Assert.That(Call(form, "UndoLastAction"), Is.True);
            Assert.That(Get<int>(form, "numPages"), Is.EqualTo(3));
            Assert.That(Get<List<RedactionBlock>>(form, "redactionBlocks")[0].PageNumber, Is.EqualTo(1));
            Assert.That(Call(form, "RedoLastAction"), Is.True);
            Assert.That(Get<int>(form, "numPages"), Is.EqualTo(4));
            var project = (ProjectData)Call(form, "BuildCurrentProjectDataSnapshot", true);
            Assert.That(project.FilePath, Is.EqualTo(original));
            Assert.That(project.EmbeddedDocument.PdfBytes, Is.Not.Empty);
            string saved = Path.Combine(directory, "organized.app");
            File.WriteAllText(saved, JsonConvert.SerializeObject(project));
            Assert.That(File.ReadAllBytes(original), Is.EqualTo(bytes));
            Get<PDFiumSharp.PdfDocument>(form, "pdf").Close();
            Set(form, "pdf", null);
            File.Delete(original);
            using var reopened = Form();
            Call(reopened, "LoadRedactionBlocks", saved, true);
            Assert.That(Get<string>(reopened, "headlessBatchError"), Is.Null);
            Assert.That(Get<int>(reopened, "numPages"), Is.EqualTo(4));
            Assert.That(Get<List<RedactionBlock>>(reopened, "redactionBlocks")[0].PageNumber, Is.EqualTo(3));
            Get<PDFiumSharp.PdfDocument>(reopened, "pdf").Close();
        }
        catch (Exception ex) { TestContext.Error.WriteLine(ex.ToString()); throw; }
        finally { try { Directory.Delete(directory, true); } catch (IOException ex) { TestContext.Error.WriteLine("Cleanup: " + ex.Message); } }
    }
}
