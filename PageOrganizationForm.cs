using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using AnonPDF.Properties;

namespace AnonPDF
{
    internal static class PageOrganizationText
    {
        internal static string Get(string key) => Resources.ResourceManager.GetString(key, Resources.Culture) ?? key;
        internal static string Format(string key, params object[] values) => string.Format(Get(key), values);
    }

    internal sealed class PageThumbnailMetrics
    {
        public int ItemHeight { get; private set; }
        public int ColumnWidth { get; private set; }
        public Size ImageSize { get; private set; }

        internal static PageThumbnailMetrics Calculate(int dpi, int fontHeight)
        {
            float scale = Math.Max(1, dpi / 96f);
            int height = (int)Math.Round(220 * scale);
            int caption = Math.Max((int)Math.Ceiling(40 * scale), fontHeight * 2 + (int)Math.Ceiling(12 * scale));
            int imageHeight = Math.Max(24, height - caption - (int)Math.Ceiling(8 * scale));
            return new PageThumbnailMetrics {
                ItemHeight = height, ColumnWidth = (int)Math.Round(180 * scale),
                ImageSize = new Size(Math.Max(1, imageHeight * 128 / 176), imageHeight)
            };
        }
    }

    internal sealed class PageThumbnailDragFeedback
    {
        private long nextScrollAt;
        internal int Gap { get; private set; } = -1;
        internal Rectangle MarkerBounds { get; private set; }

        internal void ResetScroll() => nextScrollAt = 0;

        internal Rectangle[] MoveMarker(int gap, Rectangle bounds)
        {
            Gap = gap;
            if (bounds == MarkerBounds) return Array.Empty<Rectangle>();
            var previous = MarkerBounds;
            MarkerBounds = bounds;
            if (previous.IsEmpty) return bounds.IsEmpty ? Array.Empty<Rectangle>() : new[] { bounds };
            return bounds.IsEmpty ? new[] { previous } : new[] { previous, bounds };
        }

        internal int ScrollDirection(int pointerY, int clientHeight, int edgeSize, long elapsedMilliseconds)
        {
            int direction = pointerY < edgeSize ? -1 : pointerY > clientHeight - edgeSize ? 1 : 0;
            if (direction == 0) { nextScrollAt = 0; return 0; }
            if (elapsedMilliseconds < nextScrollAt) return 0;
            nextScrollAt = elapsedMilliseconds + 180;
            return direction;
        }

        internal static Rectangle GetMarkerBounds(Rectangle itemBounds, bool after, int dpi)
        {
            int thickness = (int)Math.Ceiling(Math.Max(2, dpi / 48f));
            int inset = (int)Math.Round(4 * dpi / 96f);
            return new Rectangle(after ? itemBounds.Right - thickness : itemBounds.Left, itemBounds.Top + inset,
                thickness, Math.Max(1, itemBounds.Height - inset * 2));
        }
    }

    internal sealed class PageOrganizationForm : Form
    {
        private readonly PageOrganizationPlan plan;
        private readonly DialogTheme theme;
        private readonly Func<string> requestPassword;
        private readonly Dictionary<int, int> rotationOffsets;
        private readonly bool selectOnly;
        private readonly PageThumbnailView pages;
        private readonly Func<int, byte[]> createPageWithObjects;
        private readonly Action<Graphics, int, Rectangle> drawPageOverlays;
        private readonly PageThumbnailCache images = new PageThumbnailCache();
        private readonly FlowLayoutPanel tools;
        private readonly Label count;
        private readonly Button accept;
        private readonly Button cancel;
        private readonly Button up;
        private readonly Button down;
        private readonly Button remove;
        private readonly ComboBox insertion;
        private readonly ToolTip toolTip = new ToolTip();
        private readonly Timer thumbnails = new Timer { Interval = 60 };
        private readonly Timer dragScroll = new Timer { Interval = 180 };
        private readonly Dictionary<string, PDFiumSharp.PdfDocument> previews = new Dictionary<string, PDFiumSharp.PdfDocument>();
        private bool applying;
        private bool releasingResources;
        private readonly PageThumbnailDragFeedback dragFeedback = new PageThumbnailDragFeedback();
        private readonly Stopwatch dragElapsed = new Stopwatch();
        private List<string> dragging;
        private Point? dragStart;
        private int hoverIndex = -1;
        private Point dragPoint;
        public byte[] ComposedPdf { get; private set; }
        public int SelectedPageNumber => pages.SelectedIndices.Count == 0 ? 0 : pages.SelectedIndices.Cast<int>().Min() + 1;
        public int[] SelectedSourcePages => pages.SelectedIndices.Cast<int>().OrderBy(n => n).Select(n => plan.Pages[n].SourcePageNumber).ToArray();

