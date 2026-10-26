using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using iText.Forms;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using Newtonsoft.Json;

namespace AnonPDF
{
    public sealed class EmbeddedPdfDocument
    {
        public string Id { get; set; }
        public string OriginalPath { get; set; }
        public byte[] PdfBytes { get; set; }
    }

    internal sealed class OrganizationPage
    {
        public string Id { get; } = Guid.NewGuid().ToString("N");
        public int ExistingPageNumber { get; set; }
        public string SourceId { get; set; }
        public int SourcePageNumber { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }
        public int Rotation { get; set; }
        public string SourceName { get; set; }
        public bool MarkedForDeletion { get; set; }
        public bool IsBlank => SourceId == null;
    }

    internal sealed class OrganizationSource
    {
        public byte[] Bytes { get; set; }
        public string Password { get; set; }
        public bool IsTagged { get; set; }
        public bool HasSignatures { get; set; }
    }

    internal sealed class PageOrganizationPlan
    {
        private const string CurrentSource = "current";
        private readonly List<string> originalIds = new List<string>();
        private readonly Dictionary<string, OrganizationSource> sources = new Dictionary<string, OrganizationSource>();
        private readonly HashSet<int> initialRemovedPages = new HashSet<int>();
        private readonly Dictionary<int, bool> removalChanges = new Dictionary<int, bool>();
        private bool ignoreRestrictions;
        public List<OrganizationPage> Pages { get; } = new List<OrganizationPage>();
        public bool IsLayoutChanged => !originalIds.SequenceEqual(Pages.Select(p => p.Id));
        public bool IsChanged => IsLayoutChanged || removalChanges.Count > 0;
        public bool ContainsSignatures => sources.Values.Any(s => s.HasSignatures);
        public OrganizationSource SourceFor(OrganizationPage page) => page.IsBlank ? null : sources[page.SourceId];

        internal static bool IsEmptyPage(PdfPage page)
        {
            return page.GetAnnotations().Count == 0 && page.GetContentBytes().All(b => b == 0 || b == 9 || b == 10 || b == 12 || b == 13 || b == 32);
        }

        internal static byte[] CreatePagePreview(PdfDocument source, int pageNumber, int rotation, Action<PdfDocument, PdfPage> drawObjects)
        {
            using (var output = new MemoryStream())
            {
                using (var target = new PdfDocument(new PdfWriter(output)))
                {
                    source.CopyPagesTo(pageNumber, pageNumber, target);
                    var page = target.GetPage(1);
                    page.SetRotation(rotation);
                    drawObjects(target, page);
                }
                return output.ToArray();
            }
        }

        public static PageOrganizationPlan Open(byte[] bytes, string password = "", bool ignoreRestrictions = false,
            IEnumerable<int> removedPages = null)
        {
            var plan = new PageOrganizationPlan { ignoreRestrictions = ignoreRestrictions };
            var pages = plan.ReadSource(bytes, password, CurrentSource, null);
            plan.initialRemovedPages.UnionWith(removedPages ?? Enumerable.Empty<int>());
            foreach (var page in pages)
            {
                page.ExistingPageNumber = page.SourcePageNumber;
                page.MarkedForDeletion = plan.initialRemovedPages.Contains(page.ExistingPageNumber);
            }
            plan.Pages.AddRange(pages);
            plan.originalIds.AddRange(pages.Select(p => p.Id));
            return plan;
        }

        private PdfReader Reader(OrganizationSource source)
        {
            var properties = new ReaderProperties();
            if (!string.IsNullOrEmpty(source.Password)) properties.SetPassword(System.Text.Encoding.UTF8.GetBytes(source.Password));
            return new PdfReader(new MemoryStream(source.Bytes, false), properties).SetUnethicalReading(ignoreRestrictions);
        }

        private List<OrganizationPage> ReadSource(byte[] bytes, string password, string id, string name)
        {
            if (bytes == null || bytes.Length == 0) throw new ArgumentException("Empty PDF source.");
            var source = new OrganizationSource { Bytes = bytes, Password = password };
            using (var document = new PdfDocument(Reader(source)))
            {
                source.IsTagged = document.IsTagged();
                source.HasSignatures = new iText.Signatures.SignatureUtil(document).GetSignatureNames().Count > 0;
                var pages = Enumerable.Range(1, document.GetNumberOfPages()).Select(number => {
                    var page = document.GetPage(number);
                    var size = page.GetPageSize();
                    return new OrganizationPage {
                        SourceId = id, SourcePageNumber = number, SourceName = name,
                        Width = size.GetWidth(), Height = size.GetHeight(), Rotation = page.GetRotation()
                    };
                }).ToList();
                sources.Add(id, source);
                return pages;
            }
        }

