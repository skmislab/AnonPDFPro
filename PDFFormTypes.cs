using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.IO.Compression;
using System.ComponentModel;
using System.Configuration;
using System.Reflection;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Security.Principal;
using System.Security.Cryptography;
using iText.Html2pdf;
using iText.Kernel.Pdf;
using iText.PdfCleanup;
using iText.Forms.Fields;
using iText.Forms;
using iText.Signatures;
using iText.Kernel.Colors;
using iText.Kernel.Pdf.Xobject;
using iText.Kernel.Pdf.Extgstate;
using iText.Commons.Bouncycastle.Cert;
using PDFiumSharp;
using Newtonsoft.Json;
using PDFiumSharp.Enums;
using System.Threading.Tasks;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using iText.Kernel.Pdf.Canvas.Parser.Data;
using iText.Kernel.Exceptions;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Runtime.CompilerServices;
using AnonPDF.Properties;
using DrawingImage = System.Drawing.Image;
using DrawingRectangle = System.Drawing.Rectangle;
using KernelGeom = iText.Kernel.Geom;
using System.Runtime.InteropServices;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using iText.IO.Font;
using iText.Kernel.Font;
using iText.Layout;
using iText.StyledXmlParser.Resolver.Font;
using TesseractOCR;
using System.Data.SqlClient;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;
using Windows.Graphics.Imaging;
using Windows.Media.Ocr;
using Windows.Storage;
using Windows.Storage.Streams;
using MediaGlyphTypeface = System.Windows.Media.GlyphTypeface;
using MediaDrawingVisual = System.Windows.Media.DrawingVisual;
using MediaBrushes = System.Windows.Media.Brushes;
using MediaPixelFormats = System.Windows.Media.PixelFormats;
using MediaRenderTargetBitmap = System.Windows.Media.Imaging.RenderTargetBitmap;
using MediaPngBitmapEncoder = System.Windows.Media.Imaging.PngBitmapEncoder;
using MediaBitmapFrame = System.Windows.Media.Imaging.BitmapFrame;
using WpfPoint = System.Windows.Point;


// Suppress spell-check warning for project name 'AnonPDF'
#pragma warning disable SPELL
namespace AnonPDF
{
    public enum TextLeaderAnchorKind
    {
        TopLeft,
        TopCenter,
        TopRight,
        RightCenter,
        BottomRight,
        BottomCenter,
        BottomLeft,
        LeftCenter
    }

    public class TextAnnotation
    {
        public string Id { get; set; }

        public int PageNumber { get; set; }

        public string LayerId { get; set; }

        public string AltText { get; set; }

        public string AnnotationText { get; set; }

        public Font AnnotationFont { get; set; }

        public System.Drawing.Color AnnotationColor { get; set; }

        public int AnnotationBackgroundColorArgb { get; set; }

        public int AnnotationBorderColorArgb { get; set; }

        public float AnnotationBorderWidth { get; set; }

        public float AnnotationFrameMargin { get; set; }

        public bool HasLeaderArrow { get; set; }

        public float LeaderLineWidth { get; set; }

        public TextLeaderAnchorKind LeaderAnchorKind { get; set; }

        public PointF LeaderEndPoint { get; set; }

        public float LeaderHeadLength { get; set; }

        public float LeaderHeadWidth { get; set; }

        public int LeaderFillColorArgb { get; set; }

        public int LeaderBorderColorArgb { get; set; }

        public float LeaderBorderWidth { get; set; }

        public string AnnotationContentMode { get; set; }

        public string AnnotationRichText { get; set; }

        public System.Windows.Forms.HorizontalAlignment AnnotationAlignment { get; set; }

        public int AnnotationRotation { get; set; }

        public RectangleF AnnotationBounds { get; set; }

        public bool AnnotationIsLocked { get; set; }

        public string DuplicateGroupId { get; set; }

        public DateTime CreatedAtUtc { get; set; }

        public DateTime UpdatedAtUtc { get; set; }

        [JsonIgnore]
        public string RichContentSizeCacheKey { get; set; }

        [JsonIgnore]
        public SizeF RichContentSizeCacheValue { get; set; }

        [JsonIgnore]
        public string CachedRichContentSizeKey { get; set; }

        [JsonIgnore]
        public SizeF CachedRichContentSize { get; set; }

        [JsonIgnore]
        public bool HasCachedRichContentSize { get; set; }

        [JsonIgnore]
        public string CachedRichPreviewBitmapKey { get; set; }

        [JsonIgnore]
        public Bitmap CachedRichPreviewBitmap { get; set; }

        [JsonIgnore]
        public Rectangle CachedRichPreviewSourceRect { get; set; }

        [JsonIgnore]
        public bool HasCachedRichPreviewSourceRect { get; set; }

        [JsonIgnore]
        public bool CachedRichPreviewIncludesFrame { get; set; }

        [JsonIgnore]
        public string CachedRichPreviewFrameKey { get; set; }

        [JsonIgnore]
        public RectangleF CachedRichPreviewFrameLocalPx { get; set; }

        [JsonIgnore]
        public bool HasCachedRichPreviewFrameLocalPx { get; set; }

        [JsonIgnore]
        public string CachedRichHtmlRenderKey { get; set; }

        [JsonIgnore]
        public byte[] CachedRichHtmlRenderPdfBytes { get; set; }

        [JsonIgnore]
        public RectangleF CachedRichHtmlRenderFrameRectTopDownPt { get; set; }

        [JsonIgnore]
        public float CachedRichHtmlRenderSourceWidthPt { get; set; }

        [JsonIgnore]
        public float CachedRichHtmlRenderSourceHeightPt { get; set; }

        [JsonIgnore]
        public float CachedRichHtmlRenderTargetWidthPt { get; set; }

        [JsonIgnore]
        public float CachedRichHtmlRenderTargetHeightPt { get; set; }

        [JsonIgnore]
        public float MeasuredAtDpi { get; set; }

        public TextAnnotation()
        {
            Id = Guid.NewGuid().ToString("N");
            PageNumber = 1;
            LayerId = PDFForm.DefaultLayerId;
            AnnotationText = "";
            AnnotationFont = new Font("Arial", 12);
            AnnotationColor = System.Drawing.Color.Black;
            AnnotationBackgroundColorArgb = System.Drawing.Color.Transparent.ToArgb();
            AnnotationBorderColorArgb = System.Drawing.Color.Black.ToArgb();
            AnnotationBorderWidth = 0f;
            AnnotationFrameMargin = 0f;
            HasLeaderArrow = false;
            LeaderLineWidth = 1.5f;
            LeaderAnchorKind = TextLeaderAnchorKind.RightCenter;
            LeaderEndPoint = PointF.Empty;
            LeaderHeadLength = PDFForm.DefaultArrowHeadLength;
            LeaderHeadWidth = PDFForm.DefaultArrowHeadWidth;
            LeaderFillColorArgb = System.Drawing.Color.Transparent.ToArgb();
            LeaderBorderColorArgb = System.Drawing.Color.Transparent.ToArgb();
            LeaderBorderWidth = 0f;
            AnnotationContentMode = "plain";
            AnnotationRichText = null;
            AnnotationAlignment = System.Windows.Forms.HorizontalAlignment.Left; // Default left alignment
            AnnotationRotation = 0;
            AnnotationBounds = new RectangleF(0, 0, 100, 30); // Example rectangular area
            AnnotationIsLocked = false;
            DuplicateGroupId = null;
            CreatedAtUtc = DateTime.MinValue;
            UpdatedAtUtc = DateTime.MinValue;
        }


        public TextAnnotation(int pageNumber, string text, Font font, System.Drawing.Color color, System.Windows.Forms.HorizontalAlignment alignment, RectangleF bounds, bool isLocked = false)
        {
            Id = Guid.NewGuid().ToString("N");
            PageNumber = pageNumber;
            LayerId = PDFForm.DefaultLayerId;
            AnnotationText = text;
            AnnotationFont = font;
            AnnotationColor = color;
            AnnotationBackgroundColorArgb = System.Drawing.Color.Transparent.ToArgb();
            AnnotationBorderColorArgb = System.Drawing.Color.Black.ToArgb();
            AnnotationBorderWidth = 0f;
            AnnotationFrameMargin = 0f;
            HasLeaderArrow = false;
            LeaderLineWidth = 1.5f;
            LeaderAnchorKind = TextLeaderAnchorKind.RightCenter;
            LeaderEndPoint = PointF.Empty;
            LeaderHeadLength = PDFForm.DefaultArrowHeadLength;
            LeaderHeadWidth = PDFForm.DefaultArrowHeadWidth;
            LeaderFillColorArgb = System.Drawing.Color.Transparent.ToArgb();
            LeaderBorderColorArgb = System.Drawing.Color.Transparent.ToArgb();
            LeaderBorderWidth = 0f;
            AnnotationContentMode = "plain";
            AnnotationRichText = null;
            AnnotationAlignment = alignment;
            AnnotationRotation = 0;
            AnnotationBounds = bounds;
            AnnotationIsLocked = isLocked;
            DuplicateGroupId = null;
            CreatedAtUtc = DateTime.MinValue;
            UpdatedAtUtc = DateTime.MinValue;
        }

        public override string ToString()
        {
            // Optional, facilitates debugging and displaying annotation information.
            return FormatResource(
                "TextAnnotation_ToStringFormat",
                AnnotationText,
                AnnotationFont.FontFamily.Name,
                AnnotationFont.Size,
                AnnotationColor.Name,
                AnnotationAlignment,
                AnnotationRotation,
                AnnotationIsLocked);
        }

        private static string FormatResource(string key, params object[] args)
        {
            string format = GetResourceText(key);
            return string.Format(format, args);
        }

        private static string GetResourceText(string key)
        {
            var culture = Resources.Culture ?? CultureInfo.CurrentUICulture;
            string value = Resources.ResourceManager.GetString(key, culture);
            return string.IsNullOrWhiteSpace(value) ? key : value;
        }
    }

    public class EditTextDialog : Form
    {
        private Label lblText;
        private RichTextBox txtText;
        private CheckBox chkRichTextMode;
        private FlowLayoutPanel richTextToolbarPanel;
        private Button btnBold;
        private Button btnItalic;
        private Button btnUnderline;
        private Label lblRichTextColor;
        private Button btnRichTextColor;
        private Button btnFont;
        private Label lblTextColor;
        private Button btnColor;
        private Label lblBackgroundColor;
        private Button btnBackgroundColor;
        private CheckBox chkNoBackgroundColor;
        private Label lblBorderColor;
        private Button btnBorderColor;
        private Label lblBorderWidth;
        private NumericUpDown nudBorderWidth;
        private Label lblFrameMargin;
        private NumericUpDown nudFrameMargin;
        private CheckBox chkLeaderArrow;
        private Label lblLeaderLineWidth;
        private NumericUpDown nudLeaderLineWidth;
        private Label lblLeaderHeadLength;
        private NumericUpDown nudLeaderHeadLength;
        private Label lblLeaderHeadWidth;
        private NumericUpDown nudLeaderHeadWidth;
        private Label lblLeaderFillColor;
        private Button btnLeaderFillColor;
        private Label lblLeaderBorderColor;
        private Button btnLeaderBorderColor;
        private Label lblLeaderBorderWidth;
        private NumericUpDown nudLeaderBorderWidth;
        private Label lblFontDisplay;
        private GroupBox groupBoxAlignment;
        private RadioButton rbLeft;
        private RadioButton rbCenter;
        private RadioButton rbRight;
        private GroupBox groupBoxRotation;
        private Label lblRotation;
        private NumericUpDown nudRotation;
        private FlowLayoutPanel rotationPresetPanel;
        private GroupBox groupBoxSymbols;
        private FlowLayoutPanel symbolsPanel;
        private Button btnRestoreDefaults;
        private Button btnOK;
        private Button btnCancel;
        private System.Drawing.Color lastBackgroundColorBeforeTransparent = System.Drawing.Color.White;
        private DialogTheme dialogTheme;

        // Properties that allow reading values set by the user
        public string AnnotationText { get; set; }
        public Font AnnotationFont { get; set; }
        public System.Drawing.Color AnnotationColor { get; set; }
        public System.Drawing.Color AnnotationBackgroundColor { get; set; }
        public System.Drawing.Color AnnotationBorderColor { get; set; }
        public float AnnotationBorderWidth { get; set; }
        public float AnnotationFrameMargin { get; set; }
        public bool HasLeaderArrow { get; set; }
        public float LeaderLineWidth { get; set; }
        public float LeaderHeadLength { get; set; }
        public float LeaderHeadWidth { get; set; }
        public System.Drawing.Color LeaderFillColor { get; set; }
        public System.Drawing.Color LeaderBorderColor { get; set; }
        public float LeaderBorderWidth { get; set; }
        public bool IsRichTextMode { get; set; }
        public string AnnotationRichText { get; set; }
        public System.Windows.Forms.HorizontalAlignment AnnotationAlignment { get; set; }
        public int AnnotationRotation { get; set; }
        public Action ApplyChanges { get; set; }
        public Action<EditTextDialog> RestoreDefaultsAction { get; set; }
        private bool suppressAutoApply;
        private bool suppressEditorPresentationRefresh;
        private Font editorDisplayFont;
        private readonly Timer liveApplyTimer;
        private const int LiveApplyDelayMs = 180;
        private const float EditorDisplayFontSize = 12f;

        public EditTextDialog()
        {
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;

            // Set default values if nothing was set previously
            if (AnnotationText == null) AnnotationText = "";
            if (AnnotationFont == null) AnnotationFont = new Font("Arial", 12);
            if (AnnotationColor == System.Drawing.Color.Empty) AnnotationColor = System.Drawing.Color.Black;
            if (AnnotationBackgroundColor == System.Drawing.Color.Empty) AnnotationBackgroundColor = System.Drawing.Color.Transparent;
            if (AnnotationBorderColor == System.Drawing.Color.Empty) AnnotationBorderColor = System.Drawing.Color.Black;
            if (LeaderFillColor == System.Drawing.Color.Empty) LeaderFillColor = System.Drawing.Color.Transparent;
            if (LeaderBorderColor == System.Drawing.Color.Empty) LeaderBorderColor = System.Drawing.Color.Transparent;
            if (AnnotationRichText == null) AnnotationRichText = string.Empty;
            if (AnnotationBackgroundColor.A > 0)
            {
                lastBackgroundColorBeforeTransparent = AnnotationBackgroundColor;
            }
            LeaderLineWidth = PDFForm.NormalizeLeaderLineWidth(LeaderLineWidth <= 0f ? 1.5f : LeaderLineWidth);
            LeaderHeadLength = PDFForm.NormalizeLeaderHeadLength(LeaderHeadLength <= 0f ? PDFForm.DefaultArrowHeadLength : LeaderHeadLength);
            LeaderHeadWidth = PDFForm.NormalizeLeaderHeadWidth(LeaderHeadWidth <= 0f ? PDFForm.DefaultArrowHeadWidth : LeaderHeadWidth);
            AnnotationRotation = NormalizeAngle(AnnotationRotation);
            liveApplyTimer = new Timer { Interval = LiveApplyDelayMs };
            liveApplyTimer.Tick += (_, __) =>
            {
                liveApplyTimer.Stop();
                TryApplyChanges();
            };
            this.FormClosing += (_, __) => liveApplyTimer.Stop();
            this.FormClosed += (_, __) =>
            {
                editorDisplayFont?.Dispose();
                editorDisplayFont = null;
            };

            InitializeComponents();
        }

        private void InitializeComponents()
        {
            this.Text = Resources.EditText_Title;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterParent;
            this.ClientSize = PDFForm.ScaleSizeForDpiStatic(430, 760);
            this.MaximizeBox = false;
                        this.MinimizeBox = false;
            this.AutoScroll = true;
            this.AutoScrollMargin = new Size(0, PDFForm.ScaleForDpiStatic(16));
            int maxH = Screen.GetWorkingArea(this).Height - 80;
            if (this.Height > maxH) this.Height = maxH;

            // Label: "Enter text:"
            lblText = new Label
            {
                Text = Resources.EditText_LabelText,
                AutoSize = true,
                Location = new Point(PDFForm.ScaleForDpiStatic(10), PDFForm.ScaleForDpiStatic(10))
            };

            // TextBox - multiline for entering content
            txtText = new RichTextBox
            {
                Multiline = true,
                ScrollBars = RichTextBoxScrollBars.Both,
                Location = new Point(PDFForm.ScaleForDpiStatic(10), PDFForm.ScaleForDpiStatic(30)),
                Size = new Size(PDFForm.ScaleForDpiStatic(400), PDFForm.ScaleForDpiStatic(100)),
                WordWrap = false,
                Font = new Font("Segoe UI", 12f, FontStyle.Regular, GraphicsUnit.Point)
            };
            txtText.ZoomFactor = 1f;
            txtText.TextChanged += TxtText_TextChanged;

            chkRichTextMode = new CheckBox
            {
                Text = GetRichModeLabelText(),
                AutoSize = true,
                Location = new Point(PDFForm.ScaleForDpiStatic(10), PDFForm.ScaleForDpiStatic(138))
            };
            chkRichTextMode.CheckedChanged += RichModeCheckedChanged;

            richTextToolbarPanel = new FlowLayoutPanel
            {
                Location = new Point(PDFForm.ScaleForDpiStatic(10), PDFForm.ScaleForDpiStatic(160)),
                Size = new Size(PDFForm.ScaleForDpiStatic(400), PDFForm.ScaleForDpiStatic(32)),
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                Visible = false
            };

            btnBold = new Button
            {
                Text = "B",
                Size = new Size(PDFForm.ScaleForDpiStatic(32), PDFForm.ScaleForDpiStatic(28)),
                Font = new Font(Font, FontStyle.Bold),
                TabStop = false
            };
            btnBold.Click += (_, __) => ToggleRichSelectionFontStyle(FontStyle.Bold);

            btnItalic = new Button
            {
                Text = "I",
                Size = new Size(PDFForm.ScaleForDpiStatic(32), PDFForm.ScaleForDpiStatic(28)),
                Font = new Font(Font, FontStyle.Italic),
                TabStop = false
            };
            btnItalic.Click += (_, __) => ToggleRichSelectionFontStyle(FontStyle.Italic);

            btnUnderline = new Button
            {
                Text = "U",
                Size = new Size(PDFForm.ScaleForDpiStatic(32), PDFForm.ScaleForDpiStatic(28)),
                Font = new Font(Font, FontStyle.Underline),
                TabStop = false
            };
            btnUnderline.Click += (_, __) => ToggleRichSelectionFontStyle(FontStyle.Underline);

            btnRichTextColor = new Button
            {
                Text = GetChooseColorButtonText(),
                Size = new Size(PDFForm.ScaleForDpiStatic(90), PDFForm.ScaleForDpiStatic(28)),
                TabStop = false
            };
            btnRichTextColor.Click += BtnRichTextColor_Click;

            richTextToolbarPanel.Controls.Add(btnBold);
            richTextToolbarPanel.Controls.Add(btnItalic);
            richTextToolbarPanel.Controls.Add(btnUnderline);
            lblRichTextColor = new Label
            {
                Text = GetRichSelectionColorLabelText(),
                AutoSize = true,
                Margin = new Padding(8, 7, 0, 0)
            };
            richTextToolbarPanel.Controls.Add(lblRichTextColor);
            richTextToolbarPanel.Controls.Add(btnRichTextColor);

            // Font picker button
            btnFont = new Button
            {
                Text = Resources.EditText_ButtonFont,
                Location = new Point(PDFForm.ScaleForDpiStatic(10), PDFForm.ScaleForDpiStatic(198)),
                Size = new Size(PDFForm.ScaleForDpiStatic(100), PDFForm.ScaleForDpiStatic(30))
            };
            btnFont.Click += BtnFont_Click;

            // Label showing the current font selection
            lblFontDisplay = new Label
            {
                Text = FormatResource("EditText_FontDisplay", AnnotationFont.FontFamily.Name, AnnotationFont.Size),
                AutoSize = true,
                Location = new Point(PDFForm.ScaleForDpiStatic(120), PDFForm.ScaleForDpiStatic(205))
            };

            // Button to choose color
            lblTextColor = new Label
            {
                Text = GetTextColorLabelText(),
                AutoSize = true,
                Location = new Point(PDFForm.ScaleForDpiStatic(10), PDFForm.ScaleForDpiStatic(237))
            };

            btnColor = new Button
            {
                Text = Resources.EditText_ButtonColor,
                Location = new Point(PDFForm.ScaleForDpiStatic(145), PDFForm.ScaleForDpiStatic(230)),
                Size = new Size(PDFForm.ScaleForDpiStatic(120), PDFForm.ScaleForDpiStatic(30))
            };
            btnColor.Click += BtnColor_Click;

            lblBackgroundColor = new Label
            {
                Text = GetBackgroundColorLabelText(),
                AutoSize = true,
                Location = new Point(PDFForm.ScaleForDpiStatic(10), PDFForm.ScaleForDpiStatic(271))
            };

            btnBackgroundColor = new Button
            {
                Text = GetChooseColorButtonText(),
                Location = new Point(PDFForm.ScaleForDpiStatic(145), PDFForm.ScaleForDpiStatic(264)),
                Size = new Size(PDFForm.ScaleForDpiStatic(120), PDFForm.ScaleForDpiStatic(30))
            };
            btnBackgroundColor.Click += BtnBackgroundColor_Click;

            chkNoBackgroundColor = new CheckBox
            {
                Text = GetNoBackgroundLabelText(),
                AutoSize = true,
                Location = new Point(PDFForm.ScaleForDpiStatic(276), PDFForm.ScaleForDpiStatic(271))
            };
            chkNoBackgroundColor.CheckedChanged += NoBackgroundColorCheckedChanged;

            lblBorderColor = new Label
            {
                Text = GetResourceText("EditText_LabelBorderColor"),
                AutoSize = true,
                Location = new Point(PDFForm.ScaleForDpiStatic(10), PDFForm.ScaleForDpiStatic(305))
            };

            btnBorderColor = new Button
            {
                Text = GetChooseColorButtonText(),
                Location = new Point(PDFForm.ScaleForDpiStatic(145), PDFForm.ScaleForDpiStatic(298)),
                Size = new Size(PDFForm.ScaleForDpiStatic(120), PDFForm.ScaleForDpiStatic(30))
            };
            btnBorderColor.Click += BtnBorderColor_Click;

            lblBorderWidth = new Label
            {
                Text = GetResourceText("EditText_LabelBorderWidth"),
                AutoSize = true,
                Location = new Point(PDFForm.ScaleForDpiStatic(10), PDFForm.ScaleForDpiStatic(339))
            };
            nudBorderWidth = new NumericUpDown
            {
                Location = new Point(PDFForm.ScaleForDpiStatic(145), PDFForm.ScaleForDpiStatic(336)),
                Size = new Size(PDFForm.ScaleForDpiStatic(80), PDFForm.ScaleForDpiStatic(22)),
                DecimalPlaces = 3,
                Minimum = 0,
                Maximum = 24,
                Increment = 0.1m
            };
            nudBorderWidth.ValueChanged += (_, __) => TryApplyChanges();

            lblFrameMargin = new Label
            {
                Text = GetResourceText("EditText_LabelFrameMargin"),
                AutoSize = true,
                Location = new Point(PDFForm.ScaleForDpiStatic(235), PDFForm.ScaleForDpiStatic(339))
            };
            nudFrameMargin = new NumericUpDown
            {
                Location = new Point(PDFForm.ScaleForDpiStatic(330), PDFForm.ScaleForDpiStatic(336)),
                Size = new Size(PDFForm.ScaleForDpiStatic(80), PDFForm.ScaleForDpiStatic(22)),
                DecimalPlaces = 1,
                Minimum = 0,
                Maximum = 120,
                Increment = 1m
            };
            nudFrameMargin.ValueChanged += (_, __) => TryApplyChanges();

            chkLeaderArrow = new CheckBox
            {
                Text = GetResourceText("EditText_CheckLeaderArrow"),
                AutoSize = true,
                Location = new Point(PDFForm.ScaleForDpiStatic(10), PDFForm.ScaleForDpiStatic(370))
            };
            chkLeaderArrow.CheckedChanged += (_, __) =>
            {
                UpdateLeaderControlsState();
                TryApplyChanges();
            };

            lblLeaderLineWidth = new Label
            {
                Text = GetResourceText("EditText_LabelLeaderLineWidth"),
                AutoSize = true,
                Location = new Point(PDFForm.ScaleForDpiStatic(10), PDFForm.ScaleForDpiStatic(402))
            };
            nudLeaderLineWidth = new NumericUpDown
            {
                Location = new Point(PDFForm.ScaleForDpiStatic(145), PDFForm.ScaleForDpiStatic(399)),
                Size = new Size(PDFForm.ScaleForDpiStatic(80), PDFForm.ScaleForDpiStatic(22)),
                DecimalPlaces = 3,
                Minimum = 0.5m,
                Maximum = 24,
                Increment = 0.1m
            };
            nudLeaderLineWidth.ValueChanged += (_, __) => TryApplyChanges();

            lblLeaderHeadLength = new Label
            {
                Text = GetResourceText("EditText_LabelLeaderHeadLength"),
                AutoSize = true,
                Location = new Point(PDFForm.ScaleForDpiStatic(235), PDFForm.ScaleForDpiStatic(430))
            };
            nudLeaderHeadLength = new NumericUpDown
            {
                Location = new Point(PDFForm.ScaleForDpiStatic(330), PDFForm.ScaleForDpiStatic(427)),
                Size = new Size(PDFForm.ScaleForDpiStatic(80), PDFForm.ScaleForDpiStatic(22)),
                DecimalPlaces = 3,
                Minimum = 4,
                Maximum = 120,
                Increment = 0.1m
            };
            nudLeaderHeadLength.ValueChanged += (_, __) => TryApplyChanges();

            lblLeaderHeadWidth = new Label
            {
                Text = GetResourceText("EditText_LabelLeaderHeadWidth"),
                AutoSize = true,
                Location = new Point(PDFForm.ScaleForDpiStatic(10), PDFForm.ScaleForDpiStatic(430))
            };
            nudLeaderHeadWidth = new NumericUpDown
            {
                Location = new Point(PDFForm.ScaleForDpiStatic(145), PDFForm.ScaleForDpiStatic(427)),
                Size = new Size(PDFForm.ScaleForDpiStatic(80), PDFForm.ScaleForDpiStatic(22)),
                DecimalPlaces = 3,
                Minimum = 4,
                Maximum = 120,
                Increment = 0.1m
            };
            nudLeaderHeadWidth.ValueChanged += (_, __) => TryApplyChanges();

            lblLeaderFillColor = new Label
            {
                Text = GetResourceText("EditText_LabelLeaderFillColor"),
                AutoSize = true,
                Location = new Point(PDFForm.ScaleForDpiStatic(10), PDFForm.ScaleForDpiStatic(458))
            };
            btnLeaderFillColor = new Button
            {
                Text = GetChooseColorButtonText(),
                Location = new Point(PDFForm.ScaleForDpiStatic(145), PDFForm.ScaleForDpiStatic(454)),
                Size = new Size(PDFForm.ScaleForDpiStatic(80), PDFForm.ScaleForDpiStatic(28)),
                UseVisualStyleBackColor = false
            };
            btnLeaderFillColor.Click += BtnLeaderFillColor_Click;

            lblLeaderBorderColor = new Label
            {
                Text = GetResourceText("EditText_LabelLeaderBorderColor"),
                AutoSize = true,
                Location = new Point(PDFForm.ScaleForDpiStatic(235), PDFForm.ScaleForDpiStatic(458))
            };
            btnLeaderBorderColor = new Button
            {
                Text = GetChooseColorButtonText(),
                Location = new Point(PDFForm.ScaleForDpiStatic(330), PDFForm.ScaleForDpiStatic(454)),
                Size = new Size(PDFForm.ScaleForDpiStatic(80), PDFForm.ScaleForDpiStatic(28)),
                UseVisualStyleBackColor = false
            };
            btnLeaderBorderColor.Click += BtnLeaderBorderColor_Click;

            lblLeaderBorderWidth = new Label
            {
                Text = GetResourceText("EditText_LabelLeaderBorderWidth"),
                AutoSize = true,
                Location = new Point(PDFForm.ScaleForDpiStatic(235), PDFForm.ScaleForDpiStatic(402))
            };
            nudLeaderBorderWidth = new NumericUpDown
            {
                Location = new Point(PDFForm.ScaleForDpiStatic(330), PDFForm.ScaleForDpiStatic(399)),
                Size = new Size(PDFForm.ScaleForDpiStatic(80), PDFForm.ScaleForDpiStatic(22)),
                DecimalPlaces = 3,
                Minimum = 0,
                Maximum = 24,
                Increment = 0.1m
            };
            nudLeaderBorderWidth.ValueChanged += (_, __) => TryApplyChanges();

            // GroupBox for alignment selection
            groupBoxAlignment = new GroupBox
            {
                Text = Resources.EditText_GroupAlignment,
                Location = new Point(PDFForm.ScaleForDpiStatic(10), PDFForm.ScaleForDpiStatic(492)),
                Size = new Size(PDFForm.ScaleForDpiStatic(400), PDFForm.ScaleForDpiStatic(50))
            };

            // RadioButton for left alignment
            rbLeft = new RadioButton
            {
                Text = Resources.EditText_AlignLeft,
                Location = new Point(PDFForm.ScaleForDpiStatic(10), PDFForm.ScaleForDpiStatic(20)),
                AutoSize = true,
                Checked = true
            };

            // RadioButton for center alignment
            rbCenter = new RadioButton
            {
                Text = Resources.EditText_AlignCenter,
                Location = new Point(PDFForm.ScaleForDpiStatic(150), PDFForm.ScaleForDpiStatic(20)),
                AutoSize = true
            };

            // RadioButton for right alignment
            rbRight = new RadioButton
            {
                Text = Resources.EditText_AlignRight,
                Location = new Point(PDFForm.ScaleForDpiStatic(290), PDFForm.ScaleForDpiStatic(20)),
                AutoSize = true
            };

            groupBoxAlignment.Controls.Add(rbLeft);
            groupBoxAlignment.Controls.Add(rbCenter);
            groupBoxAlignment.Controls.Add(rbRight);

            rbLeft.CheckedChanged += RadioButton_CheckedChanged;
            rbCenter.CheckedChanged += RadioButton_CheckedChanged;
            rbRight.CheckedChanged += RadioButton_CheckedChanged;

            // GroupBox for rotation selection
            groupBoxRotation = new GroupBox
            {
                Text = Resources.EditText_GroupRotation,
                Location = new Point(PDFForm.ScaleForDpiStatic(10), PDFForm.ScaleForDpiStatic(552)),
                Size = new Size(PDFForm.ScaleForDpiStatic(400), PDFForm.ScaleForDpiStatic(55))
            };

            lblRotation = new Label
            {
                Text = Resources.EditText_RotationLabel,
                Location = new Point(PDFForm.ScaleForDpiStatic(10), PDFForm.ScaleForDpiStatic(22)),
                AutoSize = true
            };

            nudRotation = new NumericUpDown
            {
                Minimum = 0,
                Maximum = 359,
                Increment = 1,
                Value = NormalizeAngle(AnnotationRotation),
                Location = new Point(PDFForm.ScaleForDpiStatic(90), PDFForm.ScaleForDpiStatic(18)),
                Size = new Size(PDFForm.ScaleForDpiStatic(70), PDFForm.ScaleForDpiStatic(22))
            };
            nudRotation.ValueChanged += RotationValueChanged;

            rotationPresetPanel = new FlowLayoutPanel
            {
                Location = new Point(PDFForm.ScaleForDpiStatic(170), PDFForm.ScaleForDpiStatic(18)),
                Size = new Size(PDFForm.ScaleForDpiStatic(220), PDFForm.ScaleForDpiStatic(28)),
                AutoSize = false,
                WrapContents = false,
                FlowDirection = FlowDirection.LeftToRight,
                Margin = new Padding(0)
            };

            int[] presets = { 0, 30, 45, 90, 180, 270 };
            foreach (int preset in presets)
            {
                Button presetButton = new Button
                {
                    Text = preset.ToString(CultureInfo.InvariantCulture),
                    Tag = preset,
                    Size = new Size(PDFForm.ScaleForDpiStatic(34), PDFForm.ScaleForDpiStatic(24)),
                    Margin = new Padding(2, 0, 0, 0),
                    TabStop = false
                };
                presetButton.Click += RotationPresetButton_Click;
                rotationPresetPanel.Controls.Add(presetButton);
            }

            groupBoxRotation.Controls.Add(lblRotation);
            groupBoxRotation.Controls.Add(nudRotation);
            groupBoxRotation.Controls.Add(rotationPresetPanel);

            // GroupBox for symbol gallery
            groupBoxSymbols = new GroupBox
            {
                Text = Resources.EditText_GroupSymbols,
                Location = new Point(PDFForm.ScaleForDpiStatic(10), PDFForm.ScaleForDpiStatic(622)),
                Size = new Size(PDFForm.ScaleForDpiStatic(400), PDFForm.ScaleForDpiStatic(65))
            };

            symbolsPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoSize = false,
                WrapContents = false,
                FlowDirection = FlowDirection.LeftToRight,
                Padding = new Padding(6, 5, 6, 5)
            };

            string[] symbols =
            {
                "\u2500", // ─
                "\u00B0", // °
                "\u00B2", // ²
                "\u0142", // ł
                "\u00A7", // §
                "\u2022", // •
                "\u2713", // ✓
                "\u2717", // ✗
                "\u2192", // →
                "\u00B1"  // ±
            };
            foreach (string symbol in symbols)
            {
                Button btnSymbol = new Button
                {
                    Text = symbol,
                    Size = new Size(PDFForm.ScaleForDpiStatic(32), PDFForm.ScaleForDpiStatic(28)),
                    Margin = new Padding(4, 0, 0, 0),
                    TabStop = false
                };
                btnSymbol.Click += SymbolButton_Click;
                symbolsPanel.Controls.Add(btnSymbol);
            }
            groupBoxSymbols.Controls.Add(symbolsPanel);

            // OK and Cancel buttons
            btnOK = new Button
            {
                Text = Resources.Merge_OK,
                Location = new Point(PDFForm.ScaleForDpiStatic(240), PDFForm.ScaleForDpiStatic(708)),
                Size = new Size(PDFForm.ScaleForDpiStatic(80), PDFForm.ScaleForDpiStatic(30)),
                DialogResult = DialogResult.OK
            };
            btnOK.Click += BtnOK_Click;

            btnRestoreDefaults = new Button
            {
                Text = GetResourceText("UI_Button_RestoreSettings"),
                Location = new Point(PDFForm.ScaleForDpiStatic(90), PDFForm.ScaleForDpiStatic(708)),
                Size = new Size(PDFForm.ScaleForDpiStatic(140), PDFForm.ScaleForDpiStatic(30))
            };
            btnRestoreDefaults.Click += BtnRestoreDefaults_Click;

            btnCancel = new Button
            {
                Text = Resources.Merge_Cancel,
                Location = new Point(PDFForm.ScaleForDpiStatic(330), PDFForm.ScaleForDpiStatic(708)),
                Size = new Size(PDFForm.ScaleForDpiStatic(80), PDFForm.ScaleForDpiStatic(30)),
                DialogResult = DialogResult.Cancel
            };

            // Add controls to the form
            this.Controls.Add(lblText);
            this.Controls.Add(txtText);
            this.Controls.Add(chkRichTextMode);
            this.Controls.Add(richTextToolbarPanel);
            this.Controls.Add(btnFont);
            this.Controls.Add(lblFontDisplay);
            this.Controls.Add(lblTextColor);
            this.Controls.Add(btnColor);
            this.Controls.Add(lblBackgroundColor);
            this.Controls.Add(btnBackgroundColor);
            this.Controls.Add(chkNoBackgroundColor);
            this.Controls.Add(lblBorderColor);
            this.Controls.Add(btnBorderColor);
            this.Controls.Add(lblBorderWidth);
            this.Controls.Add(nudBorderWidth);
            this.Controls.Add(lblFrameMargin);
            this.Controls.Add(nudFrameMargin);
            this.Controls.Add(chkLeaderArrow);
            this.Controls.Add(lblLeaderLineWidth);
            this.Controls.Add(nudLeaderLineWidth);
            this.Controls.Add(lblLeaderHeadLength);
            this.Controls.Add(nudLeaderHeadLength);
            this.Controls.Add(lblLeaderHeadWidth);
            this.Controls.Add(nudLeaderHeadWidth);
            this.Controls.Add(lblLeaderFillColor);
            this.Controls.Add(btnLeaderFillColor);
            this.Controls.Add(lblLeaderBorderColor);
            this.Controls.Add(btnLeaderBorderColor);
            this.Controls.Add(lblLeaderBorderWidth);
            this.Controls.Add(nudLeaderBorderWidth);
            this.Controls.Add(groupBoxAlignment);
            this.Controls.Add(groupBoxRotation);
            this.Controls.Add(groupBoxSymbols);
            this.Controls.Add(btnRestoreDefaults);
            this.Controls.Add(btnOK);
            this.Controls.Add(btnCancel);


            this.CancelButton = btnCancel;
            this.AcceptButton = null;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            bool previousSuppressAutoApply = suppressAutoApply;
            bool previousSuppressPresentationRefresh = suppressEditorPresentationRefresh;
            try
            {
                suppressAutoApply = true;
                suppressEditorPresentationRefresh = true;
                ApplyPropertiesToControls();
            }
            finally
            {
                suppressAutoApply = previousSuppressAutoApply;
                suppressEditorPresentationRefresh = previousSuppressPresentationRefresh;
            }

            ApplyEditorDisplayFormatting(normalizeRichContent: IsRichTextMode);
            if (dialogTheme != null)
            {
                ApplyDialogTheme(dialogTheme);
            }
        }

        internal void ApplyDialogTheme(DialogTheme theme)
        {
            dialogTheme = theme;
            DialogThemeApplier.ApplyTo(this, theme, txtText, btnRichTextColor, btnColor, btnBackgroundColor, btnBorderColor);
            UpdateColorControls();
        }

        private void BtnRestoreDefaults_Click(object sender, EventArgs e)
        {
            if (RestoreDefaultsAction == null)
            {
                return;
            }

            string preservedText = txtText.Text ?? string.Empty;
            RestoreDefaultsAction(this);
            AnnotationText = preservedText;
            AnnotationRichText = null;
            bool previousSuppressAutoApply = suppressAutoApply;
            bool previousSuppressPresentationRefresh = suppressEditorPresentationRefresh;
            try
            {
                suppressAutoApply = true;
                suppressEditorPresentationRefresh = true;
                ApplyPropertiesToControls();
            }
            finally
            {
                suppressAutoApply = previousSuppressAutoApply;
                suppressEditorPresentationRefresh = previousSuppressPresentationRefresh;
            }
            ApplyEditorDisplayFormatting(normalizeRichContent: IsRichTextMode);
            TryApplyChanges();
        }

        private void ApplyPropertiesToControls()
        {
            UpdateFontDisplay();
            bool richLoaded = false;
            if (IsRichTextMode && !string.IsNullOrWhiteSpace(AnnotationRichText))
            {
                try
                {
                    txtText.Rtf = AnnotationRichText;
                    richLoaded = true;
                }
                catch
                {
                    richLoaded = false;
                }
            }

            if (!richLoaded)
            {
                txtText.Text = AnnotationText ?? string.Empty;
            }

            switch (AnnotationAlignment)
            {
                case System.Windows.Forms.HorizontalAlignment.Center:
                    rbCenter.Checked = true;
                    break;
                case System.Windows.Forms.HorizontalAlignment.Right:
                    rbRight.Checked = true;
                    break;
                default:
                    rbLeft.Checked = true;
                    break;
            }

            nudRotation.Value = NormalizeAngle(AnnotationRotation);
            nudBorderWidth.Value = (decimal)NormalizeAnnotationBorderWidth(AnnotationBorderWidth);
            nudFrameMargin.Value = (decimal)NormalizeAnnotationFrameMargin(AnnotationFrameMargin);
            if (chkLeaderArrow != null)
            {
                chkLeaderArrow.Checked = HasLeaderArrow;
            }
            if (nudLeaderLineWidth != null)
            {
                nudLeaderLineWidth.Value = (decimal)PDFForm.NormalizeLeaderLineWidth(LeaderLineWidth);
            }
            if (nudLeaderHeadLength != null)
            {
                nudLeaderHeadLength.Value = (decimal)PDFForm.NormalizeLeaderHeadLength(LeaderHeadLength);
            }
            if (nudLeaderHeadWidth != null)
            {
                nudLeaderHeadWidth.Value = (decimal)PDFForm.NormalizeLeaderHeadWidth(LeaderHeadWidth);
            }
            if (nudLeaderBorderWidth != null)
            {
                nudLeaderBorderWidth.Value = (decimal)NormalizeLeaderBorderWidth(LeaderBorderWidth);
            }
            if (btnLeaderFillColor != null)
            {
                LeaderFillColor = LeaderFillColor.IsEmpty ? System.Drawing.Color.Transparent : LeaderFillColor;
            }
            if (btnLeaderBorderColor != null)
            {
                LeaderBorderColor = LeaderBorderColor.IsEmpty ? System.Drawing.Color.Transparent : LeaderBorderColor;
            }
            if (chkNoBackgroundColor != null)
            {
                chkNoBackgroundColor.Checked = AnnotationBackgroundColor.A <= 0;
            }
            chkRichTextMode.Checked = IsRichTextMode;
            if (IsRichTextMode && !string.IsNullOrWhiteSpace(AnnotationRichText))
            {
                try
                {
                    txtText.Rtf = AnnotationRichText;
                }
                catch
                {
                    txtText.Text = AnnotationText ?? string.Empty;
                }
            }
            SetRichMode(IsRichTextMode, updateState: false, reloadEditorContent: false, applyFormatting: false);
            ApplyAlignmentToEditor();
            UpdateLeaderControlsState();
            UpdateColorControls();
        }

        private void UpdateLeaderControlsState()
        {
            bool enabled = chkLeaderArrow != null && chkLeaderArrow.Checked;
            if (lblLeaderLineWidth != null)
            {
                lblLeaderLineWidth.Enabled = enabled;
            }
            if (nudLeaderLineWidth != null)
            {
                nudLeaderLineWidth.Enabled = enabled;
            }
            if (lblLeaderHeadLength != null)
            {
                lblLeaderHeadLength.Enabled = enabled;
            }
            if (nudLeaderHeadLength != null)
            {
                nudLeaderHeadLength.Enabled = enabled;
            }
            if (lblLeaderHeadWidth != null)
            {
                lblLeaderHeadWidth.Enabled = enabled;
            }
            if (nudLeaderHeadWidth != null)
            {
                nudLeaderHeadWidth.Enabled = enabled;
            }
            if (lblLeaderFillColor != null)
            {
                lblLeaderFillColor.Enabled = enabled;
            }
            if (btnLeaderFillColor != null)
            {
                btnLeaderFillColor.Enabled = enabled;
            }
            if (lblLeaderBorderColor != null)
            {
                lblLeaderBorderColor.Enabled = enabled;
            }
            if (btnLeaderBorderColor != null)
            {
                btnLeaderBorderColor.Enabled = enabled;
            }
            if (lblLeaderBorderWidth != null)
            {
                lblLeaderBorderWidth.Enabled = enabled;
            }
            if (nudLeaderBorderWidth != null)
            {
                nudLeaderBorderWidth.Enabled = enabled;
            }
        }

        private static System.Drawing.Color GetContrastingTextColor(System.Drawing.Color color)
        {
            int luminance = (int)((0.299 * color.R) + (0.587 * color.G) + (0.114 * color.B));
            return luminance >= 140 ? System.Drawing.Color.Black : System.Drawing.Color.White;
        }

        private string GetTextColorLabelText()
        {
            return GetResourceText("EditText_LabelTextColor");
        }

        private string GetBackgroundColorLabelText()
        {
            return GetResourceText("EditText_LabelBackgroundColor");
        }

        private string GetChooseColorButtonText()
        {
            return GetResourceText("EditText_ButtonChooseColor");
        }

        private string GetRichModeLabelText()
        {
            return GetResourceText("EditText_RichModeLabel");
        }

        private string GetNoBackgroundLabelText()
        {
            return GetResourceText("EditText_NoBackground");
        }

        private string GetRichSelectionColorLabelText()
        {
            return GetResourceText("EditText_RichSelectionColorLabel");
        }

        private void UpdateColorControls()
        {
            System.Drawing.Color labelColor = dialogTheme?.TextSecondaryColor ?? SystemColors.ControlText;
            if (lblTextColor != null) lblTextColor.ForeColor = labelColor;

            if (btnColor != null)
            {
                btnColor.BackColor = AnnotationColor;
                btnColor.ForeColor = GetContrastingTextColor(AnnotationColor);
                btnColor.Text = GetChooseColorButtonText();
                btnColor.UseVisualStyleBackColor = false;
            }

            if (lblBackgroundColor != null) lblBackgroundColor.ForeColor = labelColor;

            if (btnBackgroundColor != null)
            {
                System.Drawing.Color previewColor = AnnotationBackgroundColor.A > 0
                    ? AnnotationBackgroundColor
                    : System.Drawing.Color.White;
                btnBackgroundColor.BackColor = previewColor;
                btnBackgroundColor.ForeColor = GetContrastingTextColor(previewColor);
                btnBackgroundColor.Text = GetChooseColorButtonText();
                btnBackgroundColor.UseVisualStyleBackColor = false;
                btnBackgroundColor.Enabled = chkNoBackgroundColor == null || !chkNoBackgroundColor.Checked;
            }

            if (lblBorderColor != null) lblBorderColor.ForeColor = labelColor;
            if (btnBorderColor != null)
            {
                System.Drawing.Color previewColor = AnnotationBorderColor.A > 0
                    ? AnnotationBorderColor
                    : System.Drawing.Color.White;
                btnBorderColor.BackColor = previewColor;
                btnBorderColor.ForeColor = GetContrastingTextColor(previewColor);
                btnBorderColor.Text = GetChooseColorButtonText();
                btnBorderColor.UseVisualStyleBackColor = false;
            }

            if (lblLeaderFillColor != null) lblLeaderFillColor.ForeColor = labelColor;
            if (btnLeaderFillColor != null)
            {
                System.Drawing.Color previewColor = GetLeaderFillPreviewColor();
                btnLeaderFillColor.BackColor = previewColor;
                btnLeaderFillColor.ForeColor = GetContrastingTextColor(previewColor);
                btnLeaderFillColor.Text = GetChooseColorButtonText();
                btnLeaderFillColor.UseVisualStyleBackColor = false;
            }

            if (lblLeaderBorderColor != null) lblLeaderBorderColor.ForeColor = labelColor;
            if (btnLeaderBorderColor != null)
            {
                System.Drawing.Color previewColor = GetLeaderBorderPreviewColor();
                btnLeaderBorderColor.BackColor = previewColor;
                btnLeaderBorderColor.ForeColor = GetContrastingTextColor(previewColor);
                btnLeaderBorderColor.Text = GetChooseColorButtonText();
                btnLeaderBorderColor.UseVisualStyleBackColor = false;
            }
        }

        private System.Drawing.Color GetLeaderFillPreviewColor()
        {
            if (LeaderFillColor.A > 0)
            {
                return LeaderFillColor;
            }

            if (AnnotationBackgroundColor.A > 0)
            {
                return AnnotationBackgroundColor;
            }

            if (AnnotationBorderColor.A > 0)
            {
                return AnnotationBorderColor;
            }

            return AnnotationColor.IsEmpty ? System.Drawing.Color.Black : AnnotationColor;
        }

        private System.Drawing.Color GetLeaderBorderPreviewColor()
        {
            if (LeaderBorderColor.A > 0)
            {
                return LeaderBorderColor;
            }

            return AnnotationBorderColor.A > 0 ? AnnotationBorderColor : System.Drawing.Color.Black;
        }

        private void SetRichMode(bool richMode, bool updateState, bool reloadEditorContent = true, bool applyFormatting = true)
        {
            IsRichTextMode = richMode;
            richTextToolbarPanel.Visible = richMode;
            ApplyRichModeLayout(richMode);

            // Global font must remain available in both modes.
            btnFont.Enabled = true;
            lblFontDisplay.Enabled = true;
            lblTextColor.Enabled = !richMode;
            btnColor.Enabled = !richMode;
            lblBackgroundColor.Enabled = true;
            btnBackgroundColor.Enabled = chkNoBackgroundColor == null || !chkNoBackgroundColor.Checked;
            if (chkNoBackgroundColor != null)
            {
                chkNoBackgroundColor.Enabled = true;
            }

            if (reloadEditorContent && richMode)
            {
                if (!string.IsNullOrWhiteSpace(AnnotationRichText))
                {
                    try
                    {
                        txtText.Rtf = AnnotationRichText;
                    }
                    catch
                    {
                        txtText.Text = AnnotationText ?? string.Empty;
                    }
                }
            }
            else if (reloadEditorContent)
            {
                AnnotationRichText = null;
                AnnotationText = txtText.Text;
            }

            ApplyAlignmentToEditor();
            if (applyFormatting)
            {
                ApplyEditorDisplayFormatting(normalizeRichContent: richMode);
            }

            if (updateState)
            {
                ScheduleApplyChanges();
            }
            UpdateColorControls();
        }

        private void ApplyRichModeLayout(bool richMode)
        {
            int offset = richMode ? 0 : -32;
            SetControlTop(btnFont, 198 + offset);
            SetControlTop(lblFontDisplay, 205 + offset);
            SetControlTop(lblTextColor, 237 + offset);
            SetControlTop(btnColor, 230 + offset);
            SetControlTop(lblBackgroundColor, 271 + offset);
            SetControlTop(btnBackgroundColor, 264 + offset);
            SetControlTop(chkNoBackgroundColor, 271 + offset);
            SetControlTop(lblBorderColor, 305 + offset);
            SetControlTop(btnBorderColor, 298 + offset);
            SetControlTop(lblBorderWidth, 339 + offset);
            SetControlTop(nudBorderWidth, 336 + offset);
            SetControlTop(lblFrameMargin, 339 + offset);
            SetControlTop(nudFrameMargin, 336 + offset);
            SetControlTop(chkLeaderArrow, 370 + offset);
            SetControlTop(lblLeaderLineWidth, 402 + offset);
            SetControlTop(nudLeaderLineWidth, 399 + offset);
            SetControlTop(lblLeaderBorderWidth, 402 + offset);
            SetControlTop(nudLeaderBorderWidth, 399 + offset);
            SetControlTop(lblLeaderHeadWidth, 430 + offset);
            SetControlTop(nudLeaderHeadWidth, 427 + offset);
            SetControlTop(lblLeaderHeadLength, 430 + offset);
            SetControlTop(nudLeaderHeadLength, 427 + offset);
            SetControlTop(lblLeaderFillColor, 458 + offset);
            SetControlTop(btnLeaderFillColor, 454 + offset);
            SetControlTop(lblLeaderBorderColor, 458 + offset);
            SetControlTop(btnLeaderBorderColor, 454 + offset);
            SetControlTop(groupBoxAlignment, 492 + offset);
            SetControlTop(groupBoxRotation, 552 + offset);
            SetControlTop(groupBoxSymbols, 622 + offset);
            SetControlTop(btnRestoreDefaults, 708 + offset);
            SetControlTop(btnOK, 708 + offset);
            SetControlTop(btnCancel, 708 + offset);
            ClientSize = PDFForm.ScaleSizeForDpiStatic(430, richMode ? 760 : 728);
        }

        private static void SetControlTop(Control control, int top)
        {
            if (control == null)
            {
                return;
            }

            control.Location = new Point(control.Location.X, PDFForm.ScaleForDpiStatic(top));
        }

        private void UpdateFontDisplay()
        {

            string fontStyles = "";
            if (AnnotationFont.Bold)
                fontStyles += "B";
            if (AnnotationFont.Italic)
                fontStyles += "I";
            if (AnnotationFont.Strikeout)
                fontStyles += "S";
            if (AnnotationFont.Underline)
                fontStyles += "U";
            if (!string.IsNullOrEmpty(fontStyles))
                fontStyles = " (" + fontStyles + ")";

            if (string.IsNullOrEmpty(fontStyles))
            {
                lblFontDisplay.Text = FormatResource("EditText_FontDisplay", AnnotationFont.FontFamily.Name, AnnotationFont.Size);
            }
            else
            {
                lblFontDisplay.Text = FormatResource("EditText_FontDisplayWithStyle", AnnotationFont.FontFamily.Name, AnnotationFont.Size, fontStyles);
            }

            if (!suppressEditorPresentationRefresh)
            {
                ApplyEditorDisplayFormatting(normalizeRichContent: IsRichTextMode);
            }
            UpdateColorControls();
        }

        private void ApplyEditorDisplayFormatting(bool normalizeRichContent)
        {
            if (txtText == null)
            {
                return;
            }

            Font sourceFont = AnnotationFont ?? this.Font ?? SystemFonts.DefaultFont;
            Font newEditorFont = new Font(sourceFont.FontFamily, EditorDisplayFontSize, sourceFont.Style, GraphicsUnit.Point);
            Font previousEditorFont = editorDisplayFont;
            editorDisplayFont = newEditorFont;
            txtText.ZoomFactor = 1f;
            previousEditorFont?.Dispose();

            if (!IsRichTextMode)
            {
                txtText.Font = editorDisplayFont;
                return;
            }

            if (!normalizeRichContent || txtText.TextLength <= 0)
            {
                txtText.Font = editorDisplayFont;
                return;
            }

            RefreshRichEditorPresentation();
        }

        private void RefreshRichEditorPresentation()
        {
            if (txtText == null || editorDisplayFont == null || txtText.TextLength <= 0)
            {
                return;
            }

            int selectionStart = txtText.SelectionStart;
            int selectionLength = txtText.SelectionLength;
            bool previousSuppressAutoApply = suppressAutoApply;
            bool previousSuppressPresentationRefresh = suppressEditorPresentationRefresh;

            try
            {
                suppressAutoApply = true;
                suppressEditorPresentationRefresh = true;
                string editorDisplayRtf = PDFForm.BuildEditorDisplayRichTextRtf(txtText.Text, txtText.Rtf, AnnotationFont);
                if (!string.IsNullOrWhiteSpace(editorDisplayRtf))
                {
                    txtText.Rtf = editorDisplayRtf;
                }

                ApplyAlignmentToEditor();
            }
            finally
            {
                int safeSelectionStart = Math.Max(0, Math.Min(selectionStart, txtText.TextLength));
                int safeSelectionLength = Math.Max(0, Math.Min(selectionLength, txtText.TextLength - safeSelectionStart));
                txtText.Select(safeSelectionStart, safeSelectionLength);
                suppressAutoApply = previousSuppressAutoApply;
                suppressEditorPresentationRefresh = previousSuppressPresentationRefresh;
            }
        }

        private void TxtText_TextChanged(object sender, EventArgs e)
        {
            if (IsRichTextMode && !suppressEditorPresentationRefresh)
            {
                RefreshRichEditorPresentation();
            }

            ScheduleApplyChanges();
        }


        private void RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (rbLeft.Checked)
            {
                AnnotationAlignment = System.Windows.Forms.HorizontalAlignment.Left; ;
            }
            else if (rbCenter.Checked)
            {
                AnnotationAlignment = System.Windows.Forms.HorizontalAlignment.Center;
            }
            else if (rbRight.Checked)
            {
                AnnotationAlignment = System.Windows.Forms.HorizontalAlignment.Right;
            }
            ApplyAlignmentToEditor();
            ScheduleApplyChanges();

        }

        private void ApplyAlignmentToEditor()
        {
            if (txtText == null)
            {
                return;
            }

            int selectionStart = txtText.SelectionStart;
            int selectionLength = txtText.SelectionLength;
            try
            {
                txtText.SelectAll();
                txtText.SelectionAlignment = AnnotationAlignment;
            }
            finally
            {
                txtText.Select(selectionStart, selectionLength);
            }
        }

        private void RotationValueChanged(object sender, EventArgs e)
        {
            AnnotationRotation = NormalizeAngle((int)nudRotation.Value);
            ScheduleApplyChanges();
        }

        private void RotationPresetButton_Click(object sender, EventArgs e)
        {
            if (sender is Button btn)
            {
                int value = 0;
                if (btn.Tag is int tagValue)
                {
                    value = tagValue;
                }
                else
                {
                    int.TryParse(btn.Text, out value);
                }

                if (value < nudRotation.Minimum)
                {
                    value = (int)nudRotation.Minimum;
                }
                else if (value > nudRotation.Maximum)
                {
                    value = (int)nudRotation.Maximum;
                }

                nudRotation.Value = value;
                nudRotation.Focus();
            }
        }

        private void BtnFont_Click(object sender, EventArgs e)
        {
            using (FontDialog fontDialog = new FontDialog())
            {
                fontDialog.Font = AnnotationFont;
                if (fontDialog.ShowDialog(this) == DialogResult.OK)
                {
                    AnnotationFont = fontDialog.Font;
                    UpdateFontDisplay();
                    ScheduleApplyChanges();
                }
            }
        }

        private void BtnColor_Click(object sender, EventArgs e)
        {
            using (ColorDialog colorDialog = new ColorDialog())
            {
                colorDialog.Color = AnnotationColor;
                if (colorDialog.ShowDialog(this) == DialogResult.OK)
                {
                    AnnotationColor = colorDialog.Color;
                    UpdateColorControls();
                    TryApplyChanges();
                }
            }
        }

        private void BtnBackgroundColor_Click(object sender, EventArgs e)
        {
            using (ColorDialog colorDialog = new ColorDialog())
            {
                colorDialog.Color = AnnotationBackgroundColor.A > 0
                    ? AnnotationBackgroundColor
                    : System.Drawing.Color.White;
                if (colorDialog.ShowDialog(this) == DialogResult.OK)
                {
                    AnnotationBackgroundColor = colorDialog.Color;
                    lastBackgroundColorBeforeTransparent = colorDialog.Color;
                    if (chkNoBackgroundColor != null)
                    {
                        chkNoBackgroundColor.Checked = false;
                    }
                    UpdateColorControls();
                    TryApplyChanges();
                }
            }
        }

        private void NoBackgroundColorCheckedChanged(object sender, EventArgs e)
        {
            if (chkNoBackgroundColor == null)
            {
                return;
            }

            if (chkNoBackgroundColor.Checked)
            {
                if (AnnotationBackgroundColor.A > 0)
                {
                    lastBackgroundColorBeforeTransparent = AnnotationBackgroundColor;
                }
                AnnotationBackgroundColor = System.Drawing.Color.Transparent;
            }
            else if (AnnotationBackgroundColor.A <= 0)
            {
                AnnotationBackgroundColor = lastBackgroundColorBeforeTransparent.A > 0
                    ? lastBackgroundColorBeforeTransparent
                    : System.Drawing.Color.White;
            }

            UpdateColorControls();
            TryApplyChanges();
        }

        private void BtnBorderColor_Click(object sender, EventArgs e)
        {
            using (ColorDialog colorDialog = new ColorDialog())
            {
                colorDialog.Color = AnnotationBorderColor.A > 0
                    ? AnnotationBorderColor
                    : System.Drawing.Color.Black;
                if (colorDialog.ShowDialog(this) == DialogResult.OK)
                {
                    AnnotationBorderColor = colorDialog.Color;
                    UpdateColorControls();
                    TryApplyChanges();
                }
            }
        }

        private void BtnLeaderFillColor_Click(object sender, EventArgs e)
        {
            using (ColorDialog colorDialog = new ColorDialog())
            {
                colorDialog.Color = GetLeaderFillPreviewColor();
                if (colorDialog.ShowDialog(this) == DialogResult.OK)
                {
                    LeaderFillColor = colorDialog.Color;
                    if (chkLeaderArrow != null)
                    {
                        chkLeaderArrow.Checked = true;
                    }
                    UpdateColorControls();
                    TryApplyChanges();
                }
            }
        }

        private void BtnLeaderBorderColor_Click(object sender, EventArgs e)
        {
            using (ColorDialog colorDialog = new ColorDialog())
            {
                colorDialog.Color = GetLeaderBorderPreviewColor();
                if (colorDialog.ShowDialog(this) == DialogResult.OK)
                {
                    LeaderBorderColor = colorDialog.Color;
                    if (chkLeaderArrow != null)
                    {
                        chkLeaderArrow.Checked = true;
                    }
                    UpdateColorControls();
                    TryApplyChanges();
                }
            }
        }

        private void RichModeCheckedChanged(object sender, EventArgs e)
        {
            if (suppressAutoApply && suppressEditorPresentationRefresh)
            {
                IsRichTextMode = chkRichTextMode.Checked;
                richTextToolbarPanel.Visible = IsRichTextMode;
                ApplyRichModeLayout(IsRichTextMode);
                UpdateColorControls();
                return;
            }

            SetRichMode(chkRichTextMode.Checked, updateState: true);
        }

        private void ToggleRichSelectionFontStyle(FontStyle style)
        {
            if (!IsRichTextMode)
            {
                return;
            }

            Font selectionFont = txtText.SelectionFont ?? txtText.Font ?? AnnotationFont ?? this.Font;
            FontStyle newStyle = selectionFont.Style.HasFlag(style)
                ? (selectionFont.Style & ~style)
                : (selectionFont.Style | style);
            txtText.SelectionFont = new Font(selectionFont, newStyle);
            txtText.Focus();
            ScheduleApplyChanges();
        }

        private void BtnRichTextColor_Click(object sender, EventArgs e)
        {
            if (!IsRichTextMode)
            {
                return;
            }

            using (ColorDialog colorDialog = new ColorDialog())
            {
                colorDialog.Color = txtText.SelectionColor.IsEmpty ? AnnotationColor : txtText.SelectionColor;
                if (colorDialog.ShowDialog(this) == DialogResult.OK)
                {
                    txtText.SelectionColor = colorDialog.Color;
                    txtText.Focus();
                    ScheduleApplyChanges();
                }
            }
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtText.Text.Trim()))
            {
                MessageBox.Show(this, Resources.EditText_EmptyError, Resources.Title_Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.DialogResult = DialogResult.None;
                return;
            }
            AnnotationText = txtText.Text.Trim();
            AnnotationRichText = IsRichTextMode
                ? BuildRichTextForApply()
                : null;
            AnnotationBorderWidth = NormalizeAnnotationBorderWidth((float)nudBorderWidth.Value);
            AnnotationFrameMargin = NormalizeAnnotationFrameMargin((float)nudFrameMargin.Value);
            HasLeaderArrow = chkLeaderArrow != null && chkLeaderArrow.Checked;
            LeaderLineWidth = PDFForm.NormalizeLeaderLineWidth((float)nudLeaderLineWidth.Value);
            LeaderHeadLength = PDFForm.NormalizeLeaderHeadLength((float)nudLeaderHeadLength.Value);
            LeaderHeadWidth = PDFForm.NormalizeLeaderHeadWidth((float)nudLeaderHeadWidth.Value);
            LeaderFillColor = LeaderFillColor.IsEmpty ? System.Drawing.Color.Transparent : LeaderFillColor;
            LeaderBorderColor = LeaderBorderColor.IsEmpty ? System.Drawing.Color.Transparent : LeaderBorderColor;
            LeaderBorderWidth = NormalizeLeaderBorderWidth((float)nudLeaderBorderWidth.Value);
        }

        private static int NormalizeAngle(int rotation)
        {
            rotation %= 360;
            if (rotation < 0)
                rotation += 360;
            return rotation;
        }

        private static float NormalizeAnnotationBorderWidth(float value)
        {
            if (float.IsNaN(value) || float.IsInfinity(value))
            {
                return 0f;
            }

            return Math.Max(0f, Math.Min(24f, value));
        }

        private static float NormalizeAnnotationFrameMargin(float value)
        {
            if (float.IsNaN(value) || float.IsInfinity(value))
            {
                return 0f;
            }

            return Math.Max(0f, Math.Min(120f, value));
        }

        private void SymbolButton_Click(object sender, EventArgs e)
        {
            if (sender is Button btn)
            {
                txtText.SelectedText = btn.Text;
                txtText.Focus();
            }
        }

        private void ScheduleApplyChanges()
        {
            if (suppressAutoApply)
            {
                liveApplyTimer?.Stop();
                return;
            }

            if (liveApplyTimer == null)
            {
                TryApplyChanges();
                return;
            }

            liveApplyTimer.Stop();
            liveApplyTimer.Start();
        }

        private void TryApplyChanges()
        {
            if (ApplyChanges == null || suppressAutoApply)
                return;
            if (string.IsNullOrWhiteSpace(txtText.Text))
                return;
            AnnotationText = txtText.Text;
            AnnotationRichText = IsRichTextMode
                ? BuildRichTextForApply()
                : null;
            AnnotationBorderWidth = NormalizeAnnotationBorderWidth((float)nudBorderWidth.Value);
            AnnotationFrameMargin = NormalizeAnnotationFrameMargin((float)nudFrameMargin.Value);
            HasLeaderArrow = chkLeaderArrow != null && chkLeaderArrow.Checked;
            LeaderLineWidth = PDFForm.NormalizeLeaderLineWidth((float)nudLeaderLineWidth.Value);
            LeaderHeadLength = PDFForm.NormalizeLeaderHeadLength((float)nudLeaderHeadLength.Value);
            LeaderHeadWidth = PDFForm.NormalizeLeaderHeadWidth((float)nudLeaderHeadWidth.Value);
            LeaderFillColor = LeaderFillColor.IsEmpty ? System.Drawing.Color.Transparent : LeaderFillColor;
            LeaderBorderColor = LeaderBorderColor.IsEmpty ? System.Drawing.Color.Transparent : LeaderBorderColor;
            LeaderBorderWidth = NormalizeLeaderBorderWidth((float)nudLeaderBorderWidth.Value);
            ApplyChanges?.Invoke();
        }

        private static float NormalizeLeaderBorderWidth(float value)
        {
            if (float.IsNaN(value) || float.IsInfinity(value))
            {
                return 0f;
            }

            return Math.Max(0f, Math.Min(24f, value));
        }

        private string BuildRichTextForApply()
        {
            if (!IsRichTextMode)
            {
                return null;
            }

            string editorText = txtText?.Text ?? string.Empty;
            string editorRtf = txtText?.Rtf ?? string.Empty;
            string renderableRtf = PDFForm.BuildRenderableRichTextRtf(editorText, editorRtf, AnnotationFont);
            int expectedLineCount = CountNormalizedLines(editorText);
            int renderableLineCount = GetLineCountFromRtf(renderableRtf);
            if (renderableLineCount >= expectedLineCount || string.IsNullOrWhiteSpace(editorRtf))
            {
                return renderableRtf;
            }

            int editorRtfLineCount = GetLineCountFromRtf(editorRtf);
            if (editorRtfLineCount >= expectedLineCount)
            {
                return editorRtf;
            }

            return renderableRtf;
        }

        private static int CountNormalizedLines(string text)
        {
            string normalized = (text ?? string.Empty).Replace("\r\n", "\n").Replace("\r", "\n");
            return Math.Max(1, normalized.Split(new[] { '\n' }, StringSplitOptions.None).Length);
        }

        private static int GetLineCountFromRtf(string rtf)
        {
            if (string.IsNullOrWhiteSpace(rtf))
            {
                return 1;
            }

            try
            {
                using (var richTextBox = new RichTextBox())
                {
                    richTextBox.Rtf = rtf;
                    return CountNormalizedLines(richTextBox.Text);
                }
            }
            catch
            {
                return 1;
            }
        }

        private static string FormatResource(string key, params object[] args)
        {
            string format = GetResourceText(key);
            return string.Format(format, args);
        }

        private static string GetResourceText(string key)
        {
            var culture = Resources.Culture ?? CultureInfo.CurrentUICulture;
            string value = Resources.ResourceManager.GetString(key, culture);
            return string.IsNullOrWhiteSpace(value) ? key : value;
        }
    }



    public class EditRasterDialog : Form
    {
        private GroupBox groupGeometry;
        private Label lblX;
        private Label lblY;
        private Label lblWidth;
        private Label lblHeight;
        private NumericUpDown nudX;
        private NumericUpDown nudY;
        private NumericUpDown nudWidth;
        private NumericUpDown nudHeight;

        private GroupBox groupRotation;
        private Label lblRotation;
        private NumericUpDown nudRotation;
        private FlowLayoutPanel rotationPresetPanel;

        private GroupBox groupOptions;
        private CheckBox chkLockAspect;
        private CheckBox chkLocked;
        private CheckBox chkTransparentBackground;
        private Label lblOpacity;
        private NumericUpDown nudOpacity;
        private CheckBox chkRecolorInk;
        private Button btnRecolorInkColor;

        private GroupBox groupSource;
        private Label lblSourceValue;
        private Button btnReplaceImage;
        private Button btnResetAspect;
        private Button btnResetOneToOne;

        private Button btnRestoreDefaults;
        private Button btnOK;
        private Button btnCancel;

        private bool suppressDimensionSync;
        private bool suppressAutoApply;
        private decimal aspectRatio = 1m;
        private decimal lockedAspectRatio = 1m;

        public float PositionX { get; set; }
        public float PositionY { get; set; }
        public float WidthValue { get; set; }
        public float HeightValue { get; set; }
        public int Rotation { get; set; }
        public float RasterOpacity { get; set; }
        public bool TransparentBackground { get; set; }
        public bool RecolorInkEnabled { get; set; }
        public System.Drawing.Color RecolorInkColor { get; set; } = System.Drawing.Color.Red;
        public bool LockAspect { get; set; }
        public bool IsLocked { get; set; }
        public string SourceType { get; set; }
        public string FilePath { get; set; }
        public string ReplacementImagePath { get; private set; }
        public decimal SourceAspectRatio { get; set; }
        public int SourcePixelWidth { get; set; }
        public int SourcePixelHeight { get; set; }
        public float ViewScaleFactor { get; set; } = 1f;
        public float PageWidth { get; set; }
        public float PageHeight { get; set; }
        public Func<string> SelectReplacementImage { get; set; }
        public Action ApplyChanges { get; set; }
        public Action<EditRasterDialog> RestoreDefaultsAction { get; set; }
        private DialogTheme dialogTheme;

        public EditRasterDialog()
        {
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            Text = Resources.EditRaster_Title;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterParent;
            Width = PDFForm.ScaleForDpiStatic(430);
            Height = PDFForm.ScaleForDpiStatic(506);
            MaximizeBox = false;
            MinimizeBox = false;
            AutoScroll = true;
            AutoScrollMargin = new Size(0, PDFForm.ScaleForDpiStatic(12));

            groupGeometry = new GroupBox
            {
                Text = Resources.EditRaster_GroupGeometry,
                Location = new Point(PDFForm.ScaleForDpiStatic(10), PDFForm.ScaleForDpiStatic(10)),
                Size = new Size(PDFForm.ScaleForDpiStatic(394), PDFForm.ScaleForDpiStatic(110))
            };

            lblX = new Label { Text = Resources.EditRaster_LabelX, Location = new Point(PDFForm.ScaleForDpiStatic(12), PDFForm.ScaleForDpiStatic(27)), AutoSize = true };
            nudX = BuildNumberInput(PDFForm.ScaleForDpiStatic(60), PDFForm.ScaleForDpiStatic(24), -100000m, 100000m, 2);
            lblY = new Label { Text = Resources.EditRaster_LabelY, Location = new Point(PDFForm.ScaleForDpiStatic(205), PDFForm.ScaleForDpiStatic(27)), AutoSize = true };
            nudY = BuildNumberInput(PDFForm.ScaleForDpiStatic(250), PDFForm.ScaleForDpiStatic(24), -100000m, 100000m, 2);
            lblWidth = new Label { Text = Resources.EditRaster_LabelWidth, Location = new Point(PDFForm.ScaleForDpiStatic(12), PDFForm.ScaleForDpiStatic(67)), AutoSize = true };
            nudWidth = BuildNumberInput(PDFForm.ScaleForDpiStatic(60), PDFForm.ScaleForDpiStatic(64), 1m, 100000m, 2);
            lblHeight = new Label { Text = Resources.EditRaster_LabelHeight, Location = new Point(PDFForm.ScaleForDpiStatic(205), PDFForm.ScaleForDpiStatic(67)), AutoSize = true };
            nudHeight = BuildNumberInput(PDFForm.ScaleForDpiStatic(250), PDFForm.ScaleForDpiStatic(64), 1m, 100000m, 2);
            nudX.ValueChanged += AnyControlValueChanged;
            nudY.ValueChanged += AnyControlValueChanged;
            nudWidth.ValueChanged += NudWidth_ValueChanged;
            nudHeight.ValueChanged += NudHeight_ValueChanged;

            groupGeometry.Controls.Add(lblX);
            groupGeometry.Controls.Add(nudX);
            groupGeometry.Controls.Add(lblY);
            groupGeometry.Controls.Add(nudY);
            groupGeometry.Controls.Add(lblWidth);
            groupGeometry.Controls.Add(nudWidth);
            groupGeometry.Controls.Add(lblHeight);
            groupGeometry.Controls.Add(nudHeight);

            groupRotation = new GroupBox
            {
                Text = Resources.EditRaster_GroupRotation,
                Location = new Point(PDFForm.ScaleForDpiStatic(10), PDFForm.ScaleForDpiStatic(128)),
                Size = new Size(PDFForm.ScaleForDpiStatic(394), PDFForm.ScaleForDpiStatic(58))
            };

            lblRotation = new Label
            {
                Text = Resources.EditRaster_RotationLabel,
                Location = new Point(PDFForm.ScaleForDpiStatic(12), PDFForm.ScaleForDpiStatic(25)),
                AutoSize = true
            };
            nudRotation = new NumericUpDown
            {
                Minimum = 0,
                Maximum = 359,
                Increment = 1,
                Location = new Point(PDFForm.ScaleForDpiStatic(70), PDFForm.ScaleForDpiStatic(21)),
                Size = new Size(PDFForm.ScaleForDpiStatic(64), PDFForm.ScaleForDpiStatic(22))
            };
            nudRotation.ValueChanged += AnyControlValueChanged;
            rotationPresetPanel = new FlowLayoutPanel
            {
                Location = new Point(PDFForm.ScaleForDpiStatic(145), PDFForm.ScaleForDpiStatic(20)),
                Size = new Size(PDFForm.ScaleForDpiStatic(240), PDFForm.ScaleForDpiStatic(26)),
                AutoSize = false,
                WrapContents = false,
                FlowDirection = FlowDirection.LeftToRight,
                Margin = new Padding(0)
            };

            int[] presets = { 0, 30, 45, 90, 180, 270 };
            foreach (int preset in presets)
            {
                Button presetButton = new Button
                {
                    Text = preset.ToString(CultureInfo.InvariantCulture),
                    Tag = preset,
                    Size = new Size(PDFForm.ScaleForDpiStatic(34), PDFForm.ScaleForDpiStatic(24)),
                    Margin = new Padding(2, 0, 0, 0),
                    TabStop = false
                };
                presetButton.Click += RotationPresetButton_Click;
                rotationPresetPanel.Controls.Add(presetButton);
            }

            groupRotation.Controls.Add(lblRotation);
            groupRotation.Controls.Add(nudRotation);
            groupRotation.Controls.Add(rotationPresetPanel);

            groupOptions = new GroupBox
            {
                Text = Resources.EditRaster_GroupOptions,
                Location = new Point(PDFForm.ScaleForDpiStatic(10), PDFForm.ScaleForDpiStatic(194)),
                Size = new Size(PDFForm.ScaleForDpiStatic(394), PDFForm.ScaleForDpiStatic(122))
            };
            chkLockAspect = new CheckBox
            {
                Text = Resources.EditRaster_CheckLockAspect,
                Location = new Point(PDFForm.ScaleForDpiStatic(12), PDFForm.ScaleForDpiStatic(24)),
                AutoSize = true
            };
            chkLockAspect.CheckedChanged += ChkLockAspect_CheckedChanged;
            chkLocked = new CheckBox
            {
                Text = Resources.EditRaster_CheckLocked,
                Location = new Point(PDFForm.ScaleForDpiStatic(190), PDFForm.ScaleForDpiStatic(24)),
                AutoSize = true
            };
            chkTransparentBackground = new CheckBox
            {
                Text = Resources.EditRaster_CheckTransparentBackground,
                Location = new Point(PDFForm.ScaleForDpiStatic(12), PDFForm.ScaleForDpiStatic(56)),
                AutoSize = true
            };
            chkTransparentBackground.CheckedChanged += AnyControlValueChanged;
            lblOpacity = new Label
            {
                Text = Resources.EditRaster_LabelOpacity,
                Location = new Point(PDFForm.ScaleForDpiStatic(190), PDFForm.ScaleForDpiStatic(56)),
                AutoSize = true
            };
            nudOpacity = new NumericUpDown
            {
                Minimum = 0,
                Maximum = 100,
                DecimalPlaces = 0,
                Increment = 1,
                Location = new Point(PDFForm.ScaleForDpiStatic(300), PDFForm.ScaleForDpiStatic(52)),
                Size = new Size(PDFForm.ScaleForDpiStatic(70), PDFForm.ScaleForDpiStatic(22)),
                ThousandsSeparator = false
            };
            nudOpacity.ValueChanged += AnyControlValueChanged;
            chkLocked.CheckedChanged += AnyControlValueChanged;
            chkRecolorInk = new CheckBox
            {
                Text = R("EditRaster_CheckRecolorInk"),
                Location = new Point(PDFForm.ScaleForDpiStatic(12), PDFForm.ScaleForDpiStatic(88)),
                Size = new Size(PDFForm.ScaleForDpiStatic(170), PDFForm.ScaleForDpiStatic(24)),
                AutoEllipsis = true
            };
            chkRecolorInk.CheckedChanged += (_, __) =>
            {
                UpdateRecolorInkColorButton();
                TryApplyChanges();
            };
            btnRecolorInkColor = new Button
            {
                Text = R("EditRaster_ButtonRecolorInkColor"),
                Location = new Point(PDFForm.ScaleForDpiStatic(190), PDFForm.ScaleForDpiStatic(84)),
                Size = new Size(PDFForm.ScaleForDpiStatic(180), PDFForm.ScaleForDpiStatic(28)),
                UseVisualStyleBackColor = false
            };
            btnRecolorInkColor.Click += BtnRecolorInkColor_Click;
            groupOptions.Controls.Add(chkLockAspect);
            groupOptions.Controls.Add(chkLocked);
            groupOptions.Controls.Add(chkTransparentBackground);
            groupOptions.Controls.Add(lblOpacity);
            groupOptions.Controls.Add(nudOpacity);
            groupOptions.Controls.Add(chkRecolorInk);
            groupOptions.Controls.Add(btnRecolorInkColor);

            groupSource = new GroupBox
            {
                Text = Resources.EditRaster_GroupSource,
                Location = new Point(PDFForm.ScaleForDpiStatic(10), PDFForm.ScaleForDpiStatic(324)),
                Size = new Size(PDFForm.ScaleForDpiStatic(394), PDFForm.ScaleForDpiStatic(86))
            };
            lblSourceValue = new Label
            {
                Location = new Point(PDFForm.ScaleForDpiStatic(12), PDFForm.ScaleForDpiStatic(25)),
                Size = new Size(PDFForm.ScaleForDpiStatic(250), PDFForm.ScaleForDpiStatic(18)),
                AutoEllipsis = true
            };
            btnReplaceImage = new Button
            {
                Text = Resources.EditRaster_ButtonReplaceImage,
                Location = new Point(PDFForm.ScaleForDpiStatic(268), PDFForm.ScaleForDpiStatic(18)),
                Size = new Size(PDFForm.ScaleForDpiStatic(116), PDFForm.ScaleForDpiStatic(28))
            };
            btnReplaceImage.Click += BtnReplaceImage_Click;
            btnResetAspect = new Button
            {
                Text = Resources.EditRaster_ButtonResetAspect,
                Location = new Point(PDFForm.ScaleForDpiStatic(268), PDFForm.ScaleForDpiStatic(50)),
                Size = new Size(PDFForm.ScaleForDpiStatic(116), PDFForm.ScaleForDpiStatic(28))
            };
            btnResetAspect.Click += BtnResetAspect_Click;
            btnResetOneToOne = new Button
            {
                Text = Resources.EditRaster_ButtonResetOneToOne,
                Location = new Point(PDFForm.ScaleForDpiStatic(212), PDFForm.ScaleForDpiStatic(50)),
                Size = new Size(PDFForm.ScaleForDpiStatic(50), PDFForm.ScaleForDpiStatic(28))
            };
            btnResetOneToOne.Click += BtnResetOneToOne_Click;
            groupSource.Controls.Add(lblSourceValue);
            groupSource.Controls.Add(btnReplaceImage);
            groupSource.Controls.Add(btnResetAspect);
            groupSource.Controls.Add(btnResetOneToOne);

            btnOK = new Button
            {
                Text = Resources.Merge_OK,
                Location = new Point(PDFForm.ScaleForDpiStatic(238), PDFForm.ScaleForDpiStatic(422)),
                Size = new Size(PDFForm.ScaleForDpiStatic(80), PDFForm.ScaleForDpiStatic(30)),
                DialogResult = DialogResult.OK
            };
            btnOK.Click += BtnOK_Click;

            btnRestoreDefaults = new Button
            {
                Text = Resources.ResourceManager.GetString("UI_Button_RestoreSettings", Resources.Culture ?? CultureInfo.CurrentUICulture) ?? "UI_Button_RestoreSettings",
                Location = new Point(PDFForm.ScaleForDpiStatic(92), PDFForm.ScaleForDpiStatic(422)),
                Size = new Size(PDFForm.ScaleForDpiStatic(136), PDFForm.ScaleForDpiStatic(30))
            };
            btnRestoreDefaults.Click += BtnRestoreDefaults_Click;

            btnCancel = new Button
            {
                Text = Resources.Merge_Cancel,
                Location = new Point(PDFForm.ScaleForDpiStatic(324), PDFForm.ScaleForDpiStatic(422)),
                Size = new Size(PDFForm.ScaleForDpiStatic(80), PDFForm.ScaleForDpiStatic(30)),
                DialogResult = DialogResult.Cancel
            };

            Controls.Add(groupGeometry);
            Controls.Add(groupRotation);
            Controls.Add(groupOptions);
            Controls.Add(groupSource);
            Controls.Add(btnRestoreDefaults);
            Controls.Add(btnOK);
            Controls.Add(btnCancel);

            CancelButton = btnCancel;
            AcceptButton = btnOK;
        }

        private static NumericUpDown BuildNumberInput(int x, int y, decimal min, decimal max, int decimals)
        {
            return new NumericUpDown
            {
                Location = new Point(x, y),
                Size = new Size(PDFForm.ScaleForDpiStatic(120), PDFForm.ScaleForDpiStatic(22)),
                Minimum = min,
                Maximum = max,
                DecimalPlaces = decimals,
                ThousandsSeparator = true,
                Increment = decimals > 0 ? 0.1m : 1m
            };
        }

        private static string R(string key)
        {
            string value = Resources.ResourceManager.GetString(key, Resources.Culture ?? CultureInfo.CurrentUICulture);
            return string.IsNullOrWhiteSpace(value) ? key : value;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            suppressDimensionSync = true;
            suppressAutoApply = true;

            nudX.Value = ClampDecimal((decimal)PositionX, nudX.Minimum, nudX.Maximum);
            nudY.Value = ClampDecimal((decimal)PositionY, nudY.Minimum, nudY.Maximum);
            nudWidth.Value = ClampDecimal((decimal)Math.Max(1f, WidthValue), nudWidth.Minimum, nudWidth.Maximum);
            nudHeight.Value = ClampDecimal((decimal)Math.Max(1f, HeightValue), nudHeight.Minimum, nudHeight.Maximum);
            nudRotation.Value = NormalizeAngle(Rotation);
            nudOpacity.Value = ClampDecimal((decimal)(Math.Max(0f, Math.Min(1f, RasterOpacity)) * 100f), nudOpacity.Minimum, nudOpacity.Maximum);
            chkTransparentBackground.Checked = TransparentBackground;
            chkRecolorInk.Checked = RecolorInkEnabled;
            chkLockAspect.Checked = LockAspect;
            chkLocked.Checked = IsLocked;
            UpdateRecolorInkColorButton();
            ReplacementImagePath = string.Empty;
            btnResetAspect.Enabled = SourceAspectRatio > 0m;
            btnResetOneToOne.Enabled = SourcePixelWidth > 0 && SourcePixelHeight > 0;

            RecalculateAspectRatio();
            lockedAspectRatio = aspectRatio > 0m ? aspectRatio : 1m;
            UpdateSourceLabel();

            SyncPropertiesFromControls();
            ApplyDialogTheme(dialogTheme);
            suppressAutoApply = false;
            suppressDimensionSync = false;
        }

        internal void ApplyDialogTheme(DialogTheme theme)
        {
            dialogTheme = theme;
            DialogThemeApplier.ApplyTo(this, theme);
            if (btnRecolorInkColor != null)
            {
                UpdateRecolorInkColorButton();
            }
        }

        private static decimal ClampDecimal(decimal value, decimal min, decimal max)
        {
            if (value < min) return min;
            if (value > max) return max;
            return value;
        }

        private static int NormalizeAngle(int rotation)
        {
            rotation %= 360;
            if (rotation < 0)
            {
                rotation += 360;
            }
            return rotation;
        }

        private void RecalculateAspectRatio()
        {
            decimal height = nudHeight.Value;
            aspectRatio = height <= 0m ? 1m : nudWidth.Value / height;
            if (aspectRatio <= 0m)
            {
                aspectRatio = 1m;
            }
        }

        private void ChkLockAspect_CheckedChanged(object sender, EventArgs e)
        {
            if (chkLockAspect.Checked)
            {
                RecalculateAspectRatio();
                lockedAspectRatio = aspectRatio > 0m ? aspectRatio : 1m;
            }
            TryApplyChanges();
        }

        private void NudWidth_ValueChanged(object sender, EventArgs e)
        {
            if (suppressDimensionSync)
            {
                return;
            }

            if (chkLockAspect.Checked && aspectRatio > 0m)
            {
                decimal ratioToUse = lockedAspectRatio > 0m ? lockedAspectRatio : aspectRatio;
                suppressDimensionSync = true;
                decimal height = nudWidth.Value / ratioToUse;
                nudHeight.Value = ClampDecimal(height, nudHeight.Minimum, nudHeight.Maximum);
                suppressDimensionSync = false;
            }
            TryApplyChanges();
        }

        private void NudHeight_ValueChanged(object sender, EventArgs e)
        {
            if (suppressDimensionSync)
            {
                return;
            }

            if (chkLockAspect.Checked && aspectRatio > 0m)
            {
                decimal ratioToUse = lockedAspectRatio > 0m ? lockedAspectRatio : aspectRatio;
                suppressDimensionSync = true;
                decimal width = nudHeight.Value * ratioToUse;
                nudWidth.Value = ClampDecimal(width, nudWidth.Minimum, nudWidth.Maximum);
                suppressDimensionSync = false;
            }
            TryApplyChanges();
        }

        private void AnyControlValueChanged(object sender, EventArgs e)
        {
            TryApplyChanges();
        }

        private void BtnRecolorInkColor_Click(object sender, EventArgs e)
        {
            using (var colorDialog = new ColorDialog())
            {
                colorDialog.Color = RecolorInkColor.IsEmpty ? System.Drawing.Color.Red : RecolorInkColor;
                if (colorDialog.ShowDialog(this) == DialogResult.OK)
                {
                    RecolorInkColor = colorDialog.Color;
                    chkRecolorInk.Checked = true;
                    UpdateRecolorInkColorButton();
                    TryApplyChanges();
                }
            }
        }

        private void UpdateRecolorInkColorButton()
        {
            System.Drawing.Color color = RecolorInkColor.IsEmpty ? System.Drawing.Color.Red : RecolorInkColor;
            btnRecolorInkColor.Enabled = chkRecolorInk.Checked;
            btnRecolorInkColor.BackColor = color;
            btnRecolorInkColor.ForeColor = color.GetBrightness() < 0.55f ? System.Drawing.Color.White : System.Drawing.Color.Black;
        }

        private void RotationPresetButton_Click(object sender, EventArgs e)
        {
            if (sender is Button btn)
            {
                int value = 0;
                if (btn.Tag is int preset)
                {
                    value = preset;
                }
                else
                {
                    int.TryParse(btn.Text, out value);
                }

                if (value < nudRotation.Minimum)
                {
                    value = (int)nudRotation.Minimum;
                }
                else if (value > nudRotation.Maximum)
                {
                    value = (int)nudRotation.Maximum;
                }

                nudRotation.Value = value;
                nudRotation.Focus();
            }
        }

        private void UpdateSourceLabel()
        {
            if (!string.IsNullOrWhiteSpace(ReplacementImagePath))
            {
                lblSourceValue.Text = Path.GetFileName(ReplacementImagePath);
                return;
            }

            string sourceValue;
            if (string.Equals(SourceType, "Clipboard", StringComparison.OrdinalIgnoreCase))
            {
                sourceValue = Resources.EditRaster_LabelSourceClipboard;
            }
            else if (string.Equals(SourceType, "File", StringComparison.OrdinalIgnoreCase))
            {
                sourceValue = string.IsNullOrWhiteSpace(FilePath)
                    ? Resources.EditRaster_LabelSourceFile
                    : Path.GetFileName(FilePath);
            }
            else
            {
                sourceValue = Resources.EditRaster_LabelSourceUnknown;
            }

            lblSourceValue.Text = sourceValue;
        }

        private void BtnReplaceImage_Click(object sender, EventArgs e)
        {
            if (SelectReplacementImage == null)
            {
                return;
            }

            string selectedPath = SelectReplacementImage();
            if (string.IsNullOrWhiteSpace(selectedPath))
            {
                return;
            }

            ReplacementImagePath = selectedPath;
            if (TryGetImageInfoFromPath(selectedPath, out int sourceWidth, out int sourceHeight, out decimal aspectFromFile))
            {
                SourcePixelWidth = sourceWidth;
                SourcePixelHeight = sourceHeight;
                SourceAspectRatio = aspectFromFile;
            }
            btnResetAspect.Enabled = SourceAspectRatio > 0m;
            btnResetOneToOne.Enabled = SourcePixelWidth > 0 && SourcePixelHeight > 0;
            UpdateSourceLabel();
            TryApplyChanges();
        }

        private static bool TryGetImageInfoFromPath(string filePath, out int width, out int height, out decimal aspectRatio)
        {
            width = 0;
            height = 0;
            aspectRatio = 0m;
            if (string.IsNullOrWhiteSpace(filePath) || !File.Exists(filePath))
            {
                return false;
            }

            try
            {
                using (var image = DrawingImage.FromFile(filePath))
                {
                    if (image.Width <= 0 || image.Height <= 0)
                    {
                        return false;
                    }

                    width = image.Width;
                    height = image.Height;
                    aspectRatio = (decimal)width / height;
                    return aspectRatio > 0m;
                }
            }
            catch
            {
                return false;
            }
        }

        private void BtnResetAspect_Click(object sender, EventArgs e)
        {
            if (SourceAspectRatio <= 0m)
            {
                return;
            }

            decimal currentArea = nudWidth.Value * nudHeight.Value;
            if (currentArea <= 0m)
            {
                currentArea = 1m;
            }

            decimal targetWidth = (decimal)Math.Sqrt((double)(currentArea * SourceAspectRatio));
            decimal targetHeight = targetWidth / SourceAspectRatio;

            suppressDimensionSync = true;
            nudWidth.Value = ClampDecimal(targetWidth, nudWidth.Minimum, nudWidth.Maximum);
            nudHeight.Value = ClampDecimal(targetHeight, nudHeight.Minimum, nudHeight.Maximum);
            suppressDimensionSync = false;

            aspectRatio = SourceAspectRatio;
            if (chkLockAspect.Checked)
            {
                lockedAspectRatio = aspectRatio > 0m ? aspectRatio : lockedAspectRatio;
            }
            TryApplyChanges();
        }

        private void BtnResetOneToOne_Click(object sender, EventArgs e)
        {
            if (SourcePixelWidth <= 0 || SourcePixelHeight <= 0)
            {
                return;
            }

            decimal scale = (decimal)Math.Max(0.01f, ViewScaleFactor);
            decimal centerX = nudX.Value + (nudWidth.Value / 2m);
            decimal centerY = nudY.Value + (nudHeight.Value / 2m);
            decimal targetWidth = SourcePixelWidth / scale;
            decimal targetHeight = SourcePixelHeight / scale;

            // If 1:1 is larger than page area, scale down with a safe margin
            // so resize handles are still easy to grab.
            if (PageWidth > 0f && PageHeight > 0f)
            {
                decimal viewScale = (decimal)Math.Max(0.01f, ViewScaleFactor);
                decimal marginInViewPixels = 28m;
                decimal marginDoc = marginInViewPixels / viewScale;
                decimal availableWidth = Math.Max(1m, (decimal)PageWidth - (2m * marginDoc));
                decimal availableHeight = Math.Max(1m, (decimal)PageHeight - (2m * marginDoc));

                double radians = (Math.PI / 180d) * NormalizeAngle((int)nudRotation.Value);
                decimal absCos = (decimal)Math.Abs(Math.Cos(radians));
                decimal absSin = (decimal)Math.Abs(Math.Sin(radians));
                decimal extentWidth = (targetWidth * absCos) + (targetHeight * absSin);
                decimal extentHeight = (targetWidth * absSin) + (targetHeight * absCos);

                if (extentWidth > availableWidth || extentHeight > availableHeight)
                {
                    decimal scaleX = availableWidth / Math.Max(1m, extentWidth);
                    decimal scaleY = availableHeight / Math.Max(1m, extentHeight);
                    decimal fitScale = Math.Min(scaleX, scaleY);
                    if (fitScale > 0m && fitScale < 1m)
                    {
                        targetWidth *= fitScale;
                        targetHeight *= fitScale;
                    }
                }
            }

            targetWidth = ClampDecimal(targetWidth, nudWidth.Minimum, nudWidth.Maximum);
            targetHeight = ClampDecimal(targetHeight, nudHeight.Minimum, nudHeight.Maximum);

            suppressDimensionSync = true;
            nudWidth.Value = targetWidth;
            nudHeight.Value = targetHeight;
            nudX.Value = ClampDecimal(centerX - (targetWidth / 2m), nudX.Minimum, nudX.Maximum);
            nudY.Value = ClampDecimal(centerY - (targetHeight / 2m), nudY.Minimum, nudY.Maximum);
            suppressDimensionSync = false;

            aspectRatio = SourceAspectRatio > 0m ? SourceAspectRatio : aspectRatio;
            if (chkLockAspect.Checked)
            {
                lockedAspectRatio = aspectRatio > 0m ? aspectRatio : lockedAspectRatio;
            }
            TryApplyChanges();
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            SyncPropertiesFromControls();
        }

        private void BtnRestoreDefaults_Click(object sender, EventArgs e)
        {
            if (RestoreDefaultsAction == null)
            {
                return;
            }

            RestoreDefaultsAction(this);
            suppressAutoApply = true;
            suppressDimensionSync = true;
            try
            {
                nudRotation.Value = NormalizeAngle(Rotation);
                nudOpacity.Value = ClampDecimal((decimal)(Math.Max(0f, Math.Min(1f, RasterOpacity)) * 100f), nudOpacity.Minimum, nudOpacity.Maximum);
                chkTransparentBackground.Checked = TransparentBackground;
                chkRecolorInk.Checked = RecolorInkEnabled;
                UpdateRecolorInkColorButton();
                chkLockAspect.Checked = LockAspect;
                chkLocked.Checked = IsLocked;
                RecalculateAspectRatio();
                lockedAspectRatio = aspectRatio > 0m ? aspectRatio : 1m;
            }
            finally
            {
                suppressDimensionSync = false;
                suppressAutoApply = false;
            }
            TryApplyChanges();
        }

        private void SyncPropertiesFromControls()
        {
            PositionX = (float)nudX.Value;
            PositionY = (float)nudY.Value;
            WidthValue = Math.Max(1f, (float)nudWidth.Value);
            HeightValue = Math.Max(1f, (float)nudHeight.Value);
            Rotation = NormalizeAngle((int)nudRotation.Value);
            RasterOpacity = (float)(nudOpacity.Value / 100m);
            TransparentBackground = chkTransparentBackground.Checked;
            RecolorInkEnabled = chkRecolorInk.Checked;
            LockAspect = chkLockAspect.Checked;
            IsLocked = chkLocked.Checked;
        }

        public void SyncControlsFromObject(RasterObject rasterObject)
        {
            if (rasterObject == null)
            {
                return;
            }

            suppressAutoApply = true;
            suppressDimensionSync = true;
            try
            {
                nudX.Value = ClampDecimal((decimal)rasterObject.Bounds.X, nudX.Minimum, nudX.Maximum);
                nudY.Value = ClampDecimal((decimal)rasterObject.Bounds.Y, nudY.Minimum, nudY.Maximum);
                nudWidth.Value = ClampDecimal((decimal)Math.Max(1f, rasterObject.Bounds.Width), nudWidth.Minimum, nudWidth.Maximum);
                nudHeight.Value = ClampDecimal((decimal)Math.Max(1f, rasterObject.Bounds.Height), nudHeight.Minimum, nudHeight.Maximum);
                nudRotation.Value = NormalizeAngle(rasterObject.Rotation);
                nudOpacity.Value = ClampDecimal((decimal)(Math.Max(0f, Math.Min(1f, rasterObject.Opacity)) * 100f), nudOpacity.Minimum, nudOpacity.Maximum);
                chkTransparentBackground.Checked = rasterObject.TransparentBackground;
                RecolorInkEnabled = rasterObject.RecolorInkEnabled;
                RecolorInkColor = System.Drawing.Color.FromArgb(rasterObject.RecolorInkColorArgb);
                chkRecolorInk.Checked = RecolorInkEnabled;
                UpdateRecolorInkColorButton();
                chkLockAspect.Checked = rasterObject.LockAspect;
                chkLocked.Checked = rasterObject.IsLocked;
                SourceType = rasterObject.SourceType;
                FilePath = rasterObject.FilePath;
                ReplacementImagePath = string.Empty;
                if (TryGetImageInfoFromPath(FilePath, out int sourceWidth, out int sourceHeight, out decimal aspectFromPath))
                {
                    SourcePixelWidth = sourceWidth;
                    SourcePixelHeight = sourceHeight;
                    SourceAspectRatio = aspectFromPath;
                }
                btnResetAspect.Enabled = SourceAspectRatio > 0m;
                btnResetOneToOne.Enabled = SourcePixelWidth > 0 && SourcePixelHeight > 0;
                RecalculateAspectRatio();
                lockedAspectRatio = aspectRatio > 0m ? aspectRatio : lockedAspectRatio;
                UpdateSourceLabel();
                SyncPropertiesFromControls();
            }
            finally
            {
                suppressDimensionSync = false;
                suppressAutoApply = false;
            }
        }

        private void TryApplyChanges()
        {
            if (ApplyChanges == null || suppressAutoApply)
            {
                return;
            }

            SyncPropertiesFromControls();
            ApplyChanges?.Invoke();
        }
    }

    public class EditArrowDialog : Form
    {
        private Button btnColor;
        private Button btnBorderColor;
        private NumericUpDown nudThickness;
        private NumericUpDown nudBorderWidth;
        private NumericUpDown nudHeadLength;
        private NumericUpDown nudHeadWidth;
        private CheckBox chkLocked;
        private Button btnRestoreDefaults;
        private Button btnOK;
        private Button btnCancel;
        private bool suppressAutoApply;
        private DialogTheme dialogTheme;

        public System.Drawing.Color ArrowColor { get; set; } = System.Drawing.Color.Red;
        public float ThicknessValue { get; set; } = 3f;
        public System.Drawing.Color BorderColor { get; set; } = System.Drawing.Color.Black;
        public float BorderWidthValue { get; set; } = 0f;
        public float HeadLengthValue { get; set; } = 18f;
        public float HeadWidthValue { get; set; } = 12f;
        public bool IsLocked { get; set; }
        public Action ApplyChanges { get; set; }
        public Action<EditArrowDialog> RestoreDefaultsAction { get; set; }

        public EditArrowDialog()
        {
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            Text = Resources.EditArrow_Title;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterParent;
            Width = PDFForm.ScaleForDpiStatic(360);
            Height = PDFForm.ScaleForDpiStatic(350);
            MaximizeBox = false;
            MinimizeBox = false;

            Label lblColor = new Label { Text = Resources.EditArrow_Color, Location = new Point(PDFForm.ScaleForDpiStatic(16), PDFForm.ScaleForDpiStatic(22)), AutoSize = true };
            btnColor = new Button { Location = new Point(PDFForm.ScaleForDpiStatic(170), PDFForm.ScaleForDpiStatic(16)), Size = new Size(PDFForm.ScaleForDpiStatic(160), PDFForm.ScaleForDpiStatic(28)), Text = Resources.EditArrow_ChooseColor };
            btnColor.Click += BtnColor_Click;

            Label lblThickness = new Label { Text = Resources.EditArrow_LineThickness, Location = new Point(PDFForm.ScaleForDpiStatic(16), PDFForm.ScaleForDpiStatic(64)), AutoSize = true };
            nudThickness = new NumericUpDown
            {
                Location = new Point(PDFForm.ScaleForDpiStatic(170), PDFForm.ScaleForDpiStatic(60)),
                Size = new Size(PDFForm.ScaleForDpiStatic(160), PDFForm.ScaleForDpiStatic(22)),
                Minimum = 1,
                Maximum = 24,
                DecimalPlaces = 3,
                Increment = 0.1m
            };
            nudThickness.ValueChanged += (_, __) => TryApplyChanges();

            Label lblBorderColor = new Label { Text = Resources.EditArrow_BorderColor, Location = new Point(PDFForm.ScaleForDpiStatic(16), PDFForm.ScaleForDpiStatic(100)), AutoSize = true };
            btnBorderColor = new Button { Location = new Point(PDFForm.ScaleForDpiStatic(170), PDFForm.ScaleForDpiStatic(94)), Size = new Size(PDFForm.ScaleForDpiStatic(160), PDFForm.ScaleForDpiStatic(28)), Text = Resources.EditArrow_ChooseColor };
            btnBorderColor.Click += BtnBorderColor_Click;

            Label lblBorderWidth = new Label { Text = Resources.EditArrow_BorderThickness, Location = new Point(PDFForm.ScaleForDpiStatic(16), PDFForm.ScaleForDpiStatic(136)), AutoSize = true };
            nudBorderWidth = new NumericUpDown
            {
                Location = new Point(PDFForm.ScaleForDpiStatic(170), PDFForm.ScaleForDpiStatic(132)),
                Size = new Size(PDFForm.ScaleForDpiStatic(160), PDFForm.ScaleForDpiStatic(22)),
                Minimum = 0,
                Maximum = 24,
                DecimalPlaces = 3,
                Increment = 0.1m
            };
            nudBorderWidth.ValueChanged += (_, __) => TryApplyChanges();

            Label lblHeadLength = new Label { Text = Resources.EditArrow_HeadLength, Location = new Point(PDFForm.ScaleForDpiStatic(16), PDFForm.ScaleForDpiStatic(172)), AutoSize = true };
            nudHeadLength = new NumericUpDown
            {
                Location = new Point(PDFForm.ScaleForDpiStatic(170), PDFForm.ScaleForDpiStatic(168)),
                Size = new Size(PDFForm.ScaleForDpiStatic(160), PDFForm.ScaleForDpiStatic(22)),
                Minimum = 4,
                Maximum = 120,
                DecimalPlaces = 3,
                Increment = 0.1m
            };
            nudHeadLength.ValueChanged += (_, __) => TryApplyChanges();

            Label lblHeadWidth = new Label { Text = Resources.EditArrow_HeadWidth, Location = new Point(PDFForm.ScaleForDpiStatic(16), PDFForm.ScaleForDpiStatic(208)), AutoSize = true };
            nudHeadWidth = new NumericUpDown
            {
                Location = new Point(PDFForm.ScaleForDpiStatic(170), PDFForm.ScaleForDpiStatic(204)),
                Size = new Size(PDFForm.ScaleForDpiStatic(160), PDFForm.ScaleForDpiStatic(22)),
                Minimum = 4,
                Maximum = 120,
                DecimalPlaces = 3,
                Increment = 0.1m
            };
            nudHeadWidth.ValueChanged += (_, __) => TryApplyChanges();

            chkLocked = new CheckBox
            {
                Text = Resources.EditRaster_CheckLocked,
                Location = new Point(PDFForm.ScaleForDpiStatic(16), PDFForm.ScaleForDpiStatic(240)),
                AutoSize = true
            };
            chkLocked.CheckedChanged += (_, __) => TryApplyChanges();

            btnOK = new Button
            {
                Text = Resources.Merge_OK,
                Location = new Point(PDFForm.ScaleForDpiStatic(166), PDFForm.ScaleForDpiStatic(262)),
                Size = new Size(PDFForm.ScaleForDpiStatic(80), PDFForm.ScaleForDpiStatic(30)),
                DialogResult = DialogResult.OK
            };
            btnOK.Click += BtnOK_Click;

            btnRestoreDefaults = new Button
            {
                Text = Resources.ResourceManager.GetString("UI_Button_RestoreSettings", Resources.Culture ?? CultureInfo.CurrentUICulture) ?? "UI_Button_RestoreSettings",
                Location = new Point(PDFForm.ScaleForDpiStatic(16), PDFForm.ScaleForDpiStatic(262)),
                Size = new Size(PDFForm.ScaleForDpiStatic(140), PDFForm.ScaleForDpiStatic(30))
            };
            btnRestoreDefaults.Click += BtnRestoreDefaults_Click;

            btnCancel = new Button
            {
                Text = Resources.Merge_Cancel,
                Location = new Point(PDFForm.ScaleForDpiStatic(252), PDFForm.ScaleForDpiStatic(262)),
                Size = new Size(PDFForm.ScaleForDpiStatic(80), PDFForm.ScaleForDpiStatic(30)),
                DialogResult = DialogResult.Cancel
            };

            Controls.Add(lblColor);
            Controls.Add(btnColor);
            Controls.Add(lblThickness);
            Controls.Add(nudThickness);
            Controls.Add(lblBorderColor);
            Controls.Add(btnBorderColor);
            Controls.Add(lblBorderWidth);
            Controls.Add(nudBorderWidth);
            Controls.Add(lblHeadLength);
            Controls.Add(nudHeadLength);
            Controls.Add(lblHeadWidth);
            Controls.Add(nudHeadWidth);
            Controls.Add(chkLocked);
            Controls.Add(btnRestoreDefaults);
            Controls.Add(btnOK);
            Controls.Add(btnCancel);

            AcceptButton = btnOK;
            CancelButton = btnCancel;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            suppressAutoApply = true;
            try
            {
                btnColor.BackColor = ArrowColor;
                btnBorderColor.BackColor = BorderColor;
                nudThickness.Value = ClampValue((decimal)ThicknessValue, nudThickness.Minimum, nudThickness.Maximum);
                nudBorderWidth.Value = ClampValue((decimal)BorderWidthValue, nudBorderWidth.Minimum, nudBorderWidth.Maximum);
                nudHeadLength.Value = ClampValue((decimal)HeadLengthValue, nudHeadLength.Minimum, nudHeadLength.Maximum);
                nudHeadWidth.Value = ClampValue((decimal)HeadWidthValue, nudHeadWidth.Minimum, nudHeadWidth.Maximum);
                chkLocked.Checked = IsLocked;
                SyncPropertiesFromControls();
                ApplyDialogTheme(dialogTheme);
            }
            finally
            {
                suppressAutoApply = false;
            }
        }

        internal void ApplyDialogTheme(DialogTheme theme)
        {
            dialogTheme = theme;
            DialogThemeApplier.ApplyTo(this, theme, btnColor, btnBorderColor);
            if (btnColor != null)
            {
                btnColor.ForeColor = GetContrastingTextColor(btnColor.BackColor);
            }
            if (btnBorderColor != null)
            {
                btnBorderColor.ForeColor = GetContrastingTextColor(btnBorderColor.BackColor);
            }
        }

        private static System.Drawing.Color GetContrastingTextColor(System.Drawing.Color color)
        {
            int luminance = (int)((0.299 * color.R) + (0.587 * color.G) + (0.114 * color.B));
            return luminance >= 140 ? System.Drawing.Color.Black : System.Drawing.Color.White;
        }

        private static decimal ClampValue(decimal value, decimal min, decimal max)
        {
            if (value < min)
            {
                return min;
            }
            if (value > max)
            {
                return max;
            }
            return value;
        }

        private void BtnColor_Click(object sender, EventArgs e)
        {
            using (var colorDialog = new ColorDialog())
            {
                colorDialog.Color = ArrowColor;
                if (colorDialog.ShowDialog(this) == DialogResult.OK)
                {
                    ArrowColor = colorDialog.Color;
                    btnColor.BackColor = ArrowColor;
                    btnColor.ForeColor = GetContrastingTextColor(ArrowColor);
                    TryApplyChanges();
                }
            }
        }

        private void BtnBorderColor_Click(object sender, EventArgs e)
        {
            using (var colorDialog = new ColorDialog())
            {
                colorDialog.Color = BorderColor;
                if (colorDialog.ShowDialog(this) == DialogResult.OK)
                {
                    BorderColor = colorDialog.Color;
                    btnBorderColor.BackColor = BorderColor;
                    btnBorderColor.ForeColor = GetContrastingTextColor(BorderColor);
                    TryApplyChanges();
                }
            }
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            SyncPropertiesFromControls();
        }

        private void BtnRestoreDefaults_Click(object sender, EventArgs e)
        {
            if (RestoreDefaultsAction == null)
            {
                return;
            }

            RestoreDefaultsAction(this);
            suppressAutoApply = true;
            try
            {
                btnColor.BackColor = ArrowColor;
                btnBorderColor.BackColor = BorderColor;
                btnColor.ForeColor = GetContrastingTextColor(ArrowColor);
                btnBorderColor.ForeColor = GetContrastingTextColor(BorderColor);
                nudThickness.Value = ClampValue((decimal)ThicknessValue, nudThickness.Minimum, nudThickness.Maximum);
                nudBorderWidth.Value = ClampValue((decimal)BorderWidthValue, nudBorderWidth.Minimum, nudBorderWidth.Maximum);
                nudHeadLength.Value = ClampValue((decimal)HeadLengthValue, nudHeadLength.Minimum, nudHeadLength.Maximum);
                nudHeadWidth.Value = ClampValue((decimal)HeadWidthValue, nudHeadWidth.Minimum, nudHeadWidth.Maximum);
                chkLocked.Checked = IsLocked;
            }
            finally
            {
                suppressAutoApply = false;
            }
            TryApplyChanges();
        }

        private void SyncPropertiesFromControls()
        {
            ThicknessValue = (float)nudThickness.Value;
            BorderWidthValue = (float)nudBorderWidth.Value;
            HeadLengthValue = (float)nudHeadLength.Value;
            HeadWidthValue = (float)nudHeadWidth.Value;
            IsLocked = chkLocked.Checked;
        }

        private void TryApplyChanges()
        {
            if (ApplyChanges == null || suppressAutoApply)
            {
                return;
            }

            SyncPropertiesFromControls();
            ApplyChanges?.Invoke();
        }
    }
    public class SplitDocumentDialog : Form
    {
        private Label lblFile;
        private TextBox txtFilePath;
        private Button btnBrowse;
        private Label lblPageCount;
        private Label lblPages;
        private TextBox txtPageNumbers;
        private Label lblStep;
        private NumericUpDown nudStep;
        private Button btnOK;
        private Button btnCancel;

        // Properties returning selected data
        public string SelectedFile { get; private set; }
        public List<int> PageNumbers { get; private set; } = new List<int>();
        public int DocumentPageCount { get; private set; }  // Number of pages in the PDF
        public int Step { get; private set; }  // Split step

        public SplitDocumentDialog(int numPages = 0, string defaultFile = "")
        {
            DocumentPageCount = numPages;
            SelectedFile = defaultFile;
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            this.Text = Resources.Split_Title;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterParent;
            this.MaximizeBox = false;
                        this.MinimizeBox = false;
            this.AutoScroll = true;
            int maxH = Screen.GetWorkingArea(this).Height - 80;
            if (this.Height > maxH) this.Height = maxH;
            this.Width = PDFForm.ScaleForDpiStatic(400);
            this.Height = PDFForm.ScaleForDpiStatic(280);

            // Label for file
            lblFile = new Label
            {
                Text = Resources.Split_FileLabel,
                AutoSize = true,
                Location = new Point(PDFForm.ScaleForDpiStatic(10), PDFForm.ScaleForDpiStatic(20))
            };

            // Text field with selected path (read-only)
            txtFilePath = new TextBox
            {
                Location = new Point(PDFForm.ScaleForDpiStatic(10), PDFForm.ScaleForDpiStatic(45)),
                Width = PDFForm.ScaleForDpiStatic(240),
                Text = SelectedFile,
                ReadOnly = true
            };

            // Label to display page count
            lblPageCount = new Label
            {
                Text = string.Format(Resources.Split_PageCountLabel, DocumentPageCount),
                AutoSize = true,
                Location = new Point(PDFForm.ScaleForDpiStatic(10), PDFForm.ScaleForDpiStatic(75))
            };

            txtPageNumbers = new TextBox
            {
                Location = new Point(PDFForm.ScaleForDpiStatic(10), PDFForm.ScaleForDpiStatic(125)),
                Width = PDFForm.ScaleForDpiStatic(360)
            };

            // Label for the step value
            lblStep = new Label
            {
                Text = Resources.Split_StepLabel,
                AutoSize = true,
                Location = new Point(PDFForm.ScaleForDpiStatic(10), PDFForm.ScaleForDpiStatic(160))
            };

            nudStep = new NumericUpDown
            {
                Location = new Point(PDFForm.ScaleForDpiStatic(140), PDFForm.ScaleForDpiStatic(160)),
                Minimum = 0,
                Width = PDFForm.ScaleForDpiStatic(50),
                Value = 0
            };

            // "Browse" button to open OpenFileDialog
            btnBrowse = new Button
            {
                Text = Resources.Split_Browse,
                Location = new Point(PDFForm.ScaleForDpiStatic(260), PDFForm.ScaleForDpiStatic(43)),
                Width = PDFForm.ScaleForDpiStatic(110),
                Height = PDFForm.ScaleForDpiStatic(28)
            };
            btnBrowse.Click += BtnBrowse_Click;

            // Text field for entering page numbers
            txtPageNumbers = new TextBox
            {
                Location = new Point(PDFForm.ScaleForDpiStatic(10), PDFForm.ScaleForDpiStatic(125)),
                Width = PDFForm.ScaleForDpiStatic(360)
            };

            nudStep = new NumericUpDown
            {
                Location = new Point(PDFForm.ScaleForDpiStatic(130), PDFForm.ScaleForDpiStatic(160)),
                Minimum = 0,
                Width = PDFForm.ScaleForDpiStatic(70),
                Value = 0
            };

            // OK button
            btnOK = new Button
            {
                Text = Resources.Merge_OK,
                Location = new Point(PDFForm.ScaleForDpiStatic(190), PDFForm.ScaleForDpiStatic(200)),
                Width = PDFForm.ScaleForDpiStatic(80),
                Height = PDFForm.ScaleForDpiStatic(28),
                DialogResult = DialogResult.OK
            };

            // Label to display page count
            lblPageCount = new Label
            {
                Text = string.Format(Resources.Split_PageCountLabel, DocumentPageCount),
                AutoSize = true,
                Location = new Point(PDFForm.ScaleForDpiStatic(10), PDFForm.ScaleForDpiStatic(75))
            };

            // Label for page numbers to split
            lblPages = new Label
            {
                Text = Resources.Split_PagesLabel,
                AutoSize = true,
                Location = new Point(PDFForm.ScaleForDpiStatic(10), PDFForm.ScaleForDpiStatic(100))
            };

            // OK button
            btnOK = new Button
            {
                Text = Resources.Merge_OK,
                Location = new Point(PDFForm.ScaleForDpiStatic(190), PDFForm.ScaleForDpiStatic(200)),
                Width = PDFForm.ScaleForDpiStatic(80),
                Height = PDFForm.ScaleForDpiStatic(28),
                DialogResult = DialogResult.OK
            };
            btnOK.Click += BtnOK_Click;

            // Cancel button
            btnCancel = new Button
            {
                Text = Resources.Merge_Cancel,
                Location = new Point(PDFForm.ScaleForDpiStatic(280), PDFForm.ScaleForDpiStatic(200)),
                Width = PDFForm.ScaleForDpiStatic(80),
                Height = PDFForm.ScaleForDpiStatic(28),
                DialogResult = DialogResult.Cancel
            };

            // Add controls to the form
            this.Controls.Add(lblFile);
            this.Controls.Add(txtFilePath);
            this.Controls.Add(btnBrowse);
            this.Controls.Add(lblPageCount);
            this.Controls.Add(lblPages);
            this.Controls.Add(txtPageNumbers);
            this.Controls.Add(lblStep);
            this.Controls.Add(nudStep);
            this.Controls.Add(btnOK);
            this.Controls.Add(btnCancel);

            this.AcceptButton = btnOK;
            this.CancelButton = btnCancel;
        }

        private void BtnBrowse_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = Resources.Dialog_Filter_PDF;
                openFileDialog.Title = Resources.Split_Dlg_Title;

                if (openFileDialog.ShowDialog(this) == DialogResult.OK)
                {
                    txtFilePath.Text = openFileDialog.FileName;
                    SelectedFile = openFileDialog.FileName;

                    // After selecting PDF file, display number of pages
                    try
                    {
                        var props = new ReaderProperties();
                        //if (!string.IsNullOrEmpty(userPassword))
                        //{

                        //}
                        using (PdfReader reader = new PdfReader(SelectedFile, props).SetUnethicalReading(Properties.Settings.Default.IgnorePdfRestrictions))
                        using (iText.Kernel.Pdf.PdfDocument pdfDoc = new iText.Kernel.Pdf.PdfDocument(reader))
                        {
                            DocumentPageCount = pdfDoc.GetNumberOfPages();
                        }
                        lblPageCount.Text = string.Format(Resources.Split_PageCountLabel, DocumentPageCount);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(this, string.Format(Resources.Split_Err_ReadFile, ex.Message), Resources.Title_Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        lblPageCount.Text = Resources.Split_PageCountUnknown;
                        DocumentPageCount = 0;
                    }
                }
            }
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            // Validation - whether file was selected and exists
            if (string.IsNullOrEmpty(SelectedFile) || !File.Exists(SelectedFile))
            {
                MessageBox.Show(this, Resources.Split_Err_SelectFile, Resources.Title_Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.DialogResult = DialogResult.None;
                return;
            }

            // Validation - whether page numbers for splitting were entered
            string input = txtPageNumbers.Text;
            var numbers = input.Split(new char[] { ',', ';', ' ' }, StringSplitOptions.RemoveEmptyEntries);
            List<int> parsedNumbers = new List<int>();
            foreach (var numStr in numbers)
            {
                if (int.TryParse(numStr.Trim(), out int num) && num > 0)
                {
                    parsedNumbers.Add(num);
                }
                else
                {
                    MessageBox.Show(this, string.Format(Resources.Err_InvalidPageNumberValue, numStr), Resources.Title_Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.DialogResult = DialogResult.None;
                    return;
                }
            }

            // Ensure page numbers are unique and sorted
            PageNumbers = parsedNumbers.Distinct().OrderBy(n => n).ToList();
            Step = (int)nudStep.Value;
            if (PageNumbers.Count == 0 && Step == 0)
            {
                MessageBox.Show(this, Resources.Delete_Err_NoData, Resources.Title_Warning, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.DialogResult = DialogResult.None;
                return;
            } else
            {
                this.Close();
            }
        }
    }



    public partial class MergeFilesForm : Form
    {
        private const string MergeFilesDragDataFormat = "AnonPDF.MergeFiles";
        private BindingList<string> pdfFiles = new BindingList<string>();
        private ListBox listBoxFiles;
        private Button buttonAddFiles;
        private Button buttonAddDirectory;
        private Button buttonRemove;
        private Button buttonUp;
        private Button buttonDown;
        private Button buttonClearAll;
        private Button buttonMerge;
        private Button buttonCancel;
        private Point listBoxDragStartPoint;
        private int listBoxDragStartIndex = -1;
        private DialogTheme dialogTheme;

        public MergeFilesForm()
        {
            this.FormClosing += MergeFilesForm_FormClosing;
            InitializeComponent();
            listBoxFiles.DataSource = pdfFiles;
        }

        private void InitializeComponent()
        {
            this.Text = Resources.Merge_Title;
            this.Size = PDFForm.ScaleSizeForDpiStatic(580, 400);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.MaximizeBox = false;
            this.MinimizeBox = false;


            listBoxFiles = new MergeFilesListBox
            {
                Location = new System.Drawing.Point(PDFForm.ScaleForDpiStatic(20), PDFForm.ScaleForDpiStatic(20)),
                Size = PDFForm.ScaleSizeForDpiStatic(400, 280),
                HorizontalScrollbar = true,
                SelectionMode = SelectionMode.MultiExtended,
                AllowDrop = true,
                DrawMode = DrawMode.OwnerDrawFixed
            };

            buttonAddFiles = new Button { Text = Resources.Merge_AddFiles, Location = new System.Drawing.Point(PDFForm.ScaleForDpiStatic(440), PDFForm.ScaleForDpiStatic(20)), Width = PDFForm.ScaleForDpiStatic(100), Height = PDFForm.ScaleForDpiStatic(28) };
            buttonAddDirectory = new Button { Text = Resources.Merge_AddDirectory, Location = new System.Drawing.Point(PDFForm.ScaleForDpiStatic(440), PDFForm.ScaleForDpiStatic(60)), Width = PDFForm.ScaleForDpiStatic(100), Height = PDFForm.ScaleForDpiStatic(28) };
            buttonRemove = new Button { Text = Resources.Merge_RemoveSelected, Location = new System.Drawing.Point(PDFForm.ScaleForDpiStatic(440), PDFForm.ScaleForDpiStatic(100)), Width = PDFForm.ScaleForDpiStatic(100), Height = PDFForm.ScaleForDpiStatic(28) };
            buttonClearAll = new Button { Text = Resources.Merge_ClearList, Location = new System.Drawing.Point(PDFForm.ScaleForDpiStatic(440), PDFForm.ScaleForDpiStatic(140)), Width = PDFForm.ScaleForDpiStatic(100), Height = PDFForm.ScaleForDpiStatic(28) };
            buttonUp = new Button { Text = Resources.Merge_Up, Location = new System.Drawing.Point(PDFForm.ScaleForDpiStatic(440), PDFForm.ScaleForDpiStatic(180)), Width = PDFForm.ScaleForDpiStatic(100), Height = PDFForm.ScaleForDpiStatic(28) };
            buttonDown = new Button { Text = Resources.Merge_Down, Location = new System.Drawing.Point(PDFForm.ScaleForDpiStatic(440), PDFForm.ScaleForDpiStatic(220)), Width = PDFForm.ScaleForDpiStatic(100), Height = PDFForm.ScaleForDpiStatic(28) };
            buttonMerge = new Button { Text = Resources.Merge_OK, Location = new System.Drawing.Point(PDFForm.ScaleForDpiStatic(300), PDFForm.ScaleForDpiStatic(320)), Width = PDFForm.ScaleForDpiStatic(100), Height = PDFForm.ScaleForDpiStatic(28) };
            buttonCancel = new Button { Text = Resources.Merge_Cancel, Location = new System.Drawing.Point(PDFForm.ScaleForDpiStatic(440), PDFForm.ScaleForDpiStatic(320)), Width = PDFForm.ScaleForDpiStatic(100), Height = PDFForm.ScaleForDpiStatic(28), DialogResult = DialogResult.Cancel };

            this.Controls.Add(listBoxFiles);
            this.Controls.Add(buttonAddFiles);
            this.Controls.Add(buttonAddDirectory);
            this.Controls.Add(buttonRemove);
            this.Controls.Add(buttonClearAll);
            this.Controls.Add(buttonUp);
            this.Controls.Add(buttonDown);
            this.Controls.Add(buttonMerge);
            this.Controls.Add(buttonCancel);

            buttonAddFiles.Click += ButtonAddFiles_Click;
            buttonAddDirectory.Click += ButtonAddDirectory_Click;
            buttonRemove.Click += ButtonRemove_Click;
            buttonClearAll.Click += ButtonClearAll_Click;
            buttonUp.Click += ButtonUp_Click;
            buttonDown.Click += ButtonDown_Click;
            buttonMerge.Click += ButtonMerge_Click;
            buttonCancel.Click += ButtonCancel_Click;
            listBoxFiles.MouseDown += ListBoxFiles_MouseDown;
            listBoxFiles.MouseMove += ListBoxFiles_MouseMove;
            listBoxFiles.DragOver += ListBoxFiles_DragOver;
            listBoxFiles.DragDrop += ListBoxFiles_DragDrop;
            listBoxFiles.DrawItem += ListBoxFiles_DrawItem;

            this.CancelButton = buttonCancel;
            this.AcceptButton = null;
        }

        internal void ApplyDialogTheme(DialogTheme theme)
        {
            dialogTheme = theme;
            DialogThemeApplier.ApplyTo(this, theme);
            listBoxFiles?.Invalidate();
        }

        private void MergeFilesForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                this.Hide();
                this.Owner?.Activate();
            }
        }

        private void ButtonAddFiles_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog()
            {
                Filter = Resources.Dialog_Filter_PDF,
                Multiselect = true
            };
            if (ofd.ShowDialog(this) == DialogResult.OK)
            {
                foreach (var file in ofd.FileNames)
                {
                    if (!pdfFiles.Contains(file))
                        pdfFiles.Add(file);
                }
            }
        }

        private void ButtonAddDirectory_Click(object sender, EventArgs e)
        {
            using (var dlg = new FolderBrowserDialog())
            {
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    var files = Directory.GetFiles(dlg.SelectedPath, "*.pdf");
                    foreach (var file in files)
                    {
                        if (!pdfFiles.Contains(file))
                            pdfFiles.Add(file);
                    }
                }
            }
        }

        private void ButtonRemove_Click(object sender, EventArgs e)
        {
            var selectedItems = listBoxFiles.SelectedItems.Cast<string>().ToList();
            foreach (var item in selectedItems)
            {
                pdfFiles.Remove(item);
            }
        }

        private void ButtonClearAll_Click(object sender, EventArgs e)
        {
            pdfFiles.Clear();
        }

        private void ButtonUp_Click(object sender, EventArgs e)
        {
            if (listBoxFiles.SelectedItems.Count == 0) return;

            var selected = listBoxFiles.SelectedItems.Cast<string>().ToList();
            var indices = selected.Select(item => pdfFiles.IndexOf(item)).Where(i => i > 0).OrderBy(i => i).ToList();

            var moved = new HashSet<string>(selected);
            for (int i = 1; i < pdfFiles.Count; i++)
            {
                if (moved.Contains(pdfFiles[i]) && !moved.Contains(pdfFiles[i - 1]))
                {
                    string temp = pdfFiles[i - 1];
                    pdfFiles[i - 1] = pdfFiles[i];
                    pdfFiles[i] = temp;
                }
            }

            ReselectItems(selected);
        }

        private void ButtonDown_Click(object sender, EventArgs e)
        {
            if (listBoxFiles.SelectedItems.Count == 0) return;
            var selected = listBoxFiles.SelectedItems.Cast<string>().ToList();
            var indices = selected.Select(item => pdfFiles.IndexOf(item)).Where(i => i < pdfFiles.Count - 1).OrderByDescending(i => i).ToList();

            foreach (var i in indices)
            {
                var temp = pdfFiles[i + 1];
                pdfFiles[i + 1] = pdfFiles[i];
                pdfFiles[i] = temp;
            }

            ReselectItems(selected);
        }

        private void ReselectItems(List<string> selected)
        {
            listBoxFiles.ClearSelected(); // pierwszy
            this.BeginInvoke(new System.Action(() =>
            {
                listBoxFiles.ClearSelected(); // second inside UI queue
                foreach (var item in selected)
                {
                    int idx = pdfFiles.IndexOf(item);
                    if (idx >= 0)
                        listBoxFiles.SetSelected(idx, true);
                }
            }));
        }

        private void ListBoxFiles_MouseDown(object sender, MouseEventArgs e)
        {
            listBoxDragStartIndex = listBoxFiles.IndexFromPoint(e.Location);
            listBoxDragStartPoint = e.Location;
        }

        private void ListBoxFiles_MouseMove(object sender, MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) != MouseButtons.Left ||
                listBoxDragStartIndex < 0 ||
                listBoxFiles.SelectedItems.Count == 0)
            {
                return;
            }

            var dragRect = new Rectangle(
                listBoxDragStartPoint.X - SystemInformation.DragSize.Width / 2,
                listBoxDragStartPoint.Y - SystemInformation.DragSize.Height / 2,
                SystemInformation.DragSize.Width,
                SystemInformation.DragSize.Height);

            if (dragRect.Contains(e.Location))
            {
                return;
            }

            var selectedItems = listBoxFiles.SelectedItems.Cast<string>().ToList();
            listBoxFiles.DoDragDrop(new DataObject(MergeFilesDragDataFormat, selectedItems), DragDropEffects.Move);
            listBoxDragStartIndex = -1;
        }

        private void ListBoxFiles_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = e.Data.GetDataPresent(MergeFilesDragDataFormat)
                ? DragDropEffects.Move
                : DragDropEffects.None;
        }

        private void ListBoxFiles_DragDrop(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(MergeFilesDragDataFormat))
            {
                return;
            }

            var selectedItems = e.Data.GetData(MergeFilesDragDataFormat) as List<string>;
            if (selectedItems == null || selectedItems.Count == 0)
            {
                return;
            }

            Point clientPoint = listBoxFiles.PointToClient(new Point(e.X, e.Y));
            int targetIndex = GetDropTargetIndex(clientPoint);
            MoveSelectedFilesToIndex(selectedItems, targetIndex);
        }

        private int GetDropTargetIndex(Point clientPoint)
        {
            int index = listBoxFiles.IndexFromPoint(clientPoint);
            if (index < 0)
            {
                return pdfFiles.Count;
            }

            Rectangle itemRect = listBoxFiles.GetItemRectangle(index);
            if (clientPoint.Y > itemRect.Top + itemRect.Height / 2)
            {
                index++;
            }

            return Math.Max(0, Math.Min(index, pdfFiles.Count));
        }

        private void MoveSelectedFilesToIndex(List<string> selectedItems, int targetIndex)
        {
            var selectedSet = new HashSet<string>(selectedItems);
            var selectedIndices = pdfFiles
                .Select((file, index) => new { file, index })
                .Where(x => selectedSet.Contains(x.file))
                .Select(x => x.index)
                .ToList();

            if (selectedIndices.Count == 0)
            {
                return;
            }

            int adjustedTargetIndex = targetIndex - selectedIndices.Count(i => i < targetIndex);
            var orderedItems = selectedIndices.Select(i => pdfFiles[i]).ToList();

            for (int i = selectedIndices.Count - 1; i >= 0; i--)
            {
                pdfFiles.RemoveAt(selectedIndices[i]);
            }

            adjustedTargetIndex = Math.Max(0, Math.Min(adjustedTargetIndex, pdfFiles.Count));
            for (int i = 0; i < orderedItems.Count; i++)
            {
                pdfFiles.Insert(adjustedTargetIndex + i, orderedItems[i]);
            }

            ReselectItems(orderedItems);
        }

        private void ListBoxFiles_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0 || e.Index >= listBoxFiles.Items.Count)
            {
                return;
            }

            bool selected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;
            System.Drawing.Color backColor = selected
                ? (dialogTheme != null ? dialogTheme.SelectionBackColor : SystemColors.Highlight)
                : listBoxFiles.BackColor;
            System.Drawing.Color foreColor = selected
                ? (dialogTheme != null ? dialogTheme.SelectionForeColor : SystemColors.HighlightText)
                : listBoxFiles.ForeColor;

            using (var backBrush = new SolidBrush(backColor))
            {
                e.Graphics.FillRectangle(backBrush, e.Bounds);
            }

            string text = listBoxFiles.Items[e.Index]?.ToString() ?? string.Empty;
            Rectangle textRect = new Rectangle(
                e.Bounds.Left + PDFForm.ScaleForDpiStatic(3),
                e.Bounds.Top,
                Math.Max(0, e.Bounds.Width - PDFForm.ScaleForDpiStatic(6)),
                e.Bounds.Height);

            TextRenderer.DrawText(
                e.Graphics,
                text,
                e.Font,
                textRect,
                foreColor,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.SingleLine | TextFormatFlags.NoPrefix);

            if ((e.State & DrawItemState.Focus) == DrawItemState.Focus)
            {
                e.DrawFocusRectangle();
            }
        }

        private sealed class MergeFilesListBox : ListBox
        {
            private const int WM_LBUTTONDOWN = 0x0201;
            private const int WM_LBUTTONUP = 0x0202;
            private const int WM_MOUSEMOVE = 0x0200;

            private int pendingSingleSelectIndex = -1;
            private Point pendingSingleSelectPoint;

            protected override void WndProc(ref Message m)
            {
                if (m.Msg == WM_LBUTTONDOWN)
                {
                    Point point = PointFromLParam(m.LParam);
                    int index = IndexFromPoint(point);
                    Keys modifiers = Control.ModifierKeys & (Keys.Shift | Keys.Control);

                    if (index >= 0 &&
                        modifiers == Keys.None &&
                        SelectedIndices.Count > 1 &&
                        GetSelected(index))
                    {
                        pendingSingleSelectIndex = index;
                        pendingSingleSelectPoint = point;
                        Focus();
                        Capture = true;
                        OnMouseDown(new MouseEventArgs(MouseButtons.Left, 1, point.X, point.Y, 0));
                        return;
                    }
                }
                else if (m.Msg == WM_MOUSEMOVE && pendingSingleSelectIndex >= 0)
                {
                    Point point = PointFromLParam(m.LParam);
                    Rectangle dragRect = new Rectangle(
                        pendingSingleSelectPoint.X - SystemInformation.DragSize.Width / 2,
                        pendingSingleSelectPoint.Y - SystemInformation.DragSize.Height / 2,
                        SystemInformation.DragSize.Width,
                        SystemInformation.DragSize.Height);

                    if (!dragRect.Contains(point))
                    {
                        pendingSingleSelectIndex = -1;
                    }
                }
                else if (m.Msg == WM_LBUTTONUP && pendingSingleSelectIndex >= 0)
                {
                    Point point = PointFromLParam(m.LParam);
                    int index = pendingSingleSelectIndex;
                    pendingSingleSelectIndex = -1;

                    if (index >= 0 && index < Items.Count)
                    {
                        ClearSelected();
                        SetSelected(index, true);
                    }

                    Capture = false;
                    OnMouseUp(new MouseEventArgs(MouseButtons.Left, 1, point.X, point.Y, 0));
                    return;
                }

                base.WndProc(ref m);
            }

            private static Point PointFromLParam(IntPtr lParam)
            {
                int value = lParam.ToInt32();
                return new Point(
                    unchecked((short)(value & 0xffff)),
                    unchecked((short)((value >> 16) & 0xffff)));
            }
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            this.Hide();
            this.Owner?.Activate();
        }

        private async void ButtonMerge_Click(object sender, EventArgs e)
        {
            if (pdfFiles.Count == 0)
            {
                MessageBox.Show(this, Resources.Msg_FileListEmpty, Resources.Title_Warning, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            // First, validate all files
            List<string> lockedFiles = new List<string>();
            foreach (var file in pdfFiles)
            {
                try
                {
                    using (var reader = new PdfReader(file))
                    using (var pdfDoc = new iText.Kernel.Pdf.PdfDocument(reader))
                    {
                        // try to get number of pages - this can already throw an exception
                        int pages = pdfDoc.GetNumberOfPages();

                        // try to copy one page to empty document in memory
                        using (var ms = new MemoryStream())
                        using (var tempWriter = new iText.Kernel.Pdf.PdfWriter(ms))
                        using (var tempDoc = new iText.Kernel.Pdf.PdfDocument(tempWriter))
                        {
                            // this line will throw an exception if the file has restrictions
                            pdfDoc.CopyPagesTo(1, Math.Min(1, pages), tempDoc);
                        }
                    }
                }
                catch (iText.Kernel.Exceptions.BadPasswordException)
                {
                    lockedFiles.Add(file);
                }
                catch (Exception ex)
                {
                    // if it's another error, also treat it as problem with this PDF
                    lockedFiles.Add(file + string.Format(GetLocalizedResourceText("Msg_ErrorSuffix"), ex.Message));
                }
            }

            if (lockedFiles.Count > 0)
            {
                // if any file is locked, don't merge
                string msg = GetLocalizedResourceText("Err_Merge_FilesHaveSecurity");
                foreach (var f in lockedFiles)
                {
                    msg += "- " + Path.GetFileName(f) + "\n";
                }
                MessageBox.Show(this, msg, Resources.Title_Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // if all files are OK, only then ask for destination
            SaveFileDialog sfd = new SaveFileDialog { Filter = Resources.Dialog_Filter_PDF };
            if (sfd.ShowDialog(this) != DialogResult.OK)
            {
                return;
            }

            buttonMerge.Enabled = false;
            this.Cursor = Cursors.WaitCursor;
            string destination = sfd.FileName;
            var filesToMerge = pdfFiles.ToList(); // snapshot for background task

            try
            {
                await Task.Run(() =>
                {
                    using (var writer = new iText.Kernel.Pdf.PdfWriter(destination))
                    using (var mergedDoc = new iText.Kernel.Pdf.PdfDocument(writer))
                    {
                        foreach (var file in filesToMerge)
                        {
                            using (var reader = new PdfReader(file))
                            using (var pdfDoc = new iText.Kernel.Pdf.PdfDocument(reader))
                            {
                                pdfDoc.CopyPagesTo(1, pdfDoc.GetNumberOfPages(), mergedDoc);
                            }
                        }

                        PDFForm.ApplyDemoWatermarkIfNeeded(mergedDoc);
                    }
                });

                MessageBox.Show(this, Resources.Merge_Success, Resources.Title_Success, MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Hide();
                this.Owner?.Activate();

                try
                {
                    if (this.Owner is PDFForm parentForm)
                    {
                        parentForm.ExitFullScreenIfNeeded();
                    }

                    var psi = new ProcessStartInfo
                    {
                        FileName = destination,
                        UseShellExecute = true
                    };
                    Process.Start(psi);
                }
                catch (System.ComponentModel.Win32Exception wex)
                {
                    MessageBox.Show(this, string.Format(Resources.Err_NoAssociatedPdfApp, wex.Message), Resources.Title_Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, string.Format(Resources.Merge_Err_Merge, ex.Message), Resources.Title_Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
                buttonMerge.Enabled = true;
            }
        }

        private static string GetLocalizedResourceText(string key)
        {
            var culture = Resources.Culture ?? CultureInfo.CurrentUICulture;
            string text = Resources.ResourceManager.GetString(key, culture);
            return string.IsNullOrWhiteSpace(text) ? key : text;
        }

    }

    public class ZoomPanel : Panel
    {
        public ZoomPanel()
        {
            DoubleBuffered = true;
            TabStop = true;
            ResizeRedraw = true;
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.Selectable, true);
            UpdateStyles();
        }

        protected override bool IsInputKey(Keys keyData)
        {
            Keys keyCode = keyData & Keys.KeyCode;
            if (keyCode == Keys.Left || keyCode == Keys.Right || keyCode == Keys.Up || keyCode == Keys.Down)
            {
                return true;
            }

            return base.IsInputKey(keyData);
        }

        protected override void WndProc(ref Message m)
        {
            const int WM_MOUSEWHEEL = 0x020A;
            if (m.Msg == WM_MOUSEWHEEL)
            {
                // Get delta value and cursor position within control
                int delta = (short)((long)m.WParam >> 16);
                Point mousePos = this.PointToClient(Cursor.Position);
                MouseEventArgs args = new MouseEventArgs(MouseButtons.None, 0, mousePos.X, mousePos.Y, delta);

                // Find parent form and call method handling event
                PDFForm pdfForm = FindForm() as PDFForm;
                if (pdfForm is PDFForm)
                {
                    pdfForm.Panel2_MouseWheel(args);
                }

                // If CTRL is pressed, "eat" message â€“ don't pass to base.WndProc
                if ((Control.ModifierKeys & Keys.Control) == Keys.Control)
                    return;
            }
            base.WndProc(ref m);
        }

    }

    public enum LocationSource
    {
        Normal,      // text within page bounds
        OutOfBounds, // text outside page boundary (hidden in render but present in content stream)
        AltText,     // accessibility alt-text of a graphical object
    }

    // Class representing text occurrence location
    public class TextLocation
    {
        public int PageNumber { get; set; }
        public int PageRotation { get; set; }
        public iText.Kernel.Geom.Rectangle Rect { get; set; }
        public bool IsOcr { get; set; }
        public bool IsExactOcrWord { get; set; }
        /// <summary>Entity label assigned by the NER plugin (e.g. PESEL, NIP, PERSON). Null for regex-detected locations.</summary>
        public string Label { get; set; }
        /// <summary>Matched text shown in the Found tab tree (entity text for NER, search term for regular search).</summary>
        public string Text { get; set; }
        /// <summary>Where in the document this location originates from.</summary>
        public LocationSource Source { get; set; } = LocationSource.Normal;
        /// <summary>True when more than one PII type was detected; Label holds the highest-priority one.</summary>
        public bool HasMultipleLabels { get; set; }
        /// <summary>Precise highlight rectangles for multi-chunk text. Rect remains the union used for navigation and sorting.</summary>
        public List<iText.Kernel.Geom.Rectangle> HighlightRects { get; set; }

        public TextLocation(int pageNumber, int pageRotation, iText.Kernel.Geom.Rectangle rect, bool isOcr = false, bool isExactOcrWord = false)
        {
            PageNumber = pageNumber;
            PageRotation = pageRotation;
            Rect = rect;
            IsOcr = isOcr;
            IsExactOcrWord = isExactOcrWord;
        }

        public IEnumerable<iText.Kernel.Geom.Rectangle> GetHighlightRects()
        {
            if (HighlightRects != null && HighlightRects.Count > 0)
            {
                return HighlightRects.Where(rect => rect != null && rect.GetWidth() > 0f && rect.GetHeight() > 0f);
            }

            return Rect != null
                ? new[] { Rect }
                : Enumerable.Empty<iText.Kernel.Geom.Rectangle>();
        }

        public override string ToString()
        {
            var culture = Properties.Resources.Culture ?? CultureInfo.CurrentUICulture;
            var format = Properties.Resources.ResourceManager.GetString("TextLocation_ToStringFormat", culture);
            if (string.IsNullOrWhiteSpace(format))
            {
                format = "TextLocation_ToStringFormat";
            }
            return string.Format(format, PageNumber, Rect);
        }
    }

    public class ThemedDataGridView : DataGridView
    {
        public ThemedDataGridView()
        {
            DoubleBuffered = true;
            ResizeRedraw = true;
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
            UpdateStyles();
        }
    }



    public class RedactionBlock
    {
        public System.Drawing.RectangleF Bounds { get; set; }
        public int PageNumber { get; set; }
        public string LayerId { get; set; }
        public int? FootnoteNumber { get; set; }
        public string ScopeId { get; set; }
        public List<string> BasisIds { get; set; }
        public string ClassificationSource { get; set; }
        public string MatchedTag { get; set; }
        public string InterestSubject { get; set; }
        public bool IsMarkerSelection { get; set; }
        public string DuplicateGroupId { get; set; }
        public DateTime CreatedAtUtc { get; set; }
        public DateTime UpdatedAtUtc { get; set; }
        [JsonIgnore]
        public List<System.Drawing.RectangleF> PreviewTextRectsPdf { get; set; }

        public bool IsCursorSelection { get; set; }

        public RedactionBlock()
        {
            LayerId = PDFForm.DefaultLayerId;
            FootnoteNumber = null;
            ScopeId = null;
            BasisIds = new List<string>();
            ClassificationSource = "none";
            MatchedTag = null;
            InterestSubject = null;
            IsMarkerSelection = false;
            IsCursorSelection = false;
            DuplicateGroupId = null;
            CreatedAtUtc = DateTime.MinValue;
            UpdatedAtUtc = DateTime.MinValue;
            PreviewTextRectsPdf = null;
        }

        public RedactionBlock(System.Drawing.RectangleF bounds, int pageNumber)
            : this()
        {
            Bounds = bounds;
            PageNumber = pageNumber;
            CreatedAtUtc = DateTime.UtcNow;
            UpdatedAtUtc = DateTime.UtcNow;
        }
    }

    public class CommentAnnotation
    {
        public string Id { get; set; }
        public int PageNumber { get; set; }
        public string LayerId { get; set; }
        public string AltText { get; set; }
        public RectangleF Bounds { get; set; }
        public string CommentText { get; set; }
        public int HighlightColorArgb { get; set; } = System.Drawing.Color.FromArgb(255, 235, 59).ToArgb();
        public int TextColorArgb { get; set; } = System.Drawing.Color.Black.ToArgb();
        public bool IsMarkerSelection { get; set; }
        public float? NoteX { get; set; }
        public float? NoteY { get; set; }
        public float? NoteWidth { get; set; }
        public float? NoteHeight { get; set; }
        public string DuplicateGroupId { get; set; }
        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAtUtc { get; set; } = DateTime.UtcNow;

        public CommentAnnotation()
        {
            Id = Guid.NewGuid().ToString("N");
            LayerId = PDFForm.DefaultLayerId;
            CommentText = string.Empty;
            DuplicateGroupId = null;
        }
    }

    public enum SignatureVerificationStatus
    {
        Indeterminate = 0,
        Valid = 1,
        Invalid = 2,
        Unreadable = 3
    }

    public class SignatureWidgetInfo
    {
        public int PageNumber { get; set; }
        public RectangleF Bounds { get; set; }
        public byte[] NeutralAppearancePdfBytes { get; set; }
    }

    public class SignatureInfo
    {
        public string FieldName { get; set; }
        public string SignerName { get; set; }
        public string SignerTitle { get; set; }
        public string SignerOrganization { get; set; }
        public DateTime SignDate { get; set; }
        public bool IsReadable { get; set; } = true;
        public SignatureVerificationStatus VerificationStatus { get; set; } = SignatureVerificationStatus.Indeterminate;
        public List<SignatureWidgetInfo> Widgets { get; set; } = new List<SignatureWidgetInfo>();
    }

    public class SelectSignaturesDialog : Form
    {
        private readonly ListView listView;
        private readonly Button btnSelectAll;
        private readonly Button btnDeselectAll;
        private readonly Button btnOK;
        private readonly Button btnCancel;

        public List<string> SelectedFieldNames { get; private set; } = new List<string>();

        private static string GetVerificationStatusText(SignatureVerificationStatus status)
        {
            switch (status)
            {
                case SignatureVerificationStatus.Valid:
                    return Resources.Signatures_Verification_Valid;
                case SignatureVerificationStatus.Invalid:
                    return Resources.Signatures_Verification_Invalid;
                case SignatureVerificationStatus.Unreadable:
                    return Resources.Signatures_Unreadable;
                default:
                    return Resources.Signatures_Verification_Indeterminate;
            }
        }

        public SelectSignaturesDialog(List<SignatureInfo> signatures, IEnumerable<string> preselectedFields)
        {
            this.Text = Resources.Signatures_Select_Title;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterParent;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Width = PDFForm.ScaleForDpiStatic(720);
            this.Height = PDFForm.ScaleForDpiStatic(360);

            listView = new ListView
            {
                View = View.Details,
                CheckBoxes = true,
                FullRowSelect = true,
                GridLines = true,
                Location = new Point(PDFForm.ScaleForDpiStatic(10), PDFForm.ScaleForDpiStatic(10)),
                Size = PDFForm.ScaleSizeForDpiStatic(680, 260)
            };

            listView.Columns.Add(Resources.Signatures_Select_Column_Name, PDFForm.ScaleForDpiStatic(170));
            listView.Columns.Add(Resources.Signatures_Select_Column_Title, PDFForm.ScaleForDpiStatic(120));
            listView.Columns.Add(Resources.Signatures_Select_Column_Date, PDFForm.ScaleForDpiStatic(130));
            listView.Columns.Add(Resources.Signatures_Report_Field_Status, PDFForm.ScaleForDpiStatic(220));

            HashSet<string> preselected = null;
            if (preselectedFields != null)
            {
                preselected = new HashSet<string>(preselectedFields, StringComparer.OrdinalIgnoreCase);
            }

            foreach (SignatureInfo sig in signatures)
            {
                string signer = string.IsNullOrWhiteSpace(sig.SignerName) ? "-" : sig.SignerName;
                string title = string.IsNullOrWhiteSpace(sig.SignerTitle) ? "-" : sig.SignerTitle;
                string date = sig.SignDate == DateTime.MinValue ? "-" : sig.SignDate.ToString("g", CultureInfo.CurrentCulture);

                ListViewItem item = new ListViewItem(signer);
                item.SubItems.Add(title);
                item.SubItems.Add(date);
                item.SubItems.Add(GetVerificationStatusText(sig.VerificationStatus));
                item.Tag = sig.FieldName ?? string.Empty;
                item.Checked = preselected != null && preselected.Contains(sig.FieldName ?? string.Empty);
                listView.Items.Add(item);
            }

            btnSelectAll = new Button
            {
                Text = Resources.ResourceManager.GetString("Found_SelectAll", CultureInfo.CurrentUICulture) ?? "Select all",
                Location = new Point(PDFForm.ScaleForDpiStatic(10), PDFForm.ScaleForDpiStatic(280)),
                Size = PDFForm.ScaleSizeForDpiStatic(110, 30)
            };
            btnSelectAll.Click += (_, __) => SetAllItemsChecked(true);

            btnDeselectAll = new Button
            {
                Text = Resources.ResourceManager.GetString("Found_DeselectAll", CultureInfo.CurrentUICulture) ?? "Clear all",
                Location = new Point(PDFForm.ScaleForDpiStatic(130), PDFForm.ScaleForDpiStatic(280)),
                Size = PDFForm.ScaleSizeForDpiStatic(110, 30)
            };
            btnDeselectAll.Click += (_, __) => SetAllItemsChecked(false);

            btnOK = new Button
            {
                Text = Resources.Merge_OK,
                Location = new Point(PDFForm.ScaleForDpiStatic(520), PDFForm.ScaleForDpiStatic(280)),
                Size = PDFForm.ScaleSizeForDpiStatic(80, 30),
                DialogResult = DialogResult.OK
            };

            btnCancel = new Button
            {
                Text = Resources.Merge_Cancel,
                Location = new Point(PDFForm.ScaleForDpiStatic(610), PDFForm.ScaleForDpiStatic(280)),
                Size = PDFForm.ScaleSizeForDpiStatic(80, 30),
                DialogResult = DialogResult.Cancel
            };

            this.Controls.Add(listView);
            this.Controls.Add(btnSelectAll);
            this.Controls.Add(btnDeselectAll);
            this.Controls.Add(btnOK);
            this.Controls.Add(btnCancel);

            this.AcceptButton = btnOK;
            this.CancelButton = btnCancel;
        }

        private void SetAllItemsChecked(bool isChecked)
        {
            foreach (ListViewItem item in listView.Items)
            {
                item.Checked = isChecked;
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (this.DialogResult == DialogResult.OK)
            {
                SelectedFieldNames = listView.Items
                    .Cast<ListViewItem>()
                    .Where(item => item.Checked && item.Tag is string)
                    .Select(item => (string)item.Tag)
                    .Where(name => !string.IsNullOrWhiteSpace(name))
                    .ToList();
            }

            base.OnFormClosing(e);
        }
    }

    public enum LegalBasisSource
    {
        Global = 0,
        Local = 1
    }

    public enum ExclusionScopeSource
    {
        Global = 0,
        Local = 1
    }

    public class ExclusionScopesCatalogFile
    {
        [JsonProperty("version")]
        public string Version { get; set; }

        [JsonProperty("exclusion_scopes")]
        public List<ExclusionScopeDefinition> ExclusionScopes { get; set; }
    }

    public class ExclusionScopeDefinition
    {
        [JsonProperty("scope_id")]
        public string ScopeId { get; set; }

        /// <summary>English name used as fallback when no localized string is found.</summary>
        [JsonProperty("friendly_name")]
        public string FriendlyName { get; set; }

        /// <summary>Resource key for the localized display name (e.g. "Scope_SCOPE_PERSON").</summary>
        [JsonProperty("friendly_name_key")]
        public string FriendlyNameKey { get; set; }

        [JsonProperty("category")]
        public string Category { get; set; }

        /// <summary>English description used as fallback tooltip.</summary>
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>Resource key for the localized tooltip description (e.g. "ScopeDesc_SCOPE_PERSON").</summary>
        [JsonProperty("description_key")]
        public string DescriptionKey { get; set; }

        [JsonProperty("auto_detect_tags")]
        public List<string> AutoDetectTags { get; set; }

        [JsonProperty("default_basis_ids")]
        public List<string> DefaultBasisIds { get; set; }

        [JsonProperty("ui_color")]
        public string UiColor { get; set; }

        /// <summary>ISO language code (e.g. "de", "pl"). Null/empty = universal (shown for every document).</summary>
        [JsonProperty("language")]
        public string Language { get; set; }

        [JsonIgnore]
        public ExclusionScopeSource SourceKind { get; set; }

        /// <summary>Returns the localized display name: resource key lookup → friendly_name → scope_id.</summary>
        public string GetLocalizedName()
        {
            if (!string.IsNullOrEmpty(FriendlyNameKey))
            {
                string localized = AnonPDF.Properties.Resources.ResourceManager.GetString(
                    FriendlyNameKey,
                    AnonPDF.Properties.Resources.Culture ?? System.Globalization.CultureInfo.CurrentUICulture);
                if (!string.IsNullOrEmpty(localized))
                    return localized;
            }
            return string.IsNullOrEmpty(FriendlyName) ? (ScopeId ?? string.Empty) : FriendlyName;
        }

        /// <summary>Returns the localized tooltip description: resource key lookup → description → scope_id.</summary>
        public string GetLocalizedDescription()
        {
            if (!string.IsNullOrEmpty(DescriptionKey))
            {
                string localized = AnonPDF.Properties.Resources.ResourceManager.GetString(
                    DescriptionKey,
                    AnonPDF.Properties.Resources.Culture ?? System.Globalization.CultureInfo.CurrentUICulture);
                if (!string.IsNullOrEmpty(localized))
                    return localized;
            }
            return string.IsNullOrEmpty(Description) ? (ScopeId ?? string.Empty) : Description;
        }
    }

    public class LegalBasesCatalogFile
    {
        [JsonProperty("source")]
        public string Source { get; set; }

        [JsonProperty("legal_bases")]
        public List<LegalBasisDefinition> LegalBases { get; set; }
    }

    public class LegalBasisDefinition
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("full_citation")]
        public string FullCitation { get; set; }

        [JsonProperty("requires_interest_subject")]
        public bool RequiresInterestSubject { get; set; }

        [JsonProperty("description_hint")]
        public string DescriptionHint { get; set; }

        [JsonIgnore]
        public LegalBasisSource SourceKind { get; set; }
    }

    public class ProjectData
    {
        public List<RedactionBlock> RedactionBlocks { get; set; }
        public List<CommentAnnotation> CommentAnnotations { get; set; }
        public HashSet<int> PagesToRemove { get; set; }
        public List<TextAnnotation> TextAnnotations { get; set; }
        public List<RasterObject> RasterObjects { get; set; }
        public List<ArrowObject> ArrowObjects { get; set; }
        public List<VectorShapeObject> VectorShapes { get; set; }
        public List<LayerDefinition> Layers { get; set; }
        public string ActiveLayerId { get; set; }
        public List<ProjectObjectLayer> ObjectLayers { get; set; }
        public Dictionary<int, int> PageRotationOffsets { get; set; }
        public int CurrentPage { get; set; }
        public float? ZoomFactor { get; set; }
        public int? ScrollX { get; set; }
        public int? ScrollY { get; set; }
        public int? PagesListTopPage { get; set; }
        public int? ThumbnailsTopPage { get; set; }
        public List<string> SignaturesToRemove { get; set; }
        public string SignaturesMode { get; set; }
        public string SignatureAppearance { get; set; }
        public bool? AutoFootnotesEnabled { get; set; }
        public string ExclusionAuthority { get; set; }
        public bool ExportVisibleLayersOnly { get; set; }
        public DocumentMarginSettings DocumentMargins { get; set; }
        public String FilePath { get; set; }
        // Pending ALT text edits: posKey ("{page}:{x}:{y}") → new text
        public Dictionary<string, string> AltTextEdits { get; set; }
    }

    public class DocumentMarginSettings
    {
        public float TopMillimeters { get; set; }
        public float BottomMillimeters { get; set; }
        public float LeftMillimeters { get; set; }
        public float RightMillimeters { get; set; }

        [JsonIgnore]
        public bool HasMargins =>
            TopMillimeters > 0.001f ||
            BottomMillimeters > 0.001f ||
            LeftMillimeters > 0.001f ||
            RightMillimeters > 0.001f;

        public DocumentMarginSettings Clone()
        {
            return new DocumentMarginSettings
            {
                TopMillimeters = TopMillimeters,
                BottomMillimeters = BottomMillimeters,
                LeftMillimeters = LeftMillimeters,
                RightMillimeters = RightMillimeters
            };
        }
    }

    public sealed class DocumentSettingsDialog : Form
    {
        private readonly NumericUpDown topInput;
        private readonly NumericUpDown bottomInput;
        private readonly NumericUpDown leftInput;
        private readonly NumericUpDown rightInput;

        public DocumentMarginSettings Margins { get; private set; }

        public DocumentSettingsDialog(
            DocumentMarginSettings margins,
            string title,
            string marginsCaption,
            string topLabel,
            string bottomLabel,
            string leftLabel,
            string rightLabel,
            string millimetersLabel,
            string resetLabel,
            string okLabel,
            string cancelLabel)
        {
            Margins = (margins ?? new DocumentMarginSettings()).Clone();

            AutoScaleMode = AutoScaleMode.Dpi;
            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = title;
            Padding = new Padding(12);

            var root = new TableLayoutPanel
            {
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                ColumnCount = 1,
                RowCount = 2,
                Dock = DockStyle.Fill,
                Margin = Padding.Empty
            };
            root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            root.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            var group = new GroupBox
            {
                Text = marginsCaption,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                Dock = DockStyle.Top,
                Padding = new Padding(10)
            };

            var grid = new TableLayoutPanel
            {
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                ColumnCount = 3,
                RowCount = 4,
                Dock = DockStyle.Top,
                Margin = Padding.Empty
            };
            grid.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            grid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 92));
            grid.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

            topInput = CreateMarginInput(Margins.TopMillimeters);
            bottomInput = CreateMarginInput(Margins.BottomMillimeters);
            leftInput = CreateMarginInput(Margins.LeftMillimeters);
            rightInput = CreateMarginInput(Margins.RightMillimeters);

            AddMarginRow(grid, 0, topLabel, topInput, millimetersLabel);
            AddMarginRow(grid, 1, bottomLabel, bottomInput, millimetersLabel);
            AddMarginRow(grid, 2, leftLabel, leftInput, millimetersLabel);
            AddMarginRow(grid, 3, rightLabel, rightInput, millimetersLabel);
            group.Controls.Add(grid);

            var buttons = new FlowLayoutPanel
            {
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                Dock = DockStyle.Top,
                FlowDirection = FlowDirection.RightToLeft,
                WrapContents = false,
                Margin = new Padding(0, 10, 0, 0)
            };

            var cancelButton = new Button
            {
                Text = cancelLabel,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                MinimumSize = new Size(90, 28),
                DialogResult = DialogResult.Cancel,
                Margin = new Padding(6, 0, 0, 0)
            };
            var okButton = new Button
            {
                Text = okLabel,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                MinimumSize = new Size(90, 28),
                DialogResult = DialogResult.OK,
                Margin = new Padding(6, 0, 0, 0)
            };
            var resetButton = new Button
            {
                Text = resetLabel,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                MinimumSize = new Size(90, 28),
                Margin = Padding.Empty
            };
            resetButton.Click += (_, __) =>
            {
                topInput.Value = 0;
                bottomInput.Value = 0;
                leftInput.Value = 0;
                rightInput.Value = 0;
            };
            okButton.Click += (_, __) =>
            {
                Margins = new DocumentMarginSettings
                {
                    TopMillimeters = (float)topInput.Value,
                    BottomMillimeters = (float)bottomInput.Value,
                    LeftMillimeters = (float)leftInput.Value,
                    RightMillimeters = (float)rightInput.Value
                };
            };

            buttons.Controls.Add(cancelButton);
            buttons.Controls.Add(okButton);
            buttons.Controls.Add(resetButton);
            root.Controls.Add(group, 0, 0);
            root.Controls.Add(buttons, 0, 1);
            Controls.Add(root);

            AcceptButton = okButton;
            CancelButton = cancelButton;
        }

        private static NumericUpDown CreateMarginInput(float value)
        {
            return new NumericUpDown
            {
                DecimalPlaces = 1,
                Increment = 0.1m,
                Minimum = 0,
                Maximum = 1000,
                Value = Math.Max(0m, Math.Min(1000m, (decimal)value)),
                Width = 92,
                Anchor = AnchorStyles.Left
            };
        }

        private static void AddMarginRow(TableLayoutPanel grid, int row, string label, Control input, string unit)
        {
            grid.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            grid.Controls.Add(new Label
            {
                Text = label,
                AutoSize = true,
                Anchor = AnchorStyles.Left,
                Margin = new Padding(0, 4, 10, 4)
            }, 0, row);
            input.Margin = new Padding(0, 2, 6, 2);
            grid.Controls.Add(input, 1, row);
            grid.Controls.Add(new Label
            {
                Text = unit,
                AutoSize = true,
                Anchor = AnchorStyles.Left,
                Margin = new Padding(0, 4, 0, 4)
            }, 2, row);
        }
    }

    public class RasterObject
    {
        public string Id { get; set; }
        public int PageNumber { get; set; }
        public string LayerId { get; set; } = PDFForm.DefaultLayerId;
        public string AltText { get; set; }
        public RectangleF Bounds { get; set; }
        public RectangleF InitialBounds { get; set; }
        public int Rotation { get; set; }
        public float Opacity { get; set; } = 1f;
        public bool TransparentBackground { get; set; } = false;
        public bool RecolorInkEnabled { get; set; } = false;
        public int RecolorInkColorArgb { get; set; } = System.Drawing.Color.Red.ToArgb();
        public bool LockAspect { get; set; }
        public bool IsLocked { get; set; }
        public string SourceType { get; set; }
        public string FilePath { get; set; }
        public byte[] EmbeddedBytes { get; set; }
        public string MimeType { get; set; }
        public string DuplicateGroupId { get; set; }
        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAtUtc { get; set; } = DateTime.UtcNow;
    }

    public class ArrowObject
    {
        public string Id { get; set; }
        public int PageNumber { get; set; }
        public string LayerId { get; set; } = PDFForm.DefaultLayerId;
        public string AltText { get; set; }
        public PointF Start { get; set; }
        public PointF End { get; set; }
        public int LineColorArgb { get; set; } = System.Drawing.Color.Red.ToArgb();
        public float Thickness { get; set; } = 3f;
        public int BorderColorArgb { get; set; } = System.Drawing.Color.Black.ToArgb();
        public float BorderWidth { get; set; } = 0f;
        public float HeadLength { get; set; } = 18f;
        public float HeadWidth { get; set; } = 12f;
        public bool IsLocked { get; set; }
        public string DuplicateGroupId { get; set; }
        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAtUtc { get; set; } = DateTime.UtcNow;
    }

    public class VectorShapeObject
    {
        public string Id { get; set; }
        public int PageNumber { get; set; }
        public string LayerId { get; set; } = PDFForm.DefaultLayerId;
        public string AltText { get; set; }
        public string ShapeType { get; set; }
        public bool IsRasterClip { get; set; }
        public List<PointF> Points { get; set; } = new List<PointF>();
        public int StrokeColorArgb { get; set; } = System.Drawing.Color.Blue.ToArgb();
        public float StrokeWidth { get; set; } = 2f;
        public int FillColorArgb { get; set; } = System.Drawing.Color.Gold.ToArgb();
        public int FillPatternColorArgb { get; set; } = System.Drawing.Color.FromArgb(0, 0, 0, 0).ToArgb();
        public float FillOpacity { get; set; } = 0.18f;
        public string StrokeStyle { get; set; } = "solid";
        public string FillPattern { get; set; } = "solid";
        public string StartLineEnding { get; set; } = "None";
        public string EndLineEnding { get; set; } = "None";
        public float StartLineEndingPrimarySize { get; set; }
        public float StartLineEndingSecondarySize { get; set; }
        public float EndLineEndingPrimarySize { get; set; }
        public float EndLineEndingSecondarySize { get; set; }
        public bool IsLocked { get; set; }
        public string DuplicateGroupId { get; set; }
        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAtUtc { get; set; } = DateTime.UtcNow;
    }

    public class ProjectObjectLayer
    {
        public string Type { get; set; }
        public string Id { get; set; }
    }

    public class LayerDefinition
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string GroupName { get; set; }
        public int Order { get; set; }
        public bool IsVisible { get; set; } = true;
        public bool IsLocked { get; set; }
        public bool ExcludeFromExport { get; set; }
        public bool IsSystem { get; set; }

        public LayerDefinition Clone()
        {
            return new LayerDefinition
            {
                Id = Id,
                Name = Name,
                GroupName = GroupName,
                Order = Order,
                IsVisible = IsVisible,
                IsLocked = IsLocked,
                ExcludeFromExport = ExcludeFromExport,
                IsSystem = IsSystem
            };
        }
    }

    public class ResumeState
    {
        public string PdfPath { get; set; }
        public string ProjectPath { get; set; }
        public int CurrentPage { get; set; }
        public float? ZoomFactor { get; set; }
        public int ScrollX { get; set; }
        public int ScrollY { get; set; }
    }

    public class PageItemStatus
    {
        public int PageNumber { get; set; }
        public bool MarkedForDeletion { get; set; }
        public bool HasSearchResults { get; set; }
        public bool HasSelections { get; set; }
        public bool HasObjects { get; set; }
        public bool HasRotation { get; set; }
    }

    public class DeletePagesDialog : Form
    {
        public int StartPage { get; private set; }
        public int EndPage { get; private set; }
        public int Step { get; private set; }
        public bool ApplyDeletion { get; private set; } // true: apply range, false: cancel selection

        private readonly NumericUpDown nudStart;
        private readonly NumericUpDown nudEnd;
        private readonly NumericUpDown nudStep;
        private readonly RadioButton rbApply;
        private readonly RadioButton rbCancel;
        private readonly Button btnOK;
        private readonly Button btnCancel;

        private readonly ErrorProvider errorProvider;

        public DeletePagesDialog(int numPages)
        {
            this.Text = Resources.Delete_Title;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterParent;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Width = PDFForm.ScaleForDpiStatic(300);
            this.Height = PDFForm.ScaleForDpiStatic(300);

            errorProvider = new ErrorProvider
            {
                BlinkStyle = ErrorBlinkStyle.NeverBlink
            };

            // Labels and NumericUpDown controls to select page range
            Label lblStart = new Label() { Text = Resources.Delete_Label_Start, Left = PDFForm.ScaleForDpiStatic(20), Top = PDFForm.ScaleForDpiStatic(20), Width = PDFForm.ScaleForDpiStatic(120) };
            nudStart = new NumericUpDown() { Left = PDFForm.ScaleForDpiStatic(150), Top = PDFForm.ScaleForDpiStatic(20), Width = PDFForm.ScaleForDpiStatic(50), Height = PDFForm.ScaleForDpiStatic(22), Minimum = 1, Maximum = numPages, Value = 1 };

            Label lblEnd = new Label() { Text = Resources.Delete_Label_End, Left = PDFForm.ScaleForDpiStatic(20), Top = PDFForm.ScaleForDpiStatic(60), Width = PDFForm.ScaleForDpiStatic(120) };
            nudEnd = new NumericUpDown() { Left = PDFForm.ScaleForDpiStatic(150), Top = PDFForm.ScaleForDpiStatic(60), Width = PDFForm.ScaleForDpiStatic(50), Height = PDFForm.ScaleForDpiStatic(22), Minimum = 1, Maximum = numPages, Value = numPages };

            Label lblStep = new Label() { Text = Resources.Delete_Label_Step, Left = PDFForm.ScaleForDpiStatic(20), Top = PDFForm.ScaleForDpiStatic(100), Width = PDFForm.ScaleForDpiStatic(120) };
            nudStep = new NumericUpDown() { Left = PDFForm.ScaleForDpiStatic(150), Top = PDFForm.ScaleForDpiStatic(100), Width = PDFForm.ScaleForDpiStatic(50), Height = PDFForm.ScaleForDpiStatic(22), Minimum = 0, Maximum = numPages, Value = 1 };

            // Two RadioButtons — one to apply, one to cancel the selection
            rbApply = new RadioButton() { Text = Resources.Delete_Radio_Apply, Left = PDFForm.ScaleForDpiStatic(20), Top = PDFForm.ScaleForDpiStatic(140), Width = PDFForm.ScaleForDpiStatic(200) };
            rbCancel = new RadioButton() { Text = Resources.Delete_Radio_Cancel, Left = PDFForm.ScaleForDpiStatic(20), Top = PDFForm.ScaleForDpiStatic(170), Width = PDFForm.ScaleForDpiStatic(200) };

            // By default set that we want to apply selection
            rbApply.Checked = true;

            // OK and Cancel buttons
            btnOK = new Button() { Text = Resources.Merge_OK, Left = PDFForm.ScaleForDpiStatic(50), Width = PDFForm.ScaleForDpiStatic(80), Height = PDFForm.ScaleForDpiStatic(28), Top = PDFForm.ScaleForDpiStatic(210), DialogResult = DialogResult.OK };
            btnCancel = new Button() { Text = Resources.Merge_Cancel, Left = PDFForm.ScaleForDpiStatic(150), Width = PDFForm.ScaleForDpiStatic(80), Height = PDFForm.ScaleForDpiStatic(28), Top = PDFForm.ScaleForDpiStatic(210), DialogResult = DialogResult.Cancel };

            // Add controls to the form
            this.Controls.Add(lblStart);
            this.Controls.Add(nudStart);
            this.Controls.Add(lblEnd);
            this.Controls.Add(nudEnd);
            this.Controls.Add(lblStep);
            this.Controls.Add(nudStep);
            this.Controls.Add(rbApply);
            this.Controls.Add(rbCancel);
            this.Controls.Add(btnOK);
            this.Controls.Add(btnCancel);

            this.AcceptButton = btnOK;
            this.CancelButton = btnCancel;

            // "Live" validation
            nudStart.ValueChanged += Nud_ValueChanged;
            nudEnd.ValueChanged += Nud_ValueChanged;
            nudStep.ValueChanged += Nud_ValueChanged;

            nudStart.MouseClick += Nud_MouseClick;
            nudEnd.MouseClick += Nud_MouseClick;
            nudStep.MouseClick += Nud_MouseClick;
        }

        private void Nud_MouseClick(object sender, MouseEventArgs e)
        {
            if (sender is NumericUpDown nud)
                nud.Select(0, nud.Text.Length);
        }

        private void Nud_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }


        private void Nud_ValueChanged(object sender, EventArgs e)
        {
            if (nudStart.Value > nudEnd.Value)
            {
                errorProvider.SetError(nudStart, Resources.Delete_Err_StartGreater);
                btnOK.Enabled = false;
            }
            else
            {
                errorProvider.SetError(nudStart, "");
                btnOK.Enabled = true;
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (this.DialogResult == DialogResult.OK)
            {
            // Final validation
                if (nudStart.Value > nudEnd.Value)
                {
                    MessageBox.Show(this, Resources.Delete_Err_StartGreater,
                                    Resources.Title_Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    e.Cancel = true;
                    return;
                }

                StartPage = (int)nudStart.Value;
                EndPage = (int)nudEnd.Value;
                Step = (int)nudStep.Value;

                // Set ApplyDeletion depending on selected RadioButton
                // If rbApply is checked, we want to apply selection, otherwise cancel.
                ApplyDeletion = rbApply.Checked;
            }
            base.OnFormClosing(e);
        }
    }

    public class PdfTextSearcher
    {
        private static readonly string PDDServerUrl = "";
        private const int MinimumNativeTextCharactersToSkipOcr = 40;
        private const float OcrSearchHorizontalPaddingRatio = 0.03f;
        private const float OcrSearchRightPaddingRatio = 0.16f;
        private const float OcrSearchVerticalPaddingRatio = 0.08f;
        private const float OcrSearchMinimumHorizontalPadding = 0.5f;
        private const float OcrSearchMinimumRightPadding = 1.5f;
        private const float OcrSearchMinimumVerticalPadding = 1.0f;
        // Cache for processed lines by file path
        private static readonly ConcurrentDictionary<string, List<CachedLine>> _lineCache = new ConcurrentDictionary<string, List<CachedLine>>();
        // Eagerly-computed personal data NER results, keyed by file path
        private static readonly ConcurrentDictionary<string, List<TextLocation>> _personalDataCache = new ConcurrentDictionary<string, List<TextLocation>>();
        // Alt text entries extracted from PDF structure tree, keyed by file path
        internal static readonly ConcurrentDictionary<string, List<AltTextEntry>> _altTextCache = new ConcurrentDictionary<string, List<AltTextEntry>>();
        // ALL tagged Figure entries (with or without Alt), used for hit-testing, keyed by file path
        internal static readonly ConcurrentDictionary<string, List<AltTextEntry>> _allFiguresCache = new ConcurrentDictionary<string, List<AltTextEntry>>();
        // Pending alt text edits: (pdfPath, posKey) → new text. posKey = "{page}:{x}:{y}"
        internal static readonly ConcurrentDictionary<(string pdfPath, string posKey), string> _pendingAltTextEdits = new ConcurrentDictionary<(string, string), string>();

        /// <summary>ISO language code detected from the most recently processed document ("pl", "de", or null if unknown).</summary>
        public static string LastDetectedLanguage { get; private set; }

        /// <summary>Returns true when <paramref name="rect"/> lies (even partially) outside the page boundaries.</summary>
        public static bool IsOutOfPageBounds(iText.Kernel.Geom.Rectangle rect, float pageWidth, float pageHeight)
        {
            if (pageWidth <= 0 || pageHeight <= 0) return false;
            return rect.GetX() < 0 || rect.GetY() < 0 ||
                   rect.GetX() + rect.GetWidth() > pageWidth + 1f ||
                   rect.GetY() + rect.GetHeight() > pageHeight + 1f;
        }

        // Structure storing line data
        internal class CachedLine
        {
            public int PageNumber { get; set; }
            public int PageRotation { get; set; }
            public string Text { get; set; } = "";
            public float YPosition { get; set; }
            public List<CharacterInfo> Characters { get; set; } = new List<CharacterInfo>();
            public bool IsOcr { get; set; }
            public float PageWidth { get; set; }
            public float PageHeight { get; set; }
            public List<KernelGeom.Rectangle> OcrWordBounds { get; set; } = new List<KernelGeom.Rectangle>();
            public List<KernelGeom.Rectangle> RawOcrWordBounds { get; set; } = new List<KernelGeom.Rectangle>();
            public List<OcrWordInfo> OcrWords { get; set; } = new List<OcrWordInfo>();
            // X coordinate where the last iText render chunk ended; used to detect inter-cell gaps.
            public float LastChunkEndX { get; set; } = float.MinValue;
        }

        internal class CharacterInfo
        {
            public char Char { get; set; }
            public KernelGeom.Rectangle BoundingBox { get; set; }
        }

        internal class OcrWordInfo
        {
            public string Text { get; set; } = "";
            public int StartIndex { get; set; }
            public int Length { get; set; }
            public KernelGeom.Rectangle BoundingBox { get; set; }
        }

        internal class AltTextEntry
        {
            public int PageNumber { get; set; }
            public int PageRotation { get; set; }
            public float PageWidth { get; set; }
            public float PageHeight { get; set; }
            public KernelGeom.Rectangle BBox { get; set; }
            public string AltText { get; set; }
            public int StructXref { get; set; }
            public int Mcid { get; set; } = -1; // marked-content ID from K entry; -1 if absent
            // Stable key based on page+bbox — works even when StructXref == 0
            public string PositionKey => BBox != null
                ? $"{PageNumber}:{(int)Math.Round(BBox.GetX())}:{(int)Math.Round(BBox.GetY())}"
                : $"{PageNumber}:0:0";
        }

        private sealed class OcrCandidateImageInfo
        {
            public KernelGeom.Rectangle BoundsPdf { get; set; }
            public int PixelWidth { get; set; }
            public int PixelHeight { get; set; }
        }

        private sealed class OcrCandidateImageExtractionListener : IEventListener
        {
            private readonly List<OcrCandidateImageInfo> images = new List<OcrCandidateImageInfo>();
            private readonly double pageArea;

            public OcrCandidateImageExtractionListener(KernelGeom.Rectangle pageSize)
            {
                pageArea = pageSize == null
                    ? 1d
                    : Math.Max(1d, pageSize.GetWidth() * pageSize.GetHeight());
            }

            public IReadOnlyList<OcrCandidateImageInfo> Images => images;

            public void EventOccurred(IEventData data, EventType type)
            {
                if (type != EventType.RENDER_IMAGE || !(data is ImageRenderInfo imageInfo))
                {
                    return;
                }

                try
                {
                    PdfImageXObject pdfImage = imageInfo.GetImage();
                    if (pdfImage == null)
                    {
                        return;
                    }

                    iText.Kernel.Geom.Matrix ctm = imageInfo.GetImageCtm();
                    iText.Kernel.Geom.Vector p0 = new iText.Kernel.Geom.Vector(0, 0, 1).Cross(ctm);
                    iText.Kernel.Geom.Vector p1 = new iText.Kernel.Geom.Vector(1, 0, 1).Cross(ctm);
                    iText.Kernel.Geom.Vector p2 = new iText.Kernel.Geom.Vector(0, 1, 1).Cross(ctm);
                    iText.Kernel.Geom.Vector p3 = new iText.Kernel.Geom.Vector(1, 1, 1).Cross(ctm);

                    float minX = Math.Min(Math.Min(p0.Get(iText.Kernel.Geom.Vector.I1), p1.Get(iText.Kernel.Geom.Vector.I1)), Math.Min(p2.Get(iText.Kernel.Geom.Vector.I1), p3.Get(iText.Kernel.Geom.Vector.I1)));
                    float maxX = Math.Max(Math.Max(p0.Get(iText.Kernel.Geom.Vector.I1), p1.Get(iText.Kernel.Geom.Vector.I1)), Math.Max(p2.Get(iText.Kernel.Geom.Vector.I1), p3.Get(iText.Kernel.Geom.Vector.I1)));
                    float minY = Math.Min(Math.Min(p0.Get(iText.Kernel.Geom.Vector.I2), p1.Get(iText.Kernel.Geom.Vector.I2)), Math.Min(p2.Get(iText.Kernel.Geom.Vector.I2), p3.Get(iText.Kernel.Geom.Vector.I2)));
                    float maxY = Math.Max(Math.Max(p0.Get(iText.Kernel.Geom.Vector.I2), p1.Get(iText.Kernel.Geom.Vector.I2)), Math.Max(p2.Get(iText.Kernel.Geom.Vector.I2), p3.Get(iText.Kernel.Geom.Vector.I2)));

                    float width = maxX - minX;
                    float height = maxY - minY;
                    if (width <= 0f || height <= 0f)
                    {
                        return;
                    }

                    int pixelWidth = (int)Math.Round(pdfImage.GetWidth());
                    int pixelHeight = (int)Math.Round(pdfImage.GetHeight());
                    double coverage = (width * height) / pageArea;
                    if (pixelWidth < 20 || pixelHeight < 10 || width < 12f || height < 8f || coverage < 0.0005d)
                    {
                        return;
                    }

                    images.Add(new OcrCandidateImageInfo
                    {
                        BoundsPdf = new KernelGeom.Rectangle(minX, minY, width, height),
                        PixelWidth = pixelWidth,
                        PixelHeight = pixelHeight
                    });
                }
                catch
                {
                    // Ignore malformed image events and keep extracting the rest.
                }
            }

            public ICollection<EventType> GetSupportedEvents()
            {
                return new List<EventType> { EventType.RENDER_IMAGE };
            }
        }

        public static event Action<string, string> OnCacheStatusChanged;

        private static void ReportCacheStatus(string status, string pdfPath = null) =>
            OnCacheStatusChanged?.Invoke(status, pdfPath);

        private sealed class LocalNerOptions
        {
            public bool Enabled { get; private set; }
            public string ExecutablePath { get; private set; }
            public string PythonExe { get; private set; }
            public string ScriptPath { get; private set; }
            public string ModelName { get; private set; }
            public int TimeoutMs { get; private set; }
            public HashSet<string> Labels { get; private set; }

            public static LocalNerOptions Load()
            {
                var labels = SplitSetting(
                    GetSetting("LocalNer.Labels", "ANONPDF_LOCAL_NER_LABELS") ??
                    "PERSON,PER,persName,LOCATION,LOC,GPE,placeName,geogName");

                return new LocalNerOptions
                {
                    Enabled = ParseBool(GetSetting("LocalNer.Enabled", "ANONPDF_LOCAL_NER_ENABLED"), defaultValue: false),
                    ExecutablePath = GetSetting("LocalNer.ExecutablePath", "ANONPDF_LOCAL_NER_EXE") ?? @"tools\ner\spacy_ner_service\spacy_ner_service.exe",
                    PythonExe = GetSetting("LocalNer.PythonExe", "ANONPDF_LOCAL_NER_PYTHON") ?? "python",
                    ScriptPath = GetSetting("LocalNer.ScriptPath", "ANONPDF_LOCAL_NER_SCRIPT") ?? @"tools\spacy_ner_service.py",
                    ModelName = GetSetting("LocalNer.Model", "ANONPDF_LOCAL_NER_MODEL") ?? "auto",
                    TimeoutMs = ParseInt(GetSetting("LocalNer.TimeoutMs", "ANONPDF_LOCAL_NER_TIMEOUT_MS"), 180000),
                    Labels = new HashSet<string>(labels, StringComparer.OrdinalIgnoreCase)
                };
            }

            public bool AcceptsLabel(string label)
            {
                return Labels == null || Labels.Count == 0 || Labels.Contains(label ?? string.Empty);
            }

            public LocalNerOptions WithPlugin(PluginManifest manifest)
            {
                return new LocalNerOptions
                {
                    Enabled = Enabled,
                    ExecutablePath = manifest.ExecutablePath,
                    PythonExe = PythonExe,
                    ScriptPath = ScriptPath,
                    ModelName = ModelName,
                    TimeoutMs = TimeoutMs,
                    Labels = manifest.Labels != null && manifest.Labels.Count > 0
                        ? new HashSet<string>(manifest.Labels, StringComparer.OrdinalIgnoreCase)
                        : Labels
                };
            }

            private static string GetSetting(string appSettingKey, string environmentVariable)
            {
                string env = Environment.GetEnvironmentVariable(environmentVariable);
                if (!string.IsNullOrWhiteSpace(env))
                {
                    return env.Trim();
                }

                try
                {
                    string value = ConfigurationManager.AppSettings[appSettingKey];
                    return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
                }
                catch
                {
                    return null;
                }
            }

            private static bool ParseBool(string value, bool defaultValue)
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    return defaultValue;
                }

                if (bool.TryParse(value, out bool parsed))
                {
                    return parsed;
                }

                return value == "1" ||
                    string.Equals(value, "yes", StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(value, "on", StringComparison.OrdinalIgnoreCase);
            }

            private static int ParseInt(string value, int defaultValue)
            {
                return int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out int parsed) && parsed > 0
                    ? parsed
                    : defaultValue;
            }

            private static IEnumerable<string> SplitSetting(string value)
            {
                return (value ?? string.Empty)
                    .Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(item => item.Trim())
                    .Where(item => item.Length > 0);
            }
        }

        private static string LocalizedText(string key)
        {
            var culture = Resources.Culture ?? CultureInfo.CurrentUICulture;
            var text = Resources.ResourceManager.GetString(key, culture);
            return string.IsNullOrWhiteSpace(text) ? key : text;
        }

        private static string LocalizedFormat(string key, params object[] args)
        {
            return string.Format(LocalizedText(key), args);
        }

        private sealed class PluginManifest
        {
            public string Language { get; set; }
            public string ExecutablePath { get; set; }
            public List<string> Labels { get; set; }
        }

        public static List<TextLocation> FindTextLocations(string pdfPath, string searchText, bool searchPersonalData, string userPassword, IWin32Window owner = null, System.Threading.CancellationToken cancellationToken = default)
        {
            // Check whether lines for this file are already cached
            if (!_lineCache.ContainsKey(pdfPath))
            {
                CacheLines(pdfPath, userPassword, cancellationToken);
            }

            cancellationToken.ThrowIfCancellationRequested();

            // Perform search based on cache
            return SearchInCachedLines(pdfPath, searchText, searchPersonalData, owner, cancellationToken, out _);
        }

        /// <summary>
        /// Runs personal-data detection and reports whether the local NER service actually
        /// produced a response. An empty result is valid only when this returns true.
        /// </summary>
        internal static bool TryFindPersonalDataLocations(
            string pdfPath,
            string userPassword,
            System.Threading.CancellationToken cancellationToken,
            out List<TextLocation> locations)
        {
            if (!_lineCache.ContainsKey(pdfPath))
            {
                CacheLines(pdfPath, userPassword, cancellationToken);
            }

            cancellationToken.ThrowIfCancellationRequested();
            locations = SearchInCachedLines(pdfPath, string.Empty, true, null, cancellationToken, out bool nerCompleted);
            return nerCompleted;
        }

        public static List<TextLocation> GetOcrDebugLocations(string pdfPath, int pageNumber)
        {
            return GetOcrDebugLocations(pdfPath, pageNumber, rawOcrBounds: false);
        }

        public static List<TextLocation> GetRawOcrDebugLocations(string pdfPath, int pageNumber)
        {
            return GetOcrDebugLocations(pdfPath, pageNumber, rawOcrBounds: true);
        }

        private static List<TextLocation> GetOcrDebugLocations(string pdfPath, int pageNumber, bool rawOcrBounds)
        {
            var locations = new List<TextLocation>();
            if (string.IsNullOrWhiteSpace(pdfPath) || pageNumber < 1)
            {
                return locations;
            }

            if (!_lineCache.TryGetValue(pdfPath, out List<CachedLine> cachedLines) || cachedLines == null)
            {
                return locations;
            }

            foreach (CachedLine line in cachedLines.Where(line => line != null && line.IsOcr && line.PageNumber == pageNumber))
            {
                List<KernelGeom.Rectangle> sourceBoxes = rawOcrBounds ? line.RawOcrWordBounds : line.OcrWordBounds;
                List<KernelGeom.Rectangle> boxes = sourceBoxes != null && sourceBoxes.Count > 0
                    ? sourceBoxes
                    : new List<KernelGeom.Rectangle> { GetTextFragmentRectangle(line, 0, line.Text?.Length ?? 0) };

                foreach (KernelGeom.Rectangle rect in boxes)
                {
                    if (rect == null || rect.GetWidth() <= 0f || rect.GetHeight() <= 0f)
                    {
                        continue;
                    }

                    locations.Add(new TextLocation(line.PageNumber, line.PageRotation, rect, isOcr: true));
                }
            }

            return locations;
        }

        private static void CacheLines(string pdfPath, string userPassword, System.Threading.CancellationToken cancellationToken = default)
        {
            // ── Disk cache hit? ───────────────────────────────────────────────────
            if (PdfDiskCache.TryLoad(pdfPath, out var cached))
            {
                cancellationToken.ThrowIfCancellationRequested();
                _lineCache[pdfPath]       = PdfDiskCache.FromDto(cached.Lines);
                _altTextCache[pdfPath]    = PdfDiskCache.AltFromDto(cached.AltTexts);
                _allFiguresCache[pdfPath] = PdfDiskCache.AltFromDto(cached.AllFigures);
                if (cached.PersonalData != null)
                    _personalDataCache[pdfPath] = PdfDiskCache.TextLocFromDto(cached.PersonalData);
                ReportCacheStatus(string.Empty, pdfPath);
                return;
            }

            var lines = new List<CachedLine>();

            var props = new ReaderProperties();
            if (!string.IsNullOrEmpty(userPassword))
            {
                props.SetPassword(System.Text.Encoding.UTF8.GetBytes(userPassword));
            }

            using (iText.Kernel.Pdf.PdfDocument pdfDoc = new iText.Kernel.Pdf.PdfDocument(new PdfReader(pdfPath, props).SetUnethicalReading(Properties.Settings.Default.IgnorePdfRestrictions)))
            {
                for (int page = 1; page <= pdfDoc.GetNumberOfPages(); page++)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    var pageObj = pdfDoc.GetPage(page);
                    int rotation = pageObj.GetRotation();
                    var strategy = new LineExtractionStrategy(page, rotation);

                    PdfCanvasProcessor processor = new PdfCanvasProcessor(strategy);
                    processor.ProcessPageContent(pageObj);

                    // Keep ALL extracted lines, including those outside the visible page area.
                    // Out-of-bounds text (Y < 0 or Y > PageHeight) is hidden in any viewer
                    // but can contain PII — we detect it and display it in the Found tab
                    // under a dedicated "Poza stroną" sub-node so users can anonymize it.
                    var pageSize = pageObj.GetPageSize();
                    float pw = pageSize.GetWidth();
                    float ph = pageSize.GetHeight();
                    foreach (var l in strategy.ExtractedLines)
                    {
                        l.PageWidth = pw;
                        l.PageHeight = ph;
                    }
                    lines.AddRange(strategy.ExtractedLines);
                    ReportCacheStatus(LocalizedFormat("CacheStatus_IndexPage", page), pdfPath);
                }

                AppendOcrLinesForPagesWithoutNativeText(pdfPath, userPassword, pdfDoc, lines, cancellationToken);
                _altTextCache[pdfPath] = ExtractAltTexts(pdfDoc);
                _allFiguresCache[pdfPath] = ExtractAllFigures(pdfDoc);
            }
            ReportCacheStatus(string.Empty, pdfPath);
            _lineCache[pdfPath] = lines;

            // Save now only when NER won't run (no plugin) — otherwise SetPersonalDataCache saves once after NER.
            if (!IsLocalNerAvailable())
                PdfDiskCache.Save(pdfPath, lines, _altTextCache[pdfPath], _allFiguresCache[pdfPath]);
        }

        private static void AppendOcrLinesForPagesWithoutNativeText(
            string pdfPath,
            string userPassword,
            iText.Kernel.Pdf.PdfDocument pdfDoc,
            List<CachedLine> lines,
            System.Threading.CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(pdfPath) || !File.Exists(pdfPath) || pdfDoc == null || lines == null)
            {
                return;
            }

            HashSet<int> pagesWithNativeText = new HashSet<int>(
                lines
                    .Where(line => !string.IsNullOrWhiteSpace(line.Text))
                    .GroupBy(line => line.PageNumber)
                    .Where(group => group.Sum(line => (line.Text ?? string.Empty).Trim().Length) >= MinimumNativeTextCharactersToSkipOcr)
                    .Select(group => group.Key));

            try
            {
                JObject debugRoot = CreateOcrDebugRoot(pdfPath);
                JArray debugPages = (JArray)debugRoot["pages"];
                JArray skippedPages = (JArray)debugRoot["skippedPagesWithNativeText"];

                using (var pdfiumDoc = new PDFiumSharp.PdfDocument(pdfPath, userPassword))
                {
                    string detectedOcrLang = DetectOcrLanguageFromLines(lines);
                    OcrEngine engine = CreateWindowsOcrEngine(detectedOcrLang);
                    if (PDFForm.IsDiagnosticModeEnabled)
                    {
                        string nerLogPath = Path.Combine(Path.GetTempPath(), "AnonPDF-ner.log");
                        File.AppendAllText(nerLogPath,
                            $"\r\n=== OCR engine {DateTime.Now:yyyy-MM-dd HH:mm:ss}" +
                            $" detected={detectedOcrLang ?? "null"}" +
                            $" selected={engine?.RecognizerLanguage?.LanguageTag ?? "null"}" +
                            $" available=[{string.Join(",", OcrEngine.AvailableRecognizerLanguages.Select(l => l.LanguageTag))}] ===\r\n",
                            System.Text.Encoding.UTF8);
                    }
                    if (engine == null)
                    {
                        Debug.WriteLine("OCR skipped: Windows OCR engine is not available.");
                        return;
                    }

                    for (int pageNumber = 1; pageNumber <= pdfDoc.GetNumberOfPages(); pageNumber++)
                    {
                        cancellationToken.ThrowIfCancellationRequested();
                        var pageObj = pdfDoc.GetPage(pageNumber);
                        bool pageHasNativeText = pagesWithNativeText.Contains(pageNumber);
                        List<KernelGeom.Rectangle> ocrImageBounds = GetOcrCandidateImageBounds(pageObj);
                        if (pageHasNativeText && ocrImageBounds.Count == 0)
                        {
                            skippedPages.Add(pageNumber);
                            continue;
                        }

                        ReportCacheStatus(LocalizedFormat("CacheStatus_OcrPage", pageNumber), pdfPath);

                        using (Bitmap pageBitmap = RenderPdfPageForOcr(pdfiumDoc.Pages[pageNumber - 1]))
                        {
                            if (pageBitmap == null)
                            {
                                continue;
                            }

                            int rotation = pageObj.GetRotation();
                            KernelGeom.Rectangle pageSize = pageObj.GetPageSize();
                            List<CachedLine> ocrLines = RecognizeOcrLinesWithWindowsOcr(engine, pageBitmap, pageNumber, rotation, pageSize, out JObject debugPage).ToList();
                            if (pageHasNativeText)
                            {
                                ocrLines = ocrLines
                                    .Where(line => IntersectsAnyOcrImageBounds(line, ocrImageBounds))
                                    .ToList();
                            }

                            lines.AddRange(ocrLines);
                            if (debugPage != null)
                            {
                                debugPage["pageHasNativeText"] = pageHasNativeText;
                                debugPage["ocrImageBounds"] = new JArray(ocrImageBounds.Select(CreatePdfBoundsJson));
                                debugPage["indexedLineCount"] = ocrLines.Count;
                                debugPages.Add(debugPage);
                            }
                        }
                    }
                }

                WriteOcrDebugJson(pdfPath, debugRoot);
            }
            catch (Exception ex)
            {
                // OCR is an auxiliary search index. Native PDF text search must continue even if OCR fails.
                Debug.WriteLine("OCR cache failed: " + ex);
            }
        }


        private static JObject CreateOcrDebugRoot(string pdfPath)
        {
            return new JObject
            {
                ["sourcePdf"] = pdfPath ?? string.Empty,
                ["generatedAtUtc"] = DateTime.UtcNow.ToString("o", CultureInfo.InvariantCulture),
                ["engine"] = "Windows.Media.Ocr",
                ["language"] = "PolishPreferred",
                ["notes"] = "Diagnostic OCR dump. Contains recognized text and geometry; do not store with production projects.",
                ["skippedPagesWithNativeText"] = new JArray(),
                ["pages"] = new JArray()
            };
        }

        private static void WriteOcrDebugJson(string pdfPath, JObject debugRoot)
        {
            if (!PDFForm.IsDiagnosticModeEnabled || string.IsNullOrWhiteSpace(pdfPath) || debugRoot == null)
            {
                return;
            }

            try
            {
                string directory = Path.GetDirectoryName(pdfPath);
                if (string.IsNullOrWhiteSpace(directory) || !Directory.Exists(directory))
                {
                    return;
                }

                string fileName = Path.GetFileNameWithoutExtension(pdfPath) + ".ocr-debug.json";
                string outputPath = Path.Combine(directory, fileName);
                File.WriteAllText(outputPath, debugRoot.ToString(Formatting.Indented), new System.Text.UTF8Encoding(false));
                Debug.WriteLine("OCR debug JSON saved: " + outputPath);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("OCR debug JSON save failed: " + ex);
            }
        }

        private static List<KernelGeom.Rectangle> GetOcrCandidateImageBounds(iText.Kernel.Pdf.PdfPage page)
        {
            var result = new List<KernelGeom.Rectangle>();
            if (page == null)
            {
                return result;
            }

            try
            {
                var listener = new OcrCandidateImageExtractionListener(page.GetPageSize());
                var processor = new PdfCanvasProcessor(listener);
                processor.ProcessPageContent(page);

                result.AddRange(listener.Images
                    .Where(image => image?.BoundsPdf != null)
                    .Select(image => image.BoundsPdf));
            }
            catch (Exception ex)
            {
                Debug.WriteLine("OCR image bounds extraction failed: " + ex);
            }

            return result;
        }

        private static string GetTessDataPath()
        {
            string exePath = Assembly.GetExecutingAssembly().Location;
            string exeDir = Path.GetDirectoryName(exePath);
            return string.IsNullOrWhiteSpace(exeDir)
                ? null
                : Path.Combine(exeDir, "tessdata");
        }

        private static Bitmap RenderPdfPageForOcr(PDFiumSharp.PdfPage page)
        {
            if (page == null || page.Width <= 0 || page.Height <= 0)
            {
                return null;
            }

            const float dpi = 300f;
            int bitmapWidth = Math.Max(1, (int)Math.Ceiling(page.Width * dpi / 72f));
            int bitmapHeight = Math.Max(1, (int)Math.Ceiling(page.Height * dpi / 72f));

            using (var pdfBitmap = new PDFiumSharp.PDFiumBitmap(bitmapWidth, bitmapHeight, true))
            {
                pdfBitmap.FillRectangle(0, 0, bitmapWidth, bitmapHeight, 0xFFFFFFFF);
                page.Render(renderTarget: pdfBitmap, flags: PDFiumSharp.Enums.RenderingFlags.Annotations);

                using (var ms = new MemoryStream())
                {
                    pdfBitmap.Save(ms);
                    ms.Position = 0;
                    using (var image = DrawingImage.FromStream(ms))
                    {
                        return new Bitmap(image);
                    }
                }
            }
        }

        private static Bitmap RenderPdfRegionForOcr(PDFiumSharp.PdfPage page, KernelGeom.Rectangle region)
        {
            if (page == null || page.Width <= 0 || page.Height <= 0 || region == null)
            {
                return null;
            }

            const float dpi = 300f;
            float scale = dpi / 72f;
            int fullWidth = Math.Max(1, (int)Math.Ceiling(page.Width * scale));
            int fullHeight = Math.Max(1, (int)Math.Ceiling(page.Height * scale));

            // Clamp region to page bounds
            float rx = Math.Max(0, region.GetX());
            float ry = Math.Max(0, region.GetY());
            float rw = Math.Min(region.GetWidth(), (float)page.Width - rx);
            float rh = Math.Min(region.GetHeight(), (float)page.Height - ry);
            if (rw <= 0 || rh <= 0)
                return null;

            int cropX = (int)Math.Floor(rx * scale);
            int cropY = (int)Math.Floor(ry * scale);
            int cropW = Math.Max(1, (int)Math.Ceiling(rw * scale));
            int cropH = Math.Max(1, (int)Math.Ceiling(rh * scale));
            cropW = Math.Min(cropW, fullWidth - cropX);
            cropH = Math.Min(cropH, fullHeight - cropY);

            using (var pdfBitmap = new PDFiumSharp.PDFiumBitmap(fullWidth, fullHeight, true))
            {
                pdfBitmap.FillRectangle(0, 0, fullWidth, fullHeight, 0xFFFFFFFF);
                page.Render(renderTarget: pdfBitmap, flags: PDFiumSharp.Enums.RenderingFlags.Annotations);

                using (var ms = new MemoryStream())
                {
                    pdfBitmap.Save(ms);
                    ms.Position = 0;
                    using (var fullImage = DrawingImage.FromStream(ms))
                    using (var fullBitmap = new Bitmap(fullImage))
                    {
                        if (cropX + cropW > fullBitmap.Width || cropY + cropH > fullBitmap.Height)
                            return null;
                        var cropped = fullBitmap.Clone(
                            new System.Drawing.Rectangle(cropX, cropY, cropW, cropH),
                            fullBitmap.PixelFormat);
                        return cropped;
                    }
                }
            }
        }

        private static OcrEngine CreateWindowsOcrEngine(string preferredLanguage = null)
        {
            try
            {
                var available = OcrEngine.AvailableRecognizerLanguages;

                // Try preferred language first (e.g. "de" for German documents).
                if (!string.IsNullOrEmpty(preferredLanguage))
                {
                    var preferred = available.FirstOrDefault(l =>
                        l.LanguageTag.StartsWith(preferredLanguage, StringComparison.OrdinalIgnoreCase));
                    if (preferred != null)
                    {
                        OcrEngine preferredEngine = OcrEngine.TryCreateFromLanguage(preferred);
                        if (preferredEngine != null)
                            return preferredEngine;
                    }
                }

                // Fall back to Polish (original default).
                var polishLanguage = available.FirstOrDefault(language =>
                    string.Equals(language.LanguageTag, "pl", StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(language.LanguageTag, "pl-PL", StringComparison.OrdinalIgnoreCase));

                if (polishLanguage != null)
                {
                    OcrEngine polishEngine = OcrEngine.TryCreateFromLanguage(polishLanguage);
                    if (polishEngine != null)
                        return polishEngine;
                }

                return OcrEngine.TryCreateFromUserProfileLanguages();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Windows OCR engine initialization failed: " + ex);
                return null;
            }
        }

        private static string DetectOcrLanguageFromLines(List<CachedLine> lines)
        {
            if (lines == null || lines.Count == 0)
            {
                if (PDFForm.IsDiagnosticModeEnabled)
                    File.AppendAllText(Path.Combine(Path.GetTempPath(), "AnonPDF-ner.log"),
                        $"    DetectOcrLang: lines={(lines == null ? "null" : "empty(0)")}\r\n",
                        System.Text.Encoding.UTF8);
                return null;
            }
            int de = 0, pl = 0, total = 0;
            foreach (var line in lines)
            {
                string text = line?.Text;
                if (string.IsNullOrEmpty(text)) continue;
                foreach (char c in text)
                {
                    if ("äöüßÄÖÜ".IndexOf(c) >= 0) de++;
                    else if ("ąęóśźżćńłĄĘÓŚŹŻĆŃŁ".IndexOf(c) >= 0) pl++;
                }
                total += text.Length;
            }
            // Keyword fallback — used when font encoding is broken and diacritics are garbled (e.g. LibreOffice PDFs).
            string[] deKeywords = { "Steuer", "Notar", "Kaufvertrag", "Handelsregister", "Grundbuch", "GmbH", "HRB", "HRA", "Beurkundung", "Auflassung" };
            string[] plKeywords = { "PESEL", "NIP", "REGON", "KRS", "notariusz", "Gmina", "Spółka" };
            string allText = string.Concat(lines.Select(l => l?.Text ?? ""));
            int deKw = deKeywords.Count(kw => allText.IndexOf(kw, StringComparison.OrdinalIgnoreCase) >= 0);
            int plKw = plKeywords.Count(kw => allText.IndexOf(kw, StringComparison.OrdinalIgnoreCase) >= 0);

            if (PDFForm.IsDiagnosticModeEnabled)
                File.AppendAllText(Path.Combine(Path.GetTempPath(), "AnonPDF-ner.log"),
                    $"    DetectOcrLang: de={de} pl={pl} total={total} threshold={total / 300.0:F1} deKw={deKw} plKw={plKw}\r\n",
                    System.Text.Encoding.UTF8);
            if (total == 0) return null;
            if (de > pl && de * 300 > total) return "de";
            if (pl > de && pl * 300 > total) return "pl";
            // Diacritic scores too low — fall back to keyword vote.
            if (deKw > plKw && deKw >= 2) return "de";
            if (plKw > deKw && plKw >= 2) return "pl";
            return null;
        }

        // Returns true when ≥80% of recognised words are single characters — typical of a
        // bitmap that is rotated 90° relative to the reading direction.
        // minWords: minimum total word count required to trigger (use a lower value for small crops).
        private static bool OcrResultAlmostAllSingleChars(OcrResult r, int minWords = 10)
        {
            if (r == null) return false;
            int total = 0, singleCount = 0;
            foreach (var l in r.Lines)
                foreach (var word in l.Words)
                {
                    total++;
                    if ((word.Text?.Trim().Length ?? 0) <= 1) singleCount++;
                }
            return total >= minWords && singleCount * 100 / total >= 80;
        }

        public static async Task<(string Text, string LanguageTag)> RecognizeBitmapTextWithWindowsOcrAsync(Bitmap bitmap)
        {
            if (bitmap == null)
            {
                return (string.Empty, string.Empty);
            }

            OcrEngine engine = CreateWindowsOcrEngine();
            if (engine == null)
            {
                Debug.WriteLine("Windows OCR skipped: engine is not available.");
                return (string.Empty, string.Empty);
            }

            string languageTag = engine.RecognizerLanguage?.LanguageTag ?? string.Empty;
            string tempImagePath = null;
            try
            {
                tempImagePath = Path.Combine(Path.GetTempPath(), "AnonPDFPro-ocr-crop-" + Guid.NewGuid().ToString("N") + ".png");
                bitmap.Save(tempImagePath, ImageFormat.Png);

                OcrResult result = await RecognizeWindowsOcrAsync(engine, tempImagePath);

                // If the crop is sideways (e.g. page with auto-rotated scan), retry after 90° CCW rotation.
                // Use minWords=3 — a small crop may contain only a few words.
                if (OcrResultAlmostAllSingleChars(result, minWords: 3))
                {
                    using (var rotBmp = new Bitmap(bitmap))
                    {
                        rotBmp.RotateFlip(RotateFlipType.Rotate270FlipNone); // 90° CCW
                        rotBmp.Save(tempImagePath, ImageFormat.Png);
                        OcrResult rotResult = await RecognizeWindowsOcrAsync(engine, tempImagePath);
                        if (rotResult != null)
                            result = rotResult;
                    }
                }

                return (result?.Text ?? string.Empty, languageTag);
            }
            finally
            {
                if (!string.IsNullOrWhiteSpace(tempImagePath))
                {
                    try
                    {
                        File.Delete(tempImagePath);
                    }
                    catch
                    {
                        // OCR temp file cleanup must not break copy/search.
                    }
                }
            }
        }

        private static IEnumerable<CachedLine> RecognizeOcrLinesWithWindowsOcr(
            OcrEngine engine,
            Bitmap bitmap,
            int pageNumber,
            int pageRotation,
            KernelGeom.Rectangle pageSize,
            out JObject debugPage)
        {
            var result = new List<CachedLine>();
            debugPage = CreateOcrDebugPage(pageNumber, pageRotation, pageSize, bitmap);
            if (engine == null || bitmap == null || pageSize == null || bitmap.Width <= 0 || bitmap.Height <= 0)
            {
                return result;
            }

            string tempImagePath = null;
            try
            {
                tempImagePath = Path.Combine(Path.GetTempPath(), "AnonPDFPro-ocr-" + Guid.NewGuid().ToString("N") + ".png");
                bitmap.Save(tempImagePath, ImageFormat.Png);

                OcrResult page = RecognizeWindowsOcrAsync(engine, tempImagePath)
                    .GetAwaiter()
                    .GetResult();

                // If OCR returned very few lines, the bitmap content may be rotated
                // (e.g. PDF page stored with Rotate=0 but scanned in landscape).
                // Try rotating the bitmap in 90° steps and pick the best result.
                int effectiveBitmapW   = bitmap.Width;
                int effectiveBitmapH   = bitmap.Height;
                int effectivePageRot = pageRotation;

                // Windows OCR cannot recognize text rotated ~90°/180°/270°.
                // Strategy:
                //   1. Always try 270°CW (landscape-in-portrait is the most common mismatch).
                //      Switch if rotated gives ≥2× more words — reliable even when initial
                //      garbage OCR yields dozens of mangled words.
                //   2. If initial result is still poor (few words or large TextAngle),
                //      also try 90°CW and 180°.
                // Diagnostic helper — logs word-length distribution for one OCR result.
                void LogOcrWordStats(string label, int pgNum, OcrResult r)
                {
                    if (!PDFForm.IsDiagnosticModeEnabled) return;
                    try
                    {
                        if (r == null) { File.AppendAllText(Path.Combine(Path.GetTempPath(), "AnonPDF-ner.log"), $"[OCR-ROT] page={pgNum} {label}: null\r\n", System.Text.Encoding.UTF8); return; }
                        var hist = new System.Collections.Generic.Dictionary<int, int>();
                        foreach (var l in r.Lines)
                            foreach (var w in l.Words)
                            {
                                int len = w.Text?.Trim().Length ?? 0;
                                hist.TryGetValue(len, out int c); hist[len] = c + 1;
                            }
                        int total = hist.Values.Sum();
                        var sb = new System.Text.StringBuilder();
                        sb.Append($"[OCR-ROT] page={pgNum} {label}: total={total} words, dist=");
                        foreach (var kv in hist.OrderBy(x => x.Key))
                            sb.Append($"{kv.Key}ch×{kv.Value} ");
                        sb.AppendLine();
                        File.AppendAllText(Path.Combine(Path.GetTempPath(), "AnonPDF-ner.log"), sb.ToString(), System.Text.Encoding.UTF8);
                    }
                    catch { }
                }

                LogOcrWordStats("orig(rot=" + pageRotation + ")", pageNumber, page);

                // If ≥80% of recognised words are single characters the bitmap is likely
                // rotated 90°. Rotate it 90° CCW (=270° CW) and re-run OCR.
                if (OcrResultAlmostAllSingleChars(page, minWords: 10))
                {
                    string rotPath = Path.Combine(Path.GetTempPath(),
                        "AnonPDFPro-ocr-" + Guid.NewGuid().ToString("N") + ".png");
                    try
                    {
                        using (var rotBmp = new Bitmap(bitmap))
                        {
                            rotBmp.RotateFlip(RotateFlipType.Rotate270FlipNone); // 90° CCW
                            rotBmp.Save(rotPath, ImageFormat.Png);
                            OcrResult rotResult = RecognizeWindowsOcrAsync(engine, rotPath)
                                .GetAwaiter().GetResult();
                            LogOcrWordStats("rot270cw(ccw90)", pageNumber, rotResult);
                            if (rotResult != null)
                            {
                                page             = rotResult;
                                effectiveBitmapW = rotBmp.Width;
                                effectiveBitmapH = rotBmp.Height;
                                effectivePageRot = (pageRotation + 270) % 360;
                                debugPage["autoRotatedCcw90"] = true;
                            }
                        }
                    }
                    finally { try { File.Delete(rotPath); } catch { } }
                }

                debugPage["engine"] = "Windows.Media.Ocr";
                debugPage["language"] = engine.RecognizerLanguage?.LanguageTag ?? string.Empty;
                debugPage["text"] = page?.Text ?? string.Empty;
                debugPage["textAngle"] = page?.TextAngle == null ? null : JToken.FromObject(page.TextAngle.Value);
                JArray debugBlocks = (JArray)debugPage["blocks"];

                if (page == null)
                {
                    return result;
                }

                int rank = 0;
                int lineIndex = 0;
                foreach (OcrLine textLine in page.Lines)
                {
                    Windows.Foundation.Rect? lineBounds = GetWindowsOcrLineBounds(textLine);
                    JObject debugLine = CreateWindowsOcrElementJson(
                        "line",
                        ++rank,
                        lineIndex,
                        textLine.Text,
                        null,
                        lineBounds,
                        pageSize,
                        effectiveBitmapW,
                        effectiveBitmapH,
                        effectivePageRot);
                    JArray debugWords = new JArray();
                    debugLine["words"] = debugWords;
                    debugBlocks.Add(debugLine);

                    int wordIndex = 0;
                    foreach (OcrWord word in textLine.Words)
                    {
                        JObject debugWord = CreateWindowsOcrElementJson(
                            "word",
                            ++rank,
                            wordIndex,
                            word.Text,
                            null,
                            word.BoundingRect,
                            pageSize,
                            effectiveBitmapW,
                            effectiveBitmapH,
                            effectivePageRot);
                        debugWords.Add(debugWord);
                        wordIndex++;
                    }

                    CachedLine cachedLine = CreateCachedLineFromWindowsOcrLine(
                        textLine,
                        pageNumber,
                        effectivePageRot,
                        pageSize,
                        effectiveBitmapW,
                        effectiveBitmapH,
                        page.TextAngle);

                    if (cachedLine != null && !string.IsNullOrWhiteSpace(cachedLine.Text))
                    {
                        result.Add(cachedLine);
                    }

                    lineIndex++;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Windows OCR page recognition failed: " + ex);
                debugPage["error"] = ex.ToString();
            }
            finally
            {
                if (!string.IsNullOrWhiteSpace(tempImagePath))
                {
                    try
                    {
                        File.Delete(tempImagePath);
                    }
                    catch
                    {
                        // Diagnostic OCR temp file cleanup must not break search.
                    }
                }
            }

            return result;
        }

        private static async Task<OcrResult> RecognizeWindowsOcrAsync(OcrEngine engine, string imagePath)
        {
            StorageFile file = await StorageFile.GetFileFromPathAsync(imagePath);
            using (IRandomAccessStream stream = await file.OpenAsync(FileAccessMode.Read))
            {
                BitmapDecoder decoder = await BitmapDecoder.CreateAsync(stream);
                SoftwareBitmap softwareBitmap = await decoder.GetSoftwareBitmapAsync(BitmapPixelFormat.Bgra8, BitmapAlphaMode.Premultiplied);
                return await engine.RecognizeAsync(softwareBitmap);
            }
        }

        private static Windows.Foundation.Rect? GetWindowsOcrLineBounds(OcrLine line)
        {
            if (line == null || line.Words == null || line.Words.Count == 0)
            {
                return null;
            }

            double left = double.MaxValue;
            double top = double.MaxValue;
            double right = double.MinValue;
            double bottom = double.MinValue;

            foreach (OcrWord word in line.Words)
            {
                Windows.Foundation.Rect rect = word.BoundingRect;
                left = Math.Min(left, rect.X);
                top = Math.Min(top, rect.Y);
                right = Math.Max(right, rect.X + rect.Width);
                bottom = Math.Max(bottom, rect.Y + rect.Height);
            }

            if (left == double.MaxValue || top == double.MaxValue || right <= left || bottom <= top)
            {
                return null;
            }

            return new Windows.Foundation.Rect(left, top, right - left, bottom - top);
        }

        private static JObject CreateWindowsOcrElementJson(
            string level,
            int rank,
            int index,
            string text,
            float? confidence,
            Windows.Foundation.Rect? bounds,
            KernelGeom.Rectangle pageSize,
            int bitmapWidth,
            int bitmapHeight,
            int rotation)
        {
            var item = new JObject
            {
                ["level"] = level ?? string.Empty,
                ["rank"] = rank,
                ["index"] = index,
                ["text"] = text ?? string.Empty,
                ["confidence"] = confidence == null ? null : JToken.FromObject(confidence.Value)
            };

            item["bounds"] = bounds.HasValue
                ? CreateWindowsOcrBoundsJson(bounds.Value, pageSize, bitmapWidth, bitmapHeight, rotation)
                : null;

            return item;
        }

        private static JObject CreateWindowsOcrBoundsJson(
            Windows.Foundation.Rect ocrRect,
            KernelGeom.Rectangle pageSize,
            int bitmapWidth,
            int bitmapHeight,
            int rotation)
        {
            var item = new JObject
            {
                ["ocrPixels"] = new JObject
                {
                    ["x"] = ocrRect.X,
                    ["y"] = ocrRect.Y,
                    ["width"] = ocrRect.Width,
                    ["height"] = ocrRect.Height
                }
            };

            if (pageSize != null && bitmapWidth > 0 && bitmapHeight > 0)
            {
                KernelGeom.Rectangle pdfRect = ConvertWindowsOcrRectToPdfRect(ocrRect, pageSize, bitmapWidth, bitmapHeight, rotation);
                item["pdf"] = CreatePdfBoundsJson(pdfRect);
            }
            else
            {
                item["pdf"] = null;
            }

            return item;
        }

        private static CachedLine CreateCachedLineFromWindowsOcrLine(
            OcrLine textLine,
            int pageNumber,
            int pageRotation,
            KernelGeom.Rectangle pageSize,
            int bitmapWidth,
            int bitmapHeight,
            double? textAngleDegrees)
        {
            if (textLine == null || textLine.Words == null || textLine.Words.Count == 0 || pageSize == null)
            {
                return null;
            }

            Windows.Foundation.Rect? lineBounds = GetWindowsOcrLineBounds(textLine);
            if (!lineBounds.HasValue)
            {
                return null;
            }
            KernelGeom.Rectangle lineRect = ConvertWindowsOcrRectToPdfRect(lineBounds.Value, pageSize, bitmapWidth, bitmapHeight, pageRotation);
            var cachedLine = new CachedLine
            {
                PageNumber = pageNumber,
                PageRotation = pageRotation,
                YPosition = lineRect.GetY(),
                IsOcr = true,
                PageWidth = pageSize.GetWidth(),
                PageHeight = pageSize.GetHeight()
            };

            bool hasWords = false;
            float previousRight = lineRect.GetX();

            foreach (OcrWord word in textLine.Words)
            {
                string wordText = (word.Text ?? string.Empty).Trim();
                if (string.IsNullOrWhiteSpace(wordText))
                {
                    continue;
                }

                Windows.Foundation.Rect correctedOcrRect = MapWindowsOcrRectToOriginalImageRect(
                    word.BoundingRect,
                    bitmapWidth,
                    bitmapHeight,
                    textAngleDegrees);
                KernelGeom.Rectangle rawWordRect = ConvertWindowsOcrRectToPdfRect(correctedOcrRect, pageSize, bitmapWidth, bitmapHeight, pageRotation);
                cachedLine.RawOcrWordBounds.Add(rawWordRect);

                KernelGeom.Rectangle wordRect = rawWordRect;
                cachedLine.OcrWordBounds.Add(wordRect);
                if (hasWords)
                {
                    AppendOcrSpace(cachedLine, previousRight, wordRect.GetX(), wordRect.GetY(), wordRect.GetHeight());
                }

                int wordStartIndex = cachedLine.Text.Length;
                AppendOcrWord(cachedLine, wordText, wordRect);
                int wordLength = cachedLine.Text.Length - wordStartIndex;
                if (wordLength > 0)
                {
                    cachedLine.OcrWords.Add(new OcrWordInfo
                    {
                        Text = wordText,
                        StartIndex = wordStartIndex,
                        Length = wordLength,
                        BoundingBox = wordRect
                    });
                }

                previousRight = wordRect.GetX() + wordRect.GetWidth();
                hasWords = true;
            }

            return cachedLine;
        }

        private static IEnumerable<CachedLine> RecognizeOcrLines(
            Engine engine,
            Bitmap bitmap,
            int pageNumber,
            int pageRotation,
            KernelGeom.Rectangle pageSize,
            out JObject debugPage)
        {
            var result = new List<CachedLine>();
            debugPage = CreateOcrDebugPage(pageNumber, pageRotation, pageSize, bitmap);
            if (engine == null || bitmap == null || pageSize == null || bitmap.Width <= 0 || bitmap.Height <= 0)
            {
                return result;
            }

            using (var ms = new MemoryStream())
            {
                bitmap.Save(ms, ImageFormat.Png);
                ms.Position = 0;

                using (var pixImage = TesseractOCR.Pix.Image.LoadFromMemory(ms))
                using (var page = engine.Process(pixImage, TesseractOCR.Enums.PageSegMode.Auto))
                {
                    debugPage["text"] = page.Text ?? string.Empty;
                    debugPage["meanConfidence"] = page.MeanConfidence;
                    JArray debugBlocks = (JArray)debugPage["blocks"];
                    int rank = 0;
                    int blockIndex = 0;
                    foreach (var block in page.Layout)
                    {
                        JObject debugBlock = CreateOcrElementJson(
                            "block",
                            ++rank,
                            blockIndex,
                            block.Text,
                            block.Confidence,
                            block.BoundingBox,
                            pageSize,
                            bitmap.Width,
                            bitmap.Height,
                            pageRotation);
                        debugBlock["blockType"] = block.BlockType.ToString();
                        JArray debugParagraphs = new JArray();
                        debugBlock["paragraphs"] = debugParagraphs;
                        debugBlocks.Add(debugBlock);

                        int paragraphIndex = 0;
                        foreach (var paragraph in block.Paragraphs)
                        {
                            JObject debugParagraph = CreateOcrElementJson(
                                "paragraph",
                                ++rank,
                                paragraphIndex,
                                paragraph.Text,
                                paragraph.Confidence,
                                paragraph.BoundingBox,
                                pageSize,
                                bitmap.Width,
                                bitmap.Height,
                                pageRotation);
                            JArray debugLines = new JArray();
                            debugParagraph["lines"] = debugLines;
                            debugParagraphs.Add(debugParagraph);

                            int lineIndex = 0;
                            foreach (var textLine in paragraph.TextLines)
                            {
                                JObject debugLine = CreateOcrElementJson(
                                    "line",
                                    ++rank,
                                    lineIndex,
                                    textLine.Text,
                                    textLine.Confidence,
                                    textLine.BoundingBox,
                                    pageSize,
                                    bitmap.Width,
                                    bitmap.Height,
                                    pageRotation);
                                JArray debugWords = new JArray();
                                debugLine["words"] = debugWords;
                                debugLines.Add(debugLine);

                                int wordIndex = 0;
                                foreach (var word in textLine.Words)
                                {
                                    JObject debugWord = CreateOcrElementJson(
                                        "word",
                                        ++rank,
                                        wordIndex,
                                        word.Text,
                                        word.Confidence,
                                        word.BoundingBox,
                                        pageSize,
                                        bitmap.Width,
                                        bitmap.Height,
                                        pageRotation);
                                    JArray debugSymbols = new JArray();
                                    debugWord["symbols"] = debugSymbols;
                                    debugWords.Add(debugWord);

                                    int symbolIndex = 0;
                                    foreach (var symbol in word.Symbols)
                                    {
                                        JObject debugSymbol = CreateOcrElementJson(
                                            "symbol",
                                            ++rank,
                                            symbolIndex,
                                            symbol.Text,
                                            symbol.Confidence,
                                            symbol.BoundingBox,
                                            pageSize,
                                            bitmap.Width,
                                            bitmap.Height,
                                            pageRotation);
                                        debugSymbols.Add(debugSymbol);
                                        symbolIndex++;
                                    }

                                    wordIndex++;
                                }

                                CachedLine cachedLine = CreateCachedLineFromOcrTextLine(
                                    textLine,
                                    pageNumber,
                                    pageRotation,
                                    pageSize,
                                    bitmap.Width,
                                    bitmap.Height);

                                if (cachedLine != null && !string.IsNullOrWhiteSpace(cachedLine.Text))
                                {
                                    result.Add(cachedLine);
                                }

                                lineIndex++;
                            }

                            paragraphIndex++;
                        }

                        blockIndex++;
                    }
                }
            }

            return result;
        }

        private static JObject CreateOcrDebugPage(int pageNumber, int pageRotation, KernelGeom.Rectangle pageSize, Bitmap bitmap)
        {
            return new JObject
            {
                ["pageNumber"] = pageNumber,
                ["pageRotation"] = pageRotation,
                ["bitmap"] = bitmap == null
                    ? null
                    : new JObject
                    {
                        ["width"] = bitmap.Width,
                        ["height"] = bitmap.Height,
                        ["horizontalDpi"] = bitmap.HorizontalResolution,
                        ["verticalDpi"] = bitmap.VerticalResolution
                    },
                ["pdfPageBounds"] = pageSize == null
                    ? null
                    : CreatePdfBoundsJson(pageSize),
                ["blocks"] = new JArray()
            };
        }

        private static JObject CreateOcrElementJson(
            string level,
            int rank,
            int index,
            string text,
            float confidence,
            TesseractOCR.Rect? bounds,
            KernelGeom.Rectangle pageSize,
            int bitmapWidth,
            int bitmapHeight,
            int pageRotation)
        {
            var item = new JObject
            {
                ["level"] = level ?? string.Empty,
                ["rank"] = rank,
                ["index"] = index,
                ["text"] = text ?? string.Empty,
                ["confidence"] = confidence
            };

            if (bounds.HasValue)
            {
                item["bounds"] = CreateOcrBoundsJson(bounds.Value, pageSize, bitmapWidth, bitmapHeight, pageRotation);
            }
            else
            {
                item["bounds"] = null;
            }

            return item;
        }

        private static JObject CreateOcrBoundsJson(
            TesseractOCR.Rect ocrRect,
            KernelGeom.Rectangle pageSize,
            int bitmapWidth,
            int bitmapHeight,
            int pageRotation)
        {
            var item = new JObject
            {
                ["ocrPixels"] = new JObject
                {
                    ["x1"] = ocrRect.X1,
                    ["y1"] = ocrRect.Y1,
                    ["x2"] = ocrRect.X2,
                    ["y2"] = ocrRect.Y2,
                    ["width"] = ocrRect.Width,
                    ["height"] = ocrRect.Height
                }
            };

            if (pageSize != null && bitmapWidth > 0 && bitmapHeight > 0)
            {
                KernelGeom.Rectangle pdfRect = ConvertOcrRectToPdfRect(ocrRect, pageSize, bitmapWidth, bitmapHeight, pageRotation);
                item["pdf"] = CreatePdfBoundsJson(pdfRect);
            }
            else
            {
                item["pdf"] = null;
            }

            return item;
        }

        private static JObject CreatePdfBoundsJson(KernelGeom.Rectangle rect)
        {
            if (rect == null)
            {
                return null;
            }

            return new JObject
            {
                ["x"] = rect.GetX(),
                ["y"] = rect.GetY(),
                ["width"] = rect.GetWidth(),
                ["height"] = rect.GetHeight(),
                ["left"] = rect.GetLeft(),
                ["right"] = rect.GetRight(),
                ["bottom"] = rect.GetBottom(),
                ["top"] = rect.GetTop()
            };
        }

        private static CachedLine CreateCachedLineFromOcrTextLine(
            TesseractOCR.Layout.TextLine textLine,
            int pageNumber,
            int pageRotation,
            KernelGeom.Rectangle pageSize,
            int bitmapWidth,
            int bitmapHeight)
        {
            if (textLine == null)
            {
                return null;
            }

            var lineBounds = textLine.BoundingBox;
            if (!lineBounds.HasValue)
            {
                return null;
            }

            KernelGeom.Rectangle lineRect = ConvertOcrRectToPdfRect(lineBounds.Value, pageSize, bitmapWidth, bitmapHeight, pageRotation);
            var cachedLine = new CachedLine
            {
                PageNumber = pageNumber,
                PageRotation = pageRotation,
                YPosition = lineRect.GetY(),
                IsOcr = true,
                PageWidth = pageSize.GetWidth(),
                PageHeight = pageSize.GetHeight()
            };

            bool hasWords = false;
            float previousRight = lineRect.GetX();

            foreach (var word in textLine.Words)
            {
                string wordText = word.Text ?? string.Empty;
                if (string.IsNullOrWhiteSpace(wordText))
                {
                    continue;
                }

                var wordBounds = word.BoundingBox;
                if (!wordBounds.HasValue)
                {
                    continue;
                }

                KernelGeom.Rectangle wordRect = ConvertOcrRectToPdfRect(wordBounds.Value, pageSize, bitmapWidth, bitmapHeight, pageRotation);
                cachedLine.OcrWordBounds.Add(wordRect);
                if (hasWords)
                {
                    AppendOcrSpace(cachedLine, previousRight, wordRect.GetX(), wordRect.GetY(), wordRect.GetHeight());
                }

                int wordStartIndex = cachedLine.Text.Length;
                if (!AppendOcrWordFromSymbols(cachedLine, word, pageSize, bitmapWidth, bitmapHeight, pageRotation))
                {
                    AppendOcrWord(cachedLine, wordText.Trim(), wordRect);
                }
                int wordLength = cachedLine.Text.Length - wordStartIndex;
                if (wordLength > 0)
                {
                    cachedLine.OcrWords.Add(new OcrWordInfo
                    {
                        Text = wordText.Trim(),
                        StartIndex = wordStartIndex,
                        Length = wordLength,
                        BoundingBox = wordRect
                    });
                }
                previousRight = wordRect.GetX() + wordRect.GetWidth();
                hasWords = true;
            }

            if (!hasWords)
            {
                string fallbackText = (textLine.Text ?? string.Empty).Trim();
                cachedLine.OcrWordBounds.Add(lineRect);
                int wordStartIndex = cachedLine.Text.Length;
                AppendOcrWord(cachedLine, fallbackText, lineRect);
                int wordLength = cachedLine.Text.Length - wordStartIndex;
                if (wordLength > 0)
                {
                    cachedLine.OcrWords.Add(new OcrWordInfo
                    {
                        Text = cachedLine.Text.Substring(wordStartIndex, wordLength),
                        StartIndex = wordStartIndex,
                        Length = wordLength,
                        BoundingBox = lineRect
                    });
                }
            }

            return cachedLine;
        }

        private static KernelGeom.Rectangle ConvertOcrRectToPdfRect(
            TesseractOCR.Rect ocrRect,
            KernelGeom.Rectangle pageSize,
            int bitmapWidth,
            int bitmapHeight,
            int pageRotation)
        {
            float pageWidth = pageSize.GetWidth();
            float pageHeight = pageSize.GetHeight();

            int rotation = (pageRotation % 360 + 360) % 360;
            float visWidth = (rotation == 90 || rotation == 270) ? pageHeight : pageWidth;
            float visHeight = (rotation == 90 || rotation == 270) ? pageWidth : pageHeight;

            double x_v = ocrRect.X1 * (double)visWidth / bitmapWidth;
            double y_v = ocrRect.Y1 * (double)visHeight / bitmapHeight;
            double w_v = ocrRect.Width * (double)visWidth / bitmapWidth;
            double h_v = ocrRect.Height * (double)visHeight / bitmapHeight;

            double x_n, y_n, w_n, h_n;
            switch (rotation)
            {
                case 90:
                    x_n = y_v;
                    y_n = x_v;
                    w_n = h_v;
                    h_n = w_v;
                    break;
                case 180:
                    x_n = visWidth - x_v - w_v;
                    y_n = y_v;
                    w_n = w_v;
                    h_n = h_v;
                    break;
                case 270:
                    x_n = visHeight - y_v - h_v;
                    y_n = visWidth - x_v - w_v;
                    w_n = h_v;
                    h_n = w_v;
                    break;
                default:
                    x_n = x_v;
                    y_n = visHeight - y_v - h_v;
                    w_n = w_v;
                    h_n = h_v;
                    break;
            }

            return new KernelGeom.Rectangle((float)x_n, (float)y_n, (float)Math.Max(0.1, w_n), (float)Math.Max(0.1, h_n));
        }

        private static KernelGeom.Rectangle ConvertWindowsOcrRectToPdfRect(
            Windows.Foundation.Rect ocrRect,
            KernelGeom.Rectangle pageSize,
            int bitmapWidth,
            int bitmapHeight,
            int pageRotation)
        {
            float pageWidth = pageSize.GetWidth();
            float pageHeight = pageSize.GetHeight();

            int rotation = (pageRotation % 360 + 360) % 360;
            float visWidth = (rotation == 90 || rotation == 270) ? pageHeight : pageWidth;
            float visHeight = (rotation == 90 || rotation == 270) ? pageWidth : pageHeight;

            double x_v = ocrRect.X * (double)visWidth / bitmapWidth;
            double y_v = ocrRect.Y * (double)visHeight / bitmapHeight;
            double w_v = ocrRect.Width * (double)visWidth / bitmapWidth;
            double h_v = ocrRect.Height * (double)visHeight / bitmapHeight;

            double x_n, y_n, w_n, h_n;
            switch (rotation)
            {
                case 90:
                    x_n = y_v;
                    y_n = x_v;
                    w_n = h_v;
                    h_n = w_v;
                    break;
                case 180:
                    x_n = visWidth - x_v - w_v;
                    y_n = y_v;
                    w_n = w_v;
                    h_n = h_v;
                    break;
                case 270:
                    x_n = visHeight - y_v - h_v;
                    y_n = visWidth - x_v - w_v;
                    w_n = h_v;
                    h_n = w_v;
                    break;
                default:
                    x_n = x_v;
                    y_n = visHeight - y_v - h_v;
                    w_n = w_v;
                    h_n = h_v;
                    break;
            }

            return new KernelGeom.Rectangle((float)x_n, (float)y_n, (float)Math.Max(0.1, w_n), (float)Math.Max(0.1, h_n));
        }

        private static Windows.Foundation.Rect MapWindowsOcrRectToOriginalImageRect(
            Windows.Foundation.Rect ocrRect,
            int imageWidth,
            int imageHeight,
            double? textAngleDegrees)
        {
            if (imageWidth <= 0 || imageHeight <= 0 || ocrRect.Width <= 0d || ocrRect.Height <= 0d)
            {
                return ocrRect;
            }

            double angle = textAngleDegrees.HasValue
                ? -textAngleDegrees.Value * Math.PI / 180d
                : 0d;

            if (Math.Abs(angle) < 0.000001d)
            {
                return ocrRect;
            }

            double cosA = Math.Cos(angle);
            double sinA = Math.Sin(angle);
            double centerX = imageWidth / 2d;
            double centerY = imageHeight / 2d;

            System.Drawing.PointF p0 = MapWindowsOcrPointToOriginalImage(ocrRect.X, ocrRect.Y, centerX, centerY, cosA, sinA);
            System.Drawing.PointF p1 = MapWindowsOcrPointToOriginalImage(ocrRect.X + ocrRect.Width, ocrRect.Y, centerX, centerY, cosA, sinA);
            System.Drawing.PointF p2 = MapWindowsOcrPointToOriginalImage(ocrRect.X + ocrRect.Width, ocrRect.Y + ocrRect.Height, centerX, centerY, cosA, sinA);
            System.Drawing.PointF p3 = MapWindowsOcrPointToOriginalImage(ocrRect.X, ocrRect.Y + ocrRect.Height, centerX, centerY, cosA, sinA);

            double left = Math.Min(Math.Min(p0.X, p1.X), Math.Min(p2.X, p3.X));
            double top = Math.Min(Math.Min(p0.Y, p1.Y), Math.Min(p2.Y, p3.Y));
            double right = Math.Max(Math.Max(p0.X, p1.X), Math.Max(p2.X, p3.X));
            double bottom = Math.Max(Math.Max(p0.Y, p1.Y), Math.Max(p2.Y, p3.Y));

            left = Math.Max(0d, left);
            top = Math.Max(0d, top);
            right = Math.Min(imageWidth, right);
            bottom = Math.Min(imageHeight, bottom);

            if (right <= left || bottom <= top)
            {
                return ocrRect;
            }

            return new Windows.Foundation.Rect(left, top, right - left, bottom - top);
        }

        private static System.Drawing.PointF MapWindowsOcrPointToOriginalImage(
            double x,
            double y,
            double centerX,
            double centerY,
            double cosA,
            double sinA)
        {
            double dx = x - centerX;
            double dy = y - centerY;
            return new System.Drawing.PointF(
                (float)(centerX + (dx * cosA) + (dy * sinA)),
                (float)(centerY - (dx * sinA) + (dy * cosA)));
        }

        private static bool IntersectsAnyOcrImageBounds(CachedLine line, List<KernelGeom.Rectangle> imageBounds)
        {
            if (line == null || imageBounds == null || imageBounds.Count == 0)
            {
                return false;
            }

            KernelGeom.Rectangle lineBounds = GetCachedLineBounds(line);
            if (lineBounds == null)
            {
                return false;
            }

            return imageBounds.Any(imageRect => RectanglesIntersectWithTolerance(lineBounds, imageRect, 1.5f));
        }

        internal static KernelGeom.Rectangle GetCachedLineBounds(CachedLine line)
        {
            if (line == null)
            {
                return null;
            }

            IEnumerable<KernelGeom.Rectangle> rects = line.OcrWordBounds != null && line.OcrWordBounds.Count > 0
                ? line.OcrWordBounds
                : line.Characters?.Select(ch => ch.BoundingBox);

            if (rects == null)
            {
                return null;
            }

            bool hasRect = false;
            float left = float.MaxValue;
            float right = float.MinValue;
            float bottom = float.MaxValue;
            float top = float.MinValue;

            foreach (KernelGeom.Rectangle rect in rects)
            {
                if (rect == null || rect.GetWidth() <= 0f || rect.GetHeight() <= 0f)
                {
                    continue;
                }

                hasRect = true;
                left = Math.Min(left, rect.GetLeft());
                right = Math.Max(right, rect.GetRight());
                bottom = Math.Min(bottom, rect.GetBottom());
                top = Math.Max(top, rect.GetTop());
            }

            if (!hasRect || right <= left || top <= bottom)
            {
                return null;
            }

            return new KernelGeom.Rectangle(left, bottom, right - left, top - bottom);
        }

        private static bool RectanglesIntersectWithTolerance(KernelGeom.Rectangle a, KernelGeom.Rectangle b, float tolerance)
        {
            if (a == null || b == null)
            {
                return false;
            }

            float aLeft = a.GetLeft() - tolerance;
            float aRight = a.GetRight() + tolerance;
            float aBottom = a.GetBottom() - tolerance;
            float aTop = a.GetTop() + tolerance;
            float bLeft = b.GetLeft();
            float bRight = b.GetRight();
            float bBottom = b.GetBottom();
            float bTop = b.GetTop();

            return aLeft < bRight && aRight > bLeft && aBottom < bTop && aTop > bBottom;
        }

        private static void AppendOcrSpace(CachedLine line, float previousRight, float nextLeft, float y, float height)
        {
            if (line == null)
            {
                return;
            }

            float gap = Math.Max(0.1f, nextLeft - previousRight);
            line.Text += " ";
            line.Characters.Add(new CharacterInfo
            {
                Char = ' ',
                BoundingBox = new KernelGeom.Rectangle(previousRight, y, gap, Math.Max(0.1f, height))
            });
        }

        private static bool AppendOcrWordFromSymbols(
            CachedLine line,
            TesseractOCR.Layout.Word word,
            KernelGeom.Rectangle pageSize,
            int bitmapWidth,
            int bitmapHeight,
            int pageRotation)
        {
            if (line == null || word == null || pageSize == null || bitmapWidth <= 0 || bitmapHeight <= 0)
            {
                return false;
            }

            string wordText = (word.Text ?? string.Empty).Trim();
            if (string.IsNullOrEmpty(wordText))
            {
                return false;
            }

            var symbolCharacters = new List<CharacterInfo>();
            int beforeCount = line.Characters.Count;
            foreach (var symbol in word.Symbols)
            {
                string symbolText = symbol.Text ?? string.Empty;
                if (string.IsNullOrEmpty(symbolText))
                {
                    continue;
                }

                var symbolBounds = symbol.BoundingBox;
                if (!symbolBounds.HasValue)
                {
                    continue;
                }

                KernelGeom.Rectangle symbolRect = ConvertOcrRectToPdfRect(symbolBounds.Value, pageSize, bitmapWidth, bitmapHeight, pageRotation);
                if (symbolRect == null || symbolRect.GetWidth() <= 0f || symbolRect.GetHeight() <= 0f)
                {
                    continue;
                }

                AppendOcrWordToCharacters(symbolCharacters, symbolText, symbolRect);
            }

            if (symbolCharacters.Count != wordText.Length)
            {
                return false;
            }

            for (int i = 0; i < wordText.Length; i++)
            {
                symbolCharacters[i].Char = wordText[i];
            }

            line.Text += wordText;
            line.Characters.AddRange(symbolCharacters);
            return line.Characters.Count > beforeCount;
        }

        private static void AppendOcrWord(CachedLine line, string text, KernelGeom.Rectangle rect)
        {
            if (line == null || string.IsNullOrEmpty(text) || rect == null)
            {
                return;
            }

            float charWidth = Math.Max(0.1f, rect.GetWidth() / Math.Max(1, text.Length));
            for (int i = 0; i < text.Length; i++)
            {
                line.Text += text[i];
                line.Characters.Add(new CharacterInfo
                {
                    Char = text[i],
                    BoundingBox = new KernelGeom.Rectangle(
                        rect.GetX() + (charWidth * i),
                        rect.GetY(),
                        charWidth,
                        Math.Max(0.1f, rect.GetHeight()))
                });
            }
        }

        private static void AppendOcrWordToCharacters(List<CharacterInfo> characters, string text, KernelGeom.Rectangle rect)
        {
            if (characters == null || string.IsNullOrEmpty(text) || rect == null)
            {
                return;
            }

            float charWidth = Math.Max(0.1f, rect.GetWidth() / Math.Max(1, text.Length));
            for (int i = 0; i < text.Length; i++)
            {
                characters.Add(new CharacterInfo
                {
                    Char = text[i],
                    BoundingBox = new KernelGeom.Rectangle(
                        rect.GetX() + (charWidth * i),
                        rect.GetY(),
                        charWidth,
                        Math.Max(0.1f, rect.GetHeight()))
                });
            }
        }

        // Funkcja do odczytu text na podstawie line_number
        private static List<TextLocation> SearchInCachedLines(
            string pdfPath,
            string searchText,
            bool searchPersonalData,
            IWin32Window owner,
            System.Threading.CancellationToken cancellationToken,
            out bool nerCompleted)
        {

            var locations = new List<TextLocation> { };
            var cachedLines = _lineCache[pdfPath];
            string searchTextLower = searchText.ToLowerInvariant();
            nerCompleted = !searchPersonalData;

            int cnt = 0;
            int lastReportedPage = -1;
            foreach (var line in cachedLines)
            {
                cancellationToken.ThrowIfCancellationRequested();

                string textLower = line.Text.ToLowerInvariant();
                if (line.PageNumber != lastReportedPage)
                {
                    lastReportedPage = line.PageNumber;
                    ReportCacheStatus(LocalizedFormat("CacheStatus_SearchPage", line.PageNumber), pdfPath);
                }
                if (!searchPersonalData && textLower.Contains(searchTextLower))
                {
                    HashSet<int> exactOcrWordMatchStarts = AddExactOcrWordMatches(line, searchText, locations);
                    // TODO: add per-line search progress notification
                    int startIndex = textLower.IndexOf(searchTextLower, StringComparison.CurrentCultureIgnoreCase);
                    while (startIndex >= 0)
                    {
                        if (exactOcrWordMatchStarts != null && exactOcrWordMatchStarts.Contains(startIndex))
                        {
                            startIndex = textLower.IndexOf(searchTextLower, startIndex + 1, StringComparison.CurrentCultureIgnoreCase);
                            continue;
                        }

                        KernelGeom.Rectangle textRect = GetSearchResultRectangle(line, startIndex, searchText.Length);
                        if (textRect != null)
                        {
                            locations.Add(new TextLocation(line.PageNumber, line.PageRotation, textRect, line.IsOcr)
                            {
                                Text = searchText,
                                Source = PdfTextSearcher.IsOutOfPageBounds(textRect, line.PageWidth, line.PageHeight)
                                    ? LocationSource.OutOfBounds
                                    : LocationSource.Normal
                            });
                        }
                        startIndex = textLower.IndexOf(searchTextLower, startIndex + 1, StringComparison.CurrentCultureIgnoreCase);
                    }
                }
                cnt++;
            }

            if (searchPersonalData && PDDServerUrl!="")
            {
                DialogResult result = ShowMessageBox(
                    owner,
                    Resources.Msg_Confirm_NameSearchSlow,
                    Resources.Title_Warning,
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Exclamation,
                    MessageBoxDefaultButton.Button2);

                if (result == DialogResult.No)
                {
                    return locations;
                }

                using (var client = new HttpClient())
                {
                    var groupedByPage = cachedLines
                        .Select((line, index) => new { Line = line, Index = index })
                        .GroupBy(x => x.Line.PageNumber);

                    foreach (var pageGroup in groupedByPage)
                    {
                        var pageLines = pageGroup.ToList();

                        ReportCacheStatus(LocalizedFormat("CacheStatus_SearchPage", pageGroup.Key), pdfPath);

                        var reqlines = pageLines.Select(x => new
                        {
                            linenumber = x.Index,
                            text = x.Line.Text
                        }).ToList();

                        var requestData = new { reqlines };
                        string jsonRequest = JsonConvert.SerializeObject(requestData);
                        var content = new StringContent(jsonRequest, System.Text.Encoding.UTF8, "application/json");
                        string jsonOut = "{}";

                        try
                        {
                            var response = client.PostAsync(PDDServerUrl, content)
                                .GetAwaiter()
                                .GetResult();
                            response.EnsureSuccessStatusCode();
                            jsonOut = response.Content.ReadAsStringAsync()
                                .GetAwaiter()
                                .GetResult();
                        }
                        catch {

                            ShowMessageBox(owner, Resources.Msg_NameSearchServiceUnavailable, Resources.Title_Warning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return locations;
                        }

                        JObject obj = JObject.Parse(jsonOut);
                        JArray respLines = obj["resplines"] as JArray;

                        if (respLines == null) continue;

                        foreach (var respLine in respLines)
                        {
                            int lineNumber = int.Parse(respLine["linenumber"]?.ToString() ?? "-1");
                            if (lineNumber < 0 || lineNumber >= cachedLines.Count) continue;

                            var cachedLine = cachedLines[lineNumber];
                            var entities = respLine["entities"] as JArray;
                            if (entities == null || entities.Count == 0) continue;

                            string textLower = cachedLine.Text.ToLowerInvariant();

                            foreach (var entity in entities)
                            {
                                string entityText = entity["text"]?.ToString();
                                if (string.IsNullOrWhiteSpace(entityText)) continue;

                                string entityLower = entityText.ToLowerInvariant();
                                int startIndex = textLower.IndexOf(entityLower, StringComparison.CurrentCultureIgnoreCase);

                                while (startIndex >= 0)
                                {
                                    var textRect = GetSearchResultRectangle(cachedLine, startIndex, entityText.Length);
                                    if (textRect != null)
                                    {
                                        locations.Add(new TextLocation(cachedLine.PageNumber, cachedLine.PageRotation, textRect, cachedLine.IsOcr)
                                        {
                                            Text = entityText,
                                            Source = PdfTextSearcher.IsOutOfPageBounds(textRect, cachedLine.PageWidth, cachedLine.PageHeight)
                                                ? LocationSource.OutOfBounds
                                                : LocationSource.Normal
                                        });
                                    }
                                    startIndex = textLower.IndexOf(entityLower, startIndex + 1, StringComparison.CurrentCultureIgnoreCase);
                                }
                            }
                        }
                    }
                }

                locations = locations
                    .OrderBy(loc => loc.PageNumber)
                    .ThenByDescending(loc => loc.Rect.GetY())
                    .ToList();

                var deduplicatedLocations = new List<TextLocation>();
                foreach (var loc in locations)
                {
                    bool isDuplicate = false;
                    for (int i = 0; i < deduplicatedLocations.Count; i++)
                    {
                        var existing = deduplicatedLocations[i];

                        float x1 = loc.Rect.GetX();
                        float y1 = loc.Rect.GetY();
                        float w1 = loc.Rect.GetWidth();
                        float h1 = loc.Rect.GetHeight();

                        float x2 = existing.Rect.GetX();
                        float y2 = existing.Rect.GetY();
                        float w2 = existing.Rect.GetWidth();
                        float h2 = existing.Rect.GetHeight();

                        float cx1 = x1 + w1 / 2f;
                        float cy1 = y1 + h1 / 2f;
                        float cx2 = x2 + w2 / 2f;
                        float cy2 = y2 + h2 / 2f;

                        float toleranceX = Math.Max(15f, w2 * 0.5f);
                        float toleranceY = Math.Max(15f, h2 * 0.8f);

                        if (loc.PageNumber == existing.PageNumber &&
                            Math.Abs(cx1 - cx2) < toleranceX &&
                            Math.Abs(cy1 - cy2) < toleranceY)
                        {
                            if (h1 > h2)
                            {
                                deduplicatedLocations[i] = loc;
                            }
                            else if (h1 == h2 && w1 > w2)
                            {
                                deduplicatedLocations[i] = loc;
                            }
                            isDuplicate = true;
                            break;
                        }
                    }
                    if (!isDuplicate)
                    {
                        deduplicatedLocations.Add(loc);
                    }
                }
                locations = deduplicatedLocations;
            }
            if (searchPersonalData)
            {
                nerCompleted = SearchLocalMlNamedEntities(pdfPath, cachedLines, locations, cancellationToken);
            }
            if (searchPersonalData)
            {
                locations = DeduplicateTextLocations(locations);
            }
            ReportCacheStatus(string.Empty, pdfPath);

            // Remove OCR hits that duplicate a native-text hit at the same position.
            // Scanned PDFs with an embedded OCR text layer (from the scanner) produce both
            // a native-text result (iText extraction) and an app-OCR result (Windows OCR on
            // the image) for the same word.  The native-text result is preferred.
            locations = RemoveOcrDuplicates(locations);

            // Sort results by page then by position (top-to-bottom) so
            // navigation follows the visual reading order regardless of
            // whether results come from native text or OCR lines.
            locations = locations
                .OrderBy(loc => loc.PageNumber)
                .ThenByDescending(loc => loc.Rect.GetY())
                .ToList();

            return locations;
        }

        private static List<TextLocation> RemoveOcrDuplicates(List<TextLocation> locations)
        {
            // When both a native-text result (iText) and an app-OCR result (Windows OCR)
            // exist at the same position, keep the one with the LARGER height.
            // For scanned PDFs the Windows OCR bounding box is taller because it is derived
            // from actual ink pixels, while the scanner's embedded text layer uses compact
            // font metrics that may not cover the full glyph height.
            // For typed PDFs there are no OCR lines, so nothing is changed.
            var result = new List<TextLocation>(locations.Count);
            foreach (var pageGroup in locations.GroupBy(l => l.PageNumber))
            {
                var native = pageGroup.Where(l => !l.IsOcr).ToList();
                var ocr    = pageGroup.Where(l => l.IsOcr).ToList();

                if (native.Count == 0 || ocr.Count == 0)
                {
                    result.AddRange(pageGroup);
                    continue;
                }

                var nativeToRemove = new HashSet<TextLocation>();
                var ocrToRemove    = new HashSet<TextLocation>();

                foreach (var ocrLoc in ocr)
                {
                    foreach (var nativeLoc in native)
                    {
                        if (RectOverlapFraction(nativeLoc.Rect, ocrLoc.Rect) > 0.30f)
                        {
                            if (ocrLoc.Rect.GetHeight() >= nativeLoc.Rect.GetHeight())
                                nativeToRemove.Add(nativeLoc);
                            else
                                ocrToRemove.Add(ocrLoc);
                            break;
                        }
                    }
                }

                result.AddRange(native.Where(n => !nativeToRemove.Contains(n)));
                result.AddRange(ocr.Where(o => !ocrToRemove.Contains(o)));
            }
            return result;
        }

        private static float RectOverlapFraction(KernelGeom.Rectangle a, KernelGeom.Rectangle b)
        {
            float ix = Math.Max(0f, Math.Min((float)(a.GetX() + a.GetWidth()), (float)(b.GetX() + b.GetWidth()))
                                  - Math.Max((float)a.GetX(), (float)b.GetX()));
            float iy = Math.Max(0f, Math.Min((float)(a.GetY() + a.GetHeight()), (float)(b.GetY() + b.GetHeight()))
                                  - Math.Max((float)a.GetY(), (float)b.GetY()));
            float intersection = ix * iy;
            if (intersection <= 0f) return 0f;
            float areaA = (float)(a.GetWidth() * a.GetHeight());
            float areaB = (float)(b.GetWidth() * b.GetHeight());
            float minArea = Math.Min(areaA, areaB);
            return minArea > 0f ? intersection / minArea : 0f;
        }

        private static bool SearchLocalMlNamedEntities(string pdfPath, List<CachedLine> cachedLines, List<TextLocation> locations, System.Threading.CancellationToken cancellationToken = default)
        {
            if (cachedLines == null || cachedLines.Count == 0 || locations == null)
                return false;

            LocalNerOptions options = LocalNerOptions.Load();
            if (!options.Enabled)
                return false;

            string language = DetectLanguageFromLines(cachedLines);
            if (language != null)
            {
                LastDetectedLanguage = language;
                PluginManifest manifest = DiscoverPluginForLanguage(language, options.ExecutablePath);
                if (manifest != null)
                    options = options.WithPlugin(manifest);
            }

            ReportCacheStatus(LocalizedText("CacheStatus_LocalNer"), pdfPath);
            try
            {
                string responseJson = RunLocalNerDaemon(cachedLines, options, cancellationToken, pdfPath);
                if (string.IsNullOrWhiteSpace(responseJson))
                    return false;

                AddLocalMlEntityLocations(cachedLines, responseJson, locations, options);
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Local NER unavailable: " + ex.Message);
                return false;
            }
        }

        // German legal keywords that are ASCII-safe (survive lossy PDF encoding)
        private static readonly string[] _deKeywords =
        {
            "Steuer", "Notar", "Kaufvertrag", "Handelsregister", "Grundbuch",
            "Beurkundung", "Gesellschaft", "notariell", "HRB", "HRA",
            "Umsatzsteuer", "Grundschuld", "Eigentum", "GmbH"
        };

        // Polish legal keywords that are ASCII-safe
        private static readonly string[] _plKeywords =
        {
            "PESEL", "NIP", "REGON", "KRS", "notariusz", "Wnioskodawca",
            "Gmina", "Powiat", "Rejonowy", "Okregowy", "Nieruchomosc", "Hipoteka"
        };

        private static string DetectLanguageFromLines(List<CachedLine> lines)
        {
            const int maxLines = 200;
            int plScore = 0, deScore = 0, totalChars = 0;
            var sb = new System.Text.StringBuilder();
            foreach (CachedLine line in lines.Take(maxLines))
            {
                string text = line?.Text;
                if (string.IsNullOrEmpty(text)) continue;
                foreach (char c in text)
                {
                    if ("ąęóśźżćńłĄĘÓŚŹŻĆŃŁ".IndexOf(c) >= 0) plScore++;
                    else if ("äöüßÄÖÜ".IndexOf(c) >= 0) deScore++;
                }
                totalChars += text.Length;
                sb.Append(text).Append(' ');
            }
            if (totalChars == 0) return null;

            // Primary: diacritic character ratio — fast and reliable when encoding intact
            if (plScore > deScore && plScore * 300 > totalChars) return "pl";
            if (deScore > plScore && deScore * 300 > totalChars) return "de";

            // Fallback: ASCII keyword scoring — handles PDFs where umlauts are stripped
            string sample = sb.ToString();
            int deKw = _deKeywords.Count(kw =>
                sample.IndexOf(kw, StringComparison.OrdinalIgnoreCase) >= 0);
            int plKw = _plKeywords.Count(kw =>
                sample.IndexOf(kw, StringComparison.OrdinalIgnoreCase) >= 0);

            if (deKw > plKw && deKw >= 2) return "de";
            if (plKw > deKw && plKw >= 2) return "pl";
            return null;
        }

        private static PluginManifest DiscoverPluginForLanguage(string language, string configuredExePath)
        {
            if (string.IsNullOrEmpty(language))
                return null;

            string pluginsBase = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "plugins");
            if (!Directory.Exists(pluginsBase)) return null;

            foreach (string subDir in Directory.GetDirectories(pluginsBase))
            {
                string configPath = Path.Combine(subDir, "config.json");
                if (!File.Exists(configPath)) continue;
                try
                {
                    var config = JObject.Parse(File.ReadAllText(configPath, System.Text.Encoding.UTF8));
                    string lang = (string)config["language"];
                    if (!string.Equals(lang, language, StringComparison.OrdinalIgnoreCase))
                        continue;

                    string folderName = Path.GetFileName(subDir);
                    string exePath = Path.Combine(subDir, folderName + ".exe");
                    if (!File.Exists(exePath))
                    {
                        string[] exes = Directory.GetFiles(subDir, "*.exe");
                        exePath = exes.Length > 0 ? exes[0] : null;
                    }
                    if (string.IsNullOrEmpty(exePath)) continue;

                    var labels = (config["labels"] as JArray)
                        ?.Select(t => (string)t)
                        .Where(s => !string.IsNullOrEmpty(s))
                        .ToList();

                    return new PluginManifest { Language = lang, ExecutablePath = exePath, Labels = labels };
                }
                catch { }
            }
            return null;
        }

        // -----------------------------------------------------------------------
        // NER daemon — subprocess kept alive across calls for per-line progress
        // -----------------------------------------------------------------------

        private static Process _nerDaemonProcess;
        private static StreamWriter _nerDaemonStdin;
        private static StreamReader _nerDaemonStdout;
        private static string _nerDaemonCommandKey;
        private static readonly object _nerDaemonLock = new object();
        // Set to true by the UI thread before shutdown so background NER work exits quickly.
        public static volatile bool IsShuttingDown = false;

        private static string BuildNerDaemonCommandKey(LocalNerOptions options) =>
            $"{options.ExecutablePath}|{options.ModelName}|{string.Join(",", options.Labels)}";

        /// <summary>Starts the NER daemon if it is not already running. Must be called under _nerDaemonLock.</summary>
        private static bool EnsureNerDaemonLocked(LocalNerOptions options)
        {
            string key = BuildNerDaemonCommandKey(options);
            if (_nerDaemonProcess != null && !_nerDaemonProcess.HasExited && _nerDaemonCommandKey == key)
                return true;

            StopNerDaemonLocked();

            if (!TryResolveLocalNerCommand(options, out string fileName, out string argumentsPrefix))
                return false;

            string arguments =
                argumentsPrefix +
                " --daemon" +
                " --model " + QuoteProcessArgument(options.ModelName) +
                " --labels " + QuoteProcessArgument(string.Join(",", options.Labels));

            var psi = new ProcessStartInfo
            {
                FileName = fileName,
                Arguments = arguments,
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                StandardOutputEncoding = new System.Text.UTF8Encoding(false),
                StandardErrorEncoding = System.Text.Encoding.UTF8
            };

            try
            {
                _nerDaemonProcess = Process.Start(psi);
                _nerDaemonStdin = new StreamWriter(
                    _nerDaemonProcess.StandardInput.BaseStream,
                    new System.Text.UTF8Encoding(false)) { AutoFlush = true };
                _nerDaemonStdout = _nerDaemonProcess.StandardOutput;

                // Wait for {"ready": true} handshake — daemon signals model is loaded.
                string readyLine = _nerDaemonStdout.ReadLine();
                if (string.IsNullOrWhiteSpace(readyLine))
                {
                    StopNerDaemonLocked();
                    return false;
                }
                JObject ready = JObject.Parse(readyLine);
                if (ready["ready"]?.Value<bool>() != true)
                {
                    StopNerDaemonLocked();
                    return false;
                }

                _nerDaemonCommandKey = key;
                Debug.WriteLine("NER daemon ready: " + ready["model"]?.ToString());
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("NER daemon start failed: " + ex.Message);
                StopNerDaemonLocked();
                return false;
            }
        }

        private static void StopNerDaemonLocked()
        {
            try { _nerDaemonStdin?.WriteLine("{\"exit\":true}"); } catch { }
            try { _nerDaemonStdin?.Close(); } catch { }
            try { _nerDaemonProcess?.Kill(); } catch { }
            _nerDaemonProcess = null;
            _nerDaemonStdin = null;
            _nerDaemonStdout = null;
            _nerDaemonCommandKey = null;
        }

        /// <summary>Stops the NER daemon. Call when closing a document or the application.</summary>
        public static void StopNerDaemon()
        {
            lock (_nerDaemonLock)
            {
                StopNerDaemonLocked();
            }
        }

        /// <summary>
        /// Pre-warms the NER daemon so the model is loaded before the user opens a document.
        /// Blocks until the daemon reports ready or fails. Safe to call from a background thread.
        /// country: ISO-2 country code ("pl", "de") used to select the right plugin; empty = base model only.
        /// </summary>
        public static bool WarmUpNerDaemon(string country)
        {
            LocalNerOptions options = LocalNerOptions.Load();
            if (!options.Enabled) return false;

            if (!string.IsNullOrEmpty(country))
            {
                PluginManifest manifest = DiscoverPluginForLanguage(country, options.ExecutablePath);
                if (manifest != null)
                    options = options.WithPlugin(manifest);
            }

            lock (_nerDaemonLock)
            {
                return EnsureNerDaemonLocked(options);
            }
        }

        private const int NerDaemonBatchSize = 20;

        private static string RunLocalNerDaemon(
            List<CachedLine> cachedLines,
            LocalNerOptions options,
            System.Threading.CancellationToken cancellationToken = default,
            string statusPdfPath = null)
        {
            lock (_nerDaemonLock)
            {
                if (!EnsureNerDaemonLocked(options))
                    return RunLocalNerProcess(cachedLines, options);

                int total = cachedLines.Count;
                var combinedRespLines = new JArray();

                string nerLogPath = PDFForm.IsDiagnosticModeEnabled
                    ? Path.Combine(Path.GetTempPath(), "AnonPDF-ner.log")
                    : null;
                if (nerLogPath != null)
                    File.AppendAllText(nerLogPath,
                        $"\r\n=== NER run {DateTime.Now:yyyy-MM-dd HH:mm:ss} ({total} lines, batch={NerDaemonBatchSize}) ===\r\n");

                for (int batchStart = 0; batchStart < total; batchStart += NerDaemonBatchSize)
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    int batchEnd = Math.Min(batchStart + NerDaemonBatchSize, total);
                    var reqLines = new JArray();
                    for (int i = batchStart; i < batchEnd; i++)
                    {
                        string lineText = cachedLines[i]?.Text ?? string.Empty;
                        reqLines.Add(new JObject { ["linenumber"] = i, ["text"] = lineText });
                        if (nerLogPath != null)
                            File.AppendAllText(nerLogPath, $"[{i:D3}] IN:  {lineText}\r\n");
                    }

                    var req = new JObject { ["reqlines"] = reqLines };
                    // Allow 5 s per line in the batch, minimum 15 s
                    int batchTimeoutMs = Math.Max(15_000, (batchEnd - batchStart) * 5_000);

                    try
                    {
                        _nerDaemonStdin.WriteLine(req.ToString(Formatting.None));

                        var readTask = _nerDaemonStdout.ReadLineAsync();
                        if (!readTask.Wait(batchTimeoutMs, cancellationToken) || readTask.Result == null)
                            throw new TimeoutException(
                                $"NER daemon timeout (lines {batchStart}-{batchEnd - 1}).");

                        JObject resp = JObject.Parse(readTask.Result);
                        if (resp["error"] != null)
                            throw new IOException("NER daemon error: " + resp["error"]);

                        if (resp["resplines"] is JArray respLines)
                        {
                            foreach (var item in respLines)
                            {
                                combinedRespLines.Add(item);
                                var ents = item["entities"] as JArray;
                                if (nerLogPath != null && ents != null && ents.Count > 0)
                                    File.AppendAllText(nerLogPath,
                                        $"[{item["linenumber"]:D3}] OUT: {ents.ToString(Formatting.None)}\r\n");
                            }
                        }
                    }
                    catch (OperationCanceledException)
                    {
                        StopNerDaemonLocked();
                        throw;
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"NER daemon failed (lines {batchStart}-{batchEnd - 1}): " + ex.Message);
                        StopNerDaemonLocked();
                        if (IsShuttingDown)
                            return new JObject { ["resplines"] = new JArray() }.ToString(Formatting.None);
                        return RunLocalNerProcess(cachedLines, options);
                    }

                    ReportCacheStatus(
                        LocalizedText("CacheStatus_LocalNer") + $": {batchEnd}/{total}",
                        statusPdfPath);
                }

                return new JObject { ["resplines"] = combinedRespLines }.ToString(Formatting.None);
            }
        }

        private static string RunLocalNerProcess(List<CachedLine> cachedLines, LocalNerOptions options)
        {
            if (!TryResolveLocalNerCommand(options, out string fileName, out string argumentsPrefix))
            {
                return string.Empty;
            }

            var request = new JObject
            {
                ["reqlines"] = new JArray(cachedLines.Select((line, index) => new JObject
                {
                    ["linenumber"] = index,
                    ["text"] = line?.Text ?? string.Empty
                }))
            };

            string arguments =
                argumentsPrefix +
                " --model " + QuoteProcessArgument(options.ModelName) +
                " --labels " + QuoteProcessArgument(string.Join(",", options.Labels));

            var startInfo = new ProcessStartInfo
            {
                FileName = fileName,
                Arguments = arguments,
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                StandardOutputEncoding = System.Text.Encoding.UTF8,
                StandardErrorEncoding = System.Text.Encoding.UTF8
            };

            using (var process = new Process { StartInfo = startInfo })
            {
                var stdout = new System.Text.StringBuilder();
                var stderr = new System.Text.StringBuilder();
                process.OutputDataReceived += (_, e) =>
                {
                    if (!string.IsNullOrWhiteSpace(e.Data))
                    {
                        stdout.AppendLine(e.Data);
                    }
                };
                process.ErrorDataReceived += (_, e) =>
                {
                    if (!string.IsNullOrWhiteSpace(e.Data))
                    {
                        stderr.AppendLine(e.Data);
                    }
                };

                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();
                using (var stdin = new StreamWriter(process.StandardInput.BaseStream, new System.Text.UTF8Encoding(false)))
                {
                    stdin.WriteLine(request.ToString(Formatting.None));
                }

                if (!process.WaitForExit(options.TimeoutMs))
                {
                    try { process.Kill(); } catch { }
                    Debug.WriteLine("Local NER timed out.");
                    return string.Empty;
                }

                process.WaitForExit();
                if (process.ExitCode != 0)
                {
                    Debug.WriteLine("Local NER failed: " + stderr);
                    return string.Empty;
                }

                return stdout.ToString();
            }
        }

        private static bool TryResolveLocalNerCommand(
            LocalNerOptions options,
            out string fileName,
            out string argumentsPrefix)
        {
            fileName = string.Empty;
            argumentsPrefix = string.Empty;
            if (options == null)
            {
                return false;
            }

            string executablePath = ResolveLocalNerPath(options.ExecutablePath);
            if (!string.IsNullOrWhiteSpace(executablePath) && File.Exists(executablePath))
            {
                string pluginSourceDir = Path.GetDirectoryName(executablePath);
                if (ShouldUsePluginFromSource(pluginSourceDir))
                {
                    fileName = executablePath;
                    return true;
                }

                string localDir = EnsurePluginCachedLocally(pluginSourceDir);
                string localExe = Path.Combine(localDir, Path.GetFileName(executablePath));
                fileName = File.Exists(localExe) ? localExe : executablePath;
                return true;
            }

            string scriptPath = ResolveLocalNerPath(options.ScriptPath);
            if (!string.IsNullOrWhiteSpace(options.PythonExe) && !string.IsNullOrWhiteSpace(scriptPath) && File.Exists(scriptPath))
            {
                fileName = options.PythonExe;
                argumentsPrefix = QuoteProcessArgument(scriptPath);
                return true;
            }

            return false;
        }

        private static void AddLocalMlEntityLocations(
            List<CachedLine> cachedLines,
            string responseJson,
            List<TextLocation> locations,
            LocalNerOptions options)
        {
            JObject obj = JObject.Parse(responseJson);
            JArray respLines = obj["resplines"] as JArray;
            if (respLines == null)
            {
                return;
            }

            foreach (JObject respLine in respLines.OfType<JObject>())
            {
                int lineNumber = respLine["linenumber"]?.Value<int>() ?? -1;
                if (lineNumber < 0 || lineNumber >= cachedLines.Count)
                {
                    continue;
                }

                CachedLine cachedLine = cachedLines[lineNumber];
                JArray entities = respLine["entities"] as JArray;
                if (cachedLine == null || entities == null || entities.Count == 0)
                {
                    continue;
                }

                foreach (JObject entity in entities.OfType<JObject>())
                {
                    string label = entity["label"]?.ToString() ?? string.Empty;
                    if (!options.AcceptsLabel(label))
                    {
                        continue;
                    }

                    int startIndex = entity["start"]?.Value<int>() ?? -1;
                    int endIndex = entity["end"]?.Value<int>() ?? -1;
                    string entityText = entity["text"]?.ToString() ?? string.Empty;
                    if (!IsAcceptableLocalMlEntity(label, entityText))
                    {
                        continue;
                    }

                    if (startIndex >= 0 && endIndex > startIndex && endIndex <= cachedLine.Text.Length)
                    {
                        if (TryGetAddressSpan(cachedLine.Text, startIndex, endIndex, out int addressStart, out int addressLength))
                        {
                            int c0 = locations.Count;
                            AddLocationForSpan(cachedLine, addressStart, addressLength, locations);
                            for (int li = c0; li < locations.Count; li++) { locations[li].Label = label; locations[li].Text = entityText; }
                            continue;
                        }

                        int c1 = locations.Count;
                        AddLocationForSpan(cachedLine, startIndex, endIndex - startIndex, locations);
                        for (int li = c1; li < locations.Count; li++) { locations[li].Label = label; locations[li].Text = entityText; }
                        continue;
                    }

                    int c2 = locations.Count;
                    AddEntityTextOccurrences(cachedLine, entityText, locations);
                    for (int li = c2; li < locations.Count; li++) { locations[li].Label = label; locations[li].Text = entityText; }
                }
            }
        }

        private static void AddEntityTextOccurrences(CachedLine cachedLine, string entityText, List<TextLocation> locations)
        {
            if (cachedLine == null || string.IsNullOrWhiteSpace(cachedLine.Text) || string.IsNullOrWhiteSpace(entityText))
            {
                return;
            }

            string textLower = cachedLine.Text.ToLowerInvariant();
            string entityLower = entityText.ToLowerInvariant();
            int startIndex = textLower.IndexOf(entityLower, StringComparison.CurrentCultureIgnoreCase);
            while (startIndex >= 0)
            {
                int endIndex = startIndex + entityText.Length;
                if (TryGetAddressSpan(cachedLine.Text, startIndex, endIndex, out int addressStart, out int addressLength))
                {
                    AddLocationForSpan(cachedLine, addressStart, addressLength, locations);
                }
                else
                {
                    AddLocationForSpan(cachedLine, startIndex, entityText.Length, locations);
                }

                startIndex = textLower.IndexOf(entityLower, startIndex + 1, StringComparison.CurrentCultureIgnoreCase);
            }
        }

        private static bool TryGetAddressSpan(string lineText, int startIndex, int endIndex, out int addressStart, out int addressLength)
        {
            addressStart = -1;
            addressLength = 0;

            if (string.IsNullOrWhiteSpace(lineText) ||
                startIndex < 0 ||
                endIndex <= startIndex ||
                startIndex > lineText.Length ||
                endIndex > lineText.Length)
            {
                return false;
            }

            foreach (Match match in AddressPattern.Matches(lineText))
            {
                Group valueGroup = match.Groups["value"];
                int matchStart = valueGroup.Success ? valueGroup.Index : match.Index;
                int matchLength = valueGroup.Success ? valueGroup.Length : match.Length;
                int matchEnd = matchStart + matchLength;

                if (matchStart <= startIndex && matchEnd >= endIndex)
                {
                    addressStart = matchStart;
                    addressLength = matchLength;
                    return true;
                }
            }

            return false;
        }

        // NLP-produced entity labels that carry free-form text and need stop-word filtering.
        private static readonly HashSet<string> _nlpEntityLabels = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "PERSON", "PER", "persName", "LOCATION", "LOC", "GPE", "placeName", "geogName"
        };

        private static bool IsAcceptableLocalMlEntity(string label, string entityText)
        {
            if (string.IsNullOrWhiteSpace(entityText))
            {
                return false;
            }

            // Identifier labels (PHONE, EMAIL, STEUER_ID, BANK_ACCOUNT, etc.) are produced by
            // validated regex patterns — text-based heuristics designed for NLP tokens don't apply.
            if (!_nlpEntityLabels.Contains(label ?? string.Empty))
            {
                return true;
            }

            string normalized = NormalizeTextForNer(entityText);
            if (string.IsNullOrWhiteSpace(normalized))
            {
                return false;
            }

            string[] parts = normalized.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 0 ||
                parts.Any(part => NerInstructionStopWords.Contains(part) || NerUiLeadWords.Contains(part) || NerEntityStopWords.Contains(part)))
            {
                return false;
            }

            if (normalized.Contains("ctrl") ||
                normalized.Contains("enter") ||
                normalized.Contains("sekcja") ||
                normalized.Contains("podpis") ||
                entityText.IndexOf('+') >= 0)
            {
                return false;
            }

            string compact = Regex.Replace(entityText ?? string.Empty, @"\s+", string.Empty);
            if (compact.Length <= 3 && compact.All(ch => char.IsUpper(ch) || char.IsDigit(ch)))
            {
                return false;
            }

            if (string.Equals(label, "persName", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(label, "PERSON", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(label, "PER", StringComparison.OrdinalIgnoreCase))
            {
                return IsLikelyPersonEntity(entityText);
            }

            return true;
        }

        private static string ResolveLocalNerPath(string configuredPath)
        {
            if (string.IsNullOrWhiteSpace(configuredPath))
            {
                return string.Empty;
            }

            string expanded = Environment.ExpandEnvironmentVariables(configuredPath.Trim());
            return Path.IsPathRooted(expanded)
                ? expanded
                : Path.Combine(AppDomain.CurrentDomain.BaseDirectory, expanded);
        }

        // ---------------------------------------------------------------------------
        // Plugin local cache (Variant A — copy from network share to %LOCALAPPDATA%)
        // ---------------------------------------------------------------------------

        private static bool ShouldUsePluginFromSource(string pluginSourceDir)
        {
            if (string.IsNullOrWhiteSpace(pluginSourceDir) || !Directory.Exists(pluginSourceDir))
            {
                return false;
            }

            try
            {
                string appPluginsDir = Path.GetFullPath(
                    Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "plugins"))
                    .TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)
                    + Path.DirectorySeparatorChar;
                string sourceDir = Path.GetFullPath(pluginSourceDir)
                    .TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)
                    + Path.DirectorySeparatorChar;

                if (!sourceDir.StartsWith(appPluginsDir, StringComparison.OrdinalIgnoreCase))
                {
                    return false;
                }

                return IsLocalFixedDrivePath(sourceDir);
            }
            catch
            {
                return false;
            }
        }

        private static bool IsLocalFixedDrivePath(string path)
        {
            try
            {
                string root = Path.GetPathRoot(Path.GetFullPath(path));
                if (string.IsNullOrWhiteSpace(root) || root.StartsWith(@"\\", StringComparison.Ordinal))
                {
                    return false;
                }

                var drive = new DriveInfo(root);
                return drive.DriveType == DriveType.Fixed;
            }
            catch
            {
                return false;
            }
        }

        private static readonly HashSet<string> _syncedPluginDirs =
            new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        private static readonly object _pluginSyncLock = new object();

        private static string GetLocalPluginCacheRoot() =>
            Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "skmislab", "AnonPDFPro", "plugins");

        /// <summary>
        /// Ensures the plugin directory is present in the local user cache and up to date.
        /// Returns the local directory path, or <paramref name="pluginSourceDir"/> as fallback.
        /// Thread-safe; sync runs at most once per session per plugin.
        /// </summary>
        private static string EnsurePluginCachedLocally(string pluginSourceDir)
        {
            if (string.IsNullOrEmpty(pluginSourceDir) || !Directory.Exists(pluginSourceDir))
                return pluginSourceDir;

            string pluginName = Path.GetFileName(
                pluginSourceDir.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar));
            string localDir = Path.Combine(GetLocalPluginCacheRoot(), pluginName);

            lock (_pluginSyncLock)
            {
                if (_syncedPluginDirs.Contains(localDir))
                    return localDir;

                try
                {
                    string sourceConfigPath = Path.Combine(pluginSourceDir, "config.json");
                    if (!File.Exists(sourceConfigPath))
                    {
                        // Plugin has no config — run from source without caching
                        _syncedPluginDirs.Add(localDir);
                        return pluginSourceDir;
                    }

                    string sourceVersion = ReadPluginVersion(sourceConfigPath);
                    string localConfigPath = Path.Combine(localDir, "config.json");
                    string localVersion = File.Exists(localConfigPath)
                        ? ReadPluginVersion(localConfigPath)
                        : null;

                    if (sourceVersion != localVersion)
                    {
                        ReportCacheStatus("instalacja pluginu NER...");
                        CopyPluginDirectory(pluginSourceDir, localDir);
                        Debug.WriteLine($"Plugin '{pluginName}' cached locally: {localDir} (v{sourceVersion})");
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Plugin cache sync failed for '{pluginSourceDir}': {ex.Message}");
                    // Use source if local cache is missing or broken
                    bool localUsable = Directory.Exists(localDir) &&
                                       File.Exists(Path.Combine(localDir, "config.json"));
                    if (!localUsable)
                    {
                        _syncedPluginDirs.Add(localDir);
                        return pluginSourceDir;
                    }
                }

                _syncedPluginDirs.Add(localDir);
                return localDir;
            }
        }

        private static string ReadPluginVersion(string configJsonPath)
        {
            try
            {
                JObject obj = JObject.Parse(File.ReadAllText(configJsonPath, System.Text.Encoding.UTF8));
                return obj["version"]?.Value<string>() ?? "0.0.0";
            }
            catch
            {
                return "0.0.0";
            }
        }

        private static void CopyPluginDirectory(string sourceDir, string targetDir)
        {
            if (Directory.Exists(targetDir))
                Directory.Delete(targetDir, recursive: true);

            foreach (string sourcePath in Directory.EnumerateFiles(sourceDir, "*", SearchOption.AllDirectories))
            {
                string relative = sourcePath
                    .Substring(sourceDir.Length)
                    .TrimStart(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
                string targetPath = Path.Combine(targetDir, relative);
                Directory.CreateDirectory(Path.GetDirectoryName(targetPath));
                File.Copy(sourcePath, targetPath, overwrite: true);
            }
        }

        private static string QuoteProcessArgument(string value)
        {
            value = value ?? string.Empty;
            return "\"" + value.Replace("\"", "\\\"") + "\"";
        }

        private static List<TextLocation> DeduplicateTextLocations(List<TextLocation> locations)
        {
            if (locations == null || locations.Count <= 1)
            {
                return locations ?? new List<TextLocation>();
            }

            var deduplicatedLocations = new List<TextLocation>();
            foreach (var loc in locations
                .OrderBy(location => location.PageNumber)
                .ThenByDescending(location => location.Rect.GetY()))
            {
                bool isDuplicate = false;
                for (int i = 0; i < deduplicatedLocations.Count; i++)
                {
                    var existing = deduplicatedLocations[i];
                    if (loc.PageNumber != existing.PageNumber)
                        continue;

                    float x1 = loc.Rect.GetX();
                    float y1 = loc.Rect.GetY();
                    float w1 = loc.Rect.GetWidth();
                    float h1 = loc.Rect.GetHeight();

                    float x2 = existing.Rect.GetX();
                    float y2 = existing.Rect.GetY();
                    float w2 = existing.Rect.GetWidth();
                    float h2 = existing.Rect.GetHeight();

                    // Only treat as duplicates if the rectangles actually overlap —
                    // centre-point proximity with large tolerances was merging entities
                    // from adjacent lines (e.g. PESEL being merged with NIP below it).
                    float xOverlap = Math.Min(x1 + w1, x2 + w2) - Math.Max(x1, x2);
                    float yOverlap = Math.Min(y1 + h1, y2 + h2) - Math.Max(y1, y2);
                    if (xOverlap <= 0f || yOverlap <= 0f)
                        continue;

                    float overlapArea = xOverlap * yOverlap;
                    float smallerArea = Math.Min(w1 * h1, w2 * h2);
                    if (smallerArea <= 0f || overlapArea / smallerArea < 0.3f)
                        continue;

                    if (h1 > h2 || (h1 == h2 && w1 > w2))
                    {
                        deduplicatedLocations[i] = loc;
                    }
                    isDuplicate = true;
                    break;
                }

                if (!isDuplicate)
                {
                    deduplicatedLocations.Add(loc);
                }
            }

            return deduplicatedLocations;
        }

        private static HashSet<int> AddExactOcrWordMatches(CachedLine line, string searchText, List<TextLocation> locations)
        {
            if (line?.IsOcr != true ||
                line.OcrWords == null ||
                line.OcrWords.Count == 0 ||
                string.IsNullOrWhiteSpace(searchText) ||
                locations == null)
            {
                return null;
            }

            var matchedStarts = new HashSet<int>();
            string normalizedSearchText = searchText.Trim();
            foreach (OcrWordInfo word in line.OcrWords)
            {
                if (word == null ||
                    word.BoundingBox == null ||
                    word.BoundingBox.GetWidth() <= 0f ||
                    word.BoundingBox.GetHeight() <= 0f ||
                    string.IsNullOrWhiteSpace(word.Text))
                {
                    continue;
                }

                if (!string.Equals(word.Text.Trim(), normalizedSearchText, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                locations.Add(new TextLocation(line.PageNumber, line.PageRotation, word.BoundingBox, isOcr: true, isExactOcrWord: true));
                matchedStarts.Add(word.StartIndex);
            }

            return matchedStarts.Count > 0 ? matchedStarts : null;
        }

        private static void AddOcrManualSearchMatches(CachedLine line, string searchText, List<TextLocation> locations)
        {
            if (line == null || !line.IsOcr || string.IsNullOrWhiteSpace(line.Text) || string.IsNullOrWhiteSpace(searchText) || locations == null)
            {
                return;
            }

            string normalizedLine = BuildOcrManualSearchText(line.Text, removeAtSign: false, out List<int> lineIndexes);
            string normalizedSearch = BuildOcrManualSearchText(searchText, removeAtSign: false, out _);
            int added = AddOcrManualSearchMatchesFromNormalizedText(
                line,
                normalizedLine,
                lineIndexes,
                normalizedSearch,
                locations,
                expandMissingAtSign: false);

            if (added > 0 || normalizedSearch.IndexOf('@') < 0)
            {
                return;
            }

            string normalizedLineWithoutAt = BuildOcrManualSearchText(line.Text, removeAtSign: true, out List<int> lineIndexesWithoutAt);
            string normalizedSearchWithoutAt = BuildOcrManualSearchText(searchText, removeAtSign: true, out _);
            if (normalizedSearchWithoutAt.Length < 2)
            {
                return;
            }

            AddOcrManualSearchMatchesFromNormalizedText(
                line,
                normalizedLineWithoutAt,
                lineIndexesWithoutAt,
                normalizedSearchWithoutAt,
                locations,
                expandMissingAtSign: true);
        }

        private static int AddOcrManualSearchMatchesFromNormalizedText(
            CachedLine line,
            string normalizedLine,
            List<int> originalIndexes,
            string normalizedSearch,
            List<TextLocation> locations,
            bool expandMissingAtSign)
        {
            if (line == null ||
                string.IsNullOrEmpty(normalizedLine) ||
                string.IsNullOrEmpty(normalizedSearch) ||
                originalIndexes == null ||
                originalIndexes.Count != normalizedLine.Length ||
                locations == null)
            {
                return 0;
            }

            int added = 0;
            int startIndex = normalizedLine.IndexOf(normalizedSearch, StringComparison.OrdinalIgnoreCase);
            while (startIndex >= 0)
            {
                if (startIndex + normalizedSearch.Length <= originalIndexes.Count)
                {
                    int originalStart = originalIndexes[startIndex];
                    int originalEnd = originalIndexes[startIndex + normalizedSearch.Length - 1] + 1;
                    int originalLength = originalEnd - originalStart;
                    if (originalStart >= 0 && originalLength > 0)
                    {
                        KernelGeom.Rectangle textRect = GetSearchResultRectangle(line, originalStart, originalLength);
                        if (expandMissingAtSign)
                        {
                            textRect = ExpandOcrRectangleForMissingAtSign(textRect, line);
                        }

                        if (textRect != null)
                        {
                            locations.Add(new TextLocation(line.PageNumber, line.PageRotation, textRect, isOcr: true));
                            added++;
                        }
                    }
                }

                startIndex = normalizedLine.IndexOf(normalizedSearch, startIndex + 1, StringComparison.OrdinalIgnoreCase);
            }

            return added;
        }

        private static string BuildOcrManualSearchText(string text, bool removeAtSign, out List<int> originalIndexes)
        {
            originalIndexes = new List<int>();
            if (string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }

            var builder = new System.Text.StringBuilder(text.Length);
            for (int i = 0; i < text.Length; i++)
            {
                char ch = NormalizeOcrManualSearchChar(text[i]);
                if (char.IsWhiteSpace(ch) || (removeAtSign && ch == '@'))
                {
                    continue;
                }

                builder.Append(char.ToLowerInvariant(ch));
                originalIndexes.Add(i);
            }

            return builder.ToString();
        }

        private static char NormalizeOcrManualSearchChar(char ch)
        {
            switch (ch)
            {
                case '＠':
                case '©':
                case '®':
                case '§':
                    return '@';
                case '．':
                case '·':
                case '•':
                case '‚':
                    return '.';
                default:
                    return ch;
            }
        }

        private static KernelGeom.Rectangle ExpandOcrRectangleForMissingAtSign(KernelGeom.Rectangle rect, CachedLine line)
        {
            if (rect == null)
            {
                return null;
            }

            float extraRight = Math.Max(2f, rect.GetHeight() * 0.75f);
            float right = rect.GetX() + rect.GetWidth() + extraRight;
            if (line != null && line.PageWidth > 0f)
            {
                right = Math.Min(line.PageWidth, right);
            }

            return new KernelGeom.Rectangle(rect.GetX(), rect.GetY(), Math.Max(rect.GetWidth(), right - rect.GetX()), rect.GetHeight());
        }

        private static DialogResult ShowMessageBox(
            IWin32Window owner,
            string text,
            string caption,
            MessageBoxButtons buttons,
            MessageBoxIcon icon,
            MessageBoxDefaultButton defaultButton = MessageBoxDefaultButton.Button1)
        {
            if (owner is Control control && control.InvokeRequired)
            {
                return (DialogResult)control.Invoke(new Func<DialogResult>(() =>
                    MessageBox.Show(owner, text, caption, buttons, icon, defaultButton)));
            }

            return owner == null
                ? MessageBox.Show(text, caption, buttons, icon, defaultButton)
                : MessageBox.Show(owner, text, caption, buttons, icon, defaultButton);
        }


        // Function to clear cache
        public static void ClearCache(string pdfPath = null)
        {
            if (pdfPath == null)
            {
                _lineCache.Clear();
                _personalDataCache.Clear();
                _altTextCache.Clear();
                _allFiguresCache.Clear();
                _pendingAltTextEdits.Clear();
            }
            else
            {
                _lineCache.TryRemove(pdfPath, out _);
                _personalDataCache.TryRemove(pdfPath, out _);
                _altTextCache.TryRemove(pdfPath, out _);
                _allFiguresCache.TryRemove(pdfPath, out _);
                var toRemove = _pendingAltTextEdits.Keys.Where(k => k.pdfPath == pdfPath).ToList();
                foreach (var k in toRemove) _pendingAltTextEdits.TryRemove(k, out _);
            }
        }

        public static void CachePdfText(string pdfPath, string userPassword)
        {
            CachePdfText(pdfPath, userPassword, System.Threading.CancellationToken.None);
        }

        public static void CachePdfText(string pdfPath, string userPassword, System.Threading.CancellationToken cancellationToken)
        {
            if (!_lineCache.ContainsKey(pdfPath))
            {
                CacheLines(pdfPath, userPassword, cancellationToken);
            }
        }

        internal static List<CachedLine> GetCachedLines(string pdfPath)
        {
            if (_lineCache.TryGetValue(pdfPath, out var lines))
                return lines;
            return new List<CachedLine>();
        }

        public static List<TextLocation> GetPersonalDataCache(string pdfPath) =>
            _personalDataCache.TryGetValue(pdfPath, out var r) ? r : null;

        internal static void SetPersonalDataCache(string pdfPath, List<TextLocation> results)
        {
            _personalDataCache[pdfPath] = results;
            // Overwrite disk cache with full data (lines + alt + NER)
            if (_lineCache.TryGetValue(pdfPath, out var lines) &&
                _altTextCache.TryGetValue(pdfPath, out var altTexts) &&
                _allFiguresCache.TryGetValue(pdfPath, out var allFigs))
            {
                PdfDiskCache.Save(pdfPath, lines, altTexts, allFigs, results);
            }
        }

        internal static void SavePartialCache(string pdfPath)
        {
            if (_lineCache.TryGetValue(pdfPath, out var lines) &&
                _altTextCache.TryGetValue(pdfPath, out var altTexts) &&
                _allFiguresCache.TryGetValue(pdfPath, out var allFigs))
            {
                PdfDiskCache.Save(pdfPath, lines, altTexts, allFigs);
            }
        }

        internal static void ClearStatusOverlay(string pdfPath = null) =>
            ReportCacheStatus(string.Empty, pdfPath);

        public static bool IsLocalNerAvailable()
        {
            var options = LocalNerOptions.Load();
            return options != null && options.Enabled &&
                   TryResolveLocalNerCommand(options, out _, out _);
        }

        internal static List<AltTextEntry> ExtractAltTexts(iText.Kernel.Pdf.PdfDocument pdfDoc)
        {
            var result = new List<AltTextEntry>();
            try
            {
                var structRoot = pdfDoc.GetStructTreeRoot();
                if (structRoot == null) return result;
                WalkStructElem(structRoot, pdfDoc, result, 0);
                ResolveNullBboxesViaMcid(result, pdfDoc);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("ExtractAltTexts: " + ex.Message);
            }
            return result;
        }

        // Returns posKey → the ACTUAL PdfDictionary from the struct tree (no xref re-lookup).
        // Includes ALL Figure-role elements (with or without Alt text) so that pending NEW Alt text
        // edits can be applied even to elements that had no Alt text in the original PDF.
        internal static Dictionary<string, iText.Kernel.Pdf.PdfDictionary> ExtractAltDictsByPosition(
            iText.Kernel.Pdf.PdfDocument pdfDoc)
        {
            var map = new Dictionary<string, iText.Kernel.Pdf.PdfDictionary>(StringComparer.Ordinal);
            try
            {
                var structRoot = pdfDoc.GetStructTreeRoot();
                if (structRoot == null) return map;

                // Collect all Figure-role struct elements with their page, bbox, mcid, and dict ref
                var items = new List<(int pageNum, KernelGeom.Rectangle bbox, int mcid,
                    iText.Kernel.Pdf.PdfDictionary dict)>();
                CollectFigureDictEntries(structRoot, pdfDoc, items, 0);

                // Supplementary xref scan: find orphaned Figure elements not reachable from root
                {
                    var foundXrefs = new HashSet<int>(
                        items.Select(x => x.dict.GetIndirectReference()?.GetObjNumber() ?? 0).Where(n => n > 0));
                    int nObjs = pdfDoc.GetNumberOfPdfObjects();
                    for (int objNum = 1; objNum <= nObjs; objNum++)
                    {
                        try
                        {
                            if (foundXrefs.Contains(objNum)) continue;
                            var pdfObj = pdfDoc.GetPdfObject(objNum);
                            if (!(pdfObj is iText.Kernel.Pdf.PdfDictionary od)) continue;
                            var typeN = od.GetAsName(iText.Kernel.Pdf.PdfName.Type);
                            if (typeN == null || !typeN.GetValue().Equals("StructElem")) continue;
                            var roleN = od.GetAsName(iText.Kernel.Pdf.PdfName.S);
                            if (roleN == null) continue;
                            string role2 = roleN.GetValue();
                            bool isFig2 = _figureRoles.Contains(role2);
                            bool hasAlt2 = !isFig2 && od.GetAsString(iText.Kernel.Pdf.PdfName.Alt) != null;
                            if (!isFig2 && !hasAlt2) continue;
                            var pgDict2 = od.GetAsDictionary(iText.Kernel.Pdf.PdfName.Pg);
                            int pageNum2 = pgDict2 != null ? pdfDoc.GetPageNumber(pgDict2) : 0;
                            if (pageNum2 <= 0) continue;
                            var bbox2 = GetAltBBox(od);
                            int mcid2 = bbox2 == null ? GetStructMcid(od) : -1;
                            items.Add((pageNum2, bbox2, mcid2, od));
                        }
                        catch { }
                    }
                }

                // Resolve null bboxes via MCID content-stream lookup
                var pagesNeedingFill = items
                    .Where(x => x.bbox == null)
                    .Select(x => x.pageNum)
                    .Distinct()
                    .ToList();
                var mcidMaps = new Dictionary<int, Dictionary<int, KernelGeom.Rectangle>>();
                foreach (int pn in pagesNeedingFill)
                    mcidMaps[pn] = ExtractMcidImageBBoxes(pdfDoc.GetPage(pn));

                for (int i = 0; i < items.Count; i++)
                {
                    var item = items[i];
                    if (item.bbox == null && item.mcid >= 0 &&
                        mcidMaps.TryGetValue(item.pageNum, out var mmap) &&
                        mmap.TryGetValue(item.mcid, out var resolved))
                    {
                        items[i] = (item.pageNum, resolved, item.mcid, item.dict);
                    }
                }

                // Build posKey → dict map (first entry wins per posKey)
                foreach (var (pageNum, bbox, _, dict) in items)
                {
                    if (bbox == null) continue;
                    string posKey = $"{pageNum}:{(int)Math.Round(bbox.GetX())}:{(int)Math.Round(bbox.GetY())}";
                    if (!map.ContainsKey(posKey))
                        map[posKey] = dict;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("ExtractAltDictsByPosition: " + ex.Message);
            }
            return map;
        }

        private static void CollectFigureDictEntries(
            iText.Kernel.Pdf.Tagging.IStructureNode node,
            iText.Kernel.Pdf.PdfDocument pdfDoc,
            List<(int pageNum, KernelGeom.Rectangle bbox, int mcid, iText.Kernel.Pdf.PdfDictionary dict)> items,
            int depth)
        {
            if (depth > 50 || node == null) return;
            if (node is iText.Kernel.Pdf.Tagging.PdfStructElem elem)
            {
                var dict = elem.GetPdfObject();
                var roleObj = dict.GetAsName(iText.Kernel.Pdf.PdfName.S);
                string role = roleObj?.GetValue() ?? string.Empty;

                // Include ALL Figure-role elements regardless of whether they have Alt text
                bool isFigure = _figureRoles.Contains(role);
                // Also include any role that already has Alt text (catches non-Figure roles with Alt)
                bool hasAlt = !isFigure && dict.GetAsString(iText.Kernel.Pdf.PdfName.Alt) != null;

                if (isFigure || hasAlt)
                {
                    var pgDict = dict.GetAsDictionary(iText.Kernel.Pdf.PdfName.Pg);
                    int pageNum = pgDict != null ? pdfDoc.GetPageNumber(pgDict) : 0;
                    if (pageNum > 0)
                    {
                        var bbox = GetAltBBox(dict);
                        int mcid = bbox == null ? GetStructMcid(dict) : -1;
                        items.Add((pageNum, bbox, mcid, dict));
                    }
                }
            }
            var kids = node.GetKids();
            if (kids == null) return;
            foreach (var kid in kids)
                CollectFigureDictEntries(kid, pdfDoc, items, depth + 1);
        }

        private static void WalkStructElem(
            iText.Kernel.Pdf.Tagging.IStructureNode node,
            iText.Kernel.Pdf.PdfDocument pdfDoc,
            List<AltTextEntry> result,
            int depth)
        {
            if (depth > 50 || node == null) return;

            if (node is iText.Kernel.Pdf.Tagging.PdfStructElem elem)
            {
                var dict = elem.GetPdfObject();
                var altStr = dict.GetAsString(iText.Kernel.Pdf.PdfName.Alt);
                if (altStr != null && !string.IsNullOrWhiteSpace(altStr.ToUnicodeString()))
                {
                    var pgDict = dict.GetAsDictionary(iText.Kernel.Pdf.PdfName.Pg);
                    int pageNum = pgDict != null ? pdfDoc.GetPageNumber(pgDict) : 0;
                    if (pageNum > 0)
                    {
                        var pg = pdfDoc.GetPage(pageNum);
                        var pgSize = pg.GetPageSize();
                        var bbox = GetAltBBox(dict);
                        int mcid = bbox == null ? GetStructMcid(dict) : -1;
                        int xrefNum = dict.GetIndirectReference()?.GetObjNumber() ?? 0;
                        result.Add(new AltTextEntry
                        {
                            PageNumber = pageNum,
                            PageRotation = pg.GetRotation(),
                            PageWidth = pgSize.GetWidth(),
                            PageHeight = pgSize.GetHeight(),
                            BBox = bbox,
                            AltText = altStr.ToUnicodeString(),
                            StructXref = xrefNum,
                            Mcid = mcid
                        });
                    }
                }
            }

            var kids = node.GetKids();
            if (kids == null) return;
            foreach (var kid in kids)
                WalkStructElem(kid, pdfDoc, result, depth + 1);
        }

        // ── MCID-aware content-stream image extractor ──────────────────────────────
        // Builds a map of marked-content-ID → image bounding box for a single page.
        // Used as fallback when struct elements don't carry an A.BBox attribute.

        private sealed class McidContentState
        {
            public readonly Stack<int> McidStack = new Stack<int>();
            public int CurrentMcid => McidStack.Count > 0 ? McidStack.Peek() : -1;
        }

        private sealed class McidImageExtractionListener : IEventListener
        {
            private readonly McidContentState _state;
            public readonly Dictionary<int, KernelGeom.Rectangle> McidToBBox =
                new Dictionary<int, KernelGeom.Rectangle>();

            public McidImageExtractionListener(McidContentState state) => _state = state;

            public void EventOccurred(IEventData data, EventType type)
            {
                int mcid = _state.CurrentMcid;
                if (mcid < 0) return;

                if (type == EventType.RENDER_IMAGE && data is ImageRenderInfo imgInfo)
                {
                    if (McidToBBox.ContainsKey(mcid)) return;
                    try
                    {
                        var ctm = imgInfo.GetImageCtm();
                        var p0 = new iText.Kernel.Geom.Vector(0, 0, 1).Cross(ctm);
                        var p1 = new iText.Kernel.Geom.Vector(1, 0, 1).Cross(ctm);
                        var p2 = new iText.Kernel.Geom.Vector(0, 1, 1).Cross(ctm);
                        var p3 = new iText.Kernel.Geom.Vector(1, 1, 1).Cross(ctm);
                        float minX = Math.Min(Math.Min(p0.Get(iText.Kernel.Geom.Vector.I1), p1.Get(iText.Kernel.Geom.Vector.I1)), Math.Min(p2.Get(iText.Kernel.Geom.Vector.I1), p3.Get(iText.Kernel.Geom.Vector.I1)));
                        float maxX = Math.Max(Math.Max(p0.Get(iText.Kernel.Geom.Vector.I1), p1.Get(iText.Kernel.Geom.Vector.I1)), Math.Max(p2.Get(iText.Kernel.Geom.Vector.I1), p3.Get(iText.Kernel.Geom.Vector.I1)));
                        float minY = Math.Min(Math.Min(p0.Get(iText.Kernel.Geom.Vector.I2), p1.Get(iText.Kernel.Geom.Vector.I2)), Math.Min(p2.Get(iText.Kernel.Geom.Vector.I2), p3.Get(iText.Kernel.Geom.Vector.I2)));
                        float maxY = Math.Max(Math.Max(p0.Get(iText.Kernel.Geom.Vector.I2), p1.Get(iText.Kernel.Geom.Vector.I2)), Math.Max(p2.Get(iText.Kernel.Geom.Vector.I2), p3.Get(iText.Kernel.Geom.Vector.I2)));
                        float w = maxX - minX, h = maxY - minY;
                        if (w > 0f && h > 0f)
                            McidToBBox[mcid] = new KernelGeom.Rectangle(minX, minY, w, h);
                    }
                    catch { }
                }
                else if (type == EventType.RENDER_PATH && data is PathRenderInfo pathInfo)
                {
                    // Skip clip-path-only operations (re W* n) — operation == NO_OP means the path
                    // is used only to define a clip region and is NOT a rendered shape.
                    if (pathInfo.GetOperation() == PathRenderInfo.NO_OP) return;
                    // First rendered path for this MCID wins (fill); ignore subsequent stroke pass.
                    if (McidToBBox.ContainsKey(mcid)) return;
                    try
                    {
                        var bbox = GetPathBBox(pathInfo.GetPath());
                        if (bbox != null)
                            McidToBBox[mcid] = bbox;
                    }
                    catch { }
                }
            }

            public ICollection<EventType> GetSupportedEvents() =>
                new List<EventType> { EventType.RENDER_IMAGE, EventType.RENDER_PATH };
        }

        private static KernelGeom.Rectangle GetPathBBox(iText.Kernel.Geom.Path path)
        {
            float minX = float.MaxValue, minY = float.MaxValue;
            float maxX = float.MinValue, maxY = float.MinValue;
            bool hasPoints = false;

            foreach (var subpath in path.GetSubpaths())
            {
                var start = subpath.GetStartPoint();
                if (start != null)
                {
                    float sx = (float)start.GetX(), sy = (float)start.GetY();
                    if (sx < minX) minX = sx; if (sx > maxX) maxX = sx;
                    if (sy < minY) minY = sy; if (sy > maxY) maxY = sy;
                    hasPoints = true;
                }
                foreach (var seg in subpath.GetSegments())
                {
                    foreach (var pt in seg.GetBasePoints())
                    {
                        float px = (float)pt.GetX(), py = (float)pt.GetY();
                        if (px < minX) minX = px; if (px > maxX) maxX = px;
                        if (py < minY) minY = py; if (py > maxY) maxY = py;
                        hasPoints = true;
                    }
                }
            }

            if (!hasPoints || maxX <= minX || maxY <= minY) return null;
            return new KernelGeom.Rectangle(minX, minY, maxX - minX, maxY - minY);
        }

        private sealed class BdcContentOperator : IContentOperator
        {
            private readonly McidContentState _state;
            private readonly iText.Kernel.Pdf.PdfDictionary _pageProps;

            public BdcContentOperator(McidContentState state, iText.Kernel.Pdf.PdfDictionary pageProps)
            { _state = state; _pageProps = pageProps; }

            public void Invoke(PdfCanvasProcessor processor, iText.Kernel.Pdf.PdfLiteral op, IList<iText.Kernel.Pdf.PdfObject> operands)
            {
                int mcid = -1;
                if (operands.Count >= 2)
                {
                    iText.Kernel.Pdf.PdfDictionary propDict = null;
                    if (operands[1] is iText.Kernel.Pdf.PdfDictionary inlineDict)
                        propDict = inlineDict;
                    else if (operands[1] is iText.Kernel.Pdf.PdfName propName && _pageProps != null)
                        propDict = _pageProps.GetAsDictionary(propName);
                    if (propDict != null)
                        mcid = propDict.GetAsNumber(new iText.Kernel.Pdf.PdfName("MCID"))?.IntValue() ?? -1;
                }
                _state.McidStack.Push(mcid);
            }
        }

        private sealed class BmcContentOperator : IContentOperator
        {
            private readonly McidContentState _state;
            public BmcContentOperator(McidContentState state) => _state = state;
            public void Invoke(PdfCanvasProcessor p, iText.Kernel.Pdf.PdfLiteral op, IList<iText.Kernel.Pdf.PdfObject> operands)
                => _state.McidStack.Push(-1);
        }

        private sealed class EmcContentOperator : IContentOperator
        {
            private readonly McidContentState _state;
            public EmcContentOperator(McidContentState state) => _state = state;
            public void Invoke(PdfCanvasProcessor p, iText.Kernel.Pdf.PdfLiteral op, IList<iText.Kernel.Pdf.PdfObject> operands)
            { if (_state.McidStack.Count > 0) _state.McidStack.Pop(); }
        }

        // Returns MCID → image bbox map for a page, or empty dict on failure.
        internal static Dictionary<int, KernelGeom.Rectangle> ExtractMcidImageBBoxes(iText.Kernel.Pdf.PdfPage page)
        {
            try
            {
                var state = new McidContentState();
                var listener = new McidImageExtractionListener(state);
                var processor = new PdfCanvasProcessor(listener);
                var propsDict = page.GetResources()?.GetPdfObject()
                    ?.GetAsDictionary(iText.Kernel.Pdf.PdfName.Properties);
                processor.RegisterContentOperator("BDC", new BdcContentOperator(state, propsDict));
                processor.RegisterContentOperator("BMC", new BmcContentOperator(state));
                processor.RegisterContentOperator("EMC", new EmcContentOperator(state));
                processor.ProcessPageContent(page);
                return listener.McidToBBox;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("ExtractMcidImageBBoxes: " + ex.Message);
                return new Dictionary<int, KernelGeom.Rectangle>();
            }
        }

        // Helper: extract MCID from a struct element's K entry.
        private static int GetStructMcid(iText.Kernel.Pdf.PdfDictionary dict)
        {
            var kVal = dict.Get(iText.Kernel.Pdf.PdfName.K);
            if (kVal is iText.Kernel.Pdf.PdfNumber kNum) return kNum.IntValue();
            if (kVal is iText.Kernel.Pdf.PdfDictionary kDict)
                return kDict.GetAsNumber(new iText.Kernel.Pdf.PdfName("MCID"))?.IntValue() ?? -1;
            if (kVal is iText.Kernel.Pdf.PdfArray kArr && kArr.Size() > 0)
            {
                var first = kArr.Get(0);
                if (first is iText.Kernel.Pdf.PdfNumber pn) return pn.IntValue();
                if (first is iText.Kernel.Pdf.PdfDictionary pd)
                    return pd.GetAsNumber(new iText.Kernel.Pdf.PdfName("MCID"))?.IntValue() ?? -1;
            }
            return -1;
        }

        // Resolves null BBox entries in place using per-page MCID → content-stream bbox maps.
        // Entries that still have null BBox after resolution are removed.
        private static void ResolveNullBboxesViaMcid(List<AltTextEntry> entries, iText.Kernel.Pdf.PdfDocument pdfDoc)
        {
            var pagesNeedingFill = entries
                .Where(e => e.BBox == null)
                .Select(e => e.PageNumber)
                .Distinct()
                .ToList();

            foreach (int pn in pagesNeedingFill)
            {
                var mcidMap = ExtractMcidImageBBoxes(pdfDoc.GetPage(pn));
                DbgLog($"  ResolveNullBboxes: page {pn} mcidMap has {mcidMap.Count} entries: [{string.Join(", ", mcidMap.Select(kv => $"mcid={kv.Key}→({kv.Value.GetX():F0},{kv.Value.GetY():F0})"))}]");
                foreach (var entry in entries.Where(e => e.PageNumber == pn && e.BBox == null))
                {
                    if (entry.Mcid >= 0 && mcidMap.TryGetValue(entry.Mcid, out var bbox))
                    {
                        DbgLog($"  Resolved mcid={entry.Mcid} → bbox ({bbox.GetX():F0},{bbox.GetY():F0},{bbox.GetWidth():F0}x{bbox.GetHeight():F0})");
                        entry.BBox = bbox;
                    }
                    else
                    {
                        DbgLog($"  Could not resolve mcid={entry.Mcid} (not in mcidMap)");
                    }
                }
            }
            entries.RemoveAll(e => e.BBox == null);
        }

        // ── End MCID infrastructure ─────────────────────────────────────────────────

        private static readonly string _dbgLog = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "AnonPDF-debug.log");
        private static void DbgLog(string msg) { if (!PDFForm.IsDiagnosticModeEnabled) return; try { System.IO.File.AppendAllText(_dbgLog, $"{DateTime.Now:HH:mm:ss.fff} [AltFig] {msg}\r\n"); } catch { } }

        // Simple all-images listener: no min-size filter, captures every rendered image on the page.
        private sealed class AllImagesListener : IEventListener
        {
            public readonly List<KernelGeom.Rectangle> Bboxes = new List<KernelGeom.Rectangle>();

            public void EventOccurred(IEventData data, EventType type)
            {
                if (type != EventType.RENDER_IMAGE || !(data is ImageRenderInfo imgInfo)) return;
                try
                {
                    var ctm = imgInfo.GetImageCtm();
                    var p0 = new iText.Kernel.Geom.Vector(0, 0, 1).Cross(ctm);
                    var p1 = new iText.Kernel.Geom.Vector(1, 0, 1).Cross(ctm);
                    var p2 = new iText.Kernel.Geom.Vector(0, 1, 1).Cross(ctm);
                    var p3 = new iText.Kernel.Geom.Vector(1, 1, 1).Cross(ctm);
                    float minX = Math.Min(Math.Min(p0.Get(iText.Kernel.Geom.Vector.I1), p1.Get(iText.Kernel.Geom.Vector.I1)), Math.Min(p2.Get(iText.Kernel.Geom.Vector.I1), p3.Get(iText.Kernel.Geom.Vector.I1)));
                    float maxX = Math.Max(Math.Max(p0.Get(iText.Kernel.Geom.Vector.I1), p1.Get(iText.Kernel.Geom.Vector.I1)), Math.Max(p2.Get(iText.Kernel.Geom.Vector.I1), p3.Get(iText.Kernel.Geom.Vector.I1)));
                    float minY = Math.Min(Math.Min(p0.Get(iText.Kernel.Geom.Vector.I2), p1.Get(iText.Kernel.Geom.Vector.I2)), Math.Min(p2.Get(iText.Kernel.Geom.Vector.I2), p3.Get(iText.Kernel.Geom.Vector.I2)));
                    float maxY = Math.Max(Math.Max(p0.Get(iText.Kernel.Geom.Vector.I2), p1.Get(iText.Kernel.Geom.Vector.I2)), Math.Max(p2.Get(iText.Kernel.Geom.Vector.I2), p3.Get(iText.Kernel.Geom.Vector.I2)));
                    float w = maxX - minX, h = maxY - minY;
                    if (w > 2f && h > 2f) Bboxes.Add(new KernelGeom.Rectangle(minX, minY, w, h));
                }
                catch { }
            }

            public ICollection<EventType> GetSupportedEvents() =>
                new List<EventType> { EventType.RENDER_IMAGE };
        }

        // Returns all image bboxes from the page content stream (no size filter).
        private static List<KernelGeom.Rectangle> ExtractPageAllImageBBoxes(iText.Kernel.Pdf.PdfPage page)
        {
            try
            {
                var listener = new AllImagesListener();
                new PdfCanvasProcessor(listener).ProcessPageContent(page);
                return listener.Bboxes;
            }
            catch (Exception ex) { DbgLog("ExtractPageAllImageBBoxes: " + ex.Message); return new List<KernelGeom.Rectangle>(); }
        }

        // Extracts ALL Figure/Image struct elements AND all content-stream images.
        // Used for hit-testing in the context menu so users can set Alt on any image.
        internal static List<AltTextEntry> ExtractAllFigures(iText.Kernel.Pdf.PdfDocument pdfDoc)
        {
            var result = new List<AltTextEntry>();
            try
            {
                // Phase 1: walk struct tree (works for tagged PDFs with A.BBox or MCID)
                var structRoot = pdfDoc.GetStructTreeRoot();
                if (structRoot != null)
                {
                    WalkStructElemAllFigures(structRoot, pdfDoc, result, 0);
                    int nullBefore = result.Count(e => e.BBox == null);
                    DbgLog($"ExtractAllFigures: {result.Count} entries after struct walk ({nullBefore} null-bbox)");
                    foreach (var e in result) DbgLog($"  struct pg={e.PageNumber} mcid={e.Mcid} bbox={(e.BBox == null ? "NULL" : $"{e.BBox.GetX():F0},{e.BBox.GetY():F0}")} alt={(string.IsNullOrEmpty(e.AltText) ? "(none)" : e.AltText.Substring(0, Math.Min(20, e.AltText.Length)))}");
                    ResolveNullBboxesViaMcid(result, pdfDoc);
                    DbgLog($"ExtractAllFigures: {result.Count} entries after MCID resolution");
                }
                else { DbgLog("ExtractAllFigures: no struct root"); }

                // Phase 1b: supplementary linear xref scan for orphaned Figure elements
                // (struct elements that exist in the xref table but are unreachable via tree walk
                //  because their parent back-pointer points to the root but root.K doesn't list them)
                {
                    var foundXrefs = new HashSet<int>(result.Select(e => e.StructXref).Where(x => x > 0));
                    int numObjects = pdfDoc.GetNumberOfPdfObjects();
                    var orphaned = new List<AltTextEntry>();
                    for (int objNum = 1; objNum <= numObjects; objNum++)
                    {
                        try
                        {
                            if (foundXrefs.Contains(objNum)) continue;
                            var pdfObj = pdfDoc.GetPdfObject(objNum);
                            if (!(pdfObj is iText.Kernel.Pdf.PdfDictionary od)) continue;
                            var typeN = od.GetAsName(iText.Kernel.Pdf.PdfName.Type);
                            if (typeN == null || !typeN.GetValue().Equals("StructElem")) continue;
                            var roleN = od.GetAsName(iText.Kernel.Pdf.PdfName.S);
                            if (roleN == null || !_figureRoles.Contains(roleN.GetValue())) continue;
                            var pgDict2 = od.GetAsDictionary(iText.Kernel.Pdf.PdfName.Pg);
                            int pageNum2 = pgDict2 != null ? pdfDoc.GetPageNumber(pgDict2) : 0;
                            if (pageNum2 <= 0) continue;
                            var pg2 = pdfDoc.GetPage(pageNum2);
                            var pgSize2 = pg2.GetPageSize();
                            string alt2 = od.GetAsString(iText.Kernel.Pdf.PdfName.Alt)?.ToUnicodeString() ?? string.Empty;
                            var bbox2 = GetAltBBox(od);
                            int mcid2 = bbox2 == null ? GetStructMcid(od) : -1;
                            orphaned.Add(new AltTextEntry
                            {
                                PageNumber = pageNum2,
                                PageRotation = pg2.GetRotation(),
                                PageWidth = pgSize2.GetWidth(),
                                PageHeight = pgSize2.GetHeight(),
                                BBox = bbox2,
                                AltText = alt2,
                                StructXref = objNum,
                                Mcid = mcid2
                            });
                            DbgLog($"  orphaned Figure xref={objNum} pg={pageNum2} bbox={(bbox2==null?"NULL":$"{bbox2.GetX():F0},{bbox2.GetY():F0}")} alt={alt2.Substring(0,Math.Min(20,alt2.Length))}");
                        }
                        catch { }
                    }
                    if (orphaned.Count > 0)
                    {
                        ResolveNullBboxesViaMcid(orphaned, pdfDoc);
                        result.AddRange(orphaned);
                        DbgLog($"ExtractAllFigures: added {orphaned.Count} orphaned entries");
                    }
                }

                // Phase 2: content-stream fallback — capture every image not already covered
                int numPages = pdfDoc.GetNumberOfPages();
                for (int pn = 1; pn <= numPages; pn++)
                {
                    var pg = pdfDoc.GetPage(pn);
                    var pgSize = pg.GetPageSize();
                    var contentImages = ExtractPageAllImageBBoxes(pg);
                    DbgLog($"  page {pn}: {contentImages.Count} content-stream images");
                    foreach (var imgBBox in contentImages)
                    {
                        bool covered = result.Any(e =>
                            e.PageNumber == pn &&
                            Math.Abs(e.BBox.GetX() - imgBBox.GetX()) < 5f &&
                            Math.Abs(e.BBox.GetY() - imgBBox.GetY()) < 5f);
                        if (!covered)
                        {
                            DbgLog($"    untagged img at ({imgBBox.GetX():F0},{imgBBox.GetY():F0},{imgBBox.GetWidth():F0}x{imgBBox.GetHeight():F0})");
                            result.Add(new AltTextEntry
                            {
                                PageNumber = pn,
                                PageRotation = pg.GetRotation(),
                                PageWidth = pgSize.GetWidth(),
                                PageHeight = pgSize.GetHeight(),
                                BBox = imgBBox,
                                AltText = string.Empty,
                                StructXref = 0,
                                Mcid = -1
                            });
                        }
                    }
                }

                DbgLog($"ExtractAllFigures: FINAL {result.Count} entries");
            }
            catch (Exception ex) { DbgLog("ExtractAllFigures EXCEPTION: " + ex); }
            return result;
        }

        private static readonly HashSet<string> _figureRoles =
            new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "Figure", "Image", "Formula", "Chart" };

        private static void WalkStructElemAllFigures(
            iText.Kernel.Pdf.Tagging.IStructureNode node,
            iText.Kernel.Pdf.PdfDocument pdfDoc,
            List<AltTextEntry> result,
            int depth)
        {
            if (depth > 50 || node == null) return;
            if (node is iText.Kernel.Pdf.Tagging.PdfStructElem elem)
            {
                var dict = elem.GetPdfObject();
                var roleObj = dict.GetAsName(iText.Kernel.Pdf.PdfName.S);
                string role = roleObj?.GetValue() ?? string.Empty;
                if (_figureRoles.Contains(role))
                {
                    var pgDict = dict.GetAsDictionary(iText.Kernel.Pdf.PdfName.Pg);
                    int pageNum = pgDict != null ? pdfDoc.GetPageNumber(pgDict) : 0;
                    if (pageNum > 0)
                    {
                        var pg = pdfDoc.GetPage(pageNum);
                        var pgSize = pg.GetPageSize();
                        string altText = dict.GetAsString(iText.Kernel.Pdf.PdfName.Alt)?.ToUnicodeString() ?? string.Empty;
                        int xrefNum = dict.GetIndirectReference()?.GetObjNumber() ?? 0;
                        var bbox = GetAltBBox(dict); // may be null — resolved later via MCID
                        int mcid = bbox == null ? GetStructMcid(dict) : -1;
                        result.Add(new AltTextEntry
                        {
                            PageNumber = pageNum,
                            PageRotation = pg.GetRotation(),
                            PageWidth = pgSize.GetWidth(),
                            PageHeight = pgSize.GetHeight(),
                            BBox = bbox,
                            AltText = altText,
                            StructXref = xrefNum,
                            Mcid = mcid
                        });
                    }
                }
            }
            var kids = node.GetKids();
            if (kids == null) return;
            foreach (var kid in kids)
                WalkStructElemAllFigures(kid, pdfDoc, result, depth + 1);
        }

        private static KernelGeom.Rectangle GetAltBBox(iText.Kernel.Pdf.PdfDictionary dict)
        {
            var aDict = dict.GetAsDictionary(iText.Kernel.Pdf.PdfName.A);
            if (aDict != null)
                return ParseBBoxFromAttrDict(aDict);

            var aArr = dict.GetAsArray(iText.Kernel.Pdf.PdfName.A);
            if (aArr != null)
            {
                for (int i = 0; i < aArr.Size(); i++)
                {
                    var r = ParseBBoxFromAttrDict(aArr.GetAsDictionary(i));
                    if (r != null) return r;
                }
            }
            return null;
        }

        private static KernelGeom.Rectangle ParseBBoxFromAttrDict(iText.Kernel.Pdf.PdfDictionary attrDict)
        {
            if (attrDict == null) return null;
            var bboxArr = attrDict.GetAsArray(iText.Kernel.Pdf.PdfName.BBox);
            if (bboxArr == null || bboxArr.Size() < 4) return null;
            float x0 = bboxArr.GetAsNumber(0)?.FloatValue() ?? 0;
            float y0 = bboxArr.GetAsNumber(1)?.FloatValue() ?? 0;
            float x1 = bboxArr.GetAsNumber(2)?.FloatValue() ?? 0;
            float y1 = bboxArr.GetAsNumber(3)?.FloatValue() ?? 0;
            if (x1 <= x0 || y1 <= y0) return null;
            return new KernelGeom.Rectangle(x0, y0, x1 - x0, y1 - y0);
        }

        public static List<TextLocation> GetAltTextLocations(string pdfPath)
        {
            _altTextCache.TryGetValue(pdfPath, out var altEntries);
            altEntries = altEntries ?? new List<AltTextEntry>();

            var result = altEntries.Select(e =>
            {
                string text = e.AltText;
                if (_pendingAltTextEdits.TryGetValue((pdfPath, e.PositionKey), out var pending))
                    text = pending;
                return new TextLocation(e.PageNumber, e.PageRotation, e.BBox, false)
                {
                    Text = text,
                    Source = LocationSource.AltText
                };
            }).ToList();

            // Include figures that had no Alt in the PDF but have been assigned Alt via pending edits
            if (_allFiguresCache.TryGetValue(pdfPath, out var allFigs) && allFigs != null)
            {
                var knownKeys = new HashSet<string>(altEntries.Select(e => e.PositionKey), StringComparer.Ordinal);
                foreach (var fig in allFigs)
                {
                    if (knownKeys.Contains(fig.PositionKey)) continue;
                    if (!_pendingAltTextEdits.TryGetValue((pdfPath, fig.PositionKey), out var pendingText)) continue;
                    if (string.IsNullOrWhiteSpace(pendingText)) continue;
                    result.Add(new TextLocation(fig.PageNumber, fig.PageRotation, fig.BBox, false)
                    {
                        Text = pendingText,
                        Source = LocationSource.AltText
                    });
                }
            }

            return result;
        }

        internal static List<AltTextEntry> GetAltEntries(string pdfPath) =>
            _altTextCache.TryGetValue(pdfPath, out var list) ? list : null;

        /// <summary>
        /// Returns only those ALT-text locations where PII was detected (regex or NER).
        /// Each returned location has <c>Label</c> set to the entity type.
        /// </summary>
        private static int AltLabelPriority(string label)
        {
            switch (label)
            {
                case "PERSON": case "PER": case "persName":
                case "PESEL": case "NIP": case "REGON": case "KRS": case "KW":
                case "IDENTITY_CARD": case "BANK_ACCOUNT": case "LOAN_NUMBER":
                case "PHONE": case "EMAIL": case "ADE": case "VIN":
                    return 0; // personal data — highest priority
                case "LOCATION": case "LOC": case "GPE": case "placeName": case "geogName":
                case "POSTAL_CODE":
                    return 1; // location
                default:
                    return 2; // other (DATE, AMOUNT, URL, ORG, etc.)
            }
        }

        private static string PickBestAltTextLabel(IEnumerable<string> labels)
            => labels.Where(l => !string.IsNullOrEmpty(l)).OrderBy(AltLabelPriority).FirstOrDefault();

        internal static List<TextLocation> GetAltTextLocationsForPersonalData(string pdfPath, System.Threading.CancellationToken cancellationToken = default)
        {
            var allAltLocs = GetAltTextLocations(pdfPath);
            if (allAltLocs.Count == 0) return allAltLocs;

            // Pass 1 — fast regex patterns (no daemon needed)
            foreach (var loc in allAltLocs)
            {
                if (string.IsNullOrWhiteSpace(loc.Text)) continue;
                var tags = DetectIdentifierTags(loc.Text);
                if (tags.Count > 0)
                {
                    loc.Label = PickBestAltTextLabel(tags);
                    loc.HasMultipleLabels = tags.Count > 1;
                }
            }

            // Pass 2 — local ML NER for entries with no label or only a low-priority regex label
            // (e.g. POSTAL_CODE alone — NER may find a higher-priority PERSON in the same text)
            var unclassified = allAltLocs
                .Select((loc, i) => new { loc, i })
                .Where(x => !string.IsNullOrWhiteSpace(x.loc.Text) &&
                            (x.loc.Label == null || AltLabelPriority(x.loc.Label) > 0))
                .ToList();

            if (unclassified.Count > 0)
            {
                LocalNerOptions options = LocalNerOptions.Load();
                if (options.Enabled)
                {
                    // Reuse language/plugin already detected for this document's text lines
                    if (_lineCache.TryGetValue(pdfPath, out var docLines) && docLines != null)
                    {
                        string lang = DetectLanguageFromLines(docLines);
                        if (lang != null)
                        {
                            PluginManifest manifest = DiscoverPluginForLanguage(lang, options.ExecutablePath);
                            if (manifest != null)
                                options = options.WithPlugin(manifest);
                        }
                    }

                    // Build fake CachedLine list (text-only; no character bounding-box data needed
                    // because we own the alt-text BBox and don't need sub-span matching).
                    var fakeLines = unclassified
                        .Select(x => new CachedLine { Text = x.loc.Text })
                        .ToList();

                    try
                    {
                        string responseJson = RunLocalNerDaemon(fakeLines, options, cancellationToken, pdfPath);
                        if (!string.IsNullOrWhiteSpace(responseJson))
                        {
                            JObject obj = JObject.Parse(responseJson);
                            JArray respLines = obj["resplines"] as JArray;
                            if (respLines != null)
                            {
                                foreach (JObject respLine in respLines.OfType<JObject>())
                                {
                                    int fakeIdx = respLine["linenumber"]?.Value<int>() ?? -1;
                                    if (fakeIdx < 0 || fakeIdx >= unclassified.Count) continue;

                                    JArray entities = respLine["entities"] as JArray;
                                    if (entities == null) continue;

                                    // Collect all accepted NER labels for this entry
                                    var nerLabels = new List<string>();
                                    foreach (JObject entity in entities.OfType<JObject>())
                                    {
                                        string label = entity["label"]?.ToString() ?? string.Empty;
                                        if (options.AcceptsLabel(label))
                                            nerLabels.Add(label);
                                    }
                                    if (nerLabels.Count == 0) continue;

                                    // Combine with existing regex label (if any) and pick best
                                    var target = allAltLocs[unclassified[fakeIdx].i];
                                    var combined = new List<string>(nerLabels);
                                    if (target.Label != null) combined.Add(target.Label);
                                    string best = PickBestAltTextLabel(combined);
                                    if (best != null)
                                    {
                                        target.Label = best;
                                        target.HasMultipleLabels = combined.Distinct().Count() > 1 || target.HasMultipleLabels;
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("NER for ALT texts failed: " + ex.Message);
                    }
                }
            }

            ReportCacheStatus(string.Empty, pdfPath);
            return allAltLocs;
        }


        private class LineExtractionStrategy : LocationTextExtractionStrategy
        {
            private readonly int _pageNum;
            private readonly int _pageRotation;
            public List<CachedLine> ExtractedLines { get; } = new List<CachedLine>();
            private const float Y_TOLERANCE = 2.0f;

            public LineExtractionStrategy(int pageNum, int pageRotation)
            {
                _pageNum = pageNum;
                _pageRotation = pageRotation;
            }

            public override void EventOccurred(IEventData data, EventType type)
            {
                if (type == EventType.RENDER_TEXT && data is TextRenderInfo renderInfo)
                {
                    var baseline = renderInfo.GetBaseline();
                    float yPos = baseline.GetStartPoint().Get(KernelGeom.Vector.I2);

                    CachedLine line = ExtractedLines.Find(l => Math.Abs(l.YPosition - yPos) < Y_TOLERANCE);
                    if (line == null)
                    {
                        line = new CachedLine { PageNumber = _pageNum, PageRotation = _pageRotation, YPosition = yPos };
                        ExtractedLines.Add(line);
                    }

                    string text = renderInfo.GetText();

                    // Insert a space when there is a significant horizontal gap between this render
                    // chunk and the previous one (e.g. adjacent table cells in separate content
                    // streams — iText does not synthesise spaces across stream boundaries).
                    // We measure the gap via LastChunkEndX (chunk baseline endpoint) rather than
                    // per-character bounding boxes because the latter can have zero width for some
                    // fonts.  Threshold 3 pt catches cell padding (≥ 5–8 pt) while avoiding
                    // false positives from normal kerning (< 1 pt) or word tracking (< 3 pt).
                    // We also add a synthetic CharacterInfo for the space so that NER start/end
                    // indices (which are based on line.Text) stay in sync with line.Characters.
                    if (line.Text.Length > 0 && text.Length > 0
                        && line.LastChunkEndX > float.MinValue
                        && !char.IsWhiteSpace(line.Text[line.Text.Length - 1])
                        && !char.IsWhiteSpace(text[0]))
                    {
                        float chunkStartX = baseline.GetStartPoint().Get(KernelGeom.Vector.I1);
                        float gap = chunkStartX - line.LastChunkEndX;
                        if (gap > 3.0f)
                        {
                            // Large gaps (> 15 pt) indicate table-column boundaries; use '|'
                            // so NER regex patterns cannot span across field boundaries.
                            // Small gaps (3–15 pt) are treated as normal word spaces.
                            char sepChar = gap > 15.0f ? '|' : ' ';
                            line.Text += sepChar;
                            float spaceY, spaceH;
                            if (line.Characters.Count > 0)
                            {
                                var prev = line.Characters[line.Characters.Count - 1];
                                spaceY = prev.BoundingBox.GetY();
                                spaceH = prev.BoundingBox.GetHeight();
                            }
                            else
                            {
                                float chkAscY    = renderInfo.GetAscentLine().GetEndPoint().Get(KernelGeom.Vector.I2);
                                float chkDesY    = renderInfo.GetDescentLine().GetStartPoint().Get(KernelGeom.Vector.I2);
                                spaceY = chkDesY;
                                spaceH = Math.Max(0.1f, chkAscY - chkDesY);
                            }
                            line.Characters.Add(new CharacterInfo
                            {
                                Char = sepChar,
                                BoundingBox = new KernelGeom.Rectangle(line.LastChunkEndX, spaceY, gap, spaceH)
                            });
                        }
                    }

                    line.Text += text;

                    int prevCharCount = line.Characters.Count;

                    var charInfos = renderInfo.GetCharacterRenderInfos();
                    if (charInfos != null)
                    {
                        foreach (var charInfo in charInfos)
                        {
                            line.Characters.Add(new CharacterInfo
                            {
                                Char = charInfo.GetText()[0],
                                BoundingBox = GetTextRenderInfoBounds(charInfo)
                            });
                        }
                    }

                    // If this chunk added fewer CharacterInfo entries than it contributed text
                    // characters (charInfos null, empty, or shorter than text), pad with synthetic
                    // entries so that line.Characters.Count always equals line.Text.Length and
                    // NER start/end indices map correctly to visual positions.
                    int deficit = text.Length - (line.Characters.Count - prevCharCount);
                    if (deficit > 0)
                    {
                        // Use the same Y/height formula as the real charInfos loop so that
                        // synthetic boxes don't inflate the highlight rectangle.
                        float cStartX  = baseline.GetStartPoint().Get(KernelGeom.Vector.I1);
                        float cEndX    = baseline.GetEndPoint().Get(KernelGeom.Vector.I1);
                        float charW    = Math.Max(0.1f, (cEndX - cStartX) / text.Length);
                        float cAscY    = renderInfo.GetAscentLine().GetEndPoint().Get(KernelGeom.Vector.I2);
                        float cDesY    = renderInfo.GetDescentLine().GetStartPoint().Get(KernelGeom.Vector.I2);
                        float cH       = Math.Max(0.1f, cAscY - cDesY);  // descent → ascent
                        // ci starts from the number of chars already covered by real charInfos
                        int firstMissing = text.Length - deficit;
                        for (int ci = firstMissing; ci < text.Length; ci++)
                        {
                            line.Characters.Add(new CharacterInfo
                            {
                                Char = text[ci],
                                BoundingBox = new KernelGeom.Rectangle(cStartX + ci * charW, cDesY, charW, cH)
                            });
                        }
                    }

                    line.LastChunkEndX = baseline.GetEndPoint().Get(KernelGeom.Vector.I1);
                }

                base.EventOccurred(data, type);
            }

            private static KernelGeom.Rectangle GetTextRenderInfoBounds(TextRenderInfo textInfo)
            {
                KernelGeom.LineSegment ascentLine = textInfo.GetAscentLine();
                KernelGeom.LineSegment descentLine = textInfo.GetDescentLine();

                KernelGeom.Vector[] points =
                {
                    ascentLine.GetStartPoint(),
                    ascentLine.GetEndPoint(),
                    descentLine.GetStartPoint(),
                    descentLine.GetEndPoint()
                };

                float minX = points.Min(point => point.Get(KernelGeom.Vector.I1));
                float maxX = points.Max(point => point.Get(KernelGeom.Vector.I1));
                float minY = points.Min(point => point.Get(KernelGeom.Vector.I2));
                float maxY = points.Max(point => point.Get(KernelGeom.Vector.I2));

                return new KernelGeom.Rectangle(
                    minX,
                    minY,
                    Math.Max(0.1f, maxX - minX),
                    Math.Max(0.1f, maxY - minY));
            }
        }

        internal static string GetNerCacheIdentity()
        {
            LocalNerOptions options = LocalNerOptions.Load();
            if (options == null || !options.Enabled)
            {
                return "disabled";
            }

            string executablePath = ResolveLocalNerPath(options.ExecutablePath);
            string pluginVersion = string.Empty;
            try
            {
                string configPath = Path.Combine(Path.GetDirectoryName(executablePath) ?? string.Empty, "config.json");
                if (File.Exists(configPath))
                {
                    pluginVersion = ReadPluginVersion(configPath);
                }
            }
            catch
            {
            }

            string labels = options.Labels == null
                ? string.Empty
                : string.Join(",", options.Labels.OrderBy(label => label, StringComparer.OrdinalIgnoreCase));
            return string.Join("|", executablePath ?? string.Empty, pluginVersion, options.ModelName ?? string.Empty, labels);
        }

        private static void SearchPersonalData_PendingRemoval(CachedLine line, List<TextLocation> locations)
        {
            string text = line.Text;
            int len = text.Length;
            bool[] matched = len > 0 ? new bool[len] : null;

            // Debug: dump every line that contains digits to help diagnose regex misses
            if (text.Any(char.IsDigit) && (text.Contains("1020") || text.Contains("4795") || text.Contains("8496") || text.Contains("3420")))
            {
                Debug.WriteLine($"SEARCH_LINE page={line.PageNumber} text='{text}' hex={BitConverter.ToString(System.Text.Encoding.UTF8.GetBytes(text)).Replace("-", " ")} len={text.Length} ocr={line.IsOcr}");
            }

            foreach (Match match in PropertyRegisterPattern.Matches(text))
            {
                if (!Overlaps(matched, match) && ValidatePropertyRegister(match.Value) && IsIdentifierMatchGeometryCompact(line, match.Index, match.Length))
                {
                    MarkMatched(matched, match);
                    int c = locations.Count;
                    AddLocationForMatch(line, match, locations);
                    for (int li = c; li < locations.Count; li++) { locations[li].Label = "KW"; locations[li].Text = match.Value; }
                }
            }

            AddNamedEntityMatches(line, matched, locations, AddressPattern, "value", IsLikelyAddressEntity, "LOCATION");

            // Names and broad locations are handled by the local ML NER provider.
            // Keep this pass limited to deterministic identifiers and structured addresses.
        }

        private static bool Overlaps(bool[] matched, Match m)
        {
            if (matched == null) return false;
            for (int i = m.Index; i < m.Index + m.Length && i < matched.Length; i++)
                if (matched[i]) return true;
            return false;
        }

        private static bool Overlaps(bool[] matched, int startIndex, int length)
        {
            if (matched == null) return false;
            if (startIndex < 0 || length <= 0) return true;
            int end = Math.Min(matched.Length, startIndex + length);
            for (int i = startIndex; i < end; i++)
                if (matched[i]) return true;
            return false;
        }

        private static void MarkMatched(bool[] matched, Match m)
        {
            if (matched == null) return;
            for (int i = m.Index; i < m.Index + m.Length && i < matched.Length; i++)
                matched[i] = true;
        }

        private static void MarkMatched(bool[] matched, int startIndex, int length)
        {
            if (matched == null || startIndex < 0 || length <= 0) return;
            for (int i = startIndex; i < startIndex + length && i < matched.Length; i++)
                matched[i] = true;
        }

        private static void SearchNamedEntities_PendingRemoval(CachedLine line, bool[] matched, List<TextLocation> locations)
        {
            if (line == null || string.IsNullOrWhiteSpace(line.Text) || locations == null)
            {
                return;
            }

            AddNamedEntityMatches(line, matched, locations, AddressPattern, "value", IsLikelyAddressEntity, "LOCATION");
            AddNamedEntityMatches(line, matched, locations, PostalCityPattern, "value", IsLikelyAddressEntity, "POSTAL_CODE");
            AddNamedEntityMatches(line, matched, locations, OrganizationPattern, "value", IsLikelyOrganizationEntity, "ORG");
            AddNamedEntityMatches(line, matched, locations, LabeledPersonPattern, "value", IsLikelyPersonEntity, "PERSON");
            AddNamedEntityMatches(line, matched, locations, PersonNamePattern, "value", IsLikelyPersonEntity, "PERSON");
        }

        private static void AddNamedEntityMatches(
            CachedLine line,
            bool[] matched,
            List<TextLocation> locations,
            Regex pattern,
            string groupName,
            Func<string, bool> validate,
            string label = null)
        {
            foreach (Match match in pattern.Matches(line.Text))
            {
                Group valueGroup = match.Groups[groupName];
                int startIndex = valueGroup.Success ? valueGroup.Index : match.Index;
                int length = valueGroup.Success ? valueGroup.Length : match.Length;
                string value = valueGroup.Success ? valueGroup.Value : match.Value;

                if (length <= 0 ||
                    Overlaps(matched, startIndex, length) ||
                    (validate != null && !validate(value)) ||
                    !IsNamedEntityGeometryUsable(line, startIndex, length))
                {
                    continue;
                }

                MarkMatched(matched, startIndex, length);
                int c = locations.Count;
                AddLocationForSpan(line, startIndex, length, locations);
                if (label != null)
                    for (int li = c; li < locations.Count; li++)
                    {
                        locations[li].Label = label;
                        locations[li].Text = value;
                    }
            }
        }

        private static bool IsNamedEntityGeometryUsable(CachedLine line, int startIndex, int length)
        {
            if (line?.Characters == null || length <= 0 || startIndex < 0)
            {
                return false;
            }

            int endExclusive = startIndex + length;
            return endExclusive <= line.Text.Length && endExclusive <= line.Characters.Count;
        }

        private static bool IsLikelyPhoneEntity(string value)
        {
            string digits = new string((value ?? string.Empty).Where(char.IsDigit).ToArray());
            return digits.Length == 9 || (digits.Length == 11 && digits.StartsWith("48", StringComparison.Ordinal));
        }

        private static bool IsLikelyAddressEntity(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return false;
            }

            string normalized = value.Trim();
            return normalized.Length >= 6 &&
                (normalized.Any(char.IsDigit) || PostalCodePattern.IsMatch(normalized));
        }

        private static bool IsLikelyOrganizationEntity(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return false;
            }

            string normalized = NormalizeTextForNer(value);
            return normalized.Length >= 4 &&
                (normalized.Contains("spolka") ||
                 normalized.Contains("sp z o o") ||
                 normalized.Contains("s a") ||
                 normalized.Contains("fundacja") ||
                 normalized.Contains("stowarzyszenie") ||
                 normalized.Contains("urzad") ||
                 normalized.Contains("gmina") ||
                 normalized.Contains("powiat") ||
                 normalized.Contains("ministerstwo") ||
                 normalized.Contains("bank") ||
                 normalized.Contains("kancelaria"));
        }

        private static bool IsLikelyPersonEntity(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return false;
            }

            string[] parts = Regex.Split(value.Trim(), @"\s+")
                .Where(part => !string.IsNullOrWhiteSpace(part))
                .ToArray();

            if (parts.Length > 4)
            {
                return false;
            }

            // Single hyphenated compound surname: "Kubicka-Formela", "Hoffmann-Müller"
            if (parts.Length == 1)
            {
                string[] segs = parts[0].Split('-');
                return segs.Length >= 2
                    && segs.All(s => s.Length >= 2 && char.IsUpper(s[0]) && s.Skip(1).Any(char.IsLower));
            }

            foreach (string part in parts)
            {
                string clean = part.Trim('-', '\'', '.', ',', ';', ':');

                // Single uppercase letter OR known professional/academic title
                // abbreviation (Dr, Prof, Mgr, Herr, Frau, Pan, Pani, Mr, Ms …)
                bool isInitial = (clean.Length == 1 && char.IsUpper(clean[0]))
                    || PersonTitleAbbreviations.Contains(clean);
                if (isInitial)
                {
                    continue;
                }

                if (clean.Length < 3 || !char.IsUpper(clean[0]) || !clean.Skip(1).Any(char.IsLower))
                {
                    return false;
                }

                string normalizedPart = NormalizeTextForNer(clean);
                if (NerPersonStopWords.Contains(normalizedPart))
                {
                    return false;
                }
            }

            string normalized = NormalizeTextForNer(value);
            string[] normalizedParts = normalized.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (normalizedParts.Any(part => NerInstructionStopWords.Contains(part)))
            {
                return false;
            }

            if (normalizedParts.Length > 0 && NerUiLeadWords.Contains(normalizedParts[0]))
            {
                return false;
            }

            if (normalizedParts.Any(part => NerEntityStopWords.Contains(part)))
            {
                return false;
            }

            return !NerOrganizationStopWords.Any(stopWord => normalized.Contains(stopWord));
        }

        private static string NormalizeTextForNer(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return string.Empty;
            }

            string decomposed = value.Trim().ToLowerInvariant().Normalize(System.Text.NormalizationForm.FormD);
            var builder = new System.Text.StringBuilder(decomposed.Length);
            foreach (char ch in decomposed)
            {
                UnicodeCategory category = CharUnicodeInfo.GetUnicodeCategory(ch);
                if (category != UnicodeCategory.NonSpacingMark)
                {
                    builder.Append(char.IsLetterOrDigit(ch) ? ch : ' ');
                }
            }

            return Regex.Replace(builder.ToString().Normalize(System.Text.NormalizationForm.FormC), @"\s+", " ").Trim();
        }

        private static bool IsIdentifierMatchGeometryCompact(CachedLine line, int startIndex, int length)
        {
            if (line?.Characters == null || length <= 1 || startIndex < 0)
            {
                return true;
            }

            int endExclusive = startIndex + length;
            if (endExclusive > line.Characters.Count)
            {
                // For identifier detection prefer precision over recall:
                // when mapping text index -> glyphs is uncertain, skip the match.
                return false;
            }

            var widths = new List<float>(length);
            for (int i = startIndex; i < endExclusive; i++)
            {
                var rect = line.Characters[i].BoundingBox;
                float width = Math.Max(0f, rect.GetWidth());
                if (width > 0.1f)
                {
                    widths.Add(width);
                }
            }

            float medianWidth = widths.Count > 0
                ? widths.OrderBy(w => w).ElementAt(widths.Count / 2)
                : 0f;
            float maxAllowedGap = Math.Max(2f, medianWidth * 1.6f);

            for (int i = startIndex; i < endExclusive - 1; i++)
            {
                var current = line.Characters[i].BoundingBox;
                var next = line.Characters[i + 1].BoundingBox;
                float currentRight = current.GetX() + current.GetWidth();
                float gap = next.GetX() - currentRight;
                if (gap > maxAllowedGap)
                {
                    return false;
                }
            }

            return true;
        }

        private static void AddLocationForMatch(CachedLine line, Match match, List<TextLocation> locations)
        {
            AddLocationForSpan(line, match.Index, match.Length, locations);
        }

        private static void AddLocationForSpan(CachedLine line, int startIndex, int length, List<TextLocation> locations)
        {
            List<KernelGeom.Rectangle> textRects = GetSearchResultRectangles(line, startIndex, length);
            KernelGeom.Rectangle textRect = UnionTextRectangles(textRects);
            if (textRect != null)
            {
                locations.Add(new TextLocation(line.PageNumber, line.PageRotation, textRect, line.IsOcr)
                {
                    HighlightRects = textRects != null && textRects.Count > 1 ? textRects : null,
                    Source = IsOutOfPageBounds(textRect, line.PageWidth, line.PageHeight)
                        ? LocationSource.OutOfBounds
                        : LocationSource.Normal
                });
            }
        }

        private static KernelGeom.Rectangle GetSearchResultRectangle(CachedLine line, int startIndex, int length)
        {
            return UnionTextRectangles(GetSearchResultRectangles(line, startIndex, length));
        }

        private static List<KernelGeom.Rectangle> GetSearchResultRectangles(CachedLine line, int startIndex, int length)
        {
            List<KernelGeom.Rectangle> textRects = GetTextFragmentRectangles(line, startIndex, length);
            KernelGeom.Rectangle textRect = UnionTextRectangles(textRects);
            if (line?.IsOcr == true && TryGetExactOcrWordRectangle(line, startIndex, length, out KernelGeom.Rectangle exactWordRect))
            {
                if (ShouldPreferExactOcrWordRectangle(exactWordRect, textRect))
                {
                    return new List<KernelGeom.Rectangle> { exactWordRect };
                }

                return textRects;
            }

            if (textRect == null || line?.IsOcr != true)
            {
                return textRects;
            }

            return new List<KernelGeom.Rectangle> { ExpandOcrTextFragmentRectangle(line, startIndex, length, textRect) };
        }

        private static bool ShouldPreferExactOcrWordRectangle(KernelGeom.Rectangle exactWordRect, KernelGeom.Rectangle textRect)
        {
            if (exactWordRect == null || exactWordRect.GetWidth() <= 0f || exactWordRect.GetHeight() <= 0f)
            {
                return false;
            }

            if (textRect == null || textRect.GetWidth() <= 0f || textRect.GetHeight() <= 0f)
            {
                return true;
            }

            float exactArea = exactWordRect.GetWidth() * exactWordRect.GetHeight();
            float textArea = textRect.GetWidth() * textRect.GetHeight();
            if (textArea <= 0f)
            {
                return true;
            }

            bool exactMuchWider = exactWordRect.GetWidth() > textRect.GetWidth() * 1.6f;
            bool exactMuchTaller = exactWordRect.GetHeight() > textRect.GetHeight() * 1.6f;
            if (exactArea > textArea * 2.25f && (exactMuchWider || exactMuchTaller))
            {
                return false;
            }

            return true;
        }

        private static bool TryGetExactOcrWordRectangle(
            CachedLine line,
            int startIndex,
            int length,
            out KernelGeom.Rectangle rectangle)
        {
            rectangle = null;
            if (line?.OcrWords == null || line.OcrWords.Count == 0 || startIndex < 0 || length <= 0)
            {
                return false;
            }

            foreach (OcrWordInfo word in line.OcrWords)
            {
                if (word == null ||
                    word.BoundingBox == null ||
                    word.BoundingBox.GetWidth() <= 0f ||
                    word.BoundingBox.GetHeight() <= 0f)
                {
                    continue;
                }

                if (word.StartIndex == startIndex && word.Length == length)
                {
                    rectangle = word.BoundingBox;
                    return true;
                }
            }

            return false;
        }

        private static KernelGeom.Rectangle ExpandOcrTextFragmentRectangle(
            CachedLine line,
            int startIndex,
            int length,
            KernelGeom.Rectangle fallbackRect)
        {
            if (line == null || fallbackRect == null)
            {
                return fallbackRect;
            }

            float paddingBasis = Math.Min(fallbackRect.GetWidth(), fallbackRect.GetHeight());
            float padX = Math.Max(OcrSearchMinimumHorizontalPadding, paddingBasis * OcrSearchHorizontalPaddingRatio);
            float padRight = Math.Max(OcrSearchMinimumRightPadding, paddingBasis * OcrSearchRightPaddingRatio);
            float padY = Math.Max(OcrSearchMinimumVerticalPadding, paddingBasis * OcrSearchVerticalPaddingRatio);
            float left = fallbackRect.GetX() - padX;
            float bottom = fallbackRect.GetY() - padY;
            float right = fallbackRect.GetX() + fallbackRect.GetWidth() + padRight;
            float top = fallbackRect.GetY() + fallbackRect.GetHeight() + padY;

            if (line.PageWidth > 0f)
            {
                left = Math.Max(0f, left);
                right = Math.Min(line.PageWidth, right);
            }

            if (line.PageHeight > 0f)
            {
                bottom = Math.Max(0f, bottom);
                top = Math.Min(line.PageHeight, top);
            }

            float width = right - left;
            float height = top - bottom;
            if (width <= 0f || height <= 0f)
            {
                return fallbackRect;
            }

            return new KernelGeom.Rectangle(left, bottom, width, height);
        }

        private static KernelGeom.Rectangle GetTextFragmentRectangle(CachedLine line, int startIndex, int length)
        {
            return UnionTextRectangles(GetTextFragmentRectangles(line, startIndex, length));
        }

        private static List<KernelGeom.Rectangle> GetTextFragmentRectangles(CachedLine line, int startIndex, int length)
        {
            if (string.IsNullOrEmpty(line.Text) || startIndex < 0 || startIndex + length > line.Text.Length)
                return null;

            if (line.Characters == null || line.Characters.Count == 0)
                return null;

            int endExclusive = Math.Min(startIndex + length, line.Characters.Count);
            if (startIndex >= endExclusive)
                return null;

            float maxAllowedGap = GetMaxAllowedTextFragmentGap(line, startIndex, endExclusive);
            var rectangles = new List<KernelGeom.Rectangle>();
            float minX = float.MaxValue;
            float maxX = float.MinValue;
            float minY = float.MaxValue;
            float maxY = float.MinValue;
            float currentRight = float.MinValue;

            for (int i = startIndex; i < endExclusive; i++)
            {
                var charInfo = line.Characters[i];
                if (charInfo?.BoundingBox == null ||
                    charInfo.BoundingBox.GetWidth() <= 0f ||
                    charInfo.BoundingBox.GetHeight() <= 0f)
                {
                    continue;
                }

                if (char.IsWhiteSpace(charInfo.Char))
                {
                    continue;
                }

                float charX = charInfo.BoundingBox.GetX();
                float charRight = charX + charInfo.BoundingBox.GetWidth();
                if (currentRight > float.MinValue &&
                    charX - currentRight > maxAllowedGap &&
                    minX != float.MaxValue)
                {
                    rectangles.Add(new KernelGeom.Rectangle(minX, minY, maxX - minX, maxY - minY));
                    minX = float.MaxValue;
                    maxX = float.MinValue;
                    minY = float.MaxValue;
                    maxY = float.MinValue;
                }

                minX = Math.Min(minX, charX);
                maxX = Math.Max(maxX, charRight);
                minY = Math.Min(minY, charInfo.BoundingBox.GetY());
                maxY = Math.Max(maxY, charInfo.BoundingBox.GetY() + charInfo.BoundingBox.GetHeight());
                currentRight = Math.Max(currentRight, charRight);
            }

            if (minX != float.MaxValue && maxX != float.MinValue && minY != float.MaxValue && maxY != float.MinValue)
            {
                rectangles.Add(new KernelGeom.Rectangle(minX, minY, maxX - minX, maxY - minY));
            }

            return rectangles.Count > 0 ? rectangles : null;
        }

        private static float GetMaxAllowedTextFragmentGap(CachedLine line, int startIndex, int endExclusive)
        {
            const float minimumGap = 4f;
            const float widthRatio = 2.5f;

            if (line?.Characters == null || startIndex < 0 || endExclusive <= startIndex)
            {
                return minimumGap;
            }

            var widths = new List<float>();
            for (int i = startIndex; i < endExclusive && i < line.Characters.Count; i++)
            {
                CharacterInfo charInfo = line.Characters[i];
                if (charInfo == null ||
                    charInfo.BoundingBox == null ||
                    char.IsWhiteSpace(charInfo.Char))
                {
                    continue;
                }

                float width = charInfo.BoundingBox.GetWidth();
                if (width > 0.1f)
                {
                    widths.Add(width);
                }
            }

            if (widths.Count == 0)
            {
                return minimumGap;
            }

            widths.Sort();
            float medianWidth = widths[widths.Count / 2];
            return Math.Max(minimumGap, medianWidth * widthRatio);
        }

        private static KernelGeom.Rectangle UnionTextRectangles(List<KernelGeom.Rectangle> rectangles)
        {
            if (rectangles == null || rectangles.Count == 0)
            {
                return null;
            }

            float minX = float.MaxValue;
            float maxX = float.MinValue;
            float minY = float.MaxValue;
            float maxY = float.MinValue;

            foreach (KernelGeom.Rectangle rect in rectangles)
            {
                if (rect == null || rect.GetWidth() <= 0f || rect.GetHeight() <= 0f)
                {
                    continue;
                }

                minX = Math.Min(minX, rect.GetX());
                maxX = Math.Max(maxX, rect.GetX() + rect.GetWidth());
                minY = Math.Min(minY, rect.GetY());
                maxY = Math.Max(maxY, rect.GetY() + rect.GetHeight());
            }

            if (minX == float.MaxValue || maxX == float.MinValue || minY == float.MaxValue || maxY == float.MinValue)
            {
                return null;
            }

            return new KernelGeom.Rectangle(minX, minY, maxX - minX, maxY - minY);
        }

        // Patterns for personal data
        private static readonly Regex PeselPattern = new Regex(@"\b\d{11}\b");
        private static readonly Regex PropertyRegisterPattern = new Regex(@"\b([A-Z]{2}\d{1}[A-Z0-9]{1})/\d{8}/\d{1}\b");
        private static readonly Regex IdCardPattern = new Regex(@"\b[A-Z]{3}\s?\d{6}\b");
        private static readonly Regex EmailPattern = new Regex(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}");
        private static readonly Regex NipPattern = new Regex(@"\b\d{3}[- ]?\d{2,3}[- ]?\d{2,3}[- ]?\d{2,3}\b");
        private static readonly Regex RegonPattern = new Regex(@"\b\d{9}(?:\d{5})?\b");
        private static readonly Regex PostalCodePattern = new Regex(@"(?<!\d)[0-9]{2}-[0-9]{3}(?!\d)");
        private static readonly Regex BankAccountPattern = new Regex(@"(?<![A-Z0-9])(?:PL\s*)?\d{2}(?:\s?\d{4}){5,6}(?!\d)", RegexOptions.IgnoreCase);
        private static readonly Regex VinPattern = new Regex(@"\b[A-HJ-NPR-Z0-9]{17}\b");
        private static readonly Regex UrlPattern = new Regex(@"https?://[^\s]{4,}");
        private static readonly Regex KrsPattern = new Regex(@"\b\d{10}\b");
        private static readonly Regex AddressPattern = new Regex(
            @"(?i)(?<![\p{L}\d])(?<value>(?:ul\.?|ulica|al\.|aleja|pl\.|plac|os\.|osiedle|rondo|skwer)\s+[\p{L}\d][\p{L}\d .'\-]{2,80}?\s+\d+[\p{L}]?(?:/\d+[\p{L}]?)?(?:\s*,\s*\d{2}-\d{3}\s+\p{Lu}[\p{L}\-]+(?:\s+\p{Lu}[\p{L}\-]+){0,2})?)(?![\p{L}\d/])");
        private static readonly Regex PostalCityPattern = new Regex(
            @"(?<!\d)(?<value>\d{2}-\d{3}\s+\p{Lu}[\p{L}\-]+(?:\s+\p{Lu}[\p{L}\-]+){0,2})(?![\p{L}\d])");
        private static readonly Regex PhonePattern = new Regex(
            @"(?<!\d)(?<value>(?:\+48[\s-]?)?(?:\d{3}[\s-]?){2}\d{3})(?!\d)");
        private static readonly Regex OrganizationPattern = new Regex(
            @"(?i)(?<![\p{L}\d])(?<value>(?:(?:[\p{Lu}0-9][\p{L}0-9&.'\-]*|i|w|we|z|im\.)\s+){0,6}(?:sp\.?\s*z\s*o\.?\s*o\.?|s\.?\s*a\.?|fundacja|stowarzyszenie|urz(?:a|\u0105)d|gmina|powiat|ministerstwo|bank|kancelaria)(?:\s+[\p{L}0-9&.'\-]+){0,8})(?![\p{L}\d])");
        private static readonly Regex LabeledPersonPattern = new Regex(
            @"(?i)\b(?:imie(?:\s+i\s+nazwisko)?|imi\u0119(?:\s+i\s+nazwisko)?|nazwisko|wnioskodawca|adresat|pelnomocnik|pe\u0142nomocnik|reprezentowany\s+przez|pan|pani)\s*[:\-]\s*(?<value>(?:\p{Lu}\p{Ll}{2,}(?:[-']\p{Lu}\p{Ll}{2,})?\s+){1,3}\p{Lu}\p{Ll}{2,}(?:[-']\p{Lu}\p{Ll}{2,})?)");
        private static readonly Regex PersonNamePattern = new Regex(
            @"(?<![\p{L}\d])(?<value>(?:\p{Lu}\p{Ll}{2,}(?:[-']\p{Lu}\p{Ll}{2,})?\s+){1,3}\p{Lu}\p{Ll}{2,}(?:[-']\p{Lu}\p{Ll}{2,})?)(?![\p{L}\d])");
        private static readonly HashSet<string> PersonTitleAbbreviations = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            // Polish
            "dr", "prof", "mgr", "inz", "inż", "lic", "mag",
            "pan", "pani",
            // German
            "hr", "fr", "herr", "frau", "ing", "dipl",
            // English / universal
            "mr", "ms", "mrs", "sir",
        };

        private static readonly HashSet<string> NerPersonStopWords = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "akt", "akta", "adres", "bank", "data", "decyzja", "dnia", "dokument", "email", "faktura",
            "gmina", "kancelaria", "konto", "kwota", "miasto", "ministerstwo", "numer", "organ",
            "pesel", "podpis", "powiat", "regon", "sad", "sadowy", "spolka", "strona", "telefon",
            "ulica", "umowa", "urzad", "wojewodztwo", "wniosek", "zalacznik",
            "ctrl", "dodaj", "drukuj", "edycja", "eksportuj", "enter", "kliknij", "menu",
            "nacisnij", "narzedzia", "opcje", "otworz", "plik", "podpisy", "pomoc",
            "projekt", "scal", "sekcja", "usun", "widok", "wybierz", "wyszukaj",
            "zapisz", "zaznacz",
            "angielski", "deutsch", "english", "german", "jezyk", "niemiecki", "polish",
            "polski"
        };
        private static readonly HashSet<string> NerInstructionStopWords = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "ctrl", "dodaj", "drukuj", "edytuj", "eksportuj", "enter", "kliknij", "menu",
            "nacisnij", "opcje", "otworz", "pobierz", "podpisy", "pokaz", "scal",
            "sekcja", "usun", "wybierz", "wyszukaj", "zapisz", "zaznacz"
        };
        private static readonly HashSet<string> NerUiLeadWords = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "menu", "kliknij", "wybierz", "otworz", "zapisz", "dodaj", "usun", "scal",
            "wyszukaj", "zaznacz", "plik", "edycja", "widok", "opcje", "pomoc",
            "ctrl", "nacisnij", "sekcja"
        };
        private static readonly HashSet<string> NerEntityStopWords = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "angielski", "de", "deutsch", "en", "english", "german", "kw", "lang",
            "language", "niemiecki", "pl", "polish", "polski"
        };
        private static readonly string[] NerOrganizationStopWords =
        {
            " sad ", " sad rejonowy", " sad okregowy", " urzad ", " gmina ", " powiat ",
            " ministerstwo ", " bank ", " kancelaria ", " spolka ", " sp z o o ", " s a ",
            " fundacja ", " stowarzyszenie "
        };

        /// <summary>
        /// Returns NER entity labels from the personal-data cache whose rectangles overlap
        /// <paramref name="pdfRect"/> on the given page. Returns null when the cache has not
        /// been populated yet (caller should fall back to regex-based detection).
        /// </summary>
        internal static IReadOnlyList<string> DetectTagsFromNerCache(
            string pdfPath, int pageNumber, KernelGeom.Rectangle pdfRect)
        {
            var cache = GetPersonalDataCache(pdfPath);
            if (cache == null)
                return null; // NER not loaded yet — signal caller to fall back

            var tags = new List<string>();
            foreach (TextLocation loc in cache)
            {
                if (loc.PageNumber != pageNumber) continue;
                if (string.IsNullOrEmpty(loc.Label)) continue;
                if (loc.Rect == null || pdfRect == null) continue;
                if (loc.Rect.GetLeft() < pdfRect.GetRight() &&
                    loc.Rect.GetRight() > pdfRect.GetLeft() &&
                    loc.Rect.GetBottom() < pdfRect.GetTop() &&
                    loc.Rect.GetTop() > pdfRect.GetBottom())
                {
                    if (!tags.Contains(loc.Label, StringComparer.OrdinalIgnoreCase))
                        tags.Add(loc.Label);
                }
            }
            return tags; // empty list = cache loaded but no entities at this location
        }

        internal static IReadOnlyList<string> DetectIdentifierTags(string text)
        {
            var detectedTags = new List<string>();
            if (string.IsNullOrWhiteSpace(text))
            {
                return detectedTags;
            }

            if (PeselPattern.Matches(text).Cast<Match>().Any(match => ValidatePesel(match.Value)))
            {
                detectedTags.Add("PESEL");
            }

            if (PropertyRegisterPattern.Matches(text).Cast<Match>().Any(match => ValidatePropertyRegister(match.Value)))
            {
                detectedTags.Add("KW");
            }

            if (IdCardPattern.Matches(text).Cast<Match>().Any(match => ValidateIdCard(match.Value)))
            {
                detectedTags.Add("IDENTITY_CARD");
            }

            if (EmailPattern.IsMatch(text))
            {
                detectedTags.Add("EMAIL");
            }

            if (NipPattern.Matches(text).Cast<Match>().Any(match => ValidateNip(match.Value)))
            {
                detectedTags.Add("NIP");
            }

            if (RegonPattern.Matches(text).Cast<Match>().Any(match => ValidateRegon(match.Value)))
            {
                detectedTags.Add("REGON");
            }

            if (KrsPattern.IsMatch(text))
            {
                detectedTags.Add("KRS");
            }

            if (PostalCodePattern.IsMatch(text))
            {
                detectedTags.Add("POSTAL_CODE");
            }

            if (BankAccountPattern.Matches(text).Cast<Match>().Any(match => ValidateBankAccount(match.Value)))
            {
                detectedTags.Add("BANK_ACCOUNT");
            }

            if (VinPattern.IsMatch(text))
            {
                detectedTags.Add("VIN");
            }

            if (UrlPattern.IsMatch(text))
            {
                detectedTags.Add("URL");
            }

            if (PhonePattern.Matches(text).Cast<Match>().Any(match => IsLikelyPhoneEntity(GetNamedMatchValue(match))))
            {
                detectedTags.Add("PHONE");
            }

            if (AddressPattern.Matches(text).Cast<Match>().Any(match => IsLikelyAddressEntity(GetNamedMatchValue(match))) ||
                PostalCityPattern.Matches(text).Cast<Match>().Any(match => IsLikelyAddressEntity(GetNamedMatchValue(match))))
            {
                detectedTags.Add("LOCATION");
            }

            if (OrganizationPattern.Matches(text).Cast<Match>().Any(match => IsLikelyOrganizationEntity(GetNamedMatchValue(match))))
            {
                detectedTags.Add("ORGANIZATION");
            }

            if (LabeledPersonPattern.Matches(text).Cast<Match>().Any(match => IsLikelyPersonEntity(GetNamedMatchValue(match))) ||
                PersonNamePattern.Matches(text).Cast<Match>().Any(match => IsLikelyPersonEntity(GetNamedMatchValue(match))))
            {
                detectedTags.Add("PERSON");
            }

            return detectedTags;
        }

        private static string GetNamedMatchValue(Match match)
        {
            if (match == null)
            {
                return string.Empty;
            }

            Group valueGroup = match.Groups["value"];
            return valueGroup.Success ? valueGroup.Value : match.Value;
        }

        private static bool ValidatePesel(string pesel)
        {
            // Check basic conditions
            if (pesel == null || pesel.Length != 11 || !pesel.All(char.IsDigit))
                return false;

            // Check control digit
            int[] weights = { 1, 3, 7, 9, 1, 3, 7, 9, 1, 3 };
            int sum = 0;
            for (int i = 0; i < 10; i++)
            {
                sum += (pesel[i] - '0') * weights[i];
            }
            int checkDigit = (10 - (sum % 10)) % 10;
            if (checkDigit != (pesel[10] - '0'))
                return false;

            // Extract birth date (only after checking control digit)
            if (!int.TryParse(pesel.Substring(0, 2), out int yearDigits) ||
                !int.TryParse(pesel.Substring(2, 2), out int monthDigits) ||
                !int.TryParse(pesel.Substring(4, 2), out int day))
                return false;

            // Determine full year and actual month
            int fullYear;
            int month;
            if (monthDigits >= 1 && monthDigits <= 12) // 1900-1999
            {
                fullYear = 1900 + yearDigits;
                month = monthDigits;
            }
            else if (monthDigits >= 21 && monthDigits <= 32) // 2000-2099
            {
                fullYear = 2000 + yearDigits;
                month = monthDigits - 20;
            }
            else if (monthDigits >= 81 && monthDigits <= 92) // 1800-1899
            {
                fullYear = 1800 + yearDigits;
                month = monthDigits - 80;
            }
            else if (monthDigits >= 41 && monthDigits <= 52) // 2100-2199
            {
                fullYear = 2100 + yearDigits;
                month = monthDigits - 40;
            }
            else if (monthDigits >= 61 && monthDigits <= 72) // 2200-2299
            {
                fullYear = 2200 + yearDigits;
                month = monthDigits - 60;
            }
            else
            {
                return false; // Invalid month range
            }

            // Birth date validation
            try
            {
                DateTime date = new DateTime(fullYear, month, day);
                return true;
            }
            catch (ArgumentOutOfRangeException)
            {
                return false; // Invalid date (e.g. February 31)
            }
        }

        private static bool ValidatePropertyRegister(string number)
        {
            if (string.IsNullOrEmpty(number))
                return false;

            string pattern = @"^([A-Z]{2}\d{1}[A-Z0-9]{1})/\d{8}/\d{1}$";
            var match = Regex.Match(number, pattern, RegexOptions.IgnoreCase);
            if (!match.Success)
                return false;

            string prefix = match.Groups[1].Value.ToUpperInvariant();

            HashSet<string> allowedPrefixes = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "BB1B", "BB1C", "BB1Z", "BI1B", "BI1P", "BI1S", "BI2P", "BI3P", "BY1B", "BY1I",
            "BY1M", "BY1N", "BY1S", "BY1T", "BY1U", "BY1Z", "BY2T", "CIKW", "CZ1C", "CZ1L",
            "CZ1M", "CZ1Z", "CZ2C", "DIRS", "EL1B", "EL1D", "EL1E", "EL1I", "EL1N", "EL1O",
            "EL2O", "GD1A", "GD1E", "GD1G", "GD1I", "GD1M", "GD1R", "GD1S", "GD1T", "GD1W",
            "GD1Y", "GD2I", "GD2M", "GD2W", "GL1G", "GL1J", "GL1R", "GL1S", "GL1T", "GL1W",
            "GL1X", "GL1Y", "GL1Z", "GW1G", "GW1K", "GW1M", "GW1S", "GW1U", "JG1B", "JG1J",
            "JG1K", "JG1L", "JG1S", "JG1Z", "KA1B", "KA1C", "KA1D", "KA1I", "KA1J", "KA1K",
            "KA1L", "KA1M", "KA1P", "KA1S", "KA1T", "KA1Y", "KI1A", "KI1B", "KI1H", "KI1I",
            "KI1J", "KI1K", "KI1L", "KI1O", "KI1P", "KI1R", "KI1S", "KI1T", "KI1W", "KN1K",
            "KN1N", "KN1S", "KN1T", "KO1B", "KO1D", "KO1E", "KO1I", "KO1K", "KO1L", "KO1W",
            "KO2B", "KR1B", "KR1C", "KR1E", "KR1H", "KR1I", "KR1K", "KR1M", "KR1O", "KR1P",
            "KR1S", "KR1W", "KR1Y", "KR2E", "KR2I", "KR2K", "KR2P", "KR2Y", "KR3I", "KS1B",
            "KS1E", "KS1J", "KS1K", "KS1S", "KS2E", "KZ1A", "KZ1E", "KZ1J", "KZ1O", "KZ1P",
            "KZ1R", "KZ1W", "LD1B", "LD1G", "LD1H", "LD1K", "LD1M", "LD1O", "LD1P", "LD1R",
            "LD1Y", "LE1G", "LE1J", "LE1L", "LE1U", "LE1Z", "LM1G", "LM1L", "LM1W", "LM1Z",
            "LU1A", "LU1B", "LU1C", "LU1I", "LU1K", "LU1O", "LU1P", "LU1R", "LU1S", "LU1U",
            "LU1W", "LU1Y", "NS1G", "NS1L", "NS1M", "NS1S", "NS1T", "NS1Z", "NS2L", "OL1B",
            "OL1C", "OL1E", "OL1G", "OL1K", "OL1L", "OL1M", "OL1N", "OL1O", "OL1P", "OL1S",
            "OL1Y", "OL2G", "OP1B", "OP1G", "OP1K", "OP1L", "OP1N", "OP1O", "OP1P", "OP1S",
            "OP1U", "OS1M", "OS1O", "OS1P", "OS1U", "OS1W", "PL1C", "PL1E", "PL1G", "PL1L",
            "PL1M", "PL1O", "PL1P", "PL1Z", "PL2M", "PO1A", "PO1B", "PO1D", "PO1E", "PO1F",
            "PO1G", "PO1H", "PO1I", "PO1K", "PO1L", "PO1M", "PO1N", "PO1O", "PO1P", "PO1R",
            "PO1S", "PO1T", "PO1Y", "PO1Z", "PO2A", "PO2H", "PO2P", "PO2T", "PR1J", "PR1L",
            "PR1P", "PR1R", "PR2R", "PT1B", "PT1O", "PT1P", "PT1R", "PT1T", "RA1G", "RA1K",
            "RA1L", "RA1P", "RA1R", "RA1S", "RA1Z", "RA2G", "RA2Z", "RZ1A", "RZ1D", "RZ1E",
            "RZ1R", "RZ1S", "RZ1Z", "RZ2Z", "SI1G", "SI1M", "SI1P", "SI1S", "SI1W", "SI2S",
            "SL1B", "SL1C", "SL1L", "SL1M", "SL1S", "SL1Z", "SO1C", "SR1L", "SR1S", "SR1W",
            "SR1Z", "SR2L", "SR2W", "SU1A", "SU1N", "SU1S", "SW1D", "SW1K", "SW1S", "SW1W",
            "SW1Z", "SW2K", "SZ1C", "SZ1G", "SZ1K", "SZ1L", "SZ1M", "SZ1O", "SZ1S", "SZ1T",
            "SZ1W", "SZ1Y", "SZ2S", "SZ2T", "TB1K", "TB1M", "TB1N", "TB1S", "TB1T", "TO1B",
            "TO1C", "TO1G", "TO1T", "TO1U", "TO1W", "TR1B", "TR1D", "TR1O", "TR1T", "TR2T",
            "WA1G", "WA1I", "WA1L", "WA1M", "WA1N", "WA1O", "WA1P", "WA1W", "WA2M", "WA3M",
            "WA4M", "WA5M", "WA6M", "WL1A", "WL1L", "WL1R", "WL1W", "WL1Y", "WR1E", "WR1K",
            "WR1L", "WR1M", "WR1O", "WR1S", "WR1T", "WR1W", "ZA1B", "ZA1H", "ZA1J", "ZA1K",
            "ZA1T", "ZA1Z", "ZG1E", "ZG1G", "ZG1K", "ZG1N", "ZG1R", "ZG1S", "ZG1W", "ZG2K",
            "ZG2S"
        };
            return allowedPrefixes.Contains(prefix);
        }

        private static bool ValidateIdCard(string idCard)
        {
            // Remove space if exists
            idCard = idCard.Replace(" ", "");
            if (idCard.Length != 9) return false;

            int[] weights = { 7, 3, 1, 7, 3, 1, 7, 3 }; // Weights for 3 letters and 5 digits (without the control digit)
            int sum = 0;

            // Letter check (positions 0-2)
            for (int i = 0; i < 3; i++)
            {
                if (!char.IsUpper(idCard[i])) return false;
                sum += (idCard[i] - 'A' + 10) * weights[i];
            }

            // Pierwsza cyfra (pozycja 3) to cyfra kontrolna
            if (!char.IsDigit(idCard[3])) return false;
            int checkDigit = idCard[3] - '0';

            // Calculate weighted sum for digits (positions 4-8, i.e. 2nd-6th digit)
            for (int i = 4; i < 9; i++)
            {
                if (!char.IsDigit(idCard[i])) return false;
                sum += (idCard[i] - '0') * weights[i - 1]; // i-1, because we skip control digit
            }

            int calculatedCheckDigit = sum % 10;
            return calculatedCheckDigit == checkDigit;
        }

        private static bool ValidateNip(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return false;
            string digits = new string(value.Where(c => char.IsDigit(c)).ToArray());
            if (digits.Length != 10 || !digits.All(char.IsDigit))
            {
                return false;
            }

            int[] weights = { 6, 5, 7, 2, 3, 4, 5, 6, 7 };
            int sum = 0;
            for (int i = 0; i < 9; i++)
            {
                sum += (digits[i] - '0') * weights[i];
            }

            int checksum = sum % 11;
            if (checksum == 10)
            {
                return false;
            }

            return checksum == (digits[9] - '0');
        }

        private static bool ValidateRegon(string value)
        {
            if (string.IsNullOrWhiteSpace(value) || !value.All(char.IsDigit))
            {
                return false;
            }

            if (value.Length == 9)
            {
                int[] weights = { 8, 9, 2, 3, 4, 5, 6, 7 };
                int sum = 0;
                for (int i = 0; i < 8; i++)
                {
                    sum += (value[i] - '0') * weights[i];
                }

                int checksum = sum % 11;
                if (checksum == 10)
                {
                    checksum = 0;
                }

                return checksum == (value[8] - '0');
            }

            if (value.Length == 14)
            {
                int[] weights = { 2, 4, 8, 5, 0, 9, 7, 3, 6, 1, 2, 4, 8 };
                int sum = 0;
                for (int i = 0; i < 13; i++)
                {
                    sum += (value[i] - '0') * weights[i];
                }

                int checksum = sum % 11;
                if (checksum == 10)
                {
                    checksum = 0;
                }

                return checksum == (value[13] - '0');
            }

            return false;
        }

        private static bool ValidateBankAccount(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return false;
            string clean = new string(value.Where(c => char.IsLetterOrDigit(c)).ToArray()).ToUpperInvariant();
            if (clean.StartsWith("PL", StringComparison.OrdinalIgnoreCase))
            {
                string ibanDigitsOnly = clean.Substring(2);
                Debug.WriteLine($"ValidateBankAccount: input='{value}' clean='{clean}' ibanDigits='{ibanDigitsOnly}' len={ibanDigitsOnly.Length}");
                if (!ibanDigitsOnly.All(char.IsDigit))
                {
                    return false;
                }

                if (ibanDigitsOnly.Length == 26)
                {
                    return ValidateIbanMod97(clean);
                }

                // In anonymization mode, a long PL-prefixed account fragment is still sensitive.
                return ibanDigitsOnly.Length >= 18 && ibanDigitsOnly.Length < 26;
            }

            string digits = new string(value.Where(char.IsDigit).ToArray());
            Debug.WriteLine($"ValidateBankAccount: input='{value}' digits='{digits}' len={digits.Length}");
            if (digits.Length != 26)
            {
                Debug.WriteLine($"ValidateBankAccount: rejected (len={digits.Length})");
                return false;
            }

            // Try as Polish NRB (26 digits): prepend "PL" implicitly for IBAN check
            if (digits.Length == 26)
            {
                // IBAN: "PL" + digits → replace P=25, L=21 → "2521" + digits
                // Move first 6 chars (2521 + first 2 digits of NRB) to end
                string ibanDigits = "2521" + digits;
                string reordered = ibanDigits.Substring(6) + ibanDigits.Substring(0, 6);
                long rem97 = Mod97(reordered);
                Debug.WriteLine($"ValidateBankAccount: NRB reordered='{reordered}' mod97={rem97}");
                if (rem97 == 1) return true;
            }

            // Try as raw IBAN (may start with letters)
            if (clean.Length >= 5 && clean.Take(2).All(char.IsLetter))
            {
                string reorderedIban = clean.Substring(4) + clean.Substring(0, 4);
                // Convert letters to numbers
                var sb = new System.Text.StringBuilder();
                foreach (char c in reorderedIban)
                {
                    if (char.IsLetter(c))
                        sb.Append((c - 'A' + 10).ToString());
                    else
                        sb.Append(c);
                }
                if (Mod97(sb.ToString()) == 1) return true;
            }

            return false;
        }

        private static bool ValidateIbanMod97(string cleanIban)
        {
            if (string.IsNullOrWhiteSpace(cleanIban) || cleanIban.Length < 5 || !cleanIban.Take(2).All(char.IsLetter))
            {
                return false;
            }

            string reorderedIban = cleanIban.Substring(4) + cleanIban.Substring(0, 4);
            var sb = new System.Text.StringBuilder();
            foreach (char c in reorderedIban)
            {
                if (char.IsLetter(c))
                    sb.Append((c - 'A' + 10).ToString());
                else if (char.IsDigit(c))
                    sb.Append(c);
                else
                    return false;
            }

            return Mod97(sb.ToString()) == 1;
        }

        private static long Mod97(string number)
        {
            long rem = 0;
            for (int i = 0; i < number.Length; i++)
            {
                rem = (rem * 10 + (number[i] - '0')) % 97;
            }
            return rem;
        }


    }



    class PdfCleanUpPreviewTextExtractionStrategy : ITextExtractionStrategy
    {
        private const float PreviewIntersectionEpsilon = 1e-4f;

        public sealed class CleanedGlyphInfo
        {
            public CleanedGlyphInfo(iText.Kernel.Geom.Rectangle bounds, string text)
            {
                Bounds = bounds;
                Text = text ?? string.Empty;
            }

            public iText.Kernel.Geom.Rectangle Bounds { get; }
            public string Text { get; }
        }

        private readonly IList<iText.Kernel.Geom.Rectangle> regions;
        private readonly CleanUpProperties properties;
        private readonly List<iText.Kernel.Geom.Rectangle> cleanedGlyphRectangles;
        private readonly List<CleanedGlyphInfo> cleanedGlyphInfos;

        public PdfCleanUpPreviewTextExtractionStrategy(
            IList<iText.Kernel.Geom.Rectangle> regions,
            CleanUpProperties properties = null)
        {
            this.regions = regions ?? new List<iText.Kernel.Geom.Rectangle>();
            this.properties = properties ?? new CleanUpProperties();
            cleanedGlyphRectangles = new List<iText.Kernel.Geom.Rectangle>();
            cleanedGlyphInfos = new List<CleanedGlyphInfo>();
        }

        public void EventOccurred(IEventData data, EventType type)
        {
            if (!type.Equals(EventType.RENDER_TEXT) || !(data is TextRenderInfo renderInfo))
            {
                return;
            }

            if (properties.GetOverlapRatio() == null && IsTextNotToBeCleaned(renderInfo))
            {
                return;
            }

            foreach (TextRenderInfo glyphRenderInfo in renderInfo.GetCharacterRenderInfos())
            {
                if (!IsTextNotToBeCleaned(glyphRenderInfo))
                {
                    iText.Kernel.Geom.Rectangle glyphBounds = GetGlyphBoundingRectangle(glyphRenderInfo);
                    if (glyphBounds != null && glyphBounds.GetWidth() > 0f && glyphBounds.GetHeight() > 0f)
                    {
                        cleanedGlyphRectangles.Add(glyphBounds);
                        cleanedGlyphInfos.Add(new CleanedGlyphInfo(glyphBounds, glyphRenderInfo.GetText()));
                    }
                }
            }
        }

        public string GetResultantText()
        {
            return string.Empty;
        }

        public string GetResultantText(ITextChunkLocation location)
        {
            return string.Empty;
        }

        public ICollection<EventType> GetSupportedEvents()
        {
            return new List<EventType> { EventType.RENDER_TEXT };
        }

        public bool TryGetCleanedGlyphRectangles(out List<iText.Kernel.Geom.Rectangle> glyphRects)
        {
            glyphRects = cleanedGlyphRectangles
                .Where(rect => rect != null && rect.GetWidth() > 0f && rect.GetHeight() > 0f)
                .ToList();
            return glyphRects.Count > 0;
        }

        public bool TryGetCleanedGlyphInfos(out List<CleanedGlyphInfo> glyphInfos)
        {
            glyphInfos = cleanedGlyphInfos
                .Where(info => info?.Bounds != null && info.Bounds.GetWidth() > 0f && info.Bounds.GetHeight() > 0f)
                .ToList();
            return glyphInfos.Count > 0;
        }

        private bool IsTextNotToBeCleaned(TextRenderInfo renderInfo)
        {
            iText.Kernel.Geom.Point[] textRect = GetTextRectangle(renderInfo);
            foreach (iText.Kernel.Geom.Rectangle region in regions)
            {
                iText.Kernel.Geom.Point[] redactRect = GetRectangleVertices(region);
                if (CheckIfRectanglesIntersect(textRect, redactRect))
                {
                    return false;
                }
            }

            return true;
        }

        private bool CheckIfRectanglesIntersect(iText.Kernel.Geom.Point[] rect1, iText.Kernel.Geom.Point[] rect2)
        {
            var clipper = new iText.Kernel.Pdf.Canvas.Parser.ClipperLib.Clipper();
            var clipperBridge = properties.GetOffsetProperties().CalculateOffsetMultiplierDynamically()
                ? new iText.Kernel.Pdf.Canvas.Parser.ClipperLib.ClipperBridge(rect1, rect2)
                : new iText.Kernel.Pdf.Canvas.Parser.ClipperLib.ClipperBridge();

            if (!clipperBridge.AddPolygonToClipper(clipper, rect2, iText.Kernel.Pdf.Canvas.Parser.ClipperLib.PolyType.CLIP))
            {
                if (!clipperBridge.AddPolygonToClipper(clipper, rect1, iText.Kernel.Pdf.Canvas.Parser.ClipperLib.PolyType.SUBJECT))
                {
                    if (!clipperBridge.AddPolylineSubjectToClipper(clipper, rect2))
                    {
                        return false;
                    }

                    if (rect1.Length != rect2.Length)
                    {
                        return false;
                    }

                    iText.Kernel.Geom.Point startPoint = rect2[0];
                    iText.Kernel.Geom.Point endPoint = rect2[0];
                    for (int i = 1; i < rect2.Length; i++)
                    {
                        if (rect2[i].Distance(startPoint) > PreviewIntersectionEpsilon)
                        {
                            endPoint = rect2[i];
                            break;
                        }
                    }

                    foreach (iText.Kernel.Geom.Point point in rect1)
                    {
                        if (IsPointOnLineSegment(point, startPoint, endPoint, true))
                        {
                            return true;
                        }
                    }
                }

                return false;
            }

            bool intersectionSubjectAdded = clipperBridge.AddPolygonToClipper(
                clipper,
                rect1,
                iText.Kernel.Pdf.Canvas.Parser.ClipperLib.PolyType.SUBJECT);
            if (intersectionSubjectAdded)
            {
                var paths = new List<List<iText.Kernel.Pdf.Canvas.Parser.ClipperLib.IntPoint>>();
                clipper.Execute(
                    iText.Kernel.Pdf.Canvas.Parser.ClipperLib.ClipType.INTERSECTION,
                    paths,
                    iText.Kernel.Pdf.Canvas.Parser.ClipperLib.PolyFillType.NON_ZERO,
                    iText.Kernel.Pdf.Canvas.Parser.ClipperLib.PolyFillType.NON_ZERO);
                return CheckIfIntersectionOccurs(paths, rect1, false, clipperBridge);
            }

            intersectionSubjectAdded = clipperBridge.AddPolylineSubjectToClipper(clipper, rect1);
            if (!intersectionSubjectAdded)
            {
                const double smallDiff = 0.01d;
                var expandedRect1 = new iText.Kernel.Geom.Point[rect1.Length + 1];
                Array.Copy(rect1, 0, expandedRect1, 0, rect1.Length);
                expandedRect1[rect1.Length] = new iText.Kernel.Geom.Point(rect1[0].GetX() + smallDiff, rect1[0].GetY());
                rect1 = expandedRect1;
                intersectionSubjectAdded = clipperBridge.AddPolylineSubjectToClipper(clipper, rect1);
                if (!intersectionSubjectAdded)
                {
                    return false;
                }
            }

            var polyTree = new iText.Kernel.Pdf.Canvas.Parser.ClipperLib.PolyTree();
            clipper.Execute(
                iText.Kernel.Pdf.Canvas.Parser.ClipperLib.ClipType.INTERSECTION,
                polyTree,
                iText.Kernel.Pdf.Canvas.Parser.ClipperLib.PolyFillType.NON_ZERO,
                iText.Kernel.Pdf.Canvas.Parser.ClipperLib.PolyFillType.NON_ZERO);
            return CheckIfIntersectionOccurs(
                iText.Kernel.Pdf.Canvas.Parser.ClipperLib.Clipper.PolyTreeToPaths(polyTree),
                rect1,
                true,
                clipperBridge);
        }

        private bool CheckIfIntersectionOccurs(
            List<List<iText.Kernel.Pdf.Canvas.Parser.ClipperLib.IntPoint>> paths,
            iText.Kernel.Geom.Point[] rect1,
            bool isDegenerate,
            iText.Kernel.Pdf.Canvas.Parser.ClipperLib.ClipperBridge clipperBridge)
        {
            if (paths == null || paths.Count == 0)
            {
                return false;
            }

            iText.Kernel.Pdf.Canvas.Parser.ClipperLib.IntRect intersectionRectangle =
                iText.Kernel.Pdf.Canvas.Parser.ClipperLib.Clipper.GetBounds(paths);
            if (properties.GetOverlapRatio() == null)
            {
                return !CheckIfIntersectionRectangleDegenerate(intersectionRectangle, isDegenerate, clipperBridge);
            }

            double overlappedArea = CalculatePolygonArea(rect1);
            double intersectionArea =
                clipperBridge.LongRectCalculateHeight(intersectionRectangle) *
                clipperBridge.LongRectCalculateWidth(intersectionRectangle);
            double percentageOfOverlapping = intersectionArea / overlappedArea;
            const float smallValueForRoundingErrors = 1e-5f;
            return percentageOfOverlapping + smallValueForRoundingErrors > properties.GetOverlapRatio();
        }

        private static bool CheckIfIntersectionRectangleDegenerate(
            iText.Kernel.Pdf.Canvas.Parser.ClipperLib.IntRect rect,
            bool isIntersectSubjectDegenerate,
            iText.Kernel.Pdf.Canvas.Parser.ClipperLib.ClipperBridge clipperBridge)
        {
            float width = clipperBridge.LongRectCalculateWidth(rect);
            float height = clipperBridge.LongRectCalculateHeight(rect);
            return isIntersectSubjectDegenerate
                ? (width < PreviewIntersectionEpsilon && height < PreviewIntersectionEpsilon)
                : (width < PreviewIntersectionEpsilon || height < PreviewIntersectionEpsilon);
        }

        private static bool IsPointOnLineSegment(
            iText.Kernel.Geom.Point currentPoint,
            iText.Kernel.Geom.Point linePoint1,
            iText.Kernel.Geom.Point linePoint2,
            bool isBetweenLinePoints)
        {
            double dxc = currentPoint.GetX() - linePoint1.GetX();
            double dyc = currentPoint.GetY() - linePoint1.GetY();
            double dxl = linePoint2.GetX() - linePoint1.GetX();
            double dyl = linePoint2.GetY() - linePoint1.GetY();
            double cross = dxc * dyl - dyc * dxl;
            if (Math.Abs(cross) <= PreviewIntersectionEpsilon)
            {
                if (!isBetweenLinePoints)
                {
                    return true;
                }

                if (Math.Abs(dxl) >= Math.Abs(dyl))
                {
                    return dxl > 0
                        ? linePoint1.GetX() - PreviewIntersectionEpsilon <= currentPoint.GetX() &&
                            currentPoint.GetX() <= linePoint2.GetX() + PreviewIntersectionEpsilon
                        : linePoint2.GetX() - PreviewIntersectionEpsilon <= currentPoint.GetX() &&
                            currentPoint.GetX() <= linePoint1.GetX() + PreviewIntersectionEpsilon;
                }

                return dyl > 0
                    ? linePoint1.GetY() - PreviewIntersectionEpsilon <= currentPoint.GetY() &&
                        currentPoint.GetY() <= linePoint2.GetY() + PreviewIntersectionEpsilon
                    : linePoint2.GetY() - PreviewIntersectionEpsilon <= currentPoint.GetY() &&
                        currentPoint.GetY() <= linePoint1.GetY() + PreviewIntersectionEpsilon;
            }

            return false;
        }

        private static iText.Kernel.Geom.Rectangle GetGlyphBoundingRectangle(TextRenderInfo renderInfo)
        {
            iText.Kernel.Geom.Point[] points = GetTextRectangle(renderInfo);
            float minX = points.Min(point => (float)point.GetX());
            float minY = points.Min(point => (float)point.GetY());
            float maxX = points.Max(point => (float)point.GetX());
            float maxY = points.Max(point => (float)point.GetY());

            float width = maxX - minX;
            float height = maxY - minY;
            if (width <= 0f || height <= 0f)
            {
                return null;
            }

            return new iText.Kernel.Geom.Rectangle(minX, minY, width, height);
        }

        private static iText.Kernel.Geom.Point[] GetTextRectangle(TextRenderInfo renderInfo)
        {
            iText.Kernel.Geom.LineSegment ascent = renderInfo.GetAscentLine();
            iText.Kernel.Geom.LineSegment descent = renderInfo.GetDescentLine();
            return new[]
            {
                new iText.Kernel.Geom.Point(ascent.GetStartPoint().Get(0), ascent.GetStartPoint().Get(1)),
                new iText.Kernel.Geom.Point(ascent.GetEndPoint().Get(0), ascent.GetEndPoint().Get(1)),
                new iText.Kernel.Geom.Point(descent.GetEndPoint().Get(0), descent.GetEndPoint().Get(1)),
                new iText.Kernel.Geom.Point(descent.GetStartPoint().Get(0), descent.GetStartPoint().Get(1))
            };
        }

        private static iText.Kernel.Geom.Point[] GetRectangleVertices(iText.Kernel.Geom.Rectangle rect)
        {
            return new[]
            {
                new iText.Kernel.Geom.Point(rect.GetLeft(), rect.GetBottom()),
                new iText.Kernel.Geom.Point(rect.GetRight(), rect.GetBottom()),
                new iText.Kernel.Geom.Point(rect.GetRight(), rect.GetTop()),
                new iText.Kernel.Geom.Point(rect.GetLeft(), rect.GetTop())
            };
        }

        private static double CalculatePolygonArea(iText.Kernel.Geom.Point[] polygon)
        {
            if (polygon == null || polygon.Length < 3)
            {
                return 0d;
            }

            double area = 0d;
            for (int i = 0; i < polygon.Length; i++)
            {
                iText.Kernel.Geom.Point current = polygon[i];
                iText.Kernel.Geom.Point next = polygon[(i + 1) % polygon.Length];
                area += (current.GetX() * next.GetY()) - (next.GetX() * current.GetY());
            }

            return Math.Abs(area) * 0.5d;
        }
    }

    class CustomTextExtractionStrategy : ITextExtractionStrategy
    {
        public struct CoveredLineInfo
        {
            public iText.Kernel.Geom.Rectangle Bounds { get; }
            public int GlyphCount { get; }

            public CoveredLineInfo(iText.Kernel.Geom.Rectangle bounds, int glyphCount)
            {
                Bounds = bounds;
                GlyphCount = glyphCount;
            }
        }

        private readonly iText.Kernel.Geom.Rectangle _targetRect;
        private readonly List<TextChunk> _textChunks;
        private readonly List<GlyphBounds> _coveredGlyphs;
        private readonly bool _expandDiacriticsForVisualBounds;
        private readonly bool _bodyBoundsOnly;
        private readonly float _yTolerance = 1.0f; // Tolerance for Y coordinate (in points)
        private readonly bool _sortByX = false; // Set to true if you want to sort by X
        private readonly bool _reverseOrder = false; // Ustaw na true dla tekstu od prawej do lewej
        private bool _hasCoveredBounds;
        private float _coveredMinX;
        private float _coveredMinY;
        private float _coveredMaxX;
        private float _coveredMaxY;

        public CustomTextExtractionStrategy(
            iText.Kernel.Geom.Rectangle targetRect,
            bool expandDiacriticsForVisualBounds = true,
            bool bodyBoundsOnly = false)
        {
            _targetRect = targetRect;
            _textChunks = new List<TextChunk>();
            _coveredGlyphs = new List<GlyphBounds>();
            _expandDiacriticsForVisualBounds = expandDiacriticsForVisualBounds;
            _bodyBoundsOnly = bodyBoundsOnly;
            _hasCoveredBounds = false;
            _coveredMinX = float.MaxValue;
            _coveredMinY = float.MaxValue;
            _coveredMaxX = float.MinValue;
            _coveredMaxY = float.MinValue;
        }

        public void EventOccurred(IEventData data, EventType type)
        {
            if (type.Equals(EventType.RENDER_TEXT))
            {
                TextRenderInfo renderInfo = (TextRenderInfo)data;
                foreach (TextRenderInfo chunk in renderInfo.GetCharacterRenderInfos())
                {
                    // Get ascent and descent lines for each character
                    var ascentLine = chunk.GetAscentLine();
                    var descentLine = chunk.GetDescentLine();

                    // Get character bounding box coordinates
                    float x1 = Math.Min(ascentLine.GetStartPoint().Get(0), descentLine.GetStartPoint().Get(0));
                    float x2 = Math.Max(ascentLine.GetEndPoint().Get(0), descentLine.GetEndPoint().Get(0));
                    float y1Detect = descentLine.GetStartPoint().Get(1); // Bottom edge (descent) — used for intersection
                    float y2 = ascentLine.GetStartPoint().Get(1); // Top edge (ascent)

                    string glyphText = chunk.GetText();
                    if (_expandDiacriticsForVisualBounds && glyphText.Length == 1 && y2 > y1Detect)
                    {
                        string nfd = glyphText[0].ToString().Normalize(System.Text.NormalizationForm.FormD);
                        for (int ni = 1; ni < nfd.Length; ni++)
                        {
                            int cp = (int)nfd[ni];
                            if (cp >= 0x0300 && cp <= 0x0315)
                            {
                                y2 += (y2 - y1Detect) * 0.20f;
                                break;
                            }
                        }
                    }

                    bool intersects = IsBoundingBoxInRectangle(x1, y1Detect, x2, y2, _targetRect);

                    if (intersects)
                    {
                        float y1Store = _bodyBoundsOnly
                            ? chunk.GetBaseline().GetStartPoint().Get(1)
                            : y1Detect;

                        _textChunks.Add(new TextChunk(chunk.GetText(), y1Store, x1));
                        _coveredGlyphs.Add(new GlyphBounds(
                            Math.Min(x1, x2),
                            Math.Min(y1Store, y2),
                            Math.Max(x1, x2),
                            Math.Max(y1Store, y2),
                            y1Store,
                            glyphText));
                        if (!_hasCoveredBounds)
                        {
                            _coveredMinX = x1;
                            _coveredMinY = y1Store;
                            _coveredMaxX = x2;
                            _coveredMaxY = y2;
                            _hasCoveredBounds = true;
                        }
                        else
                        {
                            _coveredMinX = Math.Min(_coveredMinX, x1);
                            _coveredMinY = Math.Min(_coveredMinY, y1Store);
                            _coveredMaxX = Math.Max(_coveredMaxX, x2);
                            _coveredMaxY = Math.Max(_coveredMaxY, y2);
                        }
                    }
                }
            }
        }

        private bool IsBoundingBoxInRectangle(float x1, float y1, float x2, float y2, iText.Kernel.Geom.Rectangle rect)
        {
            float rectLeft = rect.GetLeft();
            float rectRight = rect.GetRight();
            float rectBottom = rect.GetBottom();
            float rectTop = rect.GetTop();

            bool xOverlap = (x1 <= rectRight && x2 >= rectLeft); // At least partial horizontal coverage
            bool yOverlap = (y1 <= rectTop && y2 >= rectBottom); // At least partial vertical coverage

            return xOverlap && yOverlap;
        }

        public string GetResultantText()
        {
            // Group characters by line (based on Y coordinate)
            Dictionary<float, List<TextChunk>> lines = new Dictionary<float, List<TextChunk>>();
            foreach (var chunk in _textChunks)
            {
                float roundedY = (float)Math.Round(chunk.Y / _yTolerance) * _yTolerance;
                if (!lines.ContainsKey(roundedY))
                    lines[roundedY] = new List<TextChunk>();
                lines[roundedY].Add(chunk);
            }

            // Buduj tekst
            System.Text.StringBuilder result = new System.Text.StringBuilder();
            foreach (var line in lines)
            {
                // Sort characters in line by X if sorting is enabled
                if (_sortByX)
                {
                    line.Value.Sort((a, b) => _reverseOrder ? b.X.CompareTo(a.X) : a.X.CompareTo(b.X));
                }
                foreach (var chunk in line.Value)
                {
                    result.Append(chunk.Text);
                }
                result.AppendLine();
            }

            return result.ToString();
        }

        public string GetResultantText(ITextChunkLocation location) => GetResultantText();

        public ICollection<EventType> GetSupportedEvents()
        {
            return new List<EventType> { EventType.RENDER_TEXT };
        }

        public bool TryGetCoveredBounds(out iText.Kernel.Geom.Rectangle bounds)
        {
            bounds = null;
            if (!_hasCoveredBounds)
            {
                return false;
            }

            float width = _coveredMaxX - _coveredMinX;
            float height = _coveredMaxY - _coveredMinY;
            if (width <= 0f || height <= 0f)
            {
                return false;
            }

            bounds = new iText.Kernel.Geom.Rectangle(_coveredMinX, _coveredMinY, width, height);
            return true;
        }

        public bool TryGetPerLineBounds(out List<iText.Kernel.Geom.Rectangle> lineBounds)
        {
            lineBounds = null;
            if (!TryGetPerLineCoverage(out List<CoveredLineInfo> coveredLines))
            {
                return false;
            }

            lineBounds = coveredLines
                .Select(line => line.Bounds)
                .ToList();
            return lineBounds.Count > 0;
        }

        public bool TryGetPerLineCoverage(out List<CoveredLineInfo> coveredLines)
        {
            coveredLines = null;
            if (_coveredGlyphs.Count == 0)
            {
                return false;
            }

            const float baselineStep = 1.5f;
            var lines = new Dictionary<float, List<GlyphBounds>>();
            foreach (var glyph in _coveredGlyphs)
            {
                float key = (float)Math.Round(glyph.Baseline / baselineStep) * baselineStep;
                if (!lines.TryGetValue(key, out List<GlyphBounds> list))
                {
                    list = new List<GlyphBounds>();
                    lines[key] = list;
                }

                list.Add(glyph);
            }

            coveredLines = new List<CoveredLineInfo>(lines.Count);
            foreach (var entry in lines)
            {
                float minX = float.MaxValue;
                float minY = float.MaxValue;
                float maxX = float.MinValue;
                float maxY = float.MinValue;
                foreach (var glyph in entry.Value)
                {
                    minX = Math.Min(minX, glyph.Left);
                    minY = Math.Min(minY, glyph.Bottom);
                    maxX = Math.Max(maxX, glyph.Right);
                    maxY = Math.Max(maxY, glyph.Top);
                }

                float width = maxX - minX;
                float height = maxY - minY;
                if (width > 0f && height > 0f)
                {
                    coveredLines.Add(new CoveredLineInfo(
                        new iText.Kernel.Geom.Rectangle(minX, minY, width, height),
                        entry.Value.Count));
                }
            }

            return coveredLines.Count > 0;
        }

        public bool TryGetCoveredBoundsForMarkerLine(out iText.Kernel.Geom.Rectangle bounds)
        {
            bounds = null;
            if (_coveredGlyphs.Count == 0)
            {
                return false;
            }

            if (!TryGetSelectedMarkerLine(out List<GlyphBounds> selectedLine, out _, out _, out _))
            {
                return false;
            }

            return TryBuildBoundsFromGlyphs(selectedLine, out bounds);
        }

        public bool TryGetCoveredBoundsForMarkerLineVisual(out iText.Kernel.Geom.Rectangle bounds)
        {
            bounds = null;
            if (!TryGetSelectedMarkerLine(out List<GlyphBounds> selectedLine, out _, out _, out _))
            {
                return false;
            }

            if (selectedLine.Count == 0)
            {
                return false;
            }

            float minX = selectedLine.Min(g => g.Left);
            float maxX = selectedLine.Max(g => g.Right);
            List<float> bottoms = selectedLine.Select(g => g.Bottom).OrderBy(v => v).ToList();
            List<float> tops = selectedLine.Select(g => g.Top).OrderBy(v => v).ToList();

            float normalizedBottom = SelectMarkerVisualEdge(bottoms, preferUpperValues: true);
            float normalizedTop = SelectMarkerVisualEdge(tops, preferUpperValues: false);

            // Keep the visual rect valid even when normalization becomes too aggressive.
            if (normalizedTop <= normalizedBottom)
            {
                return TryBuildBoundsFromGlyphs(selectedLine, out bounds);
            }

            float width = maxX - minX;
            float height = normalizedTop - normalizedBottom;
            if (width <= 0f || height <= 0f)
            {
                return false;
            }

            bounds = new iText.Kernel.Geom.Rectangle(minX, normalizedBottom, width, height);
            return true;
        }

        private bool TryGetSelectedMarkerLine(
            out List<GlyphBounds> selectedLine,
            out float selectedKey,
            out int selectedCount,
            out float selectedDistance)
        {
            selectedLine = null;
            selectedKey = 0f;
            selectedCount = -1;
            selectedDistance = float.MaxValue;

            if (_coveredGlyphs.Count == 0)
            {
                return false;
            }

            const float baselineStep = 1.5f;
            float targetCenterY = _targetRect.GetBottom() + (_targetRect.GetHeight() / 2f);
            var lines = new Dictionary<float, List<GlyphBounds>>();
            foreach (var glyph in _coveredGlyphs)
            {
                float key = (float)Math.Round(glyph.Baseline / baselineStep) * baselineStep;
                if (!lines.TryGetValue(key, out List<GlyphBounds> list))
                {
                    list = new List<GlyphBounds>();
                    lines[key] = list;
                }

                list.Add(glyph);
            }

            foreach (var entry in lines)
            {
                int count = entry.Value.Count;
                float distance = Math.Abs(entry.Key - targetCenterY);
                if (count > selectedCount || (count == selectedCount && distance < selectedDistance))
                {
                    selectedCount = count;
                    selectedDistance = distance;
                    selectedKey = entry.Key;
                }
            }

            return lines.TryGetValue(selectedKey, out selectedLine) && selectedLine != null && selectedLine.Count > 0;
        }

        private static bool TryBuildBoundsFromGlyphs(List<GlyphBounds> glyphs, out iText.Kernel.Geom.Rectangle bounds)
        {
            bounds = null;
            if (glyphs == null || glyphs.Count == 0)
            {
                return false;
            }

            float minX = float.MaxValue;
            float minY = float.MaxValue;
            float maxX = float.MinValue;
            float maxY = float.MinValue;
            foreach (var glyph in glyphs)
            {
                minX = Math.Min(minX, glyph.Left);
                minY = Math.Min(minY, glyph.Bottom);
                maxX = Math.Max(maxX, glyph.Right);
                maxY = Math.Max(maxY, glyph.Top);
            }

            float width = maxX - minX;
            float height = maxY - minY;
            if (width <= 0f || height <= 0f)
            {
                return false;
            }

            bounds = new iText.Kernel.Geom.Rectangle(minX, minY, width, height);
            return true;
        }

        private static float SelectMarkerVisualEdge(List<float> sortedValues, bool preferUpperValues)
        {
            if (sortedValues == null || sortedValues.Count == 0)
            {
                return 0f;
            }

            if (sortedValues.Count <= 2)
            {
                return preferUpperValues ? sortedValues[sortedValues.Count - 1] : sortedValues[0];
            }

            int trimCount = Math.Max(1, sortedValues.Count / 6);
            trimCount = Math.Min(trimCount, sortedValues.Count - 1);
            int index = preferUpperValues
                ? sortedValues.Count - 1 - trimCount
                : trimCount;
            index = Math.Max(0, Math.Min(sortedValues.Count - 1, index));
            return sortedValues[index];
        }

        private struct GlyphBounds
        {
            public float Left { get; }
            public float Bottom { get; }
            public float Right { get; }
            public float Top { get; }
            public float Baseline { get; }
            public string Text { get; }

            public GlyphBounds(float left, float bottom, float right, float top, float baseline, string text)
            {
                Left = left;
                Bottom = bottom;
                Right = right;
                Top = top;
                Baseline = baseline;
                Text = text;
            }
        }

        private class TextChunk
        {
            public string Text { get; }
            public float Y { get; }
            public float X { get; }

            public TextChunk(string text, float y, float x)
            {
                Text = text;
                Y = y;
                X = x;
            }
        }

    }

    public static class AuditLogger
    {
        /// <summary>
        /// Saves to [dbo].[AnonPDF] login and ip of current user/station.
        /// Columns [id] (IDENTITY) and [datetime] (DEFAULT GETDATE()) are skipped.
        /// </summary>
        public static void LogUsage(string connectionString)
        {
            string login = GetCurrentLogin();
            string ip = GetPreferredIPv4() ?? "0.0.0.0";

            const string sql = @"INSERT INTO dbo.AnonPDF ([login], [ip]) VALUES (@login, @ip);";

            using (var conn = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@login", login);
                cmd.Parameters.AddWithValue("@ip", ip);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        private static string GetCurrentLogin()
        {
            // Full domain login if available (DOMAIN\User); fallback: Environment.UserName
            try
            {
                var id = WindowsIdentity.GetCurrent();
                if (id != null && !string.IsNullOrWhiteSpace(id.Name))
                    return id.Name;
            }
            catch { /* ignore; use fallback */ }

            return Environment.UserName ?? "unknown";
        }

        private static string GetPreferredIPv4()
        {
            try
            {
                // 1) Active interfaces (OperationalStatus.Up), exclude loopback/tunnel, IPv4 unicast
                var candidates =
                    NetworkInterface.GetAllNetworkInterfaces()
                        .Where(nic =>
                            nic.OperationalStatus == OperationalStatus.Up &&
                            nic.NetworkInterfaceType != NetworkInterfaceType.Loopback &&
                            nic.NetworkInterfaceType != NetworkInterfaceType.Tunnel)
                        .SelectMany(nic => nic.GetIPProperties().UnicastAddresses)
                        .Where(ua => ua?.Address != null && ua.Address.AddressFamily == AddressFamily.InterNetwork)
                        .Select(ua => ua.Address)
                        .Where(addr =>
                            !IPAddress.IsLoopback(addr) &&
                            addr.ToString() != "0.0.0.0" &&
                            !addr.ToString().StartsWith("169.254.")) // unikaj APIPA
                        .Select(addr => addr.ToString())
                        .Distinct()
                        .ToList();

                if (candidates.Count > 0)
                    return candidates.First();

                // 2) Fallback: Dns na hostname
                var host = Dns.GetHostEntry(Dns.GetHostName());
                var ip = host.AddressList.FirstOrDefault(a => a.AddressFamily == AddressFamily.InterNetwork);
                if (ip != null) return ip.ToString();
            }
            catch
            {
                // ignore, use default value above
            }

            return null;
        }
    }

    public class AltTextEditDialog : Form
    {
        private Label lblAltText;
        private TextBox txtAltText;
        private Button btnOK;
        private Button btnCancel;
        private Button btnClear;

        public string AltText => txtAltText.Text;

        public AltTextEditDialog(string currentAltText)
        {
            InitializeComponents(currentAltText);
        }

        private static string R(string key) =>
            Resources.ResourceManager.GetString(key) ?? key;

        private void InitializeComponents(string currentAltText)
        {
            this.Text = R("AltEdit_Title");
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterParent;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.AutoScaleDimensions = new SizeF(6F, 13F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = PDFForm.ScaleSizeForDpiStatic(420, 220);

            int maxH = Screen.GetWorkingArea(this).Height - 80;
            if (this.Height > maxH) this.Height = maxH;

            lblAltText = new Label
            {
                Text = R("AltEdit_Label"),
                AutoSize = true,
                Location = new Point(PDFForm.ScaleForDpiStatic(10), PDFForm.ScaleForDpiStatic(12))
            };

            txtAltText = new TextBox
            {
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                Text = currentAltText,
                Location = new Point(PDFForm.ScaleForDpiStatic(10), PDFForm.ScaleForDpiStatic(32)),
                Size = new Size(PDFForm.ScaleForDpiStatic(400), PDFForm.ScaleForDpiStatic(120)),
                AcceptsReturn = true,
                WordWrap = true
            };

            btnOK = new Button
            {
                Text = Resources.Merge_OK,
                DialogResult = DialogResult.OK,
                Location = new Point(PDFForm.ScaleForDpiStatic(135), PDFForm.ScaleForDpiStatic(165)),
                Width = PDFForm.ScaleForDpiStatic(80),
                Height = PDFForm.ScaleForDpiStatic(28)
            };

            btnCancel = new Button
            {
                Text = Resources.Merge_Cancel,
                DialogResult = DialogResult.Cancel,
                Location = new Point(PDFForm.ScaleForDpiStatic(225), PDFForm.ScaleForDpiStatic(165)),
                Width = PDFForm.ScaleForDpiStatic(80),
                Height = PDFForm.ScaleForDpiStatic(28)
            };

            btnClear = new Button
            {
                Text = R("AltEdit_Clear"),
                Location = new Point(PDFForm.ScaleForDpiStatic(325), PDFForm.ScaleForDpiStatic(165)),
                Width = PDFForm.ScaleForDpiStatic(80),
                Height = PDFForm.ScaleForDpiStatic(28)
            };
            btnClear.Click += (s, ev) => { txtAltText.Clear(); txtAltText.Focus(); };

            this.Controls.Add(lblAltText);
            this.Controls.Add(txtAltText);
            this.Controls.Add(btnOK);
            this.Controls.Add(btnCancel);
            this.Controls.Add(btnClear);

            this.AcceptButton = btnOK;
            this.CancelButton = btnCancel;
        }

        internal void ApplyDialogTheme(DialogTheme theme)
        {
            DialogThemeApplier.ApplyTo(this, theme);
        }
    }

}

#pragma warning restore SPELL
