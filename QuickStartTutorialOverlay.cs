using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.IO;
using System.Windows.Forms;
using AnonPDF.Properties;
using PDFiumSharp;
using PDFiumSharp.Enums;

namespace AnonPDF
{
    internal sealed class QuickStartTutorialOverlay : Control
    {
        private sealed class TutorialStep
        {
            public string Title { get; set; }
            public string[] Lines { get; set; }
        }

        private readonly Control markerRadioButton;
        private readonly Control boxRadioButton;
        private readonly Control savePdfButton;
        private readonly Control pdfViewer;
        private readonly Bitmap backgroundSnapshot;
        private readonly Bitmap tutorialPreviewBitmap;
        private readonly Button nextButton;
        private readonly Button backButton;
        private readonly Button cancelButton;
        private readonly List<TutorialStep> steps;
        private int currentStepIndex;
        private Rectangle cardBounds = Rectangle.Empty;
        private readonly List<Rectangle> stepBounds = new List<Rectangle>();

        internal QuickStartTutorialOverlay(Control markerRadioButton, Control boxRadioButton, Control pdfViewer, Control savePdfButton, Bitmap backgroundSnapshot)
        {
            this.markerRadioButton = markerRadioButton;
            this.boxRadioButton = boxRadioButton;
            this.pdfViewer = pdfViewer;
            this.savePdfButton = savePdfButton;
            this.backgroundSnapshot = backgroundSnapshot;
            tutorialPreviewBitmap = LoadTutorialPreviewBitmap();

            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw |
                     ControlStyles.UserPaint |
                     ControlStyles.SupportsTransparentBackColor, true);

            TabStop = false;
            Dock = DockStyle.Fill;

            steps = new List<TutorialStep>
            {
                new TutorialStep
                {
                    Title = Tr("QuickStart_Step1_Title"),
                    Lines = new[]
                    {
                        Tr("QuickStart_Step1_Line1"),
                        Tr("QuickStart_Step1_Line2")
                    }
                },
                new TutorialStep
                {
                    Title = Tr("QuickStart_Step2_Title"),
                    Lines = new[]
                    {
                        Tr("QuickStart_Step2_Line1"),
                        Tr("QuickStart_Step2_Line2")
                    }
                },
                new TutorialStep
                {
                    Title = Tr("QuickStart_Step3_Title"),
                    Lines = new[]
                    {
                        Tr("QuickStart_Step3_Line1")
                    }
                }
            };

            backButton = new Button
            {
                Text = Tr("QuickStart_Button_Back"),
                Width = 110,
                Height = 32,
                Font = new Font(Font.FontFamily, 10f, FontStyle.Regular)
            };
            backButton.Click += (_, __) =>
            {
                if (currentStepIndex <= 0)
                {
                    return;
                }

                currentStepIndex--;
                UpdateButtons();
                Invalidate();
            };

            nextButton = new Button
            {
                Text = Tr("QuickStart_Button_Next"),
                Width = 110,
                Height = 32,
                Font = new Font(Font.FontFamily, 10f, FontStyle.Regular)
            };
            nextButton.Click += (_, __) =>
            {
                if (currentStepIndex < steps.Count - 1)
                {
                    currentStepIndex++;
                    UpdateButtons();
                    Invalidate();
                    return;
                }

                Dismissed?.Invoke(this, EventArgs.Empty);
            };

            cancelButton = new Button
            {
                Text = Resources.ResourceManager.GetString("Dialog_Button_Cancel") ?? "Cancel",
                Width = 110,
                Height = 32,
                Font = new Font(Font.FontFamily, 10f, FontStyle.Regular)
            };
            cancelButton.Click += (_, __) => Dismissed?.Invoke(this, EventArgs.Empty);

            Controls.Add(backButton);
            Controls.Add(cancelButton);
            Controls.Add(nextButton);

            UpdateButtons();
            Resize += (_, __) => UpdateLayout();
            UpdateLayout();
        }

        internal event EventHandler Dismissed;

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            if (backgroundSnapshot != null)
            {
                g.DrawImageUnscaled(backgroundSnapshot, Point.Empty);
            }

            using (var overlayBrush = new SolidBrush(Color.FromArgb(120, 0, 0, 0)))
            {
                g.FillRectangle(overlayBrush, ClientRectangle);
            }