        public void AddPdf(byte[] bytes, string password, IEnumerable<int> pageNumbers, int index, string name = null)
        {
            ValidateGap(index);
            var id = Guid.NewGuid().ToString("N");
            var candidates = ReadSource(bytes, password, id, name);
            var numbers = pageNumbers.ToList();
            if (numbers.Count == 0 || numbers.Any(n => n < 1 || n > candidates.Count) || numbers.Distinct().Count() != numbers.Count)
            {
                sources.Remove(id);
                throw new ArgumentException("Invalid PDF page selection.");
            }
            Pages.InsertRange(index, numbers.Select(n => candidates[n - 1]));
        }

        public void AddBlank(int index, float width, float height)
        {
            ValidateGap(index);
            if (float.IsNaN(width) || float.IsNaN(height) || float.IsInfinity(width) || float.IsInfinity(height) ||
                width <= 0 || height <= 0 || width > 14400 || height > 14400)
                throw new ArgumentException("Invalid page dimensions.");
            Pages.Insert(index, new OrganizationPage { Width = width, Height = height });
        }

        private void ValidateGap(int index)
        {
            if (index < 0 || index > Pages.Count) throw new ArgumentException("Invalid page insertion point.");
        }

        public bool RemoveOrRestore(IEnumerable<int> indices)
        {
            var selected = indices.Distinct().ToArray();
            if (selected.Length == 0 || selected.Any(i => i < 0 || i >= Pages.Count))
                throw new ArgumentException("Invalid page selection.");
            var selectedPages = new HashSet<OrganizationPage>(selected.Select(i => Pages[i]));
            bool restore = selectedPages.All(p => p.ExistingPageNumber > 0 && p.MarkedForDeletion);
            bool hasRemainingPage = Pages.Any(p => selectedPages.Contains(p)
                ? p.ExistingPageNumber > 0 && restore : !p.MarkedForDeletion);
            if (!hasRemainingPage) return false;
            foreach (var page in selectedPages.Where(p => p.ExistingPageNumber > 0))
            {
                page.MarkedForDeletion = !restore;
                if (page.MarkedForDeletion == initialRemovedPages.Contains(page.ExistingPageNumber)) removalChanges.Remove(page.ExistingPageNumber);
                else removalChanges[page.ExistingPageNumber] = page.MarkedForDeletion;
            }
            Pages.RemoveAll(p => p.ExistingPageNumber == 0 && selectedPages.Contains(p));
            var usedSources = new HashSet<string>(Pages.Select(p => p.SourceId));
            foreach (string key in sources.Keys.Where(key => key != CurrentSource && !usedSources.Contains(key)).ToArray()) sources.Remove(key);
            return true;
        }

        public void Move(IEnumerable<int> indices, int gap)
        {
            ValidateGap(gap);
            var selected = indices.Distinct().OrderBy(n => n).ToList();
            if (selected.Count == 0 || selected.Any(n => n < 0 || n >= Pages.Count)) throw new ArgumentException("Invalid page selection.");
            var moving = selected.Select(n => Pages[n]).ToList();
            var destination = gap - selected.Count(n => n < gap);
            foreach (int index in selected.OrderByDescending(n => n)) Pages.RemoveAt(index);
            Pages.InsertRange(destination, moving);
        }

        public Dictionary<int, int> ExistingPageMap()
        {
            var existing = Pages.Where(p => p.ExistingPageNumber > 0).ToList();
            if (existing.Count != originalIds.Count || existing.Select(p => p.Id).Distinct().Count() != originalIds.Count ||
                !new HashSet<string>(existing.Select(p => p.Id)).SetEquals(originalIds))
                throw new InvalidOperationException("Original pages must be retained exactly once.");
            return Pages.Select((p, i) => new { Page = p, Number = i + 1 }).Where(p => p.Page.ExistingPageNumber > 0)
                .ToDictionary(p => p.Page.ExistingPageNumber, p => p.Number);
        }

