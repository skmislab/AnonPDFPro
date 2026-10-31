using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using PdfRectangle = iText.Kernel.Geom.Rectangle;

namespace AnonPDF
{
    internal static class RedactionCleanupGeometry
    {
        internal static PdfRectangle GetOcrTextHitArea(PdfRectangle area, bool verticalText = false)
        {
            return verticalText
                ? new PdfRectangle(area.GetX() + area.GetWidth() * 0.25f, area.GetY(),
                    area.GetWidth() * 0.5f, area.GetHeight())
                : new PdfRectangle(area.GetX(), area.GetY() + area.GetHeight() * 0.25f,
                    area.GetWidth(), area.GetHeight() * 0.5f);
        }

        internal static List<PdfRectangle> Build(
            RedactionBlock block, PdfRectangle area, PdfRectangle textHitArea,
            IReadOnlyCollection<PdfRectangle> ocrWords, bool graphicMode)
        {
            if (graphicMode || (!block.IsCursorSelection && !block.IsMarkerSelection && !block.IncludeWholeWords))
                return new List<PdfRectangle> { area };

            // The inset qualifies native glyphs; it must not trim the pixels of a scan.
            var result = new List<PdfRectangle>();
            if (ocrWords != null && ocrWords.Count > 0)
            {
                result.Add(area);
                result.AddRange(ocrWords.Where(r => r != null && r.GetWidth() > 0 && r.GetHeight() > 0));
            }
            else
            {
                result.Add(textHitArea);
            }
            return result;
        }
    }

    internal sealed class OcrInkBoundsRefiner
    {
        private readonly int width;
        private readonly int height;
        private readonly byte[] luminance;
        private readonly IReadOnlyList<RectangleF> words;

        internal OcrInkBoundsRefiner(Bitmap bitmap, IReadOnlyList<RectangleF> words)
        {
            width = bitmap.Width;
            height = bitmap.Height;
            this.words = words;
            luminance = new byte[width * height];
            using (var rgb = new Bitmap(width, height, PixelFormat.Format32bppArgb))
            {
                using (var g = Graphics.FromImage(rgb)) g.DrawImageUnscaled(bitmap, 0, 0);
                var data = rgb.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly, rgb.PixelFormat);
                try
                {
                    var row = new byte[width * 4];
                    for (int y = 0; y < height; y++)
                    {
                        Marshal.Copy(IntPtr.Add(data.Scan0, y * data.Stride), row, 0, row.Length);
                        for (int x = 0; x < width; x++)
                            luminance[y * width + x] = (byte)((row[x * 4] * 29 + row[x * 4 + 1] * 150 + row[x * 4 + 2] * 77) >> 8);
                    }
                }
                finally { rgb.UnlockBits(data); }
            }
        }