            DrawStepHighlight(g);
            DrawCard(g);
        }

        private void UpdateButtons()
        {
            backButton.Text = Tr("QuickStart_Button_Back");
            backButton.Enabled = currentStepIndex > 0;
            nextButton.Text = currentStepIndex >= steps.Count - 1
                ? Tr("QuickStart_Button_Understood")
                : Tr("QuickStart_Button_Next");
        }

        private void UpdateLayout()
        {
            int width = Math.Min(640, Math.Max(500, ClientSize.Width - 80));
            int height = Math.Min(420, Math.Max(360, ClientSize.Height - 80));
            cardBounds = new Rectangle(
                Math.Max(20, (ClientSize.Width - width) / 2),
                Math.Max(20, (ClientSize.Height - height) / 2),
                width,
                height);

            int buttonTop = cardBounds.Bottom - 48;
            backButton.Location = new Point(cardBounds.Left + 24, buttonTop);
            cancelButton.Location = new Point((cardBounds.Left + cardBounds.Right - cancelButton.Width) / 2, buttonTop);
            nextButton.Location = new Point(cardBounds.Right - nextButton.Width - 24, buttonTop);
            Invalidate();
        }

        private void DrawCard(Graphics g)
        {
            using (var path = BuildRoundedRectangle(cardBounds, 14))
            using (var shadowBrush = new SolidBrush(Color.FromArgb(35, 0, 0, 0)))
            using (var cardBrush = new SolidBrush(Color.White))
            using (var borderPen = new Pen(Color.FromArgb(220, 208, 208, 208), 1f))
            {
                using (var shadowPath = BuildRoundedRectangle(new Rectangle(cardBounds.X + 4, cardBounds.Y + 6, cardBounds.Width, cardBounds.Height), 14))
                {
                    g.FillPath(shadowBrush, shadowPath);
                }

                g.FillPath(cardBrush, path);
                g.DrawPath(borderPen, path);
            }

            var titleRect = new Rectangle(cardBounds.Left + 24, cardBounds.Top + 18, cardBounds.Width - 48, 26);
            using (var titleFont = new Font(Font.FontFamily, 13f, FontStyle.Bold))
            using (var titleBrush = new SolidBrush(Color.FromArgb(31, 42, 51)))
            {
                g.DrawString(Tr("QuickStart_Title"), titleFont, titleBrush, titleRect);
            }

            int stepTop = titleRect.Bottom + 16;
            int stepWidth = cardBounds.Width - 36;
            stepBounds.Clear();

            for (int i = 0; i < steps.Count; i++)
            {
                int stepHeight = MeasureStepHeight(g, steps[i], stepWidth - 24);
                Rectangle stepRect = new Rectangle(cardBounds.Left + 18, stepTop, stepWidth, stepHeight);
                stepBounds.Add(stepRect);
                DrawStep(g, stepRect, steps[i], i == currentStepIndex);
                stepTop = stepRect.Bottom + 10;
            }
        }

        private void DrawStep(Graphics g, Rectangle stepRect, TutorialStep step, bool active)
        {
            if (active)
            {
                using (var activeBrush = new SolidBrush(Color.FromArgb(255, 255, 246, 196)))
                using (var activePen = new Pen(Color.FromArgb(245, 204, 102), 1.5f))
                using (var path = BuildRoundedRectangle(stepRect, 10))
                {
                    g.FillPath(activeBrush, path);
                    g.DrawPath(activePen, path);
                }
            }

            Rectangle titleRect = new Rectangle(stepRect.Left + 12, stepRect.Top + 8, stepRect.Width - 24, 22);
            Rectangle lineRect = new Rectangle(stepRect.Left + 24, stepRect.Top + 34, stepRect.Width - 40, stepRect.Height - 42);

            using (var titleFont = new Font(Font.FontFamily, 9.5f, FontStyle.Bold))
            using (var bodyFont = new Font(Font.FontFamily, 9f, FontStyle.Regular))
            {
                TextRenderer.DrawText(
                    g,
                    step.Title,
                    titleFont,
                    titleRect,
                    Color.FromArgb(31, 42, 51),
                    TextFormatFlags.Left | TextFormatFlags.Top | TextFormatFlags.WordBreak | TextFormatFlags.NoPadding);

                foreach (string line in step.Lines.Where(line => !string.IsNullOrWhiteSpace(line)))
                {
                    Size measured = TextRenderer.MeasureText(
                        g,
                        "- " + line,
                        bodyFont,
                        new Size(lineRect.Width, int.MaxValue),
                        TextFormatFlags.Left | TextFormatFlags.Top | TextFormatFlags.WordBreak | TextFormatFlags.NoPadding);

                    Rectangle currentLineRect = new Rectangle(lineRect.Left, lineRect.Top, lineRect.Width, measured.Height);
                    TextRenderer.DrawText(
                        g,
                        "- " + line,
                        bodyFont,
                        currentLineRect,
                        Color.FromArgb(70, 78, 87),
                        TextFormatFlags.Left | TextFormatFlags.Top | TextFormatFlags.WordBreak | TextFormatFlags.NoPadding);

                    lineRect.Y += measured.Height + 4;
                }
            }
        }

        private int MeasureStepHeight(Graphics g, TutorialStep step, int width)
        {
            int height = 16;

            using (var titleFont = new Font(Font.FontFamily, 9.5f, FontStyle.Bold))
            using (var bodyFont = new Font(Font.FontFamily, 9f, FontStyle.Regular))
            {
                height += TextRenderer.MeasureText(
                    g,
                    step.Title,
                    titleFont,
                    new Size(width, int.MaxValue),
                    TextFormatFlags.Left | TextFormatFlags.Top | TextFormatFlags.WordBreak | TextFormatFlags.NoPadding).Height;
                height += 8;

                foreach (string line in step.Lines.Where(line => !string.IsNullOrWhiteSpace(line)))
                {
                    height += TextRenderer.MeasureText(
                        g,
                        "- " + line,
                        bodyFont,
                        new Size(width - 12, int.MaxValue),
                        TextFormatFlags.Left | TextFormatFlags.Top | TextFormatFlags.WordBreak | TextFormatFlags.NoPadding).Height;
                    height += 4;
                }
            }

            return Math.Max(76, height + 8);
        }

        private void DrawStepHighlight(Graphics g)
        {
            switch (currentStepIndex)
            {
                case 0:
                    DrawModeHighlight(g);
                    break;
                case 1:
                    DrawExampleSelection(g);
                    break;
                case 2:
                    DrawExampleSelection(g);
                    DrawControlHighlight(g, savePdfButton);
                    break;
            }
        }

        private void DrawControlHighlight(Graphics g, Control control)
        {
            if (control == null || !control.Visible)
            {
                return;
            }

            Rectangle rect = RectangleToClient(control.RectangleToScreen(control.ClientRectangle));
            rect.Inflate(6, 6);
            DrawHighlightFrame(g, rect);
        }

        private void DrawModeHighlight(Graphics g)
        {
            if (markerRadioButton == null || boxRadioButton == null || !markerRadioButton.Visible || !boxRadioButton.Visible)
            {
                return;
            }

            Rectangle markerRect = RectangleToClient(markerRadioButton.RectangleToScreen(markerRadioButton.ClientRectangle));
            Rectangle boxRect = RectangleToClient(boxRadioButton.RectangleToScreen(boxRadioButton.ClientRectangle));
            Rectangle combined = Rectangle.Union(markerRect, boxRect);
            combined.Inflate(10, 8);
            DrawHighlightFrame(g, combined);
        }

        private void DrawExampleSelection(Graphics g)
        {
            if (pdfViewer == null || !pdfViewer.Visible)
            {
                return;
            }

            Rectangle viewerRect = RectangleToClient(pdfViewer.RectangleToScreen(pdfViewer.ClientRectangle));
            Rectangle clippedViewerRect = Rectangle.Intersect(ClientRectangle, viewerRect);
            if (clippedViewerRect.Width <= 40 || clippedViewerRect.Height <= 40)
            {
                return;
            }

            if (tutorialPreviewBitmap != null)
            {
                DrawTutorialPreview(g, clippedViewerRect);
                return;
            }

            Rectangle selectionRect = new Rectangle(
                clippedViewerRect.Left + Math.Max(24, clippedViewerRect.Width / 6),
                clippedViewerRect.Top + Math.Max(28, clippedViewerRect.Height / 5),
                Math.Max(120, clippedViewerRect.Width / 4),
                20);

            using (var fillBrush = new SolidBrush(Color.FromArgb(190, 232, 57, 53)))
            using (var borderPen = new Pen(Color.FromArgb(255, 198, 40, 40), 2f))
            {
                g.FillRectangle(fillBrush, selectionRect);
                g.DrawRectangle(borderPen, selectionRect);
            }
        }

        private void DrawTutorialPreview(Graphics g, Rectangle viewerRect)
        {
            Rectangle previewRect = new Rectangle(
                viewerRect.Left + Math.Max(16, viewerRect.Width / 12),
                viewerRect.Top + Math.Max(24, viewerRect.Height / 7) - 110,
                Math.Min(760, Math.Max(420, viewerRect.Width - 40)),
                Math.Min(290, Math.Max(180, viewerRect.Height / 2)));

            using (var panelBrush = new SolidBrush(Color.FromArgb(245, 255, 255, 255)))
            using (var shadowBrush = new SolidBrush(Color.FromArgb(40, 0, 0, 0)))
            using (var borderPen = new Pen(Color.FromArgb(220, 208, 208, 208), 1f))
            {
                Rectangle shadowRect = previewRect;
                shadowRect.Offset(4, 5);
                g.FillRectangle(shadowBrush, shadowRect);
                g.FillRectangle(panelBrush, previewRect);
                g.DrawRectangle(borderPen, previewRect);
            }

            Rectangle imageRect = previewRect;
            imageRect.Inflate(-10, -10);

            Rectangle sourceRect = new Rectangle(
                (int)Math.Round(tutorialPreviewBitmap.Width * 0.08),
                (int)Math.Round(tutorialPreviewBitmap.Height * 0.15),
                (int)Math.Round(tutorialPreviewBitmap.Width * 0.80),
                (int)Math.Round(tutorialPreviewBitmap.Height * 0.20));
            sourceRect.Intersect(new Rectangle(Point.Empty, tutorialPreviewBitmap.Size));
            if (sourceRect.Width <= 0 || sourceRect.Height <= 0)
            {
                return;
            }

            g.DrawImage(tutorialPreviewBitmap, imageRect, sourceRect, GraphicsUnit.Pixel);

            Rectangle selectionRect = new Rectangle(
                imageRect.Left + (int)Math.Round(imageRect.Width * 0.12),
                imageRect.Top + (int)Math.Round(imageRect.Height * 0.35),
                (int)Math.Round(imageRect.Width * 0.58),
                Math.Max(12, (int)Math.Round(imageRect.Height * 0.03)));

            using (var fillBrush = new SolidBrush(Color.FromArgb(190, 232, 57, 53)))
            using (var borderPen = new Pen(Color.FromArgb(255, 198, 40, 40), 2f))
            {
                g.FillRectangle(fillBrush, selectionRect);
                g.DrawRectangle(borderPen, selectionRect);
            }
        }

        private void DrawHighlightFrame(Graphics g, Rectangle rect)
        {
            Rectangle visibleRect = Rectangle.Intersect(ClientRectangle, rect);
            if (visibleRect.Width <= 0 || visibleRect.Height <= 0)
            {
                return;
            }

            if (backgroundSnapshot != null)
            {
                g.DrawImage(backgroundSnapshot, visibleRect, visibleRect, GraphicsUnit.Pixel);
            }
            else
            {
                using (var clearBrush = new SolidBrush(Color.FromArgb(90, 255, 255, 255)))
                {
                    g.FillRectangle(clearBrush, visibleRect);
                }
            }

            using (var borderPen = new Pen(Color.FromArgb(224, 188, 0), 4f))
            {
                g.DrawRectangle(borderPen, visibleRect);
            }
        }

        private static GraphicsPath BuildRoundedRectangle(Rectangle bounds, int radius)
        {
            int diameter = radius * 2;
            var path = new GraphicsPath();
            path.AddArc(bounds.Left, bounds.Top, diameter, diameter, 180, 90);
            path.AddArc(bounds.Right - diameter, bounds.Top, diameter, diameter, 270, 90);
            path.AddArc(bounds.Right - diameter, bounds.Bottom - diameter, diameter, diameter, 0, 90);
            path.AddArc(bounds.Left, bounds.Bottom - diameter, diameter, diameter, 90, 90);
            path.CloseFigure();
            return path;
        }

        private static string Tr(string key)
        {
            return Resources.ResourceManager.GetString(key) ?? key;
        }

        private static Bitmap LoadTutorialPreviewBitmap()
        {
            try
            {
                string tutorialPdfPath = Path.Combine(Application.StartupPath, "Tutorial", "samouczek-anonpdfpro_podpisany.pdf");
                if (!File.Exists(tutorialPdfPath))
                {
                    tutorialPdfPath = Path.Combine(Application.StartupPath, "tutorial", "samouczek-anonpdfpro_podpisany.pdf");
                }

                if (!File.Exists(tutorialPdfPath))
                {
                    return null;
                }

                using (var document = new PdfDocument(tutorialPdfPath))
                {
                    if (document.Pages.Count == 0)
                    {
                        return null;
                    }

                    int pageIndex = document.Pages.Count > 1 ? 1 : 0;
                    var page = document.Pages[pageIndex];
                    int width = Math.Max(1, (int)Math.Round(page.Width * 2.0));
                    int height = Math.Max(1, (int)Math.Round(page.Height * 2.0));

                    using (var pdfBitmap = new PDFiumBitmap(width, height, true))
                    {
                        pdfBitmap.FillRectangle(0, 0, width, height, 0xFFFFFFFF);
                        page.Render(renderTarget: pdfBitmap, flags: RenderingFlags.Annotations);
                        using (var stream = new MemoryStream())
                        {
                            pdfBitmap.Save(stream);
                            stream.Position = 0;
                            using (var rendered = new Bitmap(stream))
                            {
                                return new Bitmap(rendered);
                            }
                        }
                    }
                }
            }
            catch
            {
                return null;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                backgroundSnapshot?.Dispose();
                tutorialPreviewBitmap?.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
