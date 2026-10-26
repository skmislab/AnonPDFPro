using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using AnonPDF.Properties;

namespace AnonPDF
{
    public partial class PDFForm
    {
        private ToolStripMenuItem organizePagesMenuItem;
        private EmbeddedPdfDocument organizedDocument;
        private bool organizationReload;
        private readonly Dictionary<string, OrganizationRevision> organizationRevisions = new Dictionary<string, OrganizationRevision>();
        private readonly List<string> organizationTemporaryFiles = new List<string>();
        private string DocumentDisplayPath => organizedDocument?.OriginalPath ?? inputPdfPath;

        private sealed class OrganizationRevision
        {
            public string Path;
            public string Password;
            public EmbeddedPdfDocument Info;
        }

        private void InitializePageOrganizationMenu()
        {
            organizePagesMenuItem = new ToolStripMenuItem(PageOrganizationText.Get("Pages_Title")) {
                Name = "organizePagesMenuItem", Enabled = false
            };
            organizePagesMenuItem.Click += (_, __) => OrganizePages();
            menuFileItem.DropDownItems.Insert(menuFileItem.DropDownItems.IndexOf(documentSettingsMenuItem), organizePagesMenuItem);
            menuFileItem.DropDownOpening += (_, __) => organizePagesMenuItem.Enabled = pdf != null && numPages > 0;
            Disposed += (_, __) => {
                try { pdf?.Close(); } catch (ObjectDisposedException) { }
                foreach (string file in organizationTemporaryFiles)
                    try { File.Delete(file); } catch (IOException) { } catch (UnauthorizedAccessException) { }
            };
        }

        private void ResetOrganizationForNewDocument()
        {
            if (organizationReload) return;
            organizedDocument = null;
            organizationRevisions.Clear();
        }

        private string WriteOrganizationWorkingFile(byte[] bytes)
        {
            // Never derive writable paths from a project-supplied file name or revision ID.
            string directory = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "AnonPDFPro", "organized-pages");
            Directory.CreateDirectory(directory);
            string path = System.IO.Path.Combine(directory, Guid.NewGuid().ToString("N") + ".pdf");
            File.WriteAllBytes(path, bytes);
            organizationTemporaryFiles.Add(path);
            return path;
        }

        private void RememberOrganizationRevision()
        {
            string id = organizedDocument?.Id ?? string.Empty;
            if (!organizationRevisions.ContainsKey(id))
                organizationRevisions.Add(id, new OrganizationRevision {
                    Path = inputPdfPath, Password = userPassword,
                    Info = organizedDocument == null ? null : new EmbeddedPdfDocument { Id = id, OriginalPath = organizedDocument.OriginalPath }
                });
        }