        public byte[] Compose()
        {
            ExistingPageMap();
            using (var output = new MemoryStream())
            using (var document = new PdfDocument(Reader(sources[CurrentSource]), new PdfWriter(output)))
            {
                var originals = Enumerable.Range(1, document.GetNumberOfPages()).ToDictionary(n => n, document.GetPage);
                var imports = new Dictionary<string, PdfDocument>();
                var copiers = new Dictionary<string, PdfPageFormCopier>();
                try
                {
                    if (sources.Values.Any(s => s.IsTagged) && !document.IsTagged()) document.SetTagged();
                    var ordered = new List<PdfPage>();
                    foreach (var page in Pages)
                    {
                        if (page.ExistingPageNumber > 0) ordered.Add(originals[page.ExistingPageNumber]);
                        else if (page.IsBlank) ordered.Add(document.AddNewPage(new PageSize(page.Width, page.Height)));
                        else
                        {
                            if (!imports.TryGetValue(page.SourceId, out var source))
                            {
                                source = new PdfDocument(Reader(sources[page.SourceId]));
                                imports.Add(page.SourceId, source);
                                copiers.Add(page.SourceId, new PdfPageFormCopier());
                                var form = PdfAcroForm.GetAcroForm(source, false);
                                if (form != null)
                                    foreach (var key in form.GetRootFormFields().Keys.ToList()) form.RenameField(key, "import_" + page.SourceId + "_" + key);
                            }
                            ordered.Add(source.CopyPagesTo(page.SourcePageNumber, page.SourcePageNumber, document, copiers[page.SourceId])[0]);
                        }
                    }
                    for (int i = 0; i < ordered.Count; i++)
                        if (document.GetPage(i + 1) != ordered[i]) document.MovePage(ordered[i], i + 1);
                    document.Close();
                    return output.ToArray();
                }
                finally { foreach (var source in imports.Values) source.Close(); }
            }
        }

        public ProjectData RemapProject(ProjectData original)
        {
            var map = ExistingPageMap();
            var copy = JsonConvert.DeserializeObject<ProjectData>(JsonConvert.SerializeObject(original));
            int Map(int page) => map.TryGetValue(page, out int mapped) ? mapped : page;
            foreach (var item in copy.RedactionBlocks ?? new List<RedactionBlock>()) item.PageNumber = Map(item.PageNumber);
            foreach (var item in copy.CommentAnnotations ?? new List<CommentAnnotation>()) item.PageNumber = Map(item.PageNumber);
            foreach (var item in copy.TextAnnotations ?? new List<TextAnnotation>()) item.PageNumber = Map(item.PageNumber);
            foreach (var item in copy.RasterObjects ?? new List<RasterObject>()) item.PageNumber = Map(item.PageNumber);
            foreach (var item in copy.ArrowObjects ?? new List<ArrowObject>()) item.PageNumber = Map(item.PageNumber);
            foreach (var item in copy.VectorShapes ?? new List<VectorShapeObject>()) item.PageNumber = Map(item.PageNumber);
            var removedPages = copy.PagesToRemove ?? new HashSet<int>();
            foreach (var change in removalChanges)
            {
                if (change.Value) removedPages.Add(change.Key);
                else removedPages.Remove(change.Key);
            }
            copy.PagesToRemove = new HashSet<int>(removedPages.Select(Map));
            copy.PageRotationOffsets = (copy.PageRotationOffsets ?? new Dictionary<int, int>()).ToDictionary(p => Map(p.Key), p => p.Value);
            copy.CurrentPage = Map(copy.CurrentPage);
            if (copy.PagesListTopPage.HasValue) copy.PagesListTopPage = Map(copy.PagesListTopPage.Value);
            if (copy.ThumbnailsTopPage.HasValue) copy.ThumbnailsTopPage = Map(copy.ThumbnailsTopPage.Value);
            copy.AltTextEdits = (copy.AltTextEdits ?? new Dictionary<string, string>()).ToDictionary(p => {
                int separator = p.Key.IndexOf(':');
                return separator > 0 && int.TryParse(p.Key.Substring(0, separator), out int page)
                    ? Map(page).ToString(System.Globalization.CultureInfo.InvariantCulture) + p.Key.Substring(separator) : p.Key;
            }, p => p.Value);
            copy.ScrollX = copy.ScrollY = null;
            return copy;
        }
    }
}