        internal RectangleF Refine(RectangleF word)
        {
            if (word.Width <= 0 || word.Height <= 0) return word;
            float padX = Math.Max(2, word.Height * 0.5f);
            float padY = Math.Max(2, word.Height * 0.3f);
            float left = Math.Max(0, word.Left - padX), right = Math.Min(width, word.Right + padX);
            float top = Math.Max(0, word.Top - padY), bottom = Math.Min(height, word.Bottom + padY);

            // Stop in the gap between OCR words/lines, rather than adding a fixed
            // page-space margin that could erase tightly packed neighbouring text.
            foreach (var other in words)
            {
                if (other == word || other.Width <= 0 || other.Height <= 0) continue;
                float overlapY = Math.Min(word.Bottom, other.Bottom) - Math.Max(word.Top, other.Top);
                float overlapX = Math.Min(word.Right, other.Right) - Math.Max(word.Left, other.Left);
                if (overlapY > Math.Min(word.Height, other.Height) * 0.4f)
                {
                    if (other.Right <= word.Left) left = Math.Max(left, (other.Right + word.Left) / 2);
                    if (other.Left >= word.Right) right = Math.Min(right, (other.Left + word.Right) / 2);
                }
                if (overlapX > 0)
                {
                    if (other.Bottom <= word.Top) top = Math.Max(top, (other.Bottom + word.Top) / 2);
                    if (other.Top >= word.Bottom) bottom = Math.Min(bottom, (other.Top + word.Bottom) / 2);
                }
            }

            int x0 = (int)Math.Ceiling(left), y0 = (int)Math.Ceiling(top);
            int x1 = (int)Math.Floor(right), y1 = (int)Math.Floor(bottom);
            int w = x1 - x0, h = y1 - y0;
            if (w <= 0 || h <= 0) return word;
            int threshold = GetInkThreshold(x0, y0, w, h);
            var visited = new bool[w * h];
            var pending = new Stack<int>();
            var ink = RectangleF.Empty;
            bool hasSeedInk = false;
            for (int index = 0; index < visited.Length; index++)
            {
                if (visited[index]) continue;
                visited[index] = true;
                int sx = index % w, sy = index / w;
                if (!IsInk(x0 + sx, y0 + sy, threshold)) continue;
                pending.Push(index);
                int minX = sx, maxX = sx, minY = sy, maxY = sy, count = 0;
                bool touchesSeed = false;
                while (pending.Count > 0)
                {
                    int current = pending.Pop(), x = current % w, y = current / w;
                    minX = Math.Min(minX, x); maxX = Math.Max(maxX, x);
                    minY = Math.Min(minY, y); maxY = Math.Max(maxY, y);
                    count++;
                    touchesSeed |= word.Contains(x0 + x + 0.5f, y0 + y + 0.5f);
                    for (int dy = -1; dy <= 1; dy++)
                        for (int dx = -1; dx <= 1; dx++)
                        {
                            int nx = x + dx, ny = y + dy;
                            if (nx < 0 || ny < 0 || nx >= w || ny >= h) continue;
                            int next = ny * w + nx;
                            if (visited[next]) continue;
                            visited[next] = true;
                            if (IsInk(x0 + nx, y0 + ny, threshold)) pending.Push(next);
                        }
                }
                int componentWidth = maxX - minX + 1, componentHeight = maxY - minY + 1;
                bool horizontalRule = minX == 0 && maxX == w - 1 && componentHeight <= Math.Max(2, word.Height * 0.15f);
                bool verticalRule = minY == 0 && maxY == h - 1 && componentWidth <= Math.Max(2, word.Height * 0.15f);
                if (horizontalRule || verticalRule) continue;
                var component = new RectangleF(x0 + minX, y0 + minY, componentWidth, componentHeight);
                float verticalGap = Math.Max(0, Math.Max(word.Top - component.Bottom, component.Top - word.Bottom));
                bool edgeGlyph = componentHeight >= word.Height * 0.35f && verticalGap == 0;
                bool accent = component.Right > word.Left && component.Left < word.Right &&
                              verticalGap <= Math.Max(2, word.Height * 0.15f);
                if (!touchesSeed && (count < 2 || (!edgeGlyph && !accent))) continue;
                hasSeedInk |= touchesSeed;
                ink = ink.IsEmpty ? component : RectangleF.Union(ink, component);
            }
            if (!hasSeedInk || ink.IsEmpty) return word;
            return RectangleF.Union(word, RectangleF.FromLTRB(
                Math.Max(left, ink.Left - 1), Math.Max(top, ink.Top - 1),
                Math.Min(right, ink.Right + 1), Math.Min(bottom, ink.Bottom + 1)));
        }

        private int GetInkThreshold(int x0, int y0, int w, int h)
        {
            var histogram = new int[256];
            int minimum = 255;
            for (int y = y0; y < y0 + h; y++)
                for (int x = x0; x < x0 + w; x++)
                {
                    byte value = luminance[y * width + x];
                    histogram[value]++;
                    minimum = Math.Min(minimum, value);
                }
            int background = 0, pixels = 0;
            while (background < 255 && pixels + histogram[background] < w * h * 0.85)
                pixels += histogram[background++];
            return Math.Max(0, background - Math.Max(10, (int)((background - minimum) * 0.12f)));
        }

        private bool IsInk(int x, int y, int threshold) => luminance[y * width + x] < threshold;
    }
}