        private void OrganizePages()
        {
            if (pdf == null || numPages < 1) return;
            iText.Kernel.Pdf.PdfDocument previewSource = null;
            try
            {
                var plan = PageOrganizationPlan.Open(File.ReadAllBytes(inputPdfPath), userPassword, Settings.Default.IgnorePdfRestrictions, pagesToRemove);
                var source = plan.SourceFor(plan.Pages[0]);
                var objectsByPage = GetOrderedGraphicObjectsForExport(visibleObjects: true).ToLookup(OrganizationObjectPage);
                var commentPages = new HashSet<int>(commentAnnotations.Where(c => c != null && IsLayerVisible(c.LayerId)).Select(c => c.PageNumber));
                byte[] CreatePageWithObjects(int pageNumber)
                {
                    if (!objectsByPage.Contains(pageNumber) && !commentPages.Contains(pageNumber)) return null;
                    if (previewSource == null)
                    {
                        var properties = new iText.Kernel.Pdf.ReaderProperties();
                        if (!string.IsNullOrEmpty(source.Password)) properties.SetPassword(System.Text.Encoding.UTF8.GetBytes(source.Password));
                        previewSource = new iText.Kernel.Pdf.PdfDocument(new iText.Kernel.Pdf.PdfReader(new MemoryStream(source.Bytes, false), properties)
                            .SetUnethicalReading(Settings.Default.IgnorePdfRestrictions));
                    }
                    return CreateOrganizationPageWithObjects(previewSource, pageNumber, objectsByPage[pageNumber]);
                }
                using (var dialog = new PageOrganizationForm(plan, CreateDialogTheme(), () => PromptForPassword(), currentPage, pageRotationOffsets,
                    createPageWithObjects: CreatePageWithObjects, drawPageOverlays: (graphics, pageNumber, bounds) => {
                        DrawThumbnailRedactionOverlays(graphics, pageNumber, bounds);
                    }))
                {
                    if (dialog.ShowDialog(this) != DialogResult.OK || !plan.IsChanged) return;
                    ApplyPageOrganization(plan, dialog.ComposedPdf, dialog.SelectedPageNumber);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, PageOrganizationText.Format("Pages_Error", ex.Message), Resources.Title_Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally { previewSource?.Close(); }
        }

        private static int OrganizationObjectPage(object drawable)
        {
            switch (drawable)
            {
                case TextAnnotation text: return text.PageNumber;
                case RasterObject raster: return raster.PageNumber;
                case ArrowObject arrow: return arrow.PageNumber;
                case VectorShapeObject shape: return shape.PageNumber;
                default: return 0;
            }
        }

        private byte[] CreateOrganizationPageWithObjects(iText.Kernel.Pdf.PdfDocument source, int pageNumber, IEnumerable<object> objects)
        {
            return PageOrganizationPlan.CreatePagePreview(source, pageNumber, GetEffectiveRotationDegrees(pageNumber), (target, page) => {
                RenderCommentAnnotationsToPdf(target, null, null, targetPage: page, sourcePageNumber: pageNumber);
                foreach (object drawable in objects)
                {
                    if (drawable is TextAnnotation text) RenderTextAnnotationToPdf(target, null, text, page);
                    else if (drawable is RasterObject raster) RenderRasterObjectToPdf(target, null, raster, page);
                    else if (drawable is ArrowObject arrow) RenderArrowObjectToPdf(target, null, arrow, page);
                    else if (drawable is VectorShapeObject shape) RenderVectorShapesToPdf(target, null, new[] { shape }, page);
                }
            });
        }

        internal void ApplyPageOrganization(PageOrganizationPlan plan, byte[] composedPdf, int selectedPageNumber = 0)
        {
            if (pdf == null || !plan.IsChanged) return;
            var before = BuildCurrentProjectDataSnapshot(includeViewState: true);
            before.EmbeddedDocument = null;
            var after = plan.RemapProject(before);
            if (selectedPageNumber >= 1 && selectedPageNumber <= plan.Pages.Count) after.CurrentPage = selectedPageNumber;
            if (!plan.IsLayoutChanged)
            {
                // Deletion marks are project changes; do not rewrite or reload the source PDF.
                BeginUndoCapture("OrganizePages");
                try
                {
                    ApplyProjectSnapshotToCurrentDocument(after);
                    CommitUndoCapture();
                }
                catch
                {
                    CancelUndoCapture();
                    ApplyProjectSnapshotToCurrentDocument(before);
                    throw;
                }
                return;
            }
            RememberOrganizationRevision();
            string previousId = organizedDocument?.Id ?? string.Empty;
            var data = new EmbeddedPdfDocument {
                Id = Guid.NewGuid().ToString("N"), OriginalPath = DocumentDisplayPath, PdfBytes = composedPdf
            };
            var revision = new OrganizationRevision {
                Path = WriteOrganizationWorkingFile(data.PdfBytes), Password = "",
                Info = new EmbeddedPdfDocument { Id = data.Id, OriginalPath = data.OriginalPath }
            };
            organizationRevisions.Add(data.Id, revision);
            after.DocumentRevisionId = data.Id;
            BeginUndoCapture("OrganizePages");
            try
            {
                LoadOrganizationRevision(revision, after, data);
                CommitUndoCapture();
            }
            catch
            {
                CancelUndoCapture();
                LoadOrganizationRevision(organizationRevisions[previousId], before);
                throw;
            }
        }

        private void LoadOrganizationRevision(OrganizationRevision revision, ProjectData snapshot, EmbeddedPdfDocument data = null)
        {
            var undo = undoHistory.Reverse().ToList();
            var redo = redoHistory.Reverse().ToList();
            var pending = pendingUndoCapture;
            string savedSignature = savedProjectStateSignature;
            string projectPath = inputProjectPath;
            string savedProject = lastSavedProjectName;
            string output = lastRedactedPdfOutputPath;
            var oldPdf = pdf;
            CancelThumbnailGeneration();
            CancelPreviewRender();
            organizationReload = true;
            try
            {
                organizedDocument = revision.Info == null ? null : data ?? new EmbeddedPdfDocument {
                    Id = revision.Info.Id, OriginalPath = revision.Info.OriginalPath, PdfBytes = File.ReadAllBytes(revision.Path)
                };
                inputPdfPath = revision.Path;
                LoadPdf(revision.Password);
                if (pdf == null || ReferenceEquals(pdf, oldPdf)) throw new IOException(Resources.Err_InvalidPdfFile);
                oldPdf?.Close();
                ApplyProjectSnapshotToCurrentDocument(snapshot);
            }
            finally
            {
                organizationReload = false;
                undoHistory.Clear();
                foreach (var entry in undo) undoHistory.Push(entry);
                redoHistory.Clear();
                foreach (var entry in redo) redoHistory.Push(entry);
                pendingUndoCapture = pending;
                savedProjectStateSignature = savedSignature;
                inputProjectPath = projectPath;
                lastSavedProjectName = savedProject;
                lastRedactedPdfOutputPath = output;
                UpdateWindowTitle();
            }
        }

        private bool RestoreOrganizationForUndo(ProjectData snapshot)
        {
            string requested = snapshot.DocumentRevisionId ?? string.Empty;
            if (requested == (organizedDocument?.Id ?? string.Empty)) return true;
            if (!organizationRevisions.TryGetValue(requested, out var revision)) return false;
            LoadOrganizationRevision(revision, snapshot);
            return true;
        }

        private string ResolveOrganizedProjectPdf(ProjectData project, string projectPath)
        {
            if (project.EmbeddedDocument == null) return ResolveProjectPdfPath(project.FilePath ?? "", projectPath);
            var data = project.EmbeddedDocument;
            if (data.PdfBytes == null || data.PdfBytes.Length == 0 || string.IsNullOrWhiteSpace(data.Id))
                throw new InvalidDataException(Resources.Err_InvalidProjectFormat);
            // Validate before replacing the current document/session state.
            using (var reader = new iText.Kernel.Pdf.PdfReader(new MemoryStream(data.PdfBytes)))
            using (var document = new iText.Kernel.Pdf.PdfDocument(reader))
                if (document.GetNumberOfPages() < 1) throw new InvalidDataException(Resources.Err_InvalidPdfFile);
            string path = WriteOrganizationWorkingFile(data.PdfBytes);
            organizedDocument = data;
            organizationRevisions.Clear();
            project.DocumentRevisionId = data.Id;
            organizationRevisions.Add(data.Id, new OrganizationRevision {
                Path = path, Password = "", Info = new EmbeddedPdfDocument { Id = data.Id, OriginalPath = data.OriginalPath }
            });
            return path;
        }

        private void RestoreProjectAltEdits(ProjectData project)
        {
            foreach (var key in PdfTextSearcher._pendingAltTextEdits.Keys.Where(k => k.pdfPath == inputPdfPath).ToList())
                PdfTextSearcher._pendingAltTextEdits.TryRemove(key, out _);
            foreach (var entry in project.AltTextEdits ?? new Dictionary<string, string>())
                PdfTextSearcher._pendingAltTextEdits[(inputPdfPath, entry.Key)] = entry.Value;
        }
    }
}