        internal PageOrganizationForm(PageOrganizationPlan plan, DialogTheme theme, Func<string> requestPassword,
            int currentPage = 1, Dictionary<int, int> rotationOffsets = null, bool selectOnly = false,
            Func<int, byte[]> createPageWithObjects = null, Action<Graphics, int, Rectangle> drawPageOverlays = null)
        {
            this.plan = plan;
            this.theme = theme;
            this.requestPassword = requestPassword;
            this.rotationOffsets = rotationOffsets ?? new Dictionary<int, int>();
            this.selectOnly = selectOnly;
            this.createPageWithObjects = createPageWithObjects;
            this.drawPageOverlays = drawPageOverlays;
            Name = "pageOrganizationForm";
            Text = PageOrganizationText.Get(selectOnly ? "Pages_SelectPages" : "Pages_Title");
            AutoScaleMode = AutoScaleMode.Dpi;
            AutoScaleDimensions = new SizeF(96, 96);
            Font = new Font("Segoe UI", 9);
            ClientSize = new Size(1000, 700);
            MinimumSize = new Size(700, 460);
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            var root = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 3, Padding = new Padding(10) };
            root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            root.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tools = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoSize = true, WrapContents = true, Margin = Padding.Empty };
            if (!selectOnly)
            {
                tools.Controls.Add(Command("Pages_AddPdf", (_, __) => AddPdf()));
                tools.Controls.Add(Command("Menu_AddBlankPage", (_, __) => AddBlank()));
                insertion = new ComboBox { Name = "pageInsertionPosition", DropDownStyle = ComboBoxStyle.DropDownList, Width = 210, Margin = new Padding(6) };
                insertion.Items.AddRange(new[] { PageOrganizationText.Get("Pages_Before"), PageOrganizationText.Get("Pages_After") });
                insertion.SelectedIndex = 1;
                tools.Controls.Add(insertion);
                up = IconButton("\uE74A", "Pages_MoveEarlier", (_, __) => MoveStep(-1));
                down = IconButton("\uE74B", "Pages_MoveLater", (_, __) => MoveStep(1));
                remove = IconButton("\uE74D", "Pages_Remove", (_, __) => RemoveOrRestorePages());
                remove.Name = "removeOrganizationPages";
                tools.Controls.AddRange(new Control[] { up, down, remove });
            }
            else
            {
                tools.Controls.Add(Command("Pages_SelectAll", (_, __) => SelectAll(true)));
                tools.Controls.Add(Command("Pages_SelectNone", (_, __) => SelectAll(false)));
            }
            pages = new PageThumbnailView { Name = "organizationPages", Dock = DockStyle.Fill, AllowDrop = !selectOnly, ItemText = PageCaption };
            pages.DrawItem += DrawPage;
            pages.Paint += (_, e) => {
                if (!dragFeedback.MarkerBounds.IsEmpty)
                    using (var brush = new SolidBrush(theme.TextPrimaryColor)) e.Graphics.FillRectangle(brush, dragFeedback.MarkerBounds);
            };
            pages.ViewportChanged += (_, __) => { hoverIndex = -1; toolTip.Hide(pages); SetDropGap(-1); };
            pages.SelectedIndexChanged += (_, __) => UpdateSelection();
            pages.MouseDown += (_, e) => { dragStart = e.Button == MouseButtons.Left && pages.IndexFromPoint(e.Location) >= 0 ? e.Location : (Point?)null; };
            pages.MouseUp += (_, __) => dragStart = null;
            pages.MouseMove += (_, e) => {
                if (dragging != null) return;
                int index = pages.IndexFromPoint(e.Location);
                if (hoverIndex != index) { hoverIndex = index; toolTip.SetToolTip(pages, index >= 0 ? PageCaption(index) : ""); }
                if (selectOnly || e.Button != MouseButtons.Left || !dragStart.HasValue || pages.SelectedIndices.Count == 0) return;
                var threshold = new Rectangle(dragStart.Value.X - SystemInformation.DragSize.Width / 2,
                    dragStart.Value.Y - SystemInformation.DragSize.Height / 2, SystemInformation.DragSize.Width, SystemInformation.DragSize.Height);
                if (threshold.Contains(e.Location)) return;
                dragStart = null;
                dragging = pages.SelectedItems.Cast<OrganizationPage>().Select(p => p.Id).ToList();
                toolTip.Hide(pages);
                hoverIndex = -1;
                dragFeedback.ResetScroll();
                dragElapsed.Restart();
                try { pages.DoDragDrop(new DataObject("AnonPDF.PageOrganization", dragging), DragDropEffects.Move); }
                finally
                {
                    dragging = null;
                    dragScroll.Stop();
                    dragElapsed.Stop();
                    SetDropGap(-1);
                    if (Visible && !releasingResources && !applying) thumbnails.Start();
                }
            };
            pages.DragEnter += DragOverPages;
            pages.DragOver += DragOverPages;
            pages.DragLeave += (_, __) => { dragScroll.Stop(); SetDropGap(-1); };
            pages.DragDrop += (_, e) => {
                if (dragging == null || dragFeedback.Gap < 0 || !e.Data.GetDataPresent("AnonPDF.PageOrganization")) return;
                var ids = dragging.ToList();
                int gap = dragFeedback.Gap;
                SetDropGap(-1);
                plan.Move(plan.Pages.Select((p, i) => ids.Contains(p.Id) ? i : -1).Where(i => i >= 0), gap);
                Rebuild(ids);
            };
            var footer = new TableLayoutPanel { Dock = DockStyle.Fill, AutoSize = true, ColumnCount = 2, Margin = new Padding(0, 8, 0, 0) };
            footer.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            footer.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            count = new Label { AutoSize = true, Anchor = AnchorStyles.Left, Name = "organizationPageCount" };
            var buttons = new FlowLayoutPanel { AutoSize = true, WrapContents = false, FlowDirection = FlowDirection.RightToLeft, Dock = DockStyle.Fill };
            cancel = Command("Dialog_Button_Cancel", (_, __) => DialogResult = DialogResult.Cancel);
            accept = Command(selectOnly ? "Pages_Insert" : "Pages_Apply", async (_, __) => await ApplyAsync());
            accept.Name = "applyPageOrganization";
            buttons.Controls.AddRange(new Control[] { cancel, accept });
            footer.Controls.Add(count, 0, 0);
            footer.Controls.Add(buttons, 1, 0);
            root.Controls.Add(tools, 0, 0);
            root.Controls.Add(pages, 0, 1);
            root.Controls.Add(footer, 0, 2);
            Controls.Add(root);
            AcceptButton = accept;
            CancelButton = cancel;
            DialogThemeApplier.ApplyTo(this, theme);
            pages.BackColor = theme.PanelBackColor;
            pages.ForeColor = theme.TextPrimaryColor;
            Rebuild(selectOnly ? new List<string>() : new List<string> { plan.Pages[Math.Max(0, Math.Min(currentPage - 1, plan.Pages.Count - 1))].Id });
            thumbnails.Tick += (_, __) => GenerateVisibleThumbnails();
            dragScroll.Tick += (_, __) => { if (dragging != null) UpdateDragFeedback(dragPoint); };
            VisibleChanged += (_, __) => { if (!Visible) thumbnails.Stop(); };
            Shown += (_, __) => {
                var area = Screen.FromControl(this).WorkingArea;
                MinimumSize = new Size(Math.Min(MinimumSize.Width, area.Width - 24), Math.Min(MinimumSize.Height, area.Height - 24));
                Size = new Size(Math.Min(Width, area.Width - 24), Math.Min(Height, area.Height - 24));
                pages.Focus();
                ConfigureThumbnailSizes();
                if (pages.SelectedIndices.Count > 0) pages.EnsureVisible(pages.SelectedIndices.First());
                if (Visible && !releasingResources) thumbnails.Start();
            };
            DpiChanged += (_, __) => ConfigureThumbnailSizes();
            FormClosing += (_, e) => { if (applying) e.Cancel = true; };
        }

        private Button Command(string key, EventHandler action)
        {
            var button = new Button { Text = PageOrganizationText.Get(key), AutoSize = true, AutoSizeMode = AutoSizeMode.GrowAndShrink,
                Padding = new Padding(9, 4, 9, 4), MinimumSize = new Size(80, 30), Margin = new Padding(3) };
            button.Click += action;
            return button;
        }

        private Button IconButton(string glyph, string key, EventHandler action)
        {
            var button = new Button { Text = glyph, Font = new Font("Segoe MDL2 Assets", 12), Size = new Size(34, 32), Margin = new Padding(3), AccessibleName = PageOrganizationText.Get(key) };
            button.Click += action;
            toolTip.SetToolTip(button, PageOrganizationText.Get(key));
            return button;
        }

        private void Rebuild(IEnumerable<string> selection)
        {
            var selected = new HashSet<string>(selection);
            pages.SetItems(plan.Pages, Enumerable.Range(0, plan.Pages.Count).Where(i => selected.Contains(plan.Pages[i].Id)));
            if (pages.SelectedIndices.Count > 0) pages.EnsureVisible(pages.SelectedIndices.First());
            UpdateSelection();
        }

        private void UpdateSelection()
        {
            if (count == null) return;
            var selected = pages.SelectedIndices.Cast<int>().OrderBy(i => i).ToList();
            count.Text = PageOrganizationText.Format("Pages_Count", plan.Pages.Count, selected.Count);
            accept.Enabled = selectOnly ? selected.Count > 0 : plan.IsChanged;
            if (up != null) up.Enabled = selected.Count > 0 && selected[0] > 0;
            if (down != null) down.Enabled = selected.Count > 0 && selected[selected.Count - 1] < plan.Pages.Count - 1;
            if (remove != null)
            {
                remove.Enabled = selected.Count > 0;
                bool restore = selected.Count > 0 && selected.All(i => plan.Pages[i].ExistingPageNumber > 0 && plan.Pages[i].MarkedForDeletion);
                remove.Text = restore ? "\uE777" : "\uE74D";
                remove.AccessibleName = PageOrganizationText.Get(restore ? "Pages_Restore" : "Pages_Remove");
                toolTip.SetToolTip(remove, remove.AccessibleName);
            }
        }

        private void SelectAll(bool selected)
        {
            pages.SelectAll(selected);
        }

        private int InsertionIndex()
        {
            if (pages.SelectedIndices.Count == 0) return plan.Pages.Count;
            return insertion.SelectedIndex == 0 ? pages.SelectedIndices.Cast<int>().Min() : pages.SelectedIndices.Cast<int>().Max() + 1;
        }

        private void AddPdf()
        {
            using (var open = new OpenFileDialog { Filter = "PDF (*.pdf)|*.pdf", Title = PageOrganizationText.Get("Pages_AddPdf") })
            {
                if (open.ShowDialog(this) != DialogResult.OK) return;
                try
                {
                    byte[] bytes = File.ReadAllBytes(open.FileName);
                    string password = "";
                    PageOrganizationPlan imported;
                    try { imported = PageOrganizationPlan.Open(bytes, ignoreRestrictions: AnonPDF.Properties.Settings.Default.IgnorePdfRestrictions); }
                    catch (iText.Kernel.Exceptions.BadPasswordException)
                    {
                        password = requestPassword();
                        if (string.IsNullOrEmpty(password)) return;
                        imported = PageOrganizationPlan.Open(bytes, password, AnonPDF.Properties.Settings.Default.IgnorePdfRestrictions);
                    }
                    using (var choose = new PageOrganizationForm(imported, theme, requestPassword, selectOnly: true))
                    {
                        choose.Text = PageOrganizationText.Get("Pages_SelectPages") + ": " + Path.GetFileName(open.FileName);
                        if (choose.ShowDialog(this) != DialogResult.OK) return;
                        int index = InsertionIndex();
                        int countBefore = plan.Pages.Count;
                        plan.AddPdf(bytes, password, choose.SelectedSourcePages, index, Path.GetFileName(open.FileName));
                        Rebuild(plan.Pages.Skip(index).Take(plan.Pages.Count - countBefore).Select(p => p.Id).ToList());
                    }
                }
                catch (Exception ex) { ShowError(ex); }
            }
        }

        private void AddBlank()
        {
            var selected = pages.SelectedIndices.Count > 0 ? plan.Pages[pages.SelectedIndices.First()] : plan.Pages[plan.Pages.Count - 1];
            int rotation = selected.Rotation + (rotationOffsets.TryGetValue(selected.ExistingPageNumber, out int offset) ? offset : 0);
            bool turned = Math.Abs(rotation % 180) == 90;
            using (var dialog = new BlankPageForm(turned ? selected.Height : selected.Width, turned ? selected.Width : selected.Height, theme))
            {
                if (dialog.ShowDialog(this) != DialogResult.OK) return;
                int index = InsertionIndex();
                plan.AddBlank(index, dialog.PageWidth, dialog.PageHeight);
                Rebuild(new[] { plan.Pages[index].Id });
            }
        }

        private void RemoveOrRestorePages()
        {
            var selected = pages.SelectedIndices.OrderBy(i => i).ToArray();
            if (selected.Length == 0) return;
            var ids = selected.Select(i => plan.Pages[i].Id).ToArray();
            if (!plan.RemoveOrRestore(selected))
            {
                MessageBox.Show(this, Resources.Err_TooFewPages, Resources.Title_Error, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            hoverIndex = -1;
            toolTip.Hide(pages);
            Rebuild(ids.Any(id => plan.Pages.Any(p => p.Id == id)) ? ids :
                new[] { plan.Pages[Math.Min(selected[0], plan.Pages.Count - 1)].Id });
        }

        private void MoveStep(int direction)
        {
            var selected = pages.SelectedIndices.Cast<int>().OrderBy(i => direction < 0 ? i : -i).ToList();
            var ids = selected.Select(i => plan.Pages[i].Id).ToList();
            foreach (int index in selected)
                plan.Move(new[] { index }, direction < 0 ? index - 1 : index + 2);
            Rebuild(ids);
            if (pages.SelectedIndices.Count > 0) pages.EnsureVisible(pages.SelectedIndices.First());
        }

        private void DragOverPages(object sender, DragEventArgs e)
        {
            if (dragging == null || !e.Data.GetDataPresent("AnonPDF.PageOrganization"))
            {
                e.Effect = DragDropEffects.None;
                SetDropGap(-1);
                return;
            }
            e.Effect = DragDropEffects.Move;
            dragPoint = pages.PointToClient(new Point(e.X, e.Y));
            UpdateDragFeedback(dragPoint);
            dragScroll.Start();
        }

        private void UpdateDragFeedback(Point point)
        {
            if (!pages.ViewportRectangle.Contains(point)) { SetDropGap(-1); return; }
            int direction = dragFeedback.ScrollDirection(point.Y, pages.ViewportRectangle.Height,
                (int)Math.Round(24 * DeviceDpi / 96f), dragElapsed.ElapsedMilliseconds);
            if (direction != 0)
            {
                SetDropGap(-1);
                pages.ScrollRows(direction);
            }
            int gap = pages.GetDropGap(point, out Rectangle marker);
            SetDropGap(gap, marker);
        }

        private void SetDropGap(int gap, Rectangle marker = default(Rectangle))
        {
            foreach (var bounds in dragFeedback.MoveMarker(gap, marker))
            {
                var visibleBounds = Rectangle.Intersect(bounds, pages.ViewportRectangle);
                if (!visibleBounds.IsEmpty) pages.Invalidate(visibleBounds);
            }
        }

        private async Task ApplyAsync()
        {
            if (selectOnly) { if (pages.SelectedIndices.Count > 0) DialogResult = DialogResult.OK; return; }
            if (!plan.IsChanged || applying) return;
            if (!plan.IsLayoutChanged) { DialogResult = DialogResult.OK; return; }
            if (plan.ContainsSignatures && MessageBox.Show(this, PageOrganizationText.Get("Pages_SignatureWarning"),
                PageOrganizationText.Get("Title_Warning"), MessageBoxButtons.YesNo, MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;
            applying = true;
            thumbnails.Stop();
            tools.Enabled = pages.Enabled = accept.Enabled = cancel.Enabled = false;
            count.Text = PageOrganizationText.Get("Pages_Applying");
            UseWaitCursor = true;
            try { ComposedPdf = await Task.Run(() => plan.Compose()); applying = false; DialogResult = DialogResult.OK; }
            catch (Exception ex) { applying = false; ShowError(ex); }
            finally
            {
                UseWaitCursor = false;
                tools.Enabled = pages.Enabled = cancel.Enabled = true;
                UpdateSelection();
                if (Visible && !releasingResources) thumbnails.Start();
            }
        }

        private void ShowError(Exception ex) => MessageBox.Show(this, PageOrganizationText.Format("Pages_Error", ex.Message),
            PageOrganizationText.Get("Title_Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);

        private string PageCaption(int index)
        {
            var page = plan.Pages[index];
            string source = page.IsBlank ? PageOrganizationText.Get("Pages_Blank") :
                (page.SourceName ?? PageOrganizationText.Get("Pages_Original")) + " (" + page.SourcePageNumber + ")";
            return (index + 1) + ". " + source + (page.MarkedForDeletion ? " [" + PageOrganizationText.Get("Pages_DeletionMarked") + "]" : "");
        }

        private void ConfigureThumbnailSizes()
        {
            var metrics = PageThumbnailMetrics.Calculate(DeviceDpi, Font.Height);
            pages.SetItemSize(new Size(metrics.ColumnWidth, metrics.ItemHeight));
            images.Clear();
            pages.Invalidate();
        }

        private void DrawPage(object sender, DrawItemEventArgs e)
        {
            if (releasingResources || IsDisposed || Disposing || e.Index < 0 || e.Index >= plan.Pages.Count) return;
            bool selected = (e.State & DrawItemState.Selected) != 0;
            using (var brush = new SolidBrush(selected ? theme.SelectionBackColor : pages.BackColor)) e.Graphics.FillRectangle(brush, e.Bounds);
            int imageHeight = PageThumbnailMetrics.Calculate(DeviceDpi, Font.Height).ImageSize.Height;
            int top = e.Bounds.Top + (int)Math.Round(4 * DeviceDpi / 96f);
            if (images.TryGetValue(plan.Pages[e.Index].Id, out var image))
            {
                e.Graphics.DrawImageUnscaled(image, e.Bounds.Left + (e.Bounds.Width - image.Width) / 2, top);
                if (plan.Pages[e.Index].MarkedForDeletion)
                {
                    var imageBounds = new Rectangle(e.Bounds.Left + (e.Bounds.Width - image.Width) / 2, top, image.Width, image.Height);
                    using (var pen = new Pen(Color.Black, Math.Max(2f, Math.Min(image.Width, image.Height) / 18f)))
                    {
                        pen.StartCap = pen.EndCap = System.Drawing.Drawing2D.LineCap.Round;
                        e.Graphics.DrawLine(pen, imageBounds.Left, imageBounds.Top, imageBounds.Right, imageBounds.Bottom);
                        e.Graphics.DrawLine(pen, imageBounds.Right, imageBounds.Top, imageBounds.Left, imageBounds.Bottom);
                    }
                }
            }
            var label = new Rectangle(e.Bounds.Left + 2, top + imageHeight + 3, e.Bounds.Width - 4, Math.Max(0, e.Bounds.Bottom - top - imageHeight - 3));
            TextRenderer.DrawText(e.Graphics, PageCaption(e.Index), pages.Font, label, selected ? theme.SelectionForeColor : theme.TextPrimaryColor,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.WordBreak | TextFormatFlags.EndEllipsis |
                TextFormatFlags.PreserveGraphicsTranslateTransform | TextFormatFlags.PreserveGraphicsClipping);
            e.DrawFocusRectangle();
        }

        private void GenerateVisibleThumbnails()
        {
            if (releasingResources || IsDisposed || Disposing || pages.IsDisposed || !pages.IsHandleCreated || applying) return;
            var nearby = pages.VisibleIndices(extraRows: 1).ToArray();
            images.Retain(nearby.Select(index => plan.Pages[index].Id));
            var retainedSources = new HashSet<string>(nearby.Select(index => plan.Pages[index].SourceId));
            foreach (string key in previews.Keys.Where(key => !retainedSources.Contains(key)).ToArray())
            {
                previews[key].Close();
                previews.Remove(key);
            }
            int rendered = 0;
            var work = Stopwatch.StartNew();
            foreach (int index in pages.VisibleIndices().Concat(nearby).Distinct())
            {
                var page = plan.Pages[index];
                if (images.ContainsKey(page.Id)) continue;
                var metrics = PageThumbnailMetrics.Calculate(DeviceDpi, Font.Height);
                var thumbnail = new Bitmap(metrics.ImageSize.Width, metrics.ImageSize.Height);
                {
                    using (var graphics = Graphics.FromImage(thumbnail))
                    {
                        graphics.Clear(Color.White);
                        PDFiumSharp.PdfDocument composedDocument = null;
                        try
                        {
                            if (!page.IsBlank)
                            {
                                byte[] composed = page.ExistingPageNumber > 0 ? createPageWithObjects?.Invoke(page.ExistingPageNumber) : null;
                                PDFiumSharp.PdfDocument document;
                                if (composed != null)
                                    document = composedDocument = new PDFiumSharp.PdfDocument(data: composed);
                                else if (!previews.TryGetValue(page.SourceId, out document))
                                {
                                    var source = plan.SourceFor(page);
                                    document = new PDFiumSharp.PdfDocument(data: source.Bytes, password: source.Password ?? "");
                                    previews.Add(page.SourceId, document);
                                }
                                var nativePage = document.Pages[composed != null ? 0 : page.SourcePageNumber - 1];
                                double ratio = Math.Min((thumbnail.Width - 8) / nativePage.Width, (thumbnail.Height - 8) / nativePage.Height);
                                using (var bitmap = new PDFiumSharp.PDFiumBitmap(Math.Max(1, (int)(nativePage.Width * ratio)), Math.Max(1, (int)(nativePage.Height * ratio)), true))
                                using (var stream = new MemoryStream())
                                {
                                    bitmap.FillRectangle(0, 0, bitmap.Width, bitmap.Height, 0xffffffff);
                                    nativePage.Render(bitmap, flags: PDFiumSharp.Enums.RenderingFlags.Annotations);
                                    bitmap.Save(stream);
                                    stream.Position = 0;
                                    using (var image = new Bitmap(stream))
                                    {
                                        if (composed == null && rotationOffsets.TryGetValue(page.ExistingPageNumber, out int rotation))
                                            image.RotateFlip(rotation == 90 ? RotateFlipType.Rotate90FlipNone : rotation == 180 ? RotateFlipType.Rotate180FlipNone : rotation == 270 ? RotateFlipType.Rotate270FlipNone : RotateFlipType.RotateNoneFlipNone);
                                        float fit = Math.Min((thumbnail.Width - 8f) / image.Width, (thumbnail.Height - 8f) / image.Height);
                                        graphics.DrawImage(image, (thumbnail.Width - image.Width * fit) / 2, (thumbnail.Height - image.Height * fit) / 2, image.Width * fit, image.Height * fit);
                                        if (page.ExistingPageNumber > 0)
                                        {
                                            var content = Rectangle.Round(new RectangleF((thumbnail.Width - image.Width * fit) / 2,
                                                (thumbnail.Height - image.Height * fit) / 2, image.Width * fit, image.Height * fit));
                                            drawPageOverlays?.Invoke(graphics, page.ExistingPageNumber, content);
                                        }
                                    }
                                }
                            }
                        }
                        catch { graphics.DrawIcon(SystemIcons.Warning, 48, 64); }
                        finally { composedDocument?.Close(); }
                        graphics.DrawRectangle(Pens.Gray, 0, 0, thumbnail.Width - 1, thumbnail.Height - 1);
                    }
                    images.Add(page.Id, thumbnail);
                }
                pages.Invalidate(pages.GetItemRectangle(index));
                if (++rendered >= (dragging == null ? 3 : 1) || work.ElapsedMilliseconds >= 25) break;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                releasingResources = true;
                thumbnails.Dispose();
                dragScroll.Dispose();
                foreach (var document in previews.Values) document.Close();
                images.Dispose();
                toolTip.Dispose();
            }
            base.Dispose(disposing);
        }

    }

    internal sealed class BlankPageForm : Form
    {
        private readonly ComboBox format;
        private readonly ComboBox orientation;
        private readonly NumericUpDown width;
        private readonly NumericUpDown height;
        private readonly float currentWidth;
        private readonly float currentHeight;
        public float PageWidth => Dimensions().Width;
        public float PageHeight => Dimensions().Height;

        internal BlankPageForm(float currentWidth, float currentHeight, DialogTheme theme)
        {
            this.currentWidth = currentWidth;
            this.currentHeight = currentHeight;
            Name = "blankPageForm";
            Font = new Font("Segoe UI", 9);
            Text = PageOrganizationText.Get("Menu_AddBlankPage");
            AutoScaleMode = AutoScaleMode.Dpi;
            AutoScaleDimensions = new SizeF(96, 96);
            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = MinimizeBox = ShowIcon = ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Padding = new Padding(12);
            var grid = new TableLayoutPanel { AutoSize = true, Dock = DockStyle.Fill, ColumnCount = 2, RowCount = 5 };
            grid.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            grid.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            format = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Width = 210, Name = "blankPageFormat" };
            format.Items.AddRange(new[] { PageOrganizationText.Get("Pages_CurrentFormat"), "A4", "A3", "A5", PageOrganizationText.Get("Pages_CustomFormat") });
            format.SelectedIndex = 0;
            orientation = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Width = 210, Name = "blankPageOrientation" };
            orientation.Items.AddRange(new[] { PageOrganizationText.Get("Pages_Portrait"), PageOrganizationText.Get("Pages_Landscape") });
            orientation.SelectedIndex = currentWidth > currentHeight ? 1 : 0;
            width = new NumericUpDown { Minimum = 1, Maximum = 5080, Width = 120, Name = "blankPageWidth" };
            height = new NumericUpDown { Minimum = 1, Maximum = 5080, Width = 120, Name = "blankPageHeight" };
            var controls = new Control[] { format, orientation, width, height };
            var keys = new[] { "Pages_Format", "Pages_Orientation", "Pages_Width", "Pages_Height" };
            for (int row = 0; row < controls.Length; row++)
            {
                grid.RowStyles.Add(new RowStyle(SizeType.AutoSize));
                grid.Controls.Add(new Label { Text = PageOrganizationText.Get(keys[row]), AutoSize = true, Anchor = AnchorStyles.Left, Margin = new Padding(3, 7, 12, 7) }, 0, row);
                controls[row].Margin = new Padding(3, 5, 3, 5);
                grid.Controls.Add(controls[row], 1, row);
            }
            var buttons = new FlowLayoutPanel { AutoSize = true, FlowDirection = FlowDirection.RightToLeft, WrapContents = false, Dock = DockStyle.Fill };
            var cancel = new Button { Text = PageOrganizationText.Get("Dialog_Button_Cancel"), DialogResult = DialogResult.Cancel, AutoSize = true, MinimumSize = new Size(85, 30) };
            var ok = new Button { Text = PageOrganizationText.Get("Pages_Insert"), DialogResult = DialogResult.OK, AutoSize = true, MinimumSize = new Size(85, 30) };
            buttons.Controls.AddRange(new Control[] { cancel, ok });
            grid.Controls.Add(buttons, 0, 4);
            grid.SetColumnSpan(buttons, 2);
            Controls.Add(grid);
            AcceptButton = ok;
            CancelButton = cancel;
            format.SelectedIndexChanged += (_, __) => RefreshDimensions();
            orientation.SelectedIndexChanged += (_, __) => RefreshDimensions();
            DialogThemeApplier.ApplyTo(this, theme);
            RefreshDimensions();
        }

        private SizeF Dimensions()
        {
            SizeF size;
            switch (format.SelectedIndex)
            {
                case 0: size = new SizeF(currentWidth, currentHeight); break;
                case 1: size = new SizeF(595, 842); break;
                case 2: size = new SizeF(842, 1191); break;
                case 3: size = new SizeF(420, 595); break;
                default: return new SizeF((float)width.Value * 72 / 25.4f, (float)height.Value * 72 / 25.4f);
            }
            bool landscape = orientation.SelectedIndex == 1;
            if ((size.Width > size.Height) != landscape) size = new SizeF(size.Height, size.Width);
            return size;
        }

        private void RefreshDimensions()
        {
            width.Enabled = height.Enabled = format.SelectedIndex == 4;
            orientation.Enabled = format.SelectedIndex != 4;
            if (format.SelectedIndex == 4) return;
            SizeF size = Dimensions();
            width.Value = Math.Max(1, Math.Min(width.Maximum, (decimal)Math.Round(size.Width * 25.4 / 72)));
            height.Value = Math.Max(1, Math.Min(height.Maximum, (decimal)Math.Round(size.Height * 25.4 / 72)));
        }
    }
}
