using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using MessagePack;

namespace AnonPDF
{
    // ── Disk cache for expensive per-PDF operations ──────────────────────────────
    // Stores text-line extraction, alt-text metadata and NER results so that
    // re-opening the same file skips all background processing.
    //
    // Location : %LOCALAPPDATA%\AnonPDFPro\cache\{sha256-of-path}.msgpack
    // Format   : MessagePack with LZ4-block compression (binary, not human-readable)
    // Validity : file size + last-write-time must match; format version must match.
    //
    // The file is saved in two steps:
    //   1. After text extraction completes (lines + alt texts + figures).
    //   2. After NER completes (overwrites with full data including personal-data results).
    // If the app closes between the two steps, step 1 cache is still usable (NER will
    // re-run on next open, then save step 2).
    // ─────────────────────────────────────────────────────────────────────────────

    internal static class PdfDiskCache
    {
        private const int FormatVersion = 4;

        private static string CacheDir =>
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                         "skmislab", "AnonPDFPro", "cache");

        internal static string GetCacheFilePath(string pdfPath)
        {
            using (var sha = SHA256.Create())
            {
                byte[] hash = sha.ComputeHash(Encoding.UTF8.GetBytes(pdfPath.ToLowerInvariant()));
                return Path.Combine(CacheDir, BitConverter.ToString(hash).Replace("-", "") + ".msgpack");
            }
        }

