using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace AnonPDF
{
    internal sealed class ThemedCheckBox : CheckBox
    {
        public Color DisabledForeColor { get; set; } = SystemColors.GrayText;
        public Color AccentColor { get; set; } = SystemColors.Highlight;
        public Color BorderColor { get; set; } = SystemColors.ControlDark;

        public ThemedCheckBox()
        {
            SetStyle(
                ControlStyles.UserPaint |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw |
                ControlStyles.SupportsTransparentBackColor,
                true);
            UseVisualStyleBackColor = false;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.Clear(BackColor);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            var glyphRect = GetGlyphRectangle(ClientRectangle, GetGlyphSize(), CheckAlign);
            var textRect = GetTextRectangle(ClientRectangle, glyphRect, CheckAlign);
            var flags = GetTextFormatFlags(RightToLeft, UseMnemonic);

            DrawCheckGlyph(e.Graphics, glyphRect);

            Color textColor = Enabled ? ForeColor : DisabledForeColor;
            TextRenderer.DrawText(e.Graphics, Text ?? string.Empty, Font, textRect, textColor, BackColor, flags);

            if (Focused && ShowFocusCues)
            {
                ControlPaint.DrawFocusRectangle(e.Graphics, textRect, textColor, BackColor);
            }
        }

        private Size GetGlyphSize()
        {
            int side = Math.Max(13, Math.Min(18, Font.Height - 1));
            return new Size(side, side);
        }

        private void DrawCheckGlyph(Graphics graphics, Rectangle glyphRect)
        {
            Color normalBorderColor = DarkenColor(BorderColor, 0.18f);
            Color borderColor = Enabled ? normalBorderColor : ControlPaint.Light(normalBorderColor);
            Color accentColor = Enabled ? AccentColor : ControlPaint.Light(AccentColor);
            Color boxBackColor = Enabled ? BackColor : ControlPaint.Light(BackColor);

            using (var backBrush = new SolidBrush(boxBackColor))
            using (var borderPen = new Pen(borderColor, 1f))
            {
                graphics.FillRectangle(backBrush, glyphRect);
                graphics.DrawRectangle(borderPen, glyphRect.X, glyphRect.Y, glyphRect.Width - 1, glyphRect.Height - 1);
            }

            if (CheckState == CheckState.Indeterminate)
            {
                Rectangle indeterminateRect = Rectangle.Inflate(glyphRect, -3, -3);
                using (var accentBrush = new SolidBrush(accentColor))
                {
                    graphics.FillRectangle(accentBrush, indeterminateRect);
                }
                return;
            }

            if (CheckState != CheckState.Checked)
            {
                return;
            }

            Rectangle fillRect = Rectangle.Inflate(glyphRect, -2, -2);
            using (var accentBrush = new SolidBrush(accentColor))
            {
                graphics.FillRectangle(accentBrush, fillRect);
            }

            Color checkColor = GetContrastColor(accentColor);
            using (var checkPen = new Pen(checkColor, 2f))
            {
                checkPen.StartCap = LineCap.Round;
                checkPen.EndCap = LineCap.Round;

                int x1 = glyphRect.Left + (int)(glyphRect.Width * 0.24f);
                int y1 = glyphRect.Top + (int)(glyphRect.Height * 0.55f);
                int x2 = glyphRect.Left + (int)(glyphRect.Width * 0.44f);
                int y2 = glyphRect.Top + (int)(glyphRect.Height * 0.74f);
                int x3 = glyphRect.Left + (int)(glyphRect.Width * 0.76f);
                int y3 = glyphRect.Top + (int)(glyphRect.Height * 0.30f);

                graphics.DrawLines(checkPen, new[]
                {
                    new Point(x1, y1),
                    new Point(x2, y2),
                    new Point(x3, y3)
                });
            }
        }

        private static Color GetContrastColor(Color color)
        {
            double luminance = ((0.299 * color.R) + (0.587 * color.G) + (0.114 * color.B)) / 255d;
            return luminance > 0.55 ? Color.Black : Color.White;
        }

        private static Color DarkenColor(Color color, float amount)
        {
            amount = Math.Max(0f, Math.Min(1f, amount));
            int r = (int)Math.Round(color.R * (1f - amount));
            int g = (int)Math.Round(color.G * (1f - amount));
            int b = (int)Math.Round(color.B * (1f - amount));
            return Color.FromArgb(color.A, Math.Max(0, r), Math.Max(0, g), Math.Max(0, b));
        }

        private static Rectangle GetGlyphRectangle(Rectangle bounds, Size glyphSize, ContentAlignment align)
        {
            int x = bounds.Left;
            int y = bounds.Top + (bounds.Height - glyphSize.Height) / 2;

            if (align == ContentAlignment.TopLeft || align == ContentAlignment.TopCenter || align == ContentAlignment.TopRight)
            {
                y = bounds.Top;
            }
            else if (align == ContentAlignment.BottomLeft || align == ContentAlignment.BottomCenter || align == ContentAlignment.BottomRight)
            {
                y = bounds.Bottom - glyphSize.Height;
            }

            if (align == ContentAlignment.TopRight || align == ContentAlignment.MiddleRight || align == ContentAlignment.BottomRight)
            {
                x = bounds.Right - glyphSize.Width;
            }
            else if (align == ContentAlignment.TopCenter || align == ContentAlignment.MiddleCenter || align == ContentAlignment.BottomCenter)
            {
                x = bounds.Left + (bounds.Width - glyphSize.Width) / 2;
            }

            return new Rectangle(x, y, glyphSize.Width, glyphSize.Height);
        }

        private static Rectangle GetTextRectangle(Rectangle bounds, Rectangle glyphRect, ContentAlignment align)
        {
            const int padding = 4;
            if (align == ContentAlignment.TopRight || align == ContentAlignment.MiddleRight || align == ContentAlignment.BottomRight)
            {
                return new Rectangle(bounds.Left, bounds.Top, glyphRect.Left - bounds.Left - padding, bounds.Height);
            }

            return new Rectangle(glyphRect.Right + padding, bounds.Top, bounds.Width - glyphRect.Right - padding, bounds.Height);
        }

        private static TextFormatFlags GetTextFormatFlags(RightToLeft rightToLeft, bool useMnemonic)
        {
            TextFormatFlags flags = TextFormatFlags.SingleLine | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding;
            flags |= rightToLeft == RightToLeft.Yes ? TextFormatFlags.Right | TextFormatFlags.RightToLeft : TextFormatFlags.Left;
            if (!useMnemonic)
            {
                flags |= TextFormatFlags.NoPrefix;
            }

            return flags;
        }
    }

    internal sealed class ThemedRadioButton : RadioButton
    {
        public Color DisabledForeColor { get; set; } = SystemColors.GrayText;
        public Color AccentColor { get; set; } = SystemColors.Highlight;
        public Color BorderColor { get; set; } = SystemColors.ControlDark;

        public ThemedRadioButton()
        {
            SetStyle(
                ControlStyles.UserPaint |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw |
                ControlStyles.SupportsTransparentBackColor,
                true);
            UseVisualStyleBackColor = false;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.Clear(BackColor);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            var glyphRect = GetGlyphRectangle(ClientRectangle, GetGlyphSize(), CheckAlign);
            var textRect = GetTextRectangle(ClientRectangle, glyphRect, CheckAlign);
            var flags = GetTextFormatFlags(RightToLeft, UseMnemonic);

            DrawRadioGlyph(e.Graphics, glyphRect);

            Color textColor = Enabled ? ForeColor : DisabledForeColor;
            TextRenderer.DrawText(e.Graphics, Text ?? string.Empty, Font, textRect, textColor, BackColor, flags);

            if (Focused && ShowFocusCues)
            {
                ControlPaint.DrawFocusRectangle(e.Graphics, textRect, textColor, BackColor);
            }
        }

        private Size GetGlyphSize()
        {
            int side = Math.Max(13, Math.Min(18, Font.Height - 1));
            return new Size(side, side);
        }

        private void DrawRadioGlyph(Graphics graphics, Rectangle glyphRect)
        {
            Color accentBorderColor = DarkenColor(AccentColor, 0.14f);
            Color neutralBorderColor = DarkenColor(BorderColor, 0.18f);
            Color borderColor = Checked
                ? (Enabled ? accentBorderColor : ControlPaint.Light(accentBorderColor))
                : (Enabled ? neutralBorderColor : ControlPaint.Light(neutralBorderColor));
            Color backColor = Enabled ? BackColor : ControlPaint.Light(BackColor);
            Color dotColor = Enabled
                ? GetContrastColor(AccentColor)
                : ControlPaint.Dark(GetContrastColor(AccentColor));

            using (var backBrush = new SolidBrush(backColor))
            using (var borderPen = new Pen(borderColor, 1f))
            {
                graphics.FillEllipse(backBrush, glyphRect);
                graphics.DrawEllipse(borderPen, glyphRect.X, glyphRect.Y, glyphRect.Width - 1, glyphRect.Height - 1);
            }

            if (!Checked)
            {
                return;
            }

            int accentDotSize = Math.Max(7, (int)Math.Round(glyphRect.Width * 0.74f));
            Rectangle accentDotRect = new Rectangle(
                glyphRect.Left + (glyphRect.Width - accentDotSize) / 2,
                glyphRect.Top + (glyphRect.Height - accentDotSize) / 2,
                accentDotSize,
                accentDotSize);
            using (var accentBrush = new SolidBrush(Enabled ? AccentColor : ControlPaint.Light(AccentColor)))
            {
                graphics.FillEllipse(accentBrush, accentDotRect);
            }

            int coreDotSize = Math.Max(5, (int)Math.Round(accentDotSize * 0.62f));
            Rectangle coreDotRect = new Rectangle(
                glyphRect.Left + (glyphRect.Width - coreDotSize) / 2,
                glyphRect.Top + (glyphRect.Height - coreDotSize) / 2,
                coreDotSize,
                coreDotSize);
            using (var dotBrush = new SolidBrush(dotColor))
            {
                graphics.FillEllipse(dotBrush, coreDotRect);
            }
        }

        private static Color GetContrastColor(Color color)
        {
            double luminance = ((0.299 * color.R) + (0.587 * color.G) + (0.114 * color.B)) / 255d;
            return luminance > 0.55 ? Color.Black : Color.White;
        }

        private static Color DarkenColor(Color color, float amount)
        {
            amount = Math.Max(0f, Math.Min(1f, amount));
            int r = (int)Math.Round(color.R * (1f - amount));
            int g = (int)Math.Round(color.G * (1f - amount));
            int b = (int)Math.Round(color.B * (1f - amount));
            return Color.FromArgb(color.A, Math.Max(0, r), Math.Max(0, g), Math.Max(0, b));
        }

        private static Rectangle GetGlyphRectangle(Rectangle bounds, Size glyphSize, ContentAlignment align)
        {
            int x = bounds.Left;
            int y = bounds.Top + (bounds.Height - glyphSize.Height) / 2;

            if (align == ContentAlignment.TopLeft || align == ContentAlignment.TopCenter || align == ContentAlignment.TopRight)
            {
                y = bounds.Top;
            }
            else if (align == ContentAlignment.BottomLeft || align == ContentAlignment.BottomCenter || align == ContentAlignment.BottomRight)
            {
                y = bounds.Bottom - glyphSize.Height;
            }

            if (align == ContentAlignment.TopRight || align == ContentAlignment.MiddleRight || align == ContentAlignment.BottomRight)
            {
                x = bounds.Right - glyphSize.Width;
            }
            else if (align == ContentAlignment.TopCenter || align == ContentAlignment.MiddleCenter || align == ContentAlignment.BottomCenter)
            {
                x = bounds.Left + (bounds.Width - glyphSize.Width) / 2;
            }

            return new Rectangle(x, y, glyphSize.Width, glyphSize.Height);
        }

        private static Rectangle GetTextRectangle(Rectangle bounds, Rectangle glyphRect, ContentAlignment align)
        {
            const int padding = 4;
            if (align == ContentAlignment.TopRight || align == ContentAlignment.MiddleRight || align == ContentAlignment.BottomRight)
            {
                return new Rectangle(bounds.Left, bounds.Top, glyphRect.Left - bounds.Left - padding, bounds.Height);
            }

            return new Rectangle(glyphRect.Right + padding, bounds.Top, bounds.Width - glyphRect.Right - padding, bounds.Height);
        }

        private static TextFormatFlags GetTextFormatFlags(RightToLeft rightToLeft, bool useMnemonic)
        {
            TextFormatFlags flags = TextFormatFlags.SingleLine | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding;
            flags |= rightToLeft == RightToLeft.Yes ? TextFormatFlags.Right | TextFormatFlags.RightToLeft : TextFormatFlags.Left;
            if (!useMnemonic)
            {
                flags |= TextFormatFlags.NoPrefix;
            }

            return flags;
        }
    }

    internal sealed class ThemedGroupBox : GroupBox
    {
        public Color DisabledForeColor { get; set; } = SystemColors.GrayText;

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            DrawThemedText(e.Graphics);
        }

        protected override void OnEnabledChanged(EventArgs e)
        {
            base.OnEnabledChanged(e);
            Invalidate();
        }

        private void DrawThemedText(Graphics graphics)
        {
            if (string.IsNullOrEmpty(Text))
            {
                return;
            }

            var textColor = Enabled ? ForeColor : DisabledForeColor;
            var textSize = TextRenderer.MeasureText(graphics, Text, Font, Size.Empty, TextFormatFlags.SingleLine);
            var textRect = new Rectangle(8, 0, textSize.Width + 2, textSize.Height);

            using (var backBrush = new SolidBrush(BackColor))
            {
                graphics.FillRectangle(backBrush, textRect);
            }

            TextRenderer.DrawText(graphics, Text, Font, textRect, textColor, BackColor, TextFormatFlags.Left | TextFormatFlags.VerticalCenter);
        }
    }
}
