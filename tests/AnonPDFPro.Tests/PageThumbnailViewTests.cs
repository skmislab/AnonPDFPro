using System;
using System.Drawing;
using System.Linq;
using AnonPDF;
using NUnit.Framework;

namespace AnonPDFPro.Tests;

[TestFixture]
public class PageThumbnailViewTests
{
    [Test]
    public void PagesRunLeftToRightAndThenContinueOnTheNextRow()
    {
        var layout = new PageThumbnailLayout(800, 900, new Size(180, 220));
        Assert.That(layout.Columns, Is.EqualTo(5));
        Assert.That(layout.GetItemRectangle(0, 0).Location, Is.EqualTo(new Point(0, 0)));
        Assert.That(layout.GetItemRectangle(1, 0).Location, Is.EqualTo(new Point(180, 0)));
        Assert.That(layout.GetItemRectangle(4, 0).Location, Is.EqualTo(new Point(720, 0)));
        Assert.That(layout.GetItemRectangle(5, 0).Location, Is.EqualTo(new Point(0, 220)));
        Assert.That(layout.GetItemRectangle(799, 0).Location, Is.EqualTo(new Point(720, 34980)));
    }

    [TestCase(96)]
    [TestCase(120)]
    [TestCase(144)]
    [TestCase(168)]
    [TestCase(192)]
    public void VirtualizationOnlyVisitsVisibleRowsAndANearbyBufferFor800Pages(int dpi)
    {
        var metrics = PageThumbnailMetrics.Calculate(dpi, (int)Math.Ceiling(15 * dpi / 96f));
        var layout = new PageThumbnailLayout(800, 900, new Size(metrics.ColumnWidth, metrics.ItemHeight));
        int height = 580;
        for (int scroll = 0; scroll < layout.ContentHeight; scroll += metrics.ItemHeight)
        {
            var visible = layout.VisibleIndices(scroll, height).ToArray();
            var nearby = layout.VisibleIndices(scroll, height, 1).ToArray();
            Assert.That(visible.Length, Is.LessThanOrEqualTo(layout.Columns * ((height + metrics.ItemHeight - 1) / metrics.ItemHeight + 1)));
            Assert.That(nearby.Length, Is.LessThanOrEqualTo(visible.Length + 2 * layout.Columns));
            Assert.That(visible.All(i => layout.GetItemRectangle(i, scroll).IntersectsWith(new Rectangle(0, 0, 900, height))), Is.True);
            Assert.That(nearby.All(i => i >= 0 && i < 800), Is.True);
        }
        Assert.That(layout.VisibleIndices(layout.ContentHeight - height, height), Does.Contain(799));
    }

    [Test]
    public void HitTestingTracksTheScrolledGridAndRejectsEmptyCells()
    {
        var layout = new PageThumbnailLayout(7, 900, new Size(180, 220));
        Assert.That(layout.IndexFromPoint(new Point(90, 110), 220), Is.EqualTo(5));
        Assert.That(layout.IndexFromPoint(new Point(270, 110), 220), Is.EqualTo(6));
        Assert.That(layout.IndexFromPoint(new Point(450, 110), 220), Is.EqualTo(-1));
        Assert.That(layout.IndexFromPoint(new Point(-1, 110), 0), Is.EqualTo(-1));
        Assert.That(layout.IndexFromPoint(new Point(900, 110), 0), Is.EqualTo(-1));
        Assert.That(layout.IndexFromPoint(new Point(90, -1), 0), Is.EqualTo(-1));
    }

    [Test]
    public void InsertionAtARowBoundaryUsesTheSideUnderThePointer()
    {
        var layout = new PageThumbnailLayout(12, 900, new Size(180, 220));
        Assert.That(layout.DropGap(new Point(890, 100), 0, out int item, out bool after), Is.EqualTo(5));
        Assert.That(item, Is.EqualTo(4));
        Assert.That(after, Is.True);
        Assert.That(layout.DropGap(new Point(10, 320), 0, out item, out after), Is.EqualTo(5));
        Assert.That(item, Is.EqualTo(5));
        Assert.That(after, Is.False);
        Assert.That(layout.DropGap(new Point(800, 540), 0, out item, out after), Is.EqualTo(12));
        Assert.That(item, Is.EqualTo(11));
        Assert.That(after, Is.True);
    }

    [Test]
    public void EmptyAndNarrowViewsStillHaveValidGeometry()
    {
        var empty = new PageThumbnailLayout(0, 0, new Size(180, 220));
        Assert.That(empty.VisibleIndices(0, 580), Is.Empty);
        Assert.That(empty.DropGap(Point.Empty, 0, out int index, out _), Is.Zero);
        Assert.That(index, Is.EqualTo(-1));
        var narrow = new PageThumbnailLayout(800, 100, new Size(180, 220));
        Assert.That(narrow.Columns, Is.EqualTo(1));
        Assert.That(narrow.ItemSize.Width, Is.EqualTo(100));
        Assert.That(narrow.IndexFromPoint(new Point(50, 330), 0), Is.EqualTo(1));
    }

    [Test]
    public void ShiftAndControlSelectionPreserveCompleteRangesAndIndividualChoices()
    {
        var selection = new PageThumbnailSelection();
        selection.Reset(800, Array.Empty<int>());
        selection.Select(0, false, false);
        selection.Select(3, false, true);
        Assert.That(selection.Indices, Is.EqualTo(new[] { 0, 1, 2, 3 }));
        selection.Select(1, true, false);
        Assert.That(selection.Indices, Is.EqualTo(new[] { 0, 2, 3 }));
        selection.Select(5, true, false);
        Assert.That(selection.Indices, Is.EqualTo(new[] { 0, 2, 3, 5 }));
        selection.Focus(2);
        Assert.That(selection.Indices, Is.EqualTo(new[] { 0, 2, 3, 5 }));
        selection.Select(2, false, false);
        Assert.That(selection.Indices, Is.EqualTo(new[] { 2 }));
    }

    [Test]
    public void SelectingOnePageAfterSelectAllCollapsesTheSelection()
    {
        var selection = new PageThumbnailSelection();
        selection.Reset(800, Array.Empty<int>());
        selection.SelectAll(true);
        Assert.That(selection.Indices.Count, Is.EqualTo(800));
        selection.Select(27, false, false);
        Assert.That(selection.Indices, Is.EqualTo(new[] { 27 }));
        selection.SelectAll(false);
        Assert.That(selection.Indices, Is.Empty);
    }

    [Test]
    public void ScrollingThrough800PagesKeepsOnlyNearbyBitmapsAndDisposesTheRest()
    {
        var layout = new PageThumbnailLayout(800, 900, new Size(180, 220));
        using var cache = new PageThumbnailCache();
        Bitmap first = null;
        for (int scroll = 0; scroll < layout.ContentHeight; scroll += 220)
        {
            var nearby = layout.VisibleIndices(scroll, 580, 1).Select(i => i.ToString()).ToArray();
            cache.Retain(nearby);
            foreach (string key in nearby)
            {
                if (cache.ContainsKey(key)) continue;
                var image = new Bitmap(1, 1);
                if (key == "0") first = image;
                cache.Add(key, image);
            }
            Assert.That(cache.Count, Is.EqualTo(nearby.Length).And.LessThanOrEqualTo(25));
        }
        Assert.That(first, Is.Not.Null);
        Assert.Throws<ArgumentException>(() => first.GetPixel(0, 0));
        cache.Clear();
        Assert.That(cache.Count, Is.Zero);
    }
}