        /// <summary>
        /// Tries to load a valid cache entry for <paramref name="pdfPath"/>.
        /// Returns false if the file doesn't exist, has changed or the format is stale.
        /// </summary>
        internal static bool TryLoad(string pdfPath, out PdfCacheFile cache)
        {
            cache = null;
            try
            {
                string cachePath = GetCacheFilePath(pdfPath);
                if (!File.Exists(cachePath))
                {
                    DbgLog($"TryLoad miss (no file): {Path.GetFileName(pdfPath)}");
                    return false;
                }

                var fi = new FileInfo(pdfPath);
                if (!fi.Exists) return false;

                byte[] bytes = File.ReadAllBytes(cachePath);
                var loaded = MessagePackSerializer.Deserialize<PdfCacheFile>(
                    bytes, MessagePackSerializerOptions.Standard
                               .WithCompression(MessagePackCompression.Lz4BlockArray));

                if (loaded.Version != FormatVersion)
                {
                    DbgLog($"TryLoad miss (version mismatch): {Path.GetFileName(pdfPath)}");
                    return false;
                }
                if (loaded.FileSize != fi.Length || loaded.LastWriteTimeUtcTicks != fi.LastWriteTimeUtc.Ticks)
                {
                    DbgLog($"TryLoad miss (file changed): {Path.GetFileName(pdfPath)}");
                    return false;
                }
                if (!string.Equals(loaded.NerCacheIdentity, PdfTextSearcher.GetNerCacheIdentity(), StringComparison.Ordinal))
                {
                    DbgLog($"TryLoad miss (NER plugin changed): {Path.GetFileName(pdfPath)}");
                    return false;
                }

                DbgLog($"TryLoad HIT: {Path.GetFileName(pdfPath)} lines={loaded.Lines?.Count} ner={loaded.PersonalData?.Count}");
                cache = loaded;
                return true;
            }
            catch (Exception ex)
            {
                DbgLog($"TryLoad FAILED for {Path.GetFileName(pdfPath)}: {ex.GetType().Name}: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Saves (or overwrites) the cache entry for <paramref name="pdfPath"/>.
        /// <paramref name="personalData"/> may be null — in that case NER results are
        /// not stored and will be re-computed on next open.
        /// </summary>
        internal static void Save(
            string pdfPath,
            List<PdfTextSearcher.CachedLine> lines,
            List<PdfTextSearcher.AltTextEntry> altTexts,
            List<PdfTextSearcher.AltTextEntry> allFigures,
            List<TextLocation> personalData)
        {
            try
            {
                var fi = new FileInfo(pdfPath);
                if (!fi.Exists) return;

                var file = new PdfCacheFile
                {
                    Version               = FormatVersion,
                    FileSize              = fi.Length,
                    LastWriteTimeUtcTicks = fi.LastWriteTimeUtc.Ticks,
                    Lines                 = ToDto(lines),
                    AltTexts              = ToDto(altTexts),
                    AllFigures            = ToDto(allFigures),
                    PersonalData          = personalData != null ? ToDto(personalData) : null,
                    NerCacheIdentity      = PdfTextSearcher.GetNerCacheIdentity(),
                };

                Directory.CreateDirectory(CacheDir);
                string cachePath = GetCacheFilePath(pdfPath);
                string tmpPath   = cachePath + ".tmp";

                byte[] bytes = MessagePackSerializer.Serialize(
                    file, MessagePackSerializerOptions.Standard
                              .WithCompression(MessagePackCompression.Lz4BlockArray));

                File.WriteAllBytes(tmpPath, bytes);
                if (File.Exists(cachePath)) File.Delete(cachePath);
                File.Move(tmpPath, cachePath);
                DbgLog($"Saved {Path.GetFileName(pdfPath)} → {Path.GetFileName(cachePath)} ({bytes.Length / 1024} KB)");
            }
            catch (Exception ex)
            {
                var sb = new System.Text.StringBuilder();
                for (var e = ex; e != null; e = e.InnerException)
                    sb.Append($"[{e.GetType().Name}: {e.Message}] ");
                DbgLog($"Save FAILED for {Path.GetFileName(pdfPath)}: {sb}");
            }
        }

        // ── overwrite helper ─────────────────────────────────────────────────────
        internal static void Save(
            string pdfPath,
            List<PdfTextSearcher.CachedLine> lines,
            List<PdfTextSearcher.AltTextEntry> altTexts,
            List<PdfTextSearcher.AltTextEntry> allFigures)
            => Save(pdfPath, lines, altTexts, allFigures, null);

        // ── DTO converters ───────────────────────────────────────────────────────

        private static List<CachedLineDto> ToDto(List<PdfTextSearcher.CachedLine> src)
        {
            if (src == null) return null;
            var r = new List<CachedLineDto>(src.Count);
            foreach (var l in src)
            {
                var dto = new CachedLineDto
                {
                    PageNumber  = l.PageNumber,
                    PageRotation = l.PageRotation,
                    Text        = l.Text,
                    YPosition   = l.YPosition,
                    IsOcr       = l.IsOcr,
                    PageWidth   = l.PageWidth,
                    PageHeight  = l.PageHeight,
                };
                if (l.Characters != null && l.Characters.Count > 0)
                {
                    dto.Characters = new List<CharInfoDto>(l.Characters.Count);
                    foreach (var c in l.Characters)
                    {
                        var bb = c.BoundingBox;
                        dto.Characters.Add(new CharInfoDto
                        {
                            Ch = (ushort)c.Char,
                            X  = bb != null ? bb.GetX()      : 0,
                            Y  = bb != null ? bb.GetY()      : 0,
                            W  = bb != null ? bb.GetWidth()  : 0,
                            H  = bb != null ? bb.GetHeight() : 0,
                        });
                    }
                }
                if (l.OcrWords != null && l.OcrWords.Count > 0)
                {
                    dto.OcrWords = new List<OcrWordDto>(l.OcrWords.Count);
                    foreach (var w in l.OcrWords)
                    {
                        var bb = w.BoundingBox;
                        dto.OcrWords.Add(new OcrWordDto
                        {
                            Text        = w.Text,
                            StartIndex  = w.StartIndex,
                            Length      = w.Length,
                            X = bb != null ? bb.GetX()      : 0,
                            Y = bb != null ? bb.GetY()      : 0,
                            W = bb != null ? bb.GetWidth()  : 0,
                            H = bb != null ? bb.GetHeight() : 0,
                        });
                    }
                }
                if (l.OcrWordBounds != null && l.OcrWordBounds.Count > 0)
                {
                    dto.OcrWordBoundsFlat = RectListToFlat(l.OcrWordBounds);
                    dto.RawOcrWordBoundsFlat = RectListToFlat(l.RawOcrWordBounds);
                }
                r.Add(dto);
            }
            return r;
        }

        private static List<AltEntryDto> ToDto(List<PdfTextSearcher.AltTextEntry> src)
        {
            if (src == null) return null;
            var r = new List<AltEntryDto>(src.Count);
            foreach (var e in src)
            {
                var bb = e.BBox;
                r.Add(new AltEntryDto
                {
                    PageNumber   = e.PageNumber,
                    PageRotation = e.PageRotation,
                    PageWidth    = e.PageWidth,
                    PageHeight   = e.PageHeight,
                    BBoxX = bb != null ? bb.GetX()      : 0,
                    BBoxY = bb != null ? bb.GetY()      : 0,
                    BBoxW = bb != null ? bb.GetWidth()  : 0,
                    BBoxH = bb != null ? bb.GetHeight() : 0,
                    AltText    = e.AltText,
                    StructXref = e.StructXref,
                    Mcid       = e.Mcid,
                });
            }
            return r;
        }

        private static List<TextLocDto> ToDto(List<TextLocation> src)
        {
            if (src == null) return null;
            var r = new List<TextLocDto>(src.Count);
            foreach (var loc in src)
            {
                var rc = loc.Rect;
                r.Add(new TextLocDto
                {
                    PageNumber   = loc.PageNumber,
                    PageRotation = loc.PageRotation,
                    RX = rc != null ? rc.GetX()      : 0,
                    RY = rc != null ? rc.GetY()      : 0,
                    RW = rc != null ? rc.GetWidth()  : 0,
                    RH = rc != null ? rc.GetHeight() : 0,
                    IsOcr          = loc.IsOcr,
                    IsExactOcrWord = loc.IsExactOcrWord,
                    Label  = loc.Label,
                    Text   = loc.Text,
                    Source = (int)loc.Source,
                    HighlightRectsFlat = loc.HighlightRects != null && loc.HighlightRects.Count > 0
                        ? RectListToFlat(loc.HighlightRects)
                        : null,
                });
            }
            return r;
        }

        // ── DTO → domain ─────────────────────────────────────────────────────────

        internal static List<PdfTextSearcher.CachedLine> FromDto(List<CachedLineDto> src)
        {
            if (src == null) return new List<PdfTextSearcher.CachedLine>();
            var r = new List<PdfTextSearcher.CachedLine>(src.Count);
            foreach (var dto in src)
            {
                var line = new PdfTextSearcher.CachedLine
                {
                    PageNumber   = dto.PageNumber,
                    PageRotation = dto.PageRotation,
                    Text         = dto.Text ?? "",
                    YPosition    = dto.YPosition,
                    IsOcr        = dto.IsOcr,
                    PageWidth    = dto.PageWidth,
                    PageHeight   = dto.PageHeight,
                };
                if (dto.Characters != null)
                    foreach (var c in dto.Characters)
                        line.Characters.Add(new PdfTextSearcher.CharacterInfo
                        {
                            Char = (char)c.Ch,
                            BoundingBox = new iText.Kernel.Geom.Rectangle(c.X, c.Y, c.W, c.H),
                        });
                if (dto.OcrWords != null)
                    foreach (var w in dto.OcrWords)
                        line.OcrWords.Add(new PdfTextSearcher.OcrWordInfo
                        {
                            Text        = w.Text,
                            StartIndex  = w.StartIndex,
                            Length      = w.Length,
                            BoundingBox = new iText.Kernel.Geom.Rectangle(w.X, w.Y, w.W, w.H),
                        });
                if (dto.OcrWordBoundsFlat != null)
                {
                    line.OcrWordBounds    = FlatToRectList(dto.OcrWordBoundsFlat);
                    line.RawOcrWordBounds = FlatToRectList(dto.RawOcrWordBoundsFlat);
                }
                r.Add(line);
            }
            return r;
        }

        internal static List<PdfTextSearcher.AltTextEntry> AltFromDto(List<AltEntryDto> src)
        {
            if (src == null) return new List<PdfTextSearcher.AltTextEntry>();
            var r = new List<PdfTextSearcher.AltTextEntry>(src.Count);
            foreach (var dto in src)
                r.Add(new PdfTextSearcher.AltTextEntry
                {
                    PageNumber   = dto.PageNumber,
                    PageRotation = dto.PageRotation,
                    PageWidth    = dto.PageWidth,
                    PageHeight   = dto.PageHeight,
                    BBox         = new iText.Kernel.Geom.Rectangle(dto.BBoxX, dto.BBoxY, dto.BBoxW, dto.BBoxH),
                    AltText      = dto.AltText,
                    StructXref   = dto.StructXref,
                    Mcid         = dto.Mcid,
                });
            return r;
        }

        internal static List<TextLocation> TextLocFromDto(List<TextLocDto> src)
        {
            if (src == null) return null;
            var r = new List<TextLocation>(src.Count);
            foreach (var dto in src)
                r.Add(new TextLocation(dto.PageNumber, dto.PageRotation,
                                       new iText.Kernel.Geom.Rectangle(dto.RX, dto.RY, dto.RW, dto.RH),
                                       dto.IsOcr, dto.IsExactOcrWord)
                {
                    Label  = dto.Label,
                    Text   = dto.Text,
                    Source = (LocationSource)dto.Source,
                    HighlightRects = dto.HighlightRectsFlat != null && dto.HighlightRectsFlat.Length > 0
                        ? FlatToRectList(dto.HighlightRectsFlat)
                        : null,
                });
            return r;
        }

        // ── helpers ──────────────────────────────────────────────────────────────

        private static float[] RectListToFlat(List<iText.Kernel.Geom.Rectangle> list)
        {
            if (list == null || list.Count == 0) return null;
            var flat = new float[list.Count * 4];
            for (int i = 0; i < list.Count; i++)
            {
                var r = list[i];
                flat[i * 4]     = r.GetX();
                flat[i * 4 + 1] = r.GetY();
                flat[i * 4 + 2] = r.GetWidth();
                flat[i * 4 + 3] = r.GetHeight();
            }
            return flat;
        }

        private static List<iText.Kernel.Geom.Rectangle> FlatToRectList(float[] flat)
        {
            if (flat == null || flat.Length == 0) return new List<iText.Kernel.Geom.Rectangle>();
            var list = new List<iText.Kernel.Geom.Rectangle>(flat.Length / 4);
            for (int i = 0; i + 3 < flat.Length; i += 4)
                list.Add(new iText.Kernel.Geom.Rectangle(flat[i], flat[i + 1], flat[i + 2], flat[i + 3]));
            return list;
        }

        /// <summary>Deletes the cache file for <paramref name="pdfPath"/> if it exists.</summary>
        internal static void Invalidate(string pdfPath)
        {
            try { File.Delete(GetCacheFilePath(pdfPath)); } catch { }
        }

        private static readonly string _dbgLog =
            Path.Combine(Path.GetTempPath(), "AnonPDF-debug.log");

        private static void DbgLog(string msg)
        {
            try { File.AppendAllText(_dbgLog, $"{DateTime.Now:HH:mm:ss.fff} [DiskCache] {msg}\r\n"); } catch { }
        }
    }

    // ── MessagePack DTOs ─────────────────────────────────────────────────────────

    [MessagePackObject]
    public sealed class PdfCacheFile
    {
        [Key(0)] public int    Version               { get; set; }
        [Key(1)] public long   FileSize              { get; set; }
        [Key(2)] public long   LastWriteTimeUtcTicks { get; set; }
        [Key(3)] public List<CachedLineDto> Lines    { get; set; }
        [Key(4)] public List<AltEntryDto>   AltTexts  { get; set; }
        [Key(5)] public List<AltEntryDto>   AllFigures{ get; set; }
        [Key(6)] public List<TextLocDto>    PersonalData { get; set; }
        [Key(7)] public string NerCacheIdentity { get; set; }
    }

    [MessagePackObject]
    public sealed class CachedLineDto
    {
        [Key(0)]  public int    PageNumber   { get; set; }
        [Key(1)]  public int    PageRotation { get; set; }
        [Key(2)]  public string Text         { get; set; }
        [Key(3)]  public float  YPosition    { get; set; }
        [Key(4)]  public bool   IsOcr        { get; set; }
        [Key(5)]  public float  PageWidth    { get; set; }
        [Key(6)]  public float  PageHeight   { get; set; }
        [Key(7)]  public List<CharInfoDto>  Characters       { get; set; }
        [Key(8)]  public List<OcrWordDto>   OcrWords         { get; set; }
        [Key(9)]  public float[]            OcrWordBoundsFlat    { get; set; }
        [Key(10)] public float[]            RawOcrWordBoundsFlat { get; set; }
    }

    [MessagePackObject]
    public sealed class CharInfoDto
    {
        [Key(0)] public ushort Ch { get; set; }  // char cast to ushort — char unsupported by standard resolver
        [Key(1)] public float  X  { get; set; }
        [Key(2)] public float  Y  { get; set; }
        [Key(3)] public float  W  { get; set; }
        [Key(4)] public float  H  { get; set; }
    }

    [MessagePackObject]
    public sealed class OcrWordDto
    {
        [Key(0)] public string Text       { get; set; }
        [Key(1)] public int    StartIndex { get; set; }
        [Key(2)] public int    Length     { get; set; }
        [Key(3)] public float  X { get; set; }
        [Key(4)] public float  Y { get; set; }
        [Key(5)] public float  W { get; set; }
        [Key(6)] public float  H { get; set; }
    }

    [MessagePackObject]
    public sealed class AltEntryDto
    {
        [Key(0)] public int    PageNumber   { get; set; }
        [Key(1)] public int    PageRotation { get; set; }
        [Key(2)] public float  PageWidth    { get; set; }
        [Key(3)] public float  PageHeight   { get; set; }
        [Key(4)] public float  BBoxX        { get; set; }
        [Key(5)] public float  BBoxY        { get; set; }
        [Key(6)] public float  BBoxW        { get; set; }
        [Key(7)] public float  BBoxH        { get; set; }
        [Key(8)] public string AltText      { get; set; }
        [Key(9)] public int    StructXref   { get; set; }
        [Key(10)] public int   Mcid         { get; set; }
    }

    [MessagePackObject]
    public sealed class TextLocDto
    {
        [Key(0)] public int    PageNumber     { get; set; }
        [Key(1)] public int    PageRotation   { get; set; }
        [Key(2)] public float  RX             { get; set; }
        [Key(3)] public float  RY             { get; set; }
        [Key(4)] public float  RW             { get; set; }
        [Key(5)] public float  RH             { get; set; }
        [Key(6)] public bool   IsOcr          { get; set; }
        [Key(7)] public bool   IsExactOcrWord { get; set; }
        [Key(8)] public string Label          { get; set; }
        [Key(9)] public string Text           { get; set; }
        [Key(10)] public int   Source         { get; set; }
        [Key(11)] public float[] HighlightRectsFlat { get; set; }
    }
}
